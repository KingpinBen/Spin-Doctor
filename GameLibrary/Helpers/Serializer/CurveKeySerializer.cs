using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class CurveKeySerializer : MathTypeSerializer<CurveKey>
	{
		protected internal override void Serialize(IntermediateWriter output, CurveKey value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			MathTypeSerializer<CurveKey>.WritePart(output, value.Position);
			MathTypeSerializer<CurveKey>.WritePart(output, value.Value);
			MathTypeSerializer<CurveKey>.WritePart(output, value.TangentIn);
			MathTypeSerializer<CurveKey>.WritePart(output, value.TangentOut);
			output.Xml.WriteString(value.Continuity.ToString());
		}
		protected internal override CurveKey Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			float position = MathTypeSerializer<CurveKey>.ReadSingle(input);
			float value = MathTypeSerializer<CurveKey>.ReadSingle(input);
			float tangentIn = MathTypeSerializer<CurveKey>.ReadSingle(input);
			float tangentOut = MathTypeSerializer<CurveKey>.ReadSingle(input);
			CurveContinuity continuity = (CurveContinuity)Enum.Parse(typeof(CurveContinuity), input.ReadString());
			return new CurveKey(position, value, tangentIn, tangentOut, continuity);
		}
	}
}
