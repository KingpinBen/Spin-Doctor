using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class Vector2Serializer : MathTypeSerializer<Vector2>
	{
		protected internal override void Serialize(IntermediateWriter output, Vector2 value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			MathTypeSerializer<Vector2>.WritePart(output, value.X);
			MathTypeSerializer<Vector2>.WriteLast(output, value.Y);
		}
		protected internal override Vector2 Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return new Vector2
			{
				X = MathTypeSerializer<Vector2>.ReadSingle(input),
				Y = MathTypeSerializer<Vector2>.ReadSingle(input)
			};
		}
	}
}
