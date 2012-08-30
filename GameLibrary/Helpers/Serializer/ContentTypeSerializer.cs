using System;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	/// <summary>Provides methods for serializing and deserializing a specific managed type.</summary>
	public abstract class ContentTypeSerializer
	{
		/// <summary>Callback delegate for the ScanChildren method.</summary>
		/// <param name="typeSerializer">The serializer component used to read or write the child object.</param>
		/// <param name="value">The child object currently being scanned.</param>
		protected internal delegate void ChildCallback(ContentTypeSerializer typeSerializer, object value);
		private Type targetType;
		private string xmlTypeName;
		/// <summary>Gets the type handled by this serializer component.</summary>
		public Type TargetType
		{
			get
			{
				return this.targetType;
			}
		}
		internal virtual Type BoxedTargetType
		{
			get
			{
				return this.targetType;
			}
		}
		/// <summary>Gets a short-form XML name for the target type, or null if there is none.</summary>
		public string XmlTypeName
		{
			get
			{
				return this.xmlTypeName;
			}
		}
		/// <summary>Gets a value indicating whether this component may load data into an existing object or if it must it construct a new instance of the object before loading the data.</summary>
		public virtual bool CanDeserializeIntoExistingObject
		{
			get
			{
				return false;
			}
		}
		/// <summary>Initializes a new instance of the ContentTypeSerializer class for serializing the specified type.</summary>
		/// <param name="targetType">The target type.</param>
		protected ContentTypeSerializer(Type targetType) : this(targetType, null)
		{
		}
		/// <summary>Initializes a new instance of the ContentTypeSerializer class for serializing the specified type using the specified XML shortcut name.</summary>
		/// <param name="targetType">The target type.</param>
		/// <param name="xmlTypeName">The XML shortcut name.</param>
		protected ContentTypeSerializer(Type targetType, string xmlTypeName)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			this.targetType = targetType;
			this.xmlTypeName = xmlTypeName;
		}
		/// <summary>Retrieves and caches any nested type serializers and allows reflection over the target data type.</summary>
		/// <param name="serializer">The content serializer.</param>
		protected internal virtual void Initialize(IntermediateSerializer serializer)
		{
		}
		/// <summary>Serializes an object to intermediate XML format.</summary>
		/// <param name="output">Specifies the intermediate XML location, and provides various serialization helpers.</param>
		/// <param name="value">The object to be serialized.</param>
		/// <param name="format">Specifies the content format for this object.</param>
		protected internal abstract void Serialize(IntermediateWriter output, object value, ContentSerializerAttribute format);
		/// <summary>Deserializes an object from intermediate XML format.</summary>
		/// <param name="input">Location of the intermediate XML and various deserialization helpers.</param>
		/// <param name="format">Specifies the intermediate source XML format.</param>
		/// <param name="existingInstance">The object containing the received data, or null if the deserializer should construct a new instance.</param>
		protected internal abstract object Deserialize(IntermediateReader input, ContentSerializerAttribute format, object existingInstance);
		/// <summary>Queries whether an object contains data to be serialized.</summary>
		/// <param name="value">The object to query.</param>
		public virtual bool ObjectIsEmpty(object value)
		{
			return false;
		}
		internal bool IsTargetType(Type type)
		{
			return this.targetType.IsAssignableFrom(type);
		}
		internal bool IsTargetType(object value)
		{
			return value != null && this.IsTargetType(value.GetType());
		}
		/// <summary>Examines the children of the specified object, passing each to a callback delegate.</summary>
		/// <param name="serializer">The content serializer.</param>
		/// <param name="callback">The method to be called for each examined child.</param>
		/// <param name="value">The object whose children are being scanned.</param>
		protected internal virtual void ScanChildren(IntermediateSerializer serializer, ContentTypeSerializer.ChildCallback callback, object value)
		{
		}
	}
}
