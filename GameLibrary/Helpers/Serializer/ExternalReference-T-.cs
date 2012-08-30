using System;
using System.IO;

namespace GameLibrary.Helpers.Serializer
{
	/// <summary>Specifies external references to a data file for the content item.</summary>
	public sealed class ExternalReference<T> : ContentItem
	{
		private string filename;
		/// <summary>Gets and sets the file name of an ExternalReference.</summary>
		public string Filename
		{
			get
			{
				return this.filename;
			}
			set
			{
				if (value != null)
				{
					PathUtils.ThrowIfPathNotAbsolute(value);
				}
				this.filename = value;
			}
		}
		/// <summary>Initializes a new instance of ExternalReference.</summary>
		public ExternalReference()
		{
		}
		/// <summary>Initializes a new instance of ExternalReference.</summary>
		/// <param name="filename">The name of the referenced file.</param>
		public ExternalReference(string filename)
		{
			if (filename != null)
			{
				this.filename = PathUtils.GetFullPath(filename);
			}
		}
		/// <summary>Initializes a new instance of ExternalReference, specifying the file path relative to another content item.</summary>
		/// <param name="filename">The name of the referenced file.</param>
		/// <param name="relativeToContent">The content that the path specified in filename is relative to.</param>
		public ExternalReference(string filename, ContentIdentity relativeToContent)
		{
			if (filename != null)
			{
				if (filename.Length == 0)
				{
					throw new ArgumentNullException("filename");
				}
				if (relativeToContent == null)
				{
					throw new ArgumentNullException("relativeToContent");
				}
				if (string.IsNullOrEmpty(relativeToContent.SourceFilename))
				{
					throw new ArgumentNullException("relativeToContent.SourceFilename");
				}
				string directoryName = Path.GetDirectoryName(relativeToContent.SourceFilename);
				string path = Path.Combine(directoryName, filename);
				this.filename = PathUtils.GetFullPath(path);
			}
		}
	}
}
