using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	/// <summary>Provides an implementation of many of the methods of IntermediateSerializer including serialization and state tracking for shared resources and external references.</summary>
	public sealed class IntermediateWriter
	{
		private struct ExternalReference
		{
			public Type TargetType;
			public string Filename;
			public string ID;
		}
		private IntermediateSerializer serializer;
		private XmlWriter xmlWriter;
		private Uri baseUri;
		private Dictionary<object, bool> recurseDetector = new Dictionary<object, bool>(new ReferenceEqualityComparer<object>());
		private Dictionary<object, string> sharedResourceNames = new Dictionary<object, string>(new ReferenceEqualityComparer<object>());
		private Queue<object> sharedResources = new Queue<object>();
		private List<IntermediateWriter.ExternalReference> externalReferences = new List<IntermediateWriter.ExternalReference>();
		/// <summary>Gets the parent serializer.</summary>
		public IntermediateSerializer Serializer
		{
			get
			{
				return this.serializer;
			}
		}
		/// <summary>Gets the XML output stream.</summary>
		public XmlWriter Xml
		{
			get
			{
				return this.xmlWriter;
			}
		}
		internal IntermediateWriter(IntermediateSerializer serializer, XmlWriter xmlWriter, Uri baseUri)
		{
			this.serializer = serializer;
			this.xmlWriter = xmlWriter;
			this.baseUri = baseUri;
		}
		/// <summary>Writes a single object to the output XML stream.</summary>
		/// <param name="value">The value to write.</param>
		/// <param name="format">The format of the XML.</param>
		public void WriteObject<T>(T value, ContentSerializerAttribute format)
		{
			this.WriteObject<T>(value, format, this.Serializer.GetTypeSerializer(typeof(T)));
		}
		/// <summary>Writes a single object to the output XML stream, using the specified type hint.</summary>
		/// <param name="value">The value to write.</param>
		/// <param name="format">The format of the XML.</param>
		/// <param name="typeSerializer">The type serializer.</param>
		public void WriteObject<T>(T value, ContentSerializerAttribute format, ContentTypeSerializer typeSerializer)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (typeSerializer == null)
			{
				throw new ArgumentNullException("typeSerializer");
			}
			if (!format.FlattenContent)
			{
				if (string.IsNullOrEmpty(format.ElementName))
				{
					throw new ArgumentException(Resources.NullElementName);
				}
				this.Xml.WriteStartElement(format.ElementName);
			}
			if (value == null)
			{
				if (format.FlattenContent)
				{
					throw new InvalidOperationException(Resources.CantWriteNullInFlattenContentMode);
				}
				this.Xml.WriteAttributeString("Null", "true");
			}
			else
			{
				Type type = value.GetType();
				if (type.IsSubclassOf(typeof(Type)))
				{
					type = typeof(Type);
				}
				if (type != typeSerializer.BoxedTargetType)
				{
					if (format.FlattenContent)
					{
						throw new InvalidOperationException(Resources.CantWriteDynamicTypesInFlattenContentMode);
					}
					typeSerializer = this.Serializer.GetTypeSerializer(type);
					this.Xml.WriteStartAttribute("Type");
					this.WriteTypeName(typeSerializer.TargetType);
					this.Xml.WriteEndAttribute();
				}
				if (this.recurseDetector.ContainsKey(value))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.FoundCyclicReference, new object[]
					{
						value
					}));
				}
				this.recurseDetector.Add(value, true);
				typeSerializer.Serialize(this, value, format);
				this.recurseDetector.Remove(value);
			}
			if (!format.FlattenContent)
			{
				this.Xml.WriteEndElement();
			}
		}
		/// <summary>Writes a single object to the output XML stream using the specified serializer worker.</summary>
		/// <param name="value">The value to write.</param>
		/// <param name="format">The format of the XML.</param>
		public void WriteRawObject<T>(T value, ContentSerializerAttribute format)
		{
			this.WriteRawObject<T>(value, format, this.Serializer.GetTypeSerializer(typeof(T)));
		}
		/// <summary>Writes a single object to the output XML stream as an instance of the specified type.</summary>
		/// <param name="value">The value to write.</param>
		/// <param name="format">The format of the XML.</param>
		/// <param name="typeSerializer">The type serializer.</param>
		public void WriteRawObject<T>(T value, ContentSerializerAttribute format, ContentTypeSerializer typeSerializer)
		{
			if (value == null)
			{
				throw new ArgumentException("value");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (typeSerializer == null)
			{
				throw new ArgumentNullException("typeSerializer");
			}
			if (!format.FlattenContent)
			{
				if (string.IsNullOrEmpty(format.ElementName))
				{
					throw new ArgumentException(Resources.NullElementName);
				}
				this.Xml.WriteStartElement(format.ElementName);
			}
			typeSerializer.Serialize(this, value, format);
			if (!format.FlattenContent)
			{
				this.Xml.WriteEndElement();
			}
		}
		/// <summary>Adds a shared reference to the output XML and records the object to be serialized later.</summary>
		/// <param name="value">The value to write.</param>
		/// <param name="format">The format of the XML.</param>
		public void WriteSharedResource<T>(T value, ContentSerializerAttribute format)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (!format.FlattenContent)
			{
				if (string.IsNullOrEmpty(format.ElementName))
				{
					throw new ArgumentException(Resources.NullElementName);
				}
				this.Xml.WriteStartElement(format.ElementName);
			}
			if (value != null)
			{
				string text;
				if (!this.sharedResourceNames.TryGetValue(value, out text))
				{
					text = "#Resource" + (this.sharedResourceNames.Count + 1).ToString(CultureInfo.InvariantCulture);
					this.sharedResourceNames.Add(value, text);
					this.sharedResources.Enqueue(value);
				}
				this.Xml.WriteString(text);
			}
			if (!format.FlattenContent)
			{
				this.Xml.WriteEndElement();
			}
		}
		internal void WriteSharedResources()
		{
			if (this.sharedResources.Count > 0)
			{
				this.Xml.WriteStartElement("Resources");
				ContentSerializerAttribute contentSerializerAttribute = new ContentSerializerAttribute();
				contentSerializerAttribute.ElementName = "Resource";
				contentSerializerAttribute.FlattenContent = true;
				while (this.sharedResources.Count > 0)
				{
					object obj = this.sharedResources.Dequeue();
					Type type = obj.GetType();
					ContentTypeSerializer typeSerializer = this.Serializer.GetTypeSerializer(type);
					this.Xml.WriteStartElement("Resource");
					this.Xml.WriteAttributeString("ID", this.sharedResourceNames[obj]);
					this.Xml.WriteStartAttribute("Type");
					this.WriteTypeName(type);
					this.Xml.WriteEndAttribute();
					this.WriteRawObject<object>(obj, contentSerializerAttribute, typeSerializer);
					this.Xml.WriteEndElement();
				}
				this.Xml.WriteEndElement();
			}
		}
		/// <summary>Adds an external reference to the output XML, and records the filename to be serialized later.</summary>
		/// <param name="value">The external reference to add.</param>
		public void WriteExternalReference<T>(ExternalReference<T> value)
		{
			if (value == null || value.Filename == null)
			{
				return;
			}
			string relativePath = PathUtils.GetRelativePath(this.baseUri, value.Filename);
			string text = null;
			foreach (IntermediateWriter.ExternalReference current in this.externalReferences)
			{
				if (current.TargetType == typeof(T) && current.Filename == relativePath)
				{
					text = current.ID;
					break;
				}
			}
			if (text == null)
			{
				text = "#External" + (this.externalReferences.Count + 1).ToString(CultureInfo.InvariantCulture);
				IntermediateWriter.ExternalReference item;
				item.TargetType = typeof(T);
				item.Filename = relativePath;
				item.ID = text;
				this.externalReferences.Add(item);
			}
			this.Xml.WriteElementString("Reference", text);
		}
		internal void WriteExternalReferences()
		{
			if (this.externalReferences.Count > 0)
			{
				this.Xml.WriteStartElement("ExternalReferences");
				foreach (IntermediateWriter.ExternalReference current in this.externalReferences)
				{
					this.Xml.WriteStartElement("ExternalReference");
					this.Xml.WriteAttributeString("ID", current.ID);
					this.Xml.WriteStartAttribute("TargetType");
					this.WriteTypeName(current.TargetType);
					this.Xml.WriteEndAttribute();
					this.Xml.WriteString(current.Filename);
					this.Xml.WriteEndElement();
				}
				this.Xml.WriteEndElement();
			}
		}
		/// <summary>Writes a managed type descriptor to the XML output stream.</summary>
		/// <param name="type">The type.</param>
		public void WriteTypeName(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.Serializer.typeNameHelper.WriteTypeName(this.Xml, type);
		}
	}
}
