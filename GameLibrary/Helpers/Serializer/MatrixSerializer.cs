using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class MatrixSerializer : MathTypeSerializer<Matrix>
	{
		protected internal override void Serialize(IntermediateWriter output, Matrix value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			MathTypeSerializer<Matrix>.WritePart(output, value.M11);
			MathTypeSerializer<Matrix>.WritePart(output, value.M12);
			MathTypeSerializer<Matrix>.WritePart(output, value.M13);
			MathTypeSerializer<Matrix>.WritePart(output, value.M14);
			MathTypeSerializer<Matrix>.WritePart(output, value.M21);
			MathTypeSerializer<Matrix>.WritePart(output, value.M22);
			MathTypeSerializer<Matrix>.WritePart(output, value.M23);
			MathTypeSerializer<Matrix>.WritePart(output, value.M24);
			MathTypeSerializer<Matrix>.WritePart(output, value.M31);
			MathTypeSerializer<Matrix>.WritePart(output, value.M32);
			MathTypeSerializer<Matrix>.WritePart(output, value.M33);
			MathTypeSerializer<Matrix>.WritePart(output, value.M34);
			MathTypeSerializer<Matrix>.WritePart(output, value.M41);
			MathTypeSerializer<Matrix>.WritePart(output, value.M42);
			MathTypeSerializer<Matrix>.WritePart(output, value.M43);
			MathTypeSerializer<Matrix>.WriteLast(output, value.M44);
		}
		protected internal override Matrix Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return new Matrix
			{
				M11 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M12 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M13 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M14 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M21 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M22 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M23 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M24 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M31 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M32 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M33 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M34 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M41 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M42 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M43 = MathTypeSerializer<Matrix>.ReadSingle(input),
				M44 = MathTypeSerializer<Matrix>.ReadSingle(input)
			};
		}
	}
}
