using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class DoubleSerializer : XmlListItemSerializer<double>
	{
		public DoubleSerializer() : base("double")
		{
		}
		protected internal override void Serialize(IntermediateWriter output, double value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override double Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToDouble(input.ReadString());
		}
	}
}
