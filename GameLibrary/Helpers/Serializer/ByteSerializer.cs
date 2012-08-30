using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class ByteSerializer : XmlListItemSerializer<byte>
	{
		public ByteSerializer() : base("byte")
		{
		}
		protected internal override void Serialize(IntermediateWriter output, byte value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override byte Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToByte(input.ReadString());
		}
	}
}
