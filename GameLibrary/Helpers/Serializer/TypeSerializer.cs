using System;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class TypeSerializer : ContentTypeSerializer<Type>
	{
		protected internal override void Serialize(IntermediateWriter output, Type value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.WriteTypeName(value);
		}
		protected internal override Type Deserialize(IntermediateReader input, ContentSerializerAttribute format, Type existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return input.ReadTypeName();
		}
	}
}
