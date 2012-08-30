using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class TimeSpanSerializer : ContentTypeSerializer<TimeSpan>
	{
		protected internal override void Serialize(IntermediateWriter output, TimeSpan value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override TimeSpan Deserialize(IntermediateReader input, ContentSerializerAttribute format, TimeSpan existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToTimeSpan(input.Xml.ReadContentAsString());
		}
	}
}
