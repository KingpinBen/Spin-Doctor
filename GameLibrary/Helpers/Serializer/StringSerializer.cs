using System;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class StringSerializer : ContentTypeSerializer<string>
	{
		public StringSerializer() : base("string")
		{
		}
		protected internal override void Serialize(IntermediateWriter output, string value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			output.Xml.WriteString(value);
		}
		protected internal override string Deserialize(IntermediateReader input, ContentSerializerAttribute format, string existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return input.Xml.ReadContentAsString();
		}
	}
}
