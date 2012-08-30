using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class PlaneSerializer : MathTypeSerializer<Plane>
	{
		protected internal override void Serialize(IntermediateWriter output, Plane value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			MathTypeSerializer<Plane>.WritePart(output, value.Normal.X);
			MathTypeSerializer<Plane>.WritePart(output, value.Normal.Y);
			MathTypeSerializer<Plane>.WritePart(output, value.Normal.Z);
			MathTypeSerializer<Plane>.WriteLast(output, value.D);
		}
		protected internal override Plane Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			Plane result = default(Plane);
			result.Normal.X = MathTypeSerializer<Plane>.ReadSingle(input);
			result.Normal.Y = MathTypeSerializer<Plane>.ReadSingle(input);
			result.Normal.Z = MathTypeSerializer<Plane>.ReadSingle(input);
			result.D = MathTypeSerializer<Plane>.ReadSingle(input);
			return result;
		}
	}
}
