using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class RectangleSerializer : XmlListItemSerializer<Rectangle>
	{
		protected internal override void Serialize(IntermediateWriter output, Rectangle value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value.X));
			output.Xml.WriteWhitespace(" ");
			output.Xml.WriteString(XmlConvert.ToString(value.Y));
			output.Xml.WriteWhitespace(" ");
			output.Xml.WriteString(XmlConvert.ToString(value.Width));
			output.Xml.WriteWhitespace(" ");
			output.Xml.WriteString(XmlConvert.ToString(value.Height));
		}
		protected internal override Rectangle Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return new Rectangle
			{
				X = XmlConvert.ToInt32(input.ReadString()),
				Y = XmlConvert.ToInt32(input.ReadString()),
				Width = XmlConvert.ToInt32(input.ReadString()),
				Height = XmlConvert.ToInt32(input.ReadString())
			};
		}
	}
}
