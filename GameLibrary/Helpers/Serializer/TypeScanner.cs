using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace GameLibrary.Helpers.Serializer
{
	internal class TypeScanner : IEnumerable<Type>, IEnumerable
	{
		private Type baseType;
		private Type attributeType;
		private bool includeGenericDefinitions;
		private Action<Exception> customErrorHandler;
		public TypeScanner(Type baseType, Type attributeType, bool includeGenericDefinitions, Action<Exception> customErrorHandler)
		{
			if (baseType == null)
			{
				throw new ArgumentNullException("baseType");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			this.baseType = baseType;
			this.attributeType = attributeType;
			this.includeGenericDefinitions = includeGenericDefinitions;
			this.customErrorHandler = customErrorHandler;
		}
		public IEnumerator<Type> GetEnumerator()
		{
			IEnumerable<Type> enumerable;
			if (WorkerAppDomain.InsideWorkerDomain)
			{
				enumerable = AssemblyScanner.GetSearchAssemblyTypes();
			}
			else
			{
				enumerable = this.GetAllLoadedTypes();
			}
			foreach (Type current in enumerable)
			{
				try
				{
					if (!current.IsDefined(this.attributeType, false))
					{
						continue;
					}
					if (!this.IsSuitableType(current))
					{
                        throw new Exception("Resources.TypeScannerError");
					}
				}
				catch (Exception obj)
				{
					if (this.customErrorHandler != null)
					{
						this.customErrorHandler(obj);
						continue;
					}
					throw;
				}
				yield return current;
			}
			yield break;
		}
		private IEnumerable<Type> GetAllLoadedTypes()
		{
			foreach (Assembly current in new AssemblyScanner(true))
			{
				Type[] types;
				try
				{
					types = current.GetTypes();
				}
				catch
				{
					continue;
				}
				try
				{
					Type[] array = types;
					for (int i = 0; i < array.Length; i++)
					{
						Type type = array[i];
						yield return type;
					}
				}
				finally
				{
				}
			}
			yield break;
		}
		private bool IsSuitableType(Type type)
		{
			if (!type.IsClass)
			{
				return false;
			}
			if (type.IsAbstract)
			{
				return false;
			}
			if (!this.includeGenericDefinitions && type.IsGenericTypeDefinition)
			{
				return false;
			}
			if (type.GetConstructor(Type.EmptyTypes) == null)
			{
				return false;
			}
			if (this.baseType.IsGenericTypeDefinition)
			{
				Type type2 = type.BaseType;
				while (type2 != null && (!type2.IsGenericType || type2.GetGenericTypeDefinition() != this.baseType))
				{
					type2 = type2.BaseType;
				}
				return type2 != null;
			}
			return this.baseType.IsAssignableFrom(type);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
