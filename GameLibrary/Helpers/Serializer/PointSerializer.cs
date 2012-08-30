using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class PointSerializer : XmlListItemSerializer<Point>
	{
		protected internal override void Serialize(IntermediateWriter output, Point value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value.X));
			output.Xml.WriteWhitespace(" ");
			output.Xml.WriteString(XmlConvert.ToString(value.Y));
		}
		protected internal override Point Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return new Point
			{
				X = XmlConvert.ToInt32(input.ReadString()),
				Y = XmlConvert.ToInt32(input.ReadString())
			};
		}
	}
}
