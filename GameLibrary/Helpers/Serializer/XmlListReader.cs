using System;
using System.Collections.Generic;

namespace GameLibrary.Helpers.Serializer
{
	internal class XmlListReader
	{
		private static char[] listSeparators = new char[]
		{
			' ',
			'\t',
			'\r',
			'\n'
		};
		private IntermediateReader reader;
		private IEnumerator<string> enumerator;
		private bool atEnd;
		public bool AtEnd
		{
			get
			{
				return this.atEnd;
			}
		}
		public XmlListReader(IntermediateReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.reader = reader;
			string text = reader.Xml.ReadContentAsString();
			IEnumerable<string> enumerable = text.Split(XmlListReader.listSeparators, StringSplitOptions.RemoveEmptyEntries);
			this.enumerator = enumerable.GetEnumerator();
			this.atEnd = !this.enumerator.MoveNext();
		}
		public string ReadString()
		{
			if (this.atEnd)
			{
				throw this.reader.CreateInvalidContentException(Resources.NotEnoughEntriesInXmlList, new object[0]);
			}
			string current = this.enumerator.Current;
			this.atEnd = !this.enumerator.MoveNext();
			return current;
		}
	}
}
