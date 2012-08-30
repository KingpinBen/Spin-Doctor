using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	internal abstract class MemberHelperBase<TSerializer>
	{
		public static void CreateMemberHelpers<TDerived>(TSerializer serializer, Type declaringType, List<TDerived> memberHelpers) where TDerived : MemberHelperBase<TSerializer>, new()
		{
			BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			PropertyInfo[] properties = declaringType.GetProperties(bindingAttr);
			FieldInfo[] fields = declaringType.GetFields(bindingAttr);
			PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				TDerived item = Activator.CreateInstance<TDerived>();
				if (item.TryInitialize(serializer, declaringType, propertyInfo))
				{
					memberHelpers.Add(item);
				}
			}
			FieldInfo[] array2 = fields;
			for (int j = 0; j < array2.Length; j++)
			{
				FieldInfo fieldInfo = array2[j];
				TDerived item2 = Activator.CreateInstance<TDerived>();
				if (item2.TryInitialize(serializer, declaringType, fieldInfo))
				{
					memberHelpers.Add(item2);
				}
			}
		}
		private bool TryInitialize(TSerializer serializer, Type declaringType, FieldInfo fieldInfo)
		{
			bool canRead = true;
			bool canWrite = !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral;
			if (this.ShouldSerializeMember(serializer, declaringType, fieldInfo, fieldInfo.FieldType, fieldInfo.IsPublic, canRead, canWrite))
			{
				this.Initialize(serializer, fieldInfo, canWrite);
				return true;
			}
			MemberHelperBase<TSerializer>.ValidateSkippedMember(fieldInfo);
			return false;
		}
		private bool TryInitialize(TSerializer serializer, Type declaringType, PropertyInfo propertyInfo)
		{
			if (this.ShouldSerializeProperty(serializer, declaringType, propertyInfo))
			{
				this.Initialize(serializer, propertyInfo);
				return true;
			}
			MemberHelperBase<TSerializer>.ValidateSkippedMember(propertyInfo);
			return false;
		}
		protected abstract void Initialize(TSerializer serializer, FieldInfo fieldInfo, bool canWrite);
		protected abstract void Initialize(TSerializer serializer, PropertyInfo propertyInfo);
		private bool ShouldSerializeMember(TSerializer serializer, Type declaringType, MemberInfo memberInfo, Type memberType, bool isPublic, bool canRead, bool canWrite)
		{
			return canRead && !memberInfo.IsDefined(typeof(ContentSerializerIgnoreAttribute), false) && (isPublic || Attribute.GetCustomAttribute(memberInfo, typeof(ContentSerializerAttribute)) != null) && (canWrite || this.CanDeserializeIntoExistingObject(serializer, memberType)) && (!declaringType.IsValueType || !MemberHelperBase<TSerializer>.IsSharedResource(memberInfo));
		}
		private bool ShouldSerializeProperty(TSerializer serializer, Type declaringType, PropertyInfo propertyInfo)
		{
			if (propertyInfo.GetIndexParameters().Length > 0)
			{
				return false;
			}
			bool isPublic = true;
			MethodInfo[] accessors = propertyInfo.GetAccessors(true);
			for (int i = 0; i < accessors.Length; i++)
			{
				MethodInfo methodInfo = accessors[i];
				if (methodInfo.GetBaseDefinition() != methodInfo)
				{
					return false;
				}
				if (!methodInfo.IsPublic)
				{
					isPublic = false;
				}
			}
			return this.ShouldSerializeMember(serializer, declaringType, propertyInfo, propertyInfo.PropertyType, isPublic, propertyInfo.CanRead, propertyInfo.CanWrite);
		}
		protected static bool IsSharedResource(MemberInfo memberInfo)
		{
			Attribute customAttribute = Attribute.GetCustomAttribute(memberInfo, typeof(ContentSerializerAttribute));
			return customAttribute != null && ((ContentSerializerAttribute)customAttribute).SharedResource;
		}
		private static void ValidateSkippedMember(MemberInfo memberInfo)
		{
			if (Attribute.GetCustomAttribute(memberInfo, typeof(ContentSerializerAttribute)) != null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.CantSerializeMember, new object[]
				{
					memberInfo.Name,
					memberInfo.DeclaringType
				}));
			}
		}
		protected abstract bool CanDeserializeIntoExistingObject(TSerializer serializer, Type memberType);
	}
}
