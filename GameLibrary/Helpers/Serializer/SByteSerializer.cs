using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class SByteSerializer : XmlListItemSerializer<sbyte>
	{
		public SByteSerializer() : base("sbyte")
		{
		}
		protected internal override void Serialize(IntermediateWriter output, sbyte value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override sbyte Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToSByte(input.ReadString());
		}
	}
}
