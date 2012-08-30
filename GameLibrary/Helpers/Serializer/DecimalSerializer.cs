using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class DecimalSerializer : ContentTypeSerializer<decimal>
	{
		protected internal override void Serialize(IntermediateWriter output, decimal value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override decimal Deserialize(IntermediateReader input, ContentSerializerAttribute format, decimal existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToDecimal(input.Xml.ReadContentAsString());
		}
	}
}
