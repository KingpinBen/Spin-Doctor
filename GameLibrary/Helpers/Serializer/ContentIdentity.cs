using System;
using Microsoft.Xna.Framework.Content;
namespace GameLibrary.Helpers.Serializer
{
	/// <summary>Provides properties describing the origin of the game asset, such as the original source file and creation tool. This information is used for error reporting, and by processors that need to determine from what directory the asset was originally loaded.</summary>
	[Serializable]
	public class ContentIdentity
	{
		private string sourceFilename;
		private string sourceTool;
		private string fragmentIdentifier;
		/// <summary>Gets or sets the file name of the asset source.</summary>
		[ContentSerializer(Optional = true)]
		public string SourceFilename
		{
			get
			{
				return this.sourceFilename;
			}
			set
			{
				this.sourceFilename = value;
			}
		}
		/// <summary>Gets or sets the creation tool of the asset.</summary>
		[ContentSerializer(Optional = true)]
		public string SourceTool
		{
			get
			{
				return this.sourceTool;
			}
			set
			{
				this.sourceTool = value;
			}
		}
		/// <summary>Gets or sets the specific location of the content item within the larger source file.</summary>
		[ContentSerializer(Optional = true)]
		public string FragmentIdentifier
		{
			get
			{
				return this.fragmentIdentifier;
			}
			set
			{
				this.fragmentIdentifier = value;
			}
		}
		/// <summary>Initializes a new instance of ContentIdentity.</summary>
		public ContentIdentity()
		{
		}
		/// <summary>Initializes a new instance of ContentIdentity with the specified values.</summary>
		/// <param name="sourceFilename">The absolute path to the file name of the asset source.</param>
		public ContentIdentity(string sourceFilename)
		{
			this.SourceFilename = sourceFilename;
		}
		/// <summary>Initializes a new instance of ContentIdentity with the specified values.</summary>
		/// <param name="sourceFilename">The absolute path to the file name of the asset source.</param>
		/// <param name="sourceTool">The name of the digital content creation (DCC) tool that created the asset.</param>
		public ContentIdentity(string sourceFilename, string sourceTool)
		{
			this.SourceFilename = sourceFilename;
			this.SourceTool = sourceTool;
		}
		/// <summary>Initializes a new instance of ContentIdentity with the specified values.</summary>
		/// <param name="sourceFilename">The absolute path to the file name of the asset source.</param>
		/// <param name="sourceTool">The name of the digital content creation (DCC) tool that created the asset.</param>
		/// <param name="fragmentIdentifier">Specific location of the content item within the larger source file. For example, this could be a line number in the file.</param>
		public ContentIdentity(string sourceFilename, string sourceTool, string fragmentIdentifier)
		{
			this.sourceFilename = sourceFilename;
			this.sourceTool = sourceTool;
			this.fragmentIdentifier = fragmentIdentifier;
		}
	}
}
