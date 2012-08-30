using System;
using System.Reflection;
using System.Reflection.Emit;
namespace GameLibrary.Helpers.Serializer
{
	internal static class ReflectionEmitUtils
	{
		public delegate object GetterMethod(object parent);
		public delegate void SetterMethod(object parent, object value);
		public delegate object ConstructorMethod();
		private static T GenerateMethod<T>(Type returnType, Type[] argumentTypes, Type ownerType, Action<ILGenerator> codeGenerator)
		{
			DynamicMethod dynamicMethod;
			if (ownerType.IsArray || ownerType.IsInterface)
			{
				dynamicMethod = new DynamicMethod("ReflectionEmitUtils", returnType, argumentTypes, ownerType.Module);
			}
			else
			{
				dynamicMethod = new DynamicMethod("ReflectionEmitUtils", returnType, argumentTypes, ownerType);
			}
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			codeGenerator(iLGenerator);
			iLGenerator.Emit(OpCodes.Ret);
			return (T)((object)dynamicMethod.CreateDelegate(typeof(T)));
		}
		private static ReflectionEmitUtils.GetterMethod GenerateGetter(Type memberType, Type parentType, Action<ILGenerator> codeGenerator)
		{
			Type typeFromHandle = typeof(object);
			Type[] argumentTypes = new Type[]
			{
				typeof(object)
			};
			return ReflectionEmitUtils.GenerateMethod<ReflectionEmitUtils.GetterMethod>(typeFromHandle, argumentTypes, parentType, delegate(ILGenerator il)
			{
				il.Emit(OpCodes.Ldarg_0);
				if (parentType.IsValueType)
				{
					il.Emit(OpCodes.Unbox, parentType);
				}
				else
				{
					il.Emit(OpCodes.Castclass, parentType);
				}
				codeGenerator(il);
				if (memberType.IsValueType)
				{
					il.Emit(OpCodes.Box, memberType);
				}
			});
		}
		public static ReflectionEmitUtils.GetterMethod GenerateGetter(FieldInfo fieldInfo)
		{
			return ReflectionEmitUtils.GenerateGetter(fieldInfo.FieldType, fieldInfo.DeclaringType, delegate(ILGenerator il)
			{
				il.Emit(OpCodes.Ldfld, fieldInfo);
			});
		}
		public static ReflectionEmitUtils.GetterMethod GenerateGetter(PropertyInfo propertyInfo)
		{
			return ReflectionEmitUtils.GenerateGetter(propertyInfo.PropertyType, propertyInfo.DeclaringType, delegate(ILGenerator il)
			{
				MethodInfo getMethod = propertyInfo.GetGetMethod(true);
				il.Emit(OpCodes.Callvirt, getMethod);
			});
		}
		private static ReflectionEmitUtils.SetterMethod GenerateSetter(Type memberType, Type parentType, Action<ILGenerator> codeGenerator)
		{
			Type typeFromHandle = typeof(void);
			Type[] argumentTypes = new Type[]
			{
				typeof(object),
				typeof(object)
			};
			return ReflectionEmitUtils.GenerateMethod<ReflectionEmitUtils.SetterMethod>(typeFromHandle, argumentTypes, parentType, delegate(ILGenerator il)
			{
				il.Emit(OpCodes.Ldarg_0);
				if (parentType.IsValueType)
				{
					il.Emit(OpCodes.Unbox, parentType);
				}
				else
				{
					il.Emit(OpCodes.Castclass, parentType);
				}
				il.Emit(OpCodes.Ldarg_1);
				il.Emit(OpCodes.Unbox_Any, memberType);
				codeGenerator(il);
			});
		}
		public static ReflectionEmitUtils.SetterMethod GenerateSetter(FieldInfo fieldInfo)
		{
			return ReflectionEmitUtils.GenerateSetter(fieldInfo.FieldType, fieldInfo.DeclaringType, delegate(ILGenerator il)
			{
				il.Emit(OpCodes.Stfld, fieldInfo);
			});
		}
		public static ReflectionEmitUtils.SetterMethod GenerateSetter(PropertyInfo propertyInfo)
		{
			return ReflectionEmitUtils.GenerateSetter(propertyInfo.PropertyType, propertyInfo.DeclaringType, delegate(ILGenerator il)
			{
				MethodInfo setMethod = propertyInfo.GetSetMethod(true);
				il.Emit(OpCodes.Callvirt, setMethod);
			});
		}
		public static ReflectionEmitUtils.ConstructorMethod GenerateConstructor(Type type)
		{
			ConstructorInfo constructor = null;
			if (!type.IsValueType)
			{
				constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
				if (constructor == null)
				{
					return null;
				}
			}
			Type typeFromHandle = typeof(object);
			Type[] emptyTypes = Type.EmptyTypes;
			return ReflectionEmitUtils.GenerateMethod<ReflectionEmitUtils.ConstructorMethod>(typeFromHandle, emptyTypes, type, delegate(ILGenerator il)
			{
				if (type.IsValueType)
				{
					il.DeclareLocal(type);
					il.Emit(OpCodes.Ldloca_S, 0);
					il.Emit(OpCodes.Initobj, type);
					il.Emit(OpCodes.Ldloc_0);
					il.Emit(OpCodes.Box, type);
					return;
				}
				il.Emit(OpCodes.Newobj, constructor);
			});
		}
		public static ReflectionEmitUtils.SetterMethod GenerateAddToCollection(Type collectionType, Type elementType)
		{
			Type typeFromHandle = typeof(void);
			Type[] argumentTypes = new Type[]
			{
				typeof(object),
				typeof(object)
			};
			return ReflectionEmitUtils.GenerateMethod<ReflectionEmitUtils.SetterMethod>(typeFromHandle, argumentTypes, elementType, delegate(ILGenerator il)
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Castclass, collectionType);
				il.Emit(OpCodes.Ldarg_1);
				il.Emit(OpCodes.Unbox_Any, elementType);
				Type[] types = new Type[]
				{
					elementType
				};
				MethodInfo method = collectionType.GetMethod("Add", types);
				il.Emit(OpCodes.Callvirt, method);
			});
		}
	}
}
