using System;
using System.Collections;
using System.Collections.Generic;

namespace GameLibrary.Helpers.Serializer
{
	/// <summary>Base class for dictionaries that map string identifiers to data values.</summary>
	public class NamedValueDictionary<T> : IDictionary<string, T>, ICollection<KeyValuePair<string, T>>, IEnumerable<KeyValuePair<string, T>>, IEnumerable
	{
		private Dictionary<string, T> items = new Dictionary<string, T>();
		/// <summary />
		protected internal virtual Type DefaultSerializerType
		{
			get
			{
				return typeof(T);
			}
		}
		/// <summary>Gets the number of items in the dictionary.</summary>
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}
		/// <summary>Gets all keys contained in the dictionary.</summary>
		public ICollection<string> Keys
		{
			get
			{
				return this.items.Keys;
			}
		}
		/// <summary>Gets all values contained in the dictionary.</summary>
		public ICollection<T> Values
		{
			get
			{
				return this.items.Values;
			}
		}
		/// <summary>Gets or sets the specified item.</summary>
		/// <param name="key" />
		public T this[string key]
		{
			get
			{
				return this.items[key];
			}
			set
			{
				this.SetItem(key, value);
			}
		}
		bool ICollection<KeyValuePair<string, T>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}
		/// <summary>Adds an element to the dictionary.</summary>
		/// <param name="key">Identity of the key of the new element.</param>
		/// <param name="value">The value of the new element.</param>
		protected virtual void AddItem(string key, T value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.items.Add(key, value);
		}
		/// <summary>Removes all elements from the dictionary.</summary>
		protected virtual void ClearItems()
		{
			this.items.Clear();
		}
		/// <summary>Removes the specified element from the dictionary.</summary>
		/// <param name="key">Identity of the key of the data pair to be removed.</param>
		protected virtual bool RemoveItem(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			return this.items.Remove(key);
		}
		/// <summary>Modifies the value of an existing element.</summary>
		/// <param name="key">Identity of the element to be modified.</param>
		/// <param name="value">The value to be set.</param>
		protected virtual void SetItem(string key, T value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.items[key] = value;
		}
		/// <summary>Adds the specified key and value to the dictionary.</summary>
		/// <param name="key">Identity of the key of the new data pair.</param>
		/// <param name="value">The value of the new data pair.</param>
		public void Add(string key, T value)
		{
			this.AddItem(key, value);
		}
		/// <summary>Removes all keys and values from the dictionary.</summary>
		public void Clear()
		{
			this.ClearItems();
		}
		/// <summary>Determines whether the specified key is present in the dictionary.</summary>
		/// <param name="key">Identity of a key.</param>
		public bool ContainsKey(string key)
		{
			return this.items.ContainsKey(key);
		}
		/// <summary>Removes the specified key and value from the dictionary.</summary>
		/// <param name="key">Identity of the key to be removed.</param>
		public bool Remove(string key)
		{
			return this.RemoveItem(key);
		}
		/// <summary>Gets the value associated with the specified key.</summary>
		/// <param name="key">Identity of the key of the element whose value is to be retrieved.</param>
		/// <param name="value">[OutAttribute] The current value of the element.</param>
		public bool TryGetValue(string key, out T value)
		{
			return this.items.TryGetValue(key, out value);
		}
		/// <summary>Gets an enumerator that iterates through items in a dictionary.</summary>
		public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
		/// <summary>Returns an enumerator that can iterate through this collection.</summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}
		void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item)
		{
			this.AddItem(item.Key, item.Value);
		}
		bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item)
		{
			ICollection<KeyValuePair<string, T>> collection = this.items;
			return collection.Contains(item);
		}
		void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
		{
			ICollection<KeyValuePair<string, T>> collection = this.items;
			collection.CopyTo(array, arrayIndex);
		}
		bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
		{
			ICollection<KeyValuePair<string, T>> collection = this.items;
			return collection.Contains(item) && this.RemoveItem(item.Key);
		}
	}
}
