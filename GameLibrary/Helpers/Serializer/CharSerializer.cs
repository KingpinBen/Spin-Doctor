using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class CharSerializer : ContentTypeSerializer<char>
	{
		protected internal override void Serialize(IntermediateWriter output, char value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override char Deserialize(IntermediateReader input, ContentSerializerAttribute format, char existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToChar(input.Xml.ReadContentAsString());
		}
	}
}
