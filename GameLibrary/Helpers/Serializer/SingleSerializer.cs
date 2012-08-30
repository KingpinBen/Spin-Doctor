using System;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class SingleSerializer : XmlListItemSerializer<float>
	{
		public SingleSerializer() : base("float")
		{
		}
		protected internal override void Serialize(IntermediateWriter output, float value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(XmlConvert.ToString(value));
		}
		protected internal override float Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return XmlConvert.ToSingle(input.ReadString());
		}
	}
}
