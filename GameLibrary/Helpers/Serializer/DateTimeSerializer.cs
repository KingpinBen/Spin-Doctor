using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class DateTimeSerializer : ContentTypeSerializer<DateTime>
	{
		protected internal override void Serialize(IntermediateWriter output, DateTime value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value, XmlDateTimeSerializationMode.RoundtripKind));
		}
		protected internal override DateTime Deserialize(IntermediateReader input, ContentSerializerAttribute format, DateTime existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToDateTime(input.Xml.ReadContentAsString(), XmlDateTimeSerializationMode.RoundtripKind);
		}
	}
}
