using System;
using System.Xml;

namespace GameLibrary.Helpers.Serializer
{
	internal abstract class MathTypeSerializer<T> : XmlListItemSerializer<T>
	{
		protected static void WritePart(IntermediateWriter output, float value)
		{
			output.Xml.WriteString(XmlConvert.ToString(value));
			output.Xml.WriteWhitespace(" ");
		}
		protected static void WriteLast(IntermediateWriter output, float value)
		{
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected static float ReadSingle(XmlListReader input)
		{
			return XmlConvert.ToSingle(input.ReadString());
		}
	}
}
