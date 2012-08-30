using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class ColorSerializer : PackedVectorSerializer32<Color>
	{
		protected internal override void Serialize(IntermediateWriter output, Color value, ContentSerializerAttribute format)
		{
			base.Serialize(output, ColorSerializer.SwapBgra(value), format);
		}
		protected internal override Color Deserialize(XmlListReader input)
		{
			return ColorSerializer.SwapBgra(base.Deserialize(input));
		}
		private static Color SwapBgra(Color value)
		{
			uint packedValue = value.PackedValue;
			value.PackedValue = ((packedValue & 255u) << 16 | (packedValue & 16711680u) >> 16 | (packedValue & 4278255360u));
			return value;
		}
	}
}
