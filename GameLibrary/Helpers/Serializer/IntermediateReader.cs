using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework.Content;
using System.Resources;

namespace GameLibrary.Helpers.Serializer
{
	/// <summary>Provides an implementation of many of the methods of IntermediateSerializer. Deserializes and tracks state for shared resources and external references.</summary>
	public sealed class IntermediateReader
	{
		private struct ExternalReferenceFixup
		{
			public string ID;
			public Type TargetType;
			public Action<string> Fixup;
		}
		private IntermediateSerializer serializer;
		private XmlReader xmlReader;
		private Uri baseUri;
		private Dictionary<string, List<Action<object>>> sharedResourceFixups = new Dictionary<string, List<Action<object>>>();
		private List<IntermediateReader.ExternalReferenceFixup> externalReferenceFixups = new List<IntermediateReader.ExternalReferenceFixup>();
		/// <summary>Gets the parent serializer.</summary>
		public IntermediateSerializer Serializer
		{
			get
			{
				return this.serializer;
			}
		}
		/// <summary>Gets the XML input stream.</summary>
		public XmlReader Xml
		{
			get
			{
				return this.xmlReader;
			}
		}
		internal IntermediateReader(IntermediateSerializer serializer, XmlReader xmlReader, Uri baseUri)
		{
			this.serializer = serializer;
			this.xmlReader = xmlReader;
			this.baseUri = baseUri;
		}
		/// <summary>Reads a single object from the input XML stream.</summary>
		/// <param name="format">The format expected by the type serializer.</param>
		public T ReadObject<T>(ContentSerializerAttribute format)
		{
			return this.ReadObjectInternal<T>(format, this.Serializer.GetTypeSerializer(typeof(T)), null);
		}
		/// <summary>Reads a single object from the input XML stream, optionally specifying an existing instance to receive the data.</summary>
		/// <param name="format">The format expected by the type serializer.</param>
		/// <param name="existingInstance">The object receiving the data, or null if a new instance should be created.</param>
		public T ReadObject<T>(ContentSerializerAttribute format, T existingInstance)
		{
			return this.ReadObjectInternal<T>(format, this.Serializer.GetTypeSerializer(typeof(T)), existingInstance);
		}
		/// <summary>Reads a single object from the input XML stream, using the specified type hint.</summary>
		/// <param name="format">The format of the XML.</param>
		/// <param name="typeSerializer">The type serializer.</param>
		public T ReadObject<T>(ContentSerializerAttribute format, ContentTypeSerializer typeSerializer)
		{
			return this.ReadObjectInternal<T>(format, typeSerializer, null);
		}
		/// <summary>Reads a single object from the input XML stream using the specified type hint, optionally specifying an existing instance to receive the data.</summary>
		/// <param name="format">The format of the XML.</param>
		/// <param name="typeSerializer">The type serializer.</param>
		/// <param name="existingInstance">The object receiving the data, or null if a new instance should be created.</param>
		public T ReadObject<T>(ContentSerializerAttribute format, ContentTypeSerializer typeSerializer, T existingInstance)
		{
			return this.ReadObjectInternal<T>(format, typeSerializer, existingInstance);
		}
		private T ReadObjectInternal<T>(ContentSerializerAttribute format, ContentTypeSerializer typeSerializer, object existingInstance)
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
				if (!this.MoveToElement(format.ElementName))
				{
                    throw new Exception("Element not found.");
				}

				string attribute = this.Xml.GetAttribute("Null");
				if (attribute != null && XmlConvert.ToBoolean(attribute))
				{
					if (!format.AllowNull)
					{
                        throw new Exception("null element not allowed");
					}
					this.Xml.Skip();
					return default(T);
				}
				else
				{
					if (this.Xml.MoveToAttribute("Type"))
					{
						Type type = this.ReadTypeName();
						if (!typeSerializer.IsTargetType(type))
						{
                            throw new Exception("Wrong xml type.");
						}

						typeSerializer = this.Serializer.GetTypeSerializer(type);
						this.Xml.MoveToElement();
					}
					else
					{
						if (typeSerializer.TargetType == typeof(object))
						{
                            throw new Exception("UnknownDeserializationType");
						}
					}
				}
			}
			return this.ReadRawObjectInternal<T>(format, typeSerializer, existingInstance);
		}
		/// <summary>Reads a single object from the input XML stream as an instance of the specified type, optionally specifying an existing instance to receive the data.</summary>
		/// <param name="format">The format of the XML.</param>
		public T ReadRawObject<T>(ContentSerializerAttribute format)
		{
			return this.ReadRawObjectInternal<T>(format, this.Serializer.GetTypeSerializer(typeof(T)), null);
		}
		/// <summary>Reads a single object from the input XML stream, as an instance of the specified type.</summary>
		/// <param name="format">The object.</param>
		/// <param name="existingInstance">The object receiving the data, or null if a new instance should be created.</param>
		public T ReadRawObject<T>(ContentSerializerAttribute format, T existingInstance)
		{
			return this.ReadRawObjectInternal<T>(format, this.Serializer.GetTypeSerializer(typeof(T)), existingInstance);
		}
		/// <summary>Reads a single object from the input XML stream as an instance of the specified type using the specified type hint.</summary>
		/// <param name="format">The format of the XML.</param>
		/// <param name="typeSerializer">The type serializer.</param>
		public T ReadRawObject<T>(ContentSerializerAttribute format, ContentTypeSerializer typeSerializer)
		{
			return this.ReadRawObjectInternal<T>(format, typeSerializer, null);
		}
		/// <summary>Reads a single object from the input XML stream as an instance of the specified type using the specified type hint, optionally specifying an existing instance to receive the data.</summary>
		/// <param name="format">The format of the XML.</param>
		/// <param name="typeSerializer">The type serializer.</param>
		/// <param name="existingInstance">The object receiving the data, or null if a new instance should be created.</param>
		public T ReadRawObject<T>(ContentSerializerAttribute format, ContentTypeSerializer typeSerializer, T existingInstance)
		{
			return this.ReadRawObjectInternal<T>(format, typeSerializer, existingInstance);
		}
		private T ReadRawObjectInternal<T>(ContentSerializerAttribute format, ContentTypeSerializer typeSerializer, object existingInstance)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (typeSerializer == null)
			{
				throw new ArgumentNullException("typeSerializer");
			}
			object obj;
			if (format.FlattenContent)
			{
				this.Xml.MoveToContent();
				obj = typeSerializer.Deserialize(this, format, existingInstance);
			}
			else
			{
				if (!this.MoveToElement(format.ElementName))
				{
					throw this.CreateInvalidContentException(Resources.ElementNotFound, new object[]
					{
						format.ElementName
					});
				}
				XmlReader xmlReader = this.xmlReader;
				if (this.Xml.IsEmptyElement)
				{
					this.xmlReader = FakeEmptyElementReader.Instance;
				}
				xmlReader.ReadStartElement();
				obj = typeSerializer.Deserialize(this, format, existingInstance);
				if (this.xmlReader == xmlReader)
				{
					this.xmlReader.ReadEndElement();
				}
				else
				{
					this.xmlReader = xmlReader;
				}
			}
			if (obj == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DeserializerReturnedNull, new object[]
				{
					typeSerializer.GetType(),
					typeSerializer.TargetType
				}));
			}
			if (existingInstance != null && obj != existingInstance)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DeserializerConstructedNewInstance, new object[]
				{
					typeSerializer.GetType(),
					typeSerializer.TargetType
				}));
			}
			if (!(obj is T))
			{
				throw this.CreateInvalidContentException(Resources.WrongXmlType, new object[]
				{
					typeof(T),
					obj.GetType()
				});
			}
			return (T)((object)obj);
		}
		/// <summary>Reads a shared resource ID and records it for subsequent operations.</summary>
		/// <param name="format">The format of the XML.</param>
		/// <param name="fixup">The fixup operation to perform.</param>
		public void ReadSharedResource<T>(ContentSerializerAttribute format, Action<T> fixup)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (fixup == null)
			{
				throw new ArgumentNullException("fixup");
			}
			string text;
			if (format.FlattenContent)
			{
				text = this.Xml.ReadContentAsString();
			}
			else
			{
				if (!this.MoveToElement(format.ElementName))
				{
                    throw new Exception("element not found");
				}
				text = this.Xml.ReadElementContentAsString();
			}
			if (string.IsNullOrEmpty(text))
			{
				if (!format.AllowNull)
				{
                    throw new Exception("null element found");
				}
			}
			else
			{
				if (!this.sharedResourceFixups.ContainsKey(text))
				{
					this.sharedResourceFixups.Add(text, new List<Action<object>>());
				}
				this.sharedResourceFixups[text].Add(delegate(object value)
				{
					if (!(value is T))
					{
                        throw new Exception("wrong shared resource type");
					}
					fixup((T)((object)value));
				});
			}
		}
		internal void ReadSharedResources()
		{
			if (this.MoveToElement("Resources"))
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				ContentSerializerAttribute contentSerializerAttribute = new ContentSerializerAttribute();
				contentSerializerAttribute.ElementName = "Resource";
				this.Xml.ReadStartElement();
				while (this.MoveToElement("Resource"))
				{
					string attribute = this.Xml.GetAttribute("ID");
					if (string.IsNullOrEmpty(attribute))
					{
                        throw new Exception("missing attribute.");
					}
					if (dictionary.ContainsKey(attribute))
					{
                        throw new Exception("duplicate id found.");
					}
					object value = this.ReadObject<object>(contentSerializerAttribute);
					dictionary.Add(attribute, value);
				}
				this.Xml.ReadEndElement();
				using (Dictionary<string, List<Action<object>>>.Enumerator enumerator = this.sharedResourceFixups.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, List<Action<object>>> current = enumerator.Current;
						object obj;
						if (!dictionary.TryGetValue(current.Key, out obj))
						{
                            throw new Exception("Missing resource");
						}
						foreach (Action<object> current2 in current.Value)
						{
							current2(obj);
						}
					}
					return;
				}
			}
			if (this.sharedResourceFixups.Count > 0)
			{
                throw new Exception("Element not found.");
			}
		}
		/// <summary>Reads an external reference ID and records it for subsequent operations.</summary>
		/// <param name="existingInstance">The object receiving the data, or null if a new instance of the object should be created.</param>
		public void ReadExternalReference<T>(ExternalReference<T> existingInstance)
		{
			if (existingInstance == null)
			{
				throw new ArgumentNullException("existingInstance");
			}
			if (!this.MoveToElement("Reference"))
			{
				return;
			}
			string text = this.Xml.ReadElementContentAsString();
			if (!string.IsNullOrEmpty(text))
			{
				IntermediateReader.ExternalReferenceFixup item;
				item.ID = text;
				item.TargetType = typeof(T);
				item.Fixup = delegate(string filename)
				{
					existingInstance.Filename = filename;
				};
				this.externalReferenceFixups.Add(item);
			}
		}
		internal void ReadExternalReferences()
		{
			if (this.MoveToElement("ExternalReferences"))
			{
				Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
				Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
				this.Xml.ReadStartElement();
				while (this.MoveToElement("ExternalReference"))
				{
					string attribute = this.Xml.GetAttribute("ID");
					if (string.IsNullOrEmpty(attribute))
					{
						throw this.CreateInvalidContentException(Resources.MissingAttribute, new object[]
						{
							"ID"
						});
					}
					if (dictionary.ContainsKey(attribute))
					{
						throw this.CreateInvalidContentException(Resources.DuplicateID, new object[]
						{
							attribute
						});
					}
					if (!this.Xml.MoveToAttribute("TargetType"))
					{
						throw this.CreateInvalidContentException(Resources.MissingAttribute, new object[]
						{
							"TargetType"
						});
					}
					dictionary.Add(attribute, this.ReadTypeName());
					this.Xml.MoveToElement();
					string filename = this.Xml.ReadElementString();
					string absolutePath = PathUtils.GetAbsolutePath(this.baseUri, filename);
					dictionary2.Add(attribute, absolutePath);
				}
				this.Xml.ReadEndElement();
				using (List<IntermediateReader.ExternalReferenceFixup>.Enumerator enumerator = this.externalReferenceFixups.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntermediateReader.ExternalReferenceFixup current = enumerator.Current;
						if (!dictionary.ContainsKey(current.ID))
						{
							throw this.CreateInvalidContentException(Resources.MissingExternalReference, new object[]
							{
								current.ID
							});
						}
						if (dictionary[current.ID] != current.TargetType)
						{
							throw this.CreateInvalidContentException(Resources.WrongExternalReferenceType, new object[]
							{
								current.ID
							});
						}
						current.Fixup(dictionary2[current.ID]);
					}
					return;
				}
			}
			if (this.externalReferenceFixups.Count > 0)
			{
				throw this.CreateInvalidContentException(Resources.ElementNotFound, new object[]
				{
					"ExternalReferences"
				});
			}
		}
		/// <summary>Reads and decodes a type descriptor from the XML input stream.</summary>
		public Type ReadTypeName()
		{
			string typeName = this.Xml.ReadContentAsString();
			return this.Serializer.typeNameHelper.ParseTypeName(this.Xml, typeName);
		}
		/// <summary>Moves to the specified element if the element name exists.</summary>
		/// <param name="elementName">The element name.</param>
		public bool MoveToElement(string elementName)
		{
			if (string.IsNullOrEmpty(elementName))
			{
				throw new ArgumentException(Resources.NullElementName);
			}
			return this.Xml.MoveToContent() == XmlNodeType.Element && this.Xml.Name == elementName;
		}
		internal Exception CreateInvalidContentException(string message, params object[] messageArgs)
		{
			return this.CreateInvalidContentException(null, message, messageArgs);
		}
		internal Exception CreateInvalidContentException(Exception innerException, string message, params object[] messageArgs)
		{
			ContentIdentity contentIdentity = new ContentIdentity();
			if (this.baseUri != null)
			{
				contentIdentity.SourceFilename = this.baseUri.LocalPath;
			}
			else
			{
				contentIdentity.SourceFilename = this.Xml.BaseURI;
			}
			IXmlLineInfo xmlLineInfo = this.Xml as IXmlLineInfo;
			if (xmlLineInfo != null)
			{
				contentIdentity.FragmentIdentifier = string.Format(CultureInfo.InvariantCulture, "{0},{1}", new object[]
				{
					xmlLineInfo.LineNumber,
					xmlLineInfo.LinePosition
				});
			}
			string message2 = string.Format(CultureInfo.CurrentCulture, message, messageArgs);
			return new InvalidContentException(message2, contentIdentity, innerException);
		}
	}
}
