using System;
using System.Collections;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	internal class NonGenericIListSerializer : ContentTypeSerializer
	{
		public override bool CanDeserializeIntoExistingObject
		{
			get
			{
				return true;
			}
		}
		public NonGenericIListSerializer(Type targetType) : base(targetType)
		{
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
			ContentSerializerAttribute contentSerializerAttribute = new ContentSerializerAttribute();
			contentSerializerAttribute.ElementName = format.CollectionItemName;
			foreach (object current in this.CastType(value))
			{
				output.WriteObject<object>(current, contentSerializerAttribute);
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
			IList list;
			if (existingInstance == null)
			{
				list = (IList)Activator.CreateInstance(base.TargetType);
			}
			else
			{
				list = this.CastType(existingInstance);
			}
			ContentSerializerAttribute contentSerializerAttribute = new ContentSerializerAttribute();
			contentSerializerAttribute.ElementName = format.CollectionItemName;
			while (input.MoveToElement(format.CollectionItemName))
			{
				list.Add(input.ReadObject<object>(contentSerializerAttribute));
			}
			return list;
		}
		public override bool ObjectIsEmpty(object value)
		{
			return this.CastType(value).Count == 0;
		}
		private IList CastType(object value)
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
			return (IList)value;
		}
	}
}
