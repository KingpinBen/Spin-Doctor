using System;
using System.Collections.Generic;

namespace GameLibrary.Helpers.Serializer
{
	internal abstract class CollectionSerializer<TCollection, TElement> : ContentTypeSerializer<TCollection> where TCollection : ICollection<TElement>
	{
		public override bool CanDeserializeIntoExistingObject
		{
			get
			{
				return true;
			}
		}
		public override bool ObjectIsEmpty(TCollection value)
		{
			return value.Count == 0;
		}
	}
}
