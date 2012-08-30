using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class QuaternionSerializer : MathTypeSerializer<Quaternion>
	{
		protected internal override void Serialize(IntermediateWriter output, Quaternion value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			MathTypeSerializer<Quaternion>.WritePart(output, value.X);
			MathTypeSerializer<Quaternion>.WritePart(output, value.Y);
			MathTypeSerializer<Quaternion>.WritePart(output, value.Z);
			MathTypeSerializer<Quaternion>.WriteLast(output, value.W);
		}
		protected internal override Quaternion Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return new Quaternion
			{
				X = MathTypeSerializer<Quaternion>.ReadSingle(input),
				Y = MathTypeSerializer<Quaternion>.ReadSingle(input),
				Z = MathTypeSerializer<Quaternion>.ReadSingle(input),
				W = MathTypeSerializer<Quaternion>.ReadSingle(input)
			};
		}
	}
}
