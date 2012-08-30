using System;
using System.Collections.Generic;

namespace GameLibrary.Helpers.Serializer
{
	internal class ReferenceEqualityComparer<T> : IEqualityComparer<T>
	{
		public bool Equals(T x, T y)
		{
			return object.ReferenceEquals(x, y);
		}
		public int GetHashCode(T obj)
		{
			return obj.GetHashCode();
		}
	}
}
