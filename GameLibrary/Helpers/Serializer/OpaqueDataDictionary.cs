using System;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	/// <summary>Provides properties that define opaque data for a game asset.</summary>
	[ContentSerializerCollectionItemName("Data")]
	public sealed class OpaqueDataDictionary : NamedValueDictionary<object>
	{
		private string contentAsXml;
		protected internal override Type DefaultSerializerType
		{
			get
			{
				return typeof(string);
			}
		}
		/// <summary>Gets the value of the specified key/value pair of the asset.</summary>
		/// <param name="key">The name of the key.</param>
		/// <param name="defaultValue">The value to return if the key cannot be found. This can be null for reference types, 0 for primitive types, and a zero-filled structure for structure types.</param>
		public T GetValue<T>(string key, T defaultValue)
		{
			object obj;
			if (base.TryGetValue(key, out obj) && obj is T)
			{
				return (T)((object)obj);
			}
			return defaultValue;
		}
		/// <summary />
		public string GetContentAsXml()
		{
			if (this.contentAsXml == null)
			{
				if (base.Count == 0)
				{
					this.contentAsXml = string.Empty;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
					{
						IntermediateSerializer.Serialize<OpaqueDataDictionary>(xmlWriter, this, null);
					}
					this.contentAsXml = stringBuilder.ToString();
				}
			}
			return this.contentAsXml;
		}
		protected override void AddItem(string key, object value)
		{
			this.contentAsXml = null;
			base.AddItem(key, value);
		}
		protected override void ClearItems()
		{
			this.contentAsXml = null;
			base.ClearItems();
		}
		protected override bool RemoveItem(string key)
		{
			this.contentAsXml = null;
			return base.RemoveItem(key);
		}
		protected override void SetItem(string key, object value)
		{
			this.contentAsXml = null;
			base.SetItem(key, value);
		}
	}
}
