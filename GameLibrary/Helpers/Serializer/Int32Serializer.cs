using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class Int32Serializer : XmlListItemSerializer<int>
	{
		public Int32Serializer() : base("int")
		{
		}
		protected internal override void Serialize(IntermediateWriter output, int value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override int Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToInt32(input.ReadString());
		}
	}
}
