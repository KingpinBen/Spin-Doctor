using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class ListSerializer<T> : CollectionSerializer<List<T>, T>
	{
		private CollectionHelper collectionHelper;
		protected internal override void Initialize(IntermediateSerializer serializer)
		{
			this.collectionHelper = serializer.GetCollectionHelper(typeof(List<T>));
		}
		protected internal override void Serialize(IntermediateWriter output, List<T> value, ContentSerializerAttribute format)
		{
			this.collectionHelper.Serialize(output, format, value);
		}
		protected internal override List<T> Deserialize(IntermediateReader input, ContentSerializerAttribute format, List<T> existingInstance)
		{
			List<T> list = existingInstance;
			if (list == null)
			{
				list = new List<T>();
			}
			this.collectionHelper.Deserialize(input, format, list);
			return list;
		}
		protected internal override void ScanChildren(IntermediateSerializer serializer, ContentTypeSerializer.ChildCallback callback, List<T> value)
		{
			this.collectionHelper.ScanElements(callback, value);
		}
	}
}
