using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class BooleanSerializer : XmlListItemSerializer<bool>
	{
		public BooleanSerializer() : base("bool")
		{
		}
		protected internal override void Serialize(IntermediateWriter output, bool value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override bool Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToBoolean(input.ReadString());
		}
	}
}
