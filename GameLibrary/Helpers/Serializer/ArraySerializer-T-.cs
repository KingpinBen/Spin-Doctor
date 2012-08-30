using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	internal class ArraySerializer<T> : ContentTypeSerializer<T[]>
	{
		private CollectionHelper arrayHelper;
		private CollectionHelper listHelper;
		protected internal override void Initialize(IntermediateSerializer serializer)
		{
			this.arrayHelper = serializer.GetCollectionHelper(typeof(T[]));
			this.listHelper = serializer.GetCollectionHelper(typeof(List<T>));
		}
		protected internal override void Serialize(IntermediateWriter output, T[] value, ContentSerializerAttribute format)
		{
			this.arrayHelper.Serialize(output, format, value);
		}
		protected internal override T[] Deserialize(IntermediateReader input, ContentSerializerAttribute format, T[] existingInstance)
		{
			List<T> list = new List<T>();
			this.listHelper.Deserialize(input, format, list);
			return list.ToArray();
		}
		protected internal override void ScanChildren(IntermediateSerializer serializer, ContentTypeSerializer.ChildCallback callback, T[] value)
		{
			this.arrayHelper.ScanElements(callback, value);
		}
	}
}
