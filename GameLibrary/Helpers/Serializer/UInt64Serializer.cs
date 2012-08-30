using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class UInt64Serializer : XmlListItemSerializer<ulong>
	{
		public UInt64Serializer() : base("ulong")
		{
		}
		protected internal override void Serialize(IntermediateWriter output, ulong value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override ulong Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToUInt64(input.ReadString());
		}
	}
}
