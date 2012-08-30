using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameLibrary.Helpers.Serializer
{
	internal static class CollectionUtils
	{
		public static bool IsCollection(Type type, bool inherit)
		{
			return CollectionUtils.CollectionElementType(type, inherit) != null;
		}
		public static Type CollectionElementType(Type type, bool inherit)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!inherit && type.BaseType != null && CollectionUtils.FindCollectionInterface(type.BaseType) != null)
			{
				return null;
			}
			Type type2 = CollectionUtils.FindCollectionInterface(type);
			if (type2 == null)
			{
				return null;
			}
			return type2.GetGenericArguments()[0];
		}
		private static Type FindCollectionInterface(Type type)
		{
			Type[] array = type.FindInterfaces(new TypeFilter(CollectionUtils.IsCollectionInterface), null);
			if (array.Length == 1)
			{
				return array[0];
			}
			return null;
		}
		private static bool IsCollectionInterface(Type type, object o)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>);
		}
	}
}
