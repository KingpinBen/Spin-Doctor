using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class Vector4Serializer : MathTypeSerializer<Vector4>
	{
		protected internal override void Serialize(IntermediateWriter output, Vector4 value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			MathTypeSerializer<Vector4>.WritePart(output, value.X);
			MathTypeSerializer<Vector4>.WritePart(output, value.Y);
			MathTypeSerializer<Vector4>.WritePart(output, value.Z);
			MathTypeSerializer<Vector4>.WriteLast(output, value.W);
		}
		protected internal override Vector4 Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return new Vector4
			{
				X = MathTypeSerializer<Vector4>.ReadSingle(input),
				Y = MathTypeSerializer<Vector4>.ReadSingle(input),
				Z = MathTypeSerializer<Vector4>.ReadSingle(input),
				W = MathTypeSerializer<Vector4>.ReadSingle(input)
			};
		}
	}
}
