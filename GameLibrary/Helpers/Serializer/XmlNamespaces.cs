using System;
using System.Collections.Generic;

namespace GameLibrary.Helpers.Serializer
{
	internal class XmlNamespaces
	{
		private IntermediateWriter writer;
		private Dictionary<object, bool> seenObjects = new Dictionary<object, bool>(new ReferenceEqualityComparer<object>());
		private Dictionary<string, bool> seenNamespaces = new Dictionary<string, bool>();
		private List<string> namespaces = new List<string>();
		public XmlNamespaces(IntermediateWriter writer)
		{
			this.writer = writer;
		}
		public void WriteNamespaces(object value)
		{
			this.ScanObject(null, value);
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (string current in this.namespaces)
			{
				int num = current.LastIndexOf('.');
				string text = current.Substring(num + 1);
				if (!(text == current) && !dictionary.ContainsKey(text))
				{
					this.writer.Xml.WriteAttributeString("xmlns", text, null, current);
					dictionary.Add(text, true);
				}
			}
		}
		private void ScanObject(ContentTypeSerializer typeSerializer, object value)
		{
			if (value == null)
			{
				return;
			}
			Type type = value.GetType();
			if (!type.IsValueType)
			{
				if (this.seenObjects.ContainsKey(value))
				{
					return;
				}
				this.seenObjects.Add(value, true);
			}
			if (typeSerializer == null || type != typeSerializer.BoxedTargetType)
			{
				this.RecordType(type);
				typeSerializer = this.writer.Serializer.GetTypeSerializer(type);
			}
			typeSerializer.ScanChildren(this.writer.Serializer, new ContentTypeSerializer.ChildCallback(this.ScanObject), value);
		}
		private void RecordType(Type type)
		{
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			string @namespace = type.Namespace;
			if (string.IsNullOrEmpty(@namespace))
			{
				return;
			}
			if (!this.seenNamespaces.ContainsKey(@namespace))
			{
				this.seenNamespaces.Add(@namespace, true);
				this.namespaces.Add(@namespace);
			}
			if (type.IsGenericType)
			{
				Type[] genericArguments = type.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					Type type2 = genericArguments[i];
					this.RecordType(type2);
				}
			}
		}
	}
}
