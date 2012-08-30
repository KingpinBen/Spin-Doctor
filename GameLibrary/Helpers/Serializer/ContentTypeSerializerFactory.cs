using System;
using System.Collections;

namespace GameLibrary.Helpers.Serializer
{
	internal class ContentTypeSerializerFactory : TypeHandlerFactory<ContentTypeSerializer>
	{
		protected override Type AttributeType
		{
			get
			{
				return typeof(ContentTypeSerializerAttribute);
			}
		}
		protected override Type GenericType
		{
			get
			{
				return typeof(ContentTypeSerializer<>);
			}
		}
		public ContentTypeSerializer CreateSerializer(Type type)
		{
			ContentTypeSerializer contentTypeSerializer = base.CreateHandler(type);
			if (contentTypeSerializer != null)
			{
				return contentTypeSerializer;
			}
			if (type.IsArray)
			{
				return ContentTypeSerializerFactory.InstantiateArraySerializer(type);
			}
			if (type.IsEnum)
			{
				return new EnumSerializer(type);
			}
			if (typeof(IDictionary).IsAssignableFrom(type) && !CollectionUtils.IsCollection(type, true))
			{
				return new NonGenericIDictionarySerializer(type);
			}
			if (typeof(IList).IsAssignableFrom(type) && !CollectionUtils.IsCollection(type, true))
			{
				return new NonGenericIListSerializer(type);
			}
			return new ReflectiveSerializer(type);
		}
		private static ContentTypeSerializer InstantiateArraySerializer(Type type)
		{
			if (type.GetArrayRank() != 1)
			{
				throw new RankException(Resources.CantSerializeMultidimensionalArrays);
			}
			Type type2 = typeof(ArraySerializer<>).MakeGenericType(new Type[]
			{
				type.GetElementType()
			});
			return (ContentTypeSerializer)Activator.CreateInstance(type2);
		}
	}
}
