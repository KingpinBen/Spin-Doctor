using System;
using System.Collections.Generic;
using System.Globalization;

namespace GameLibrary.Helpers.Serializer
{
	internal abstract class TypeHandlerFactory<T> where T : class
	{
		private Dictionary<Type, Type> genericHandlers = new Dictionary<Type, Type>();
		protected abstract Type AttributeType
		{
			get;
		}
		protected abstract Type GenericType
		{
			get;
		}
		public List<T> Initialize()
		{
			List<T> list = new List<T>();
			foreach (Type current in new TypeScanner(typeof(T), this.AttributeType, true, null))
			{
				if (current.IsGenericTypeDefinition)
				{
					this.RegisterGenericHandler(current);
				}
				else
				{
					list.Add((T)((object)Activator.CreateInstance(current)));
				}
			}
			return list;
		}
		private void RegisterGenericHandler(Type handler)
		{
			Type baseType = handler.BaseType;
			while (!baseType.IsGenericType || baseType.GetGenericTypeDefinition() != this.GenericType)
			{
				baseType = baseType.BaseType;
				if (baseType == null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.BadGenericTypeHandler, new object[]
					{
						typeof(T).Name,
						handler
					}));
				}
			}
			Type type = baseType.GetGenericArguments()[0];
			if (!type.IsGenericType)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.BadGenericTypeHandler, new object[]
				{
					typeof(T).Name,
					handler
				}));
			}
			if (!TypeHandlerFactory<T>.TypeArraysAreEqual(handler.GetGenericArguments(), type.GetGenericArguments()))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.BadGenericTypeHandler, new object[]
				{
					typeof(T).Name,
					handler
				}));
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			if (this.genericHandlers.ContainsKey(genericTypeDefinition))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateTypeHandler, new object[]
				{
					typeof(T).Name,
					handler.AssemblyQualifiedName,
					this.genericHandlers[genericTypeDefinition].AssemblyQualifiedName,
					genericTypeDefinition
				}));
			}
			this.genericHandlers.Add(genericTypeDefinition, handler);
		}
		private static bool TypeArraysAreEqual(Type[] a, Type[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}
		protected T CreateHandler(Type type)
		{
			if (type.IsByRef || type.IsPointer)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.BadTypePointerOrReference, new object[]
				{
					type
				}));
			}
			if (type.ContainsGenericParameters)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.BadTypeOpenGeneric, new object[]
				{
					type
				}));
			}
			if (type.IsSubclassOf(typeof(Delegate)))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.BadTypeDelegate, new object[]
				{
					type
				}));
			}
			Type type2;
			if (type.IsGenericType && this.genericHandlers.TryGetValue(type.GetGenericTypeDefinition(), out type2))
			{
				Type type3 = type2.MakeGenericType(type.GetGenericArguments());
				return (T)((object)Activator.CreateInstance(type3));
			}
			return default(T);
		}
	}
}
