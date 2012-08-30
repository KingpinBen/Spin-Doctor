using System;
using System.Collections;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	internal class NonGenericIDictionarySerializer : ContentTypeSerializer
	{
		private ContentSerializerAttribute keyFormat;
		private ContentSerializerAttribute valueFormat;
		public override bool CanDeserializeIntoExistingObject
		{
			get
			{
				return true;
			}
		}
		public NonGenericIDictionarySerializer(Type targetType) : base(targetType)
		{
			this.keyFormat = new ContentSerializerAttribute();
			this.valueFormat = new ContentSerializerAttribute();
			this.keyFormat.ElementName = "Key";
			this.valueFormat.ElementName = "Value";
		}
		protected internal override void Serialize(IntermediateWriter output, object value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			foreach (DictionaryEntry dictionaryEntry in this.CastType(value))
			{
				output.Xml.WriteStartElement(format.CollectionItemName);
				output.WriteObject<object>(dictionaryEntry.Key, this.keyFormat);
				output.WriteObject<object>(dictionaryEntry.Value, this.valueFormat);
				output.Xml.WriteEndElement();
			}
		}
		protected internal override object Deserialize(IntermediateReader input, ContentSerializerAttribute format, object existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			IDictionary dictionary;
			if (existingInstance == null)
			{
				dictionary = (IDictionary)Activator.CreateInstance(base.TargetType);
			}
			else
			{
				dictionary = this.CastType(existingInstance);
			}
			while (input.MoveToElement(format.CollectionItemName))
			{
				input.Xml.ReadStartElement();
				object key = input.ReadObject<object>(this.keyFormat);
				object value = input.ReadObject<object>(this.valueFormat);
				dictionary.Add(key, value);
				input.Xml.ReadEndElement();
			}
			return dictionary;
		}
		public override bool ObjectIsEmpty(object value)
		{
			return this.CastType(value).Count == 0;
		}
		private IDictionary CastType(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!base.TargetType.IsAssignableFrom(value.GetType()))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.WrongArgumentType, new object[]
				{
					base.TargetType,
					value.GetType()
				}));
			}
			return (IDictionary)value;
		}
	}
}
