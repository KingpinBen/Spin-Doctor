using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	internal class CollectionHelper
	{
		private Type targetType;
		private ContentTypeSerializer contentSerializer;
		private ReflectionEmitUtils.GetterMethod countPropertyGetter;
		private ReflectionEmitUtils.SetterMethod addToCollection;
		internal CollectionHelper(IntermediateSerializer serializer, Type type)
		{
			this.targetType = type;
			Type type2 = CollectionUtils.CollectionElementType(type, false);
			if (type2 == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.NotACollectionType, new object[]
				{
					type
				}));
			}
			this.contentSerializer = serializer.GetTypeSerializer(type2);
			Type type3 = typeof(ICollection<>).MakeGenericType(new Type[]
			{
				type2
			});
			this.countPropertyGetter = ReflectionEmitUtils.GenerateGetter(type3.GetProperty("Count"));
			this.addToCollection = ReflectionEmitUtils.GenerateAddToCollection(type3, type2);
		}
		public void Serialize(IntermediateWriter output, ContentSerializerAttribute format, object collection)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			this.ValidateCollectionType(collection);
			IEnumerable enumerable = (IEnumerable)collection;
			if (this.contentSerializer is IXmlListItemSerializer)
			{
				ContentSerializerAttribute contentSerializerAttribute = new ContentSerializerAttribute();
				contentSerializerAttribute.FlattenContent = true;
				bool flag = true;
				IEnumerator enumerator = enumerable.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						if (flag)
						{
							flag = false;
						}
						else
						{
							output.Xml.WriteWhitespace(" ");
						}
						output.WriteRawObject<object>(current, contentSerializerAttribute, this.contentSerializer);
					}
					return;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			ContentSerializerAttribute contentSerializerAttribute2 = new ContentSerializerAttribute();
			contentSerializerAttribute2.ElementName = format.CollectionItemName;
			foreach (object current2 in enumerable)
			{
				output.WriteObject<object>(current2, contentSerializerAttribute2, this.contentSerializer);
			}
		}
		public void Deserialize(IntermediateReader input, ContentSerializerAttribute format, object collection)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			this.ValidateCollectionType(collection);
			IXmlListItemSerializer xmlListItemSerializer = this.contentSerializer as IXmlListItemSerializer;
			if (xmlListItemSerializer != null)
			{
				XmlListReader xmlListReader = new XmlListReader(input);
				while (!xmlListReader.AtEnd)
				{
					this.addToCollection(collection, xmlListItemSerializer.Deserialize(xmlListReader));
				}
				return;
			}
			ContentSerializerAttribute contentSerializerAttribute = new ContentSerializerAttribute();
			contentSerializerAttribute.ElementName = format.CollectionItemName;
			while (input.MoveToElement(format.CollectionItemName))
			{
				this.addToCollection(collection, input.ReadObject<object>(contentSerializerAttribute, this.contentSerializer));
			}
		}
		public bool ObjectIsEmpty(object collection)
		{
			return (int)this.countPropertyGetter(collection) == 0;
		}

		public void ScanElements(ContentTypeSerializer.ChildCallback callback, object collection)
		{
			this.ValidateCollectionType(collection);
			IEnumerable enumerable = (IEnumerable)collection;
			foreach (object current in enumerable)
			{
				if (current != null)
				{
					callback(this.contentSerializer, current);
				}
			}
		}
		private void ValidateCollectionType(object collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			if (!this.targetType.IsAssignableFrom(collection.GetType()))
			{
                throw new Exception("wrong argument type");
			}
		}
	}
}
