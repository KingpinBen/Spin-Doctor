using System;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	internal abstract class XmlListItemSerializer<T> : ContentTypeSerializer<T>, IXmlListItemSerializer
	{
		protected XmlListItemSerializer()
		{
		}
		protected XmlListItemSerializer(string xmlTypeName) : base(xmlTypeName)
		{
		}
		protected internal abstract T Deserialize(XmlListReader input);
		object IXmlListItemSerializer.Deserialize(XmlListReader list)
		{
			return this.Deserialize(list);
		}
		protected internal override T Deserialize(IntermediateReader input, ContentSerializerAttribute format, T existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			XmlListReader xmlListReader = new XmlListReader(input);
			T result = this.Deserialize(xmlListReader);
			if (!xmlListReader.AtEnd)
			{
				throw input.CreateInvalidContentException(Resources.TooManyEntriesInXmlList, new object[0]);
			}
			return result;
		}
	}
}
