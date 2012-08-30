using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;
namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class UInt16Serializer : XmlListItemSerializer<ushort>
	{
		public UInt16Serializer() : base("ushort")
		{
		}
		protected internal override void Serialize(IntermediateWriter output, ushort value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override ushort Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToUInt16(input.ReadString());
		}
	}
}
