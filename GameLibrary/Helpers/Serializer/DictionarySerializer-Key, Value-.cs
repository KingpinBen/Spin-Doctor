using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class DictionarySerializer<Key, Value> : CollectionSerializer<Dictionary<Key, Value>, KeyValuePair<Key, Value>>
	{
		private ContentTypeSerializer keySerializer;
		private ContentTypeSerializer valueSerializer;
		private ContentSerializerAttribute keyFormat;
		private ContentSerializerAttribute valueFormat;
		protected internal override void Initialize(IntermediateSerializer serializer)
		{
			this.keySerializer = serializer.GetTypeSerializer(typeof(Key));
			this.valueSerializer = serializer.GetTypeSerializer(typeof(Value));
			this.keyFormat = new ContentSerializerAttribute();
			this.valueFormat = new ContentSerializerAttribute();
			this.keyFormat.ElementName = "Key";
			this.valueFormat.ElementName = "Value";
			this.keyFormat.AllowNull = false;
			if (typeof(Value).IsValueType)
			{
				this.valueFormat.AllowNull = false;
			}
		}
		protected internal override void Serialize(IntermediateWriter output, Dictionary<Key, Value> value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			foreach (KeyValuePair<Key, Value> current in value)
			{
				output.Xml.WriteStartElement(format.CollectionItemName);
				output.WriteObject<Key>(current.Key, this.keyFormat, this.keySerializer);
				output.WriteObject<Value>(current.Value, this.valueFormat, this.valueSerializer);
				output.Xml.WriteEndElement();
			}
		}
		protected internal override Dictionary<Key, Value> Deserialize(IntermediateReader input, ContentSerializerAttribute format, Dictionary<Key, Value> existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			Dictionary<Key, Value> dictionary = existingInstance;
			if (dictionary == null)
			{
				dictionary = new Dictionary<Key, Value>();
			}
			while (input.MoveToElement(format.CollectionItemName))
			{
				input.Xml.ReadStartElement();
				Key key = input.ReadObject<Key>(this.keyFormat, this.keySerializer);
				Value value = input.ReadObject<Value>(this.valueFormat, this.valueSerializer);
				dictionary.Add(key, value);
				input.Xml.ReadEndElement();
			}
			return dictionary;
		}
		protected internal override void ScanChildren(IntermediateSerializer serializer, ContentTypeSerializer.ChildCallback callback, Dictionary<Key, Value> value)
		{
			foreach (KeyValuePair<Key, Value> current in value)
			{
				callback(this.keySerializer, current.Key);
				callback(this.valueSerializer, current.Value);
			}
		}
	}
}
