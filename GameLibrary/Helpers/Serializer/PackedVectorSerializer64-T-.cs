using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	internal class PackedVectorSerializer64<T> : XmlListItemSerializer<T> where T : struct, IPackedVector<ulong>
	{
		protected internal override void Serialize(IntermediateWriter output, T value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Xml.WriteString(value.PackedValue.ToString("X16", CultureInfo.InvariantCulture));
		}
		protected internal override T Deserialize(XmlListReader input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			T result = default(T);
			result.PackedValue = ulong.Parse(input.ReadString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			return result;
		}
	}
}
