using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class Vector3Serializer : MathTypeSerializer<Vector3>
	{
		protected internal override void Serialize(IntermediateWriter output, Vector3 value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			MathTypeSerializer<Vector3>.WritePart(output, value.X);
			MathTypeSerializer<Vector3>.WritePart(output, value.Y);
			MathTypeSerializer<Vector3>.WriteLast(output, value.Z);
		}
		protected internal override Vector3 Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return new Vector3
			{
				X = MathTypeSerializer<Vector3>.ReadSingle(input),
				Y = MathTypeSerializer<Vector3>.ReadSingle(input),
				Z = MathTypeSerializer<Vector3>.ReadSingle(input)
			};
		}
	}
}
