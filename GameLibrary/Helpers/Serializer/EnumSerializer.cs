using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	internal class EnumSerializer : ContentTypeSerializer
	{
		public EnumSerializer(Type targetType) : base(targetType)
		{
		}
		protected internal override void Serialize(IntermediateWriter output, object value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!base.IsTargetType(value))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.WrongArgumentType, new object[]
				{
					base.TargetType,
					value.GetType()
				}));
			}
			output.Xml.WriteString(value.ToString());
		}
		protected internal override object Deserialize(IntermediateReader input, ContentSerializerAttribute format, object existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			string text = input.Xml.ReadContentAsString();
			object result;
			try
			{
				result = Enum.Parse(base.TargetType, text);
			}
			catch (ArgumentException innerException)
			{
				throw input.CreateInvalidContentException(innerException, Resources.InvalidEnumValue, new object[]
				{
					text,
					base.TargetType
				});
			}
			return result;
		}
	}
}
