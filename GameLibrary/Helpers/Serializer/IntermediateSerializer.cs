using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	/// <summary>Provides methods for reading and writing XNA intermediate XML format.</summary>
	public sealed class IntermediateSerializer
	{
		private static IntermediateSerializer singletonInstance;
		private ContentTypeSerializerFactory typeSerializerFactory = new ContentTypeSerializerFactory();
		private Dictionary<Type, ContentTypeSerializer> serializerInstances = new Dictionary<Type, ContentTypeSerializer>();
		internal TypeNameHelper typeNameHelper = new TypeNameHelper();
		private Dictionary<Type, CollectionHelper> collectionHelpers = new Dictionary<Type, CollectionHelper>();
		private static IntermediateSerializer SingletonInstance
		{
			get
			{
				if (IntermediateSerializer.singletonInstance == null)
				{
					IntermediateSerializer.singletonInstance = new IntermediateSerializer();
				}
				return IntermediateSerializer.singletonInstance;
			}
		}
		private IntermediateSerializer()
		{
			foreach (ContentTypeSerializer current in this.typeSerializerFactory.Initialize())
			{
				this.AddTypeSerializer(current);
			}
			List<ContentTypeSerializer> list = new List<ContentTypeSerializer>(this.serializerInstances.Values);
			foreach (ContentTypeSerializer current2 in list)
			{
				current2.Initialize(this);
			}
		}
		/// <summary>Serializes an object into an intermediate XML file.</summary>
		/// <param name="output">The output XML stream.</param>
		/// <param name="value">The object to be serialized.</param>
		/// <param name="referenceRelocationPath">Final name of the output file, used to relative encode external reference filenames.</param>
		public static void Serialize<T>(XmlWriter output, T value, string referenceRelocationPath)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Uri baseUri = IntermediateSerializer.RelocationPathToUri(referenceRelocationPath);
			IntermediateWriter intermediateWriter = new IntermediateWriter(IntermediateSerializer.SingletonInstance, output, baseUri);
			output.WriteStartElement("XnaContent");
			new XmlNamespaces(intermediateWriter).WriteNamespaces(value);
			ContentSerializerAttribute contentSerializerAttribute = new ContentSerializerAttribute();
			contentSerializerAttribute.ElementName = "Asset";
			intermediateWriter.WriteObject<object>(value, contentSerializerAttribute);
			intermediateWriter.WriteSharedResources();
			intermediateWriter.WriteExternalReferences();
			output.WriteEndElement();
		}
		/// <summary>Deserializes an intermediate XML file into a managed object.</summary>
		/// <param name="input">Intermediate XML file.</param>
		/// <param name="referenceRelocationPath">Final name of the output file used to relative encode external reference filenames.</param>
		public static T Deserialize<T>(XmlReader input, string referenceRelocationPath)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			Uri baseUri = IntermediateSerializer.RelocationPathToUri(referenceRelocationPath);
			IntermediateReader intermediateReader = new IntermediateReader(IntermediateSerializer.SingletonInstance, input, baseUri);
			T result;
			try
			{
				if (!intermediateReader.MoveToElement("XnaContent"))
				{
					throw intermediateReader.CreateInvalidContentException(Resources.NotIntermediateXml, new object[0]);
				}
				input.ReadStartElement();
				T t = intermediateReader.ReadObject<T>(new ContentSerializerAttribute
				{
					ElementName = "Asset"
				});
				intermediateReader.ReadSharedResources();
				intermediateReader.ReadExternalReferences();
				input.ReadEndElement();
				result = t;
			}
			catch (XmlException ex)
			{
				throw intermediateReader.CreateInvalidContentException(ex, Resources.XmDeserializelException, new object[]
				{
					ex.Message
				});
			}
			catch (FormatException ex2)
			{
				throw intermediateReader.CreateInvalidContentException(ex2, Resources.XmDeserializelException, new object[]
				{
					ex2.Message
				});
			}
			catch (OverflowException ex3)
			{
				throw intermediateReader.CreateInvalidContentException(ex3, Resources.XmDeserializelException, new object[]
				{
					ex3.Message
				});
			}
			catch (ArgumentException ex4)
			{
				throw intermediateReader.CreateInvalidContentException(ex4, Resources.XmDeserializelException, new object[]
				{
					ex4.Message
				});
			}
			return result;
		}
		private static Uri RelocationPathToUri(string referenceRelocationPath)
		{
			if (referenceRelocationPath == null)
			{
				return null;
			}
			return PathUtils.GetAbsoluteUri(referenceRelocationPath);
		}
		/// <summary>Retrieves the worker serializer for a specified type.</summary>
		/// <param name="type">The type.</param>
		public ContentTypeSerializer GetTypeSerializer(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ContentTypeSerializer contentTypeSerializer;
			if (!this.serializerInstances.TryGetValue(type, out contentTypeSerializer))
			{
				contentTypeSerializer = this.typeSerializerFactory.CreateSerializer(type);
				this.AddTypeSerializer(contentTypeSerializer);
				contentTypeSerializer.Initialize(this);
			}
			return contentTypeSerializer;
		}
		private void AddTypeSerializer(ContentTypeSerializer serializer)
		{
			if (this.serializerInstances.ContainsKey(serializer.TargetType))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateTypeHandler, new object[]
				{
					typeof(ContentTypeSerializer).Name,
					serializer.GetType().AssemblyQualifiedName,
					this.serializerInstances[serializer.TargetType].GetType().AssemblyQualifiedName,
					serializer.TargetType
				}));
			}
			if (serializer.XmlTypeName != null)
			{
				this.typeNameHelper.AddXmlTypeName(serializer);
			}
			this.serializerInstances.Add(serializer.TargetType, serializer);
		}
		internal CollectionHelper GetCollectionHelper(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			CollectionHelper collectionHelper;
			if (!this.collectionHelpers.TryGetValue(type, out collectionHelper))
			{
				collectionHelper = new CollectionHelper(this, type);
				this.collectionHelpers[type] = collectionHelper;
			}
			return collectionHelper;
		}
	}
}
