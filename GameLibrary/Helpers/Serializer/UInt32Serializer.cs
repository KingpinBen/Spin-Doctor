using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class UInt32Serializer : XmlListItemSerializer<uint>
	{
		public UInt32Serializer() : base("uint")
		{
		}
		protected internal override void Serialize(IntermediateWriter output, uint value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override uint Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToUInt32(input.ReadString());
		}
	}
}
