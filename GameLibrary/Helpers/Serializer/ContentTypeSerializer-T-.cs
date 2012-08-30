using System;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	/// <summary>Provides a generic implementation of ContentTypeSerializer methods and properties for serializing and deserializing a specific managed type.</summary>
	public abstract class ContentTypeSerializer<T> : ContentTypeSerializer
	{
		/// <summary>Initializes a new instance of the ContentTypeSerializer class.</summary>
		protected ContentTypeSerializer() : base(typeof(T))
		{
		}
		/// <summary>Initializes a new instance of the ContentTypeSerializer class using the specified XML shortcut name.</summary>
		/// <param name="xmlTypeName">The XML shortcut name.</param>
		protected ContentTypeSerializer(string xmlTypeName) : base(typeof(T), xmlTypeName)
		{
		}
		/// <summary>Serializes an object to intermediate XML format.</summary>
		/// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
		/// <param name="value">The strongly typed object to be serialized.</param>
		/// <param name="format">Specifies the content format for this object.</param>
		protected internal abstract void Serialize(IntermediateWriter output, T value, ContentSerializerAttribute format);
		/// <summary>Serializes an object to intermediate XML format.</summary>
		/// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
		/// <param name="value">The object to be serialized.</param>
		/// <param name="format">Specifies the content format for this object.</param>
		protected internal override void Serialize(IntermediateWriter output, object value, ContentSerializerAttribute format)
		{
			this.Serialize(output, ContentTypeSerializer<T>.CastType(value), format);
		}
		/// <summary>Deserializes a strongly typed object from intermediate XML format.</summary>
		/// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
		/// <param name="format">Specifies the intermediate source XML format.</param>
		/// <param name="existingInstance">The strongly typed object containing the received data, or null if the deserializer should construct a new instance.</param>
		protected internal abstract T Deserialize(IntermediateReader input, ContentSerializerAttribute format, T existingInstance);
		/// <summary>Deserializes an object from intermediate XML format.</summary>
		/// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
		/// <param name="format">Specifies the intermediate source XML format.</param>
		/// <param name="existingInstance">The object containing the received data, or null if the deserializer should construct a new instance.</param>
		protected internal override object Deserialize(IntermediateReader input, ContentSerializerAttribute format, object existingInstance)
		{
			T existingInstance2;
			if (existingInstance == null)
			{
				existingInstance2 = default(T);
			}
			else
			{
				existingInstance2 = ContentTypeSerializer<T>.CastType(existingInstance);
			}
			return this.Deserialize(input, format, existingInstance2);
		}
		/// <summary>Queries whether an object contains data to be serialized.</summary>
		/// <param name="value">The object to query.</param>
		public virtual bool ObjectIsEmpty(T value)
		{
			return base.ObjectIsEmpty(value);
		}
		/// <summary>Queries whether a strongly-typed object contains data to be serialized.</summary>
		/// <param name="value">The object to query.</param>
		public override bool ObjectIsEmpty(object value)
		{
			return this.ObjectIsEmpty(ContentTypeSerializer<T>.CastType(value));
		}
		/// <summary>Examines the children of the specified object, passing each to a callback delegate.</summary>
		/// <param name="serializer">The content serializer.</param>
		/// <param name="callback">The method to be called for each examined child.</param>
		/// <param name="value">The strongly typed object whose children are being scanned.</param>
		protected internal virtual void ScanChildren(IntermediateSerializer serializer, ContentTypeSerializer.ChildCallback callback, T value)
		{
		}
		/// <summary>Examines the children of the specified object, passing each to a callback delegate.</summary>
		/// <param name="serializer">The content serializer.</param>
		/// <param name="callback">The method to be called for each examined child.</param>
		/// <param name="value">The object whose children are being scanned.</param>
		protected internal override void ScanChildren(IntermediateSerializer serializer, ContentTypeSerializer.ChildCallback callback, object value)
		{
			this.ScanChildren(serializer, callback, ContentTypeSerializer<T>.CastType(value));
		}
		private static T CastType(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is T))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.WrongArgumentType, new object[]
				{
					typeof(T),
					value.GetType()
				}));
			}
			return (T)((object)value);
		}
	}
}
