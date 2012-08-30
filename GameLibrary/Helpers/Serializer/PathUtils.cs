using System;
using System.Globalization;
using System.IO;
namespace GameLibrary.Helpers.Serializer
{
	internal static class PathUtils
	{
		public static string GetFullPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			string fullPath;
			try
			{
				fullPath = Path.GetFullPath(path);
			}
			catch (ArgumentException innerException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilename, new object[]
				{
					path
				}), innerException);
			}
			catch (NotSupportedException innerException2)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilename, new object[]
				{
					path
				}), innerException2);
			}
			catch (PathTooLongException innerException3)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.PathTooLong, new object[]
				{
					path
				}), innerException3);
			}
			return fullPath;
		}
		public static void ThrowIfPathNotAbsolute(string path)
		{
			string fullPath = PathUtils.GetFullPath(path);
			string value = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			if (!fullPath.Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.FilenameNotAbsolute, new object[]
				{
					path
				}));
			}
		}
		public static Uri GetAbsoluteUri(string path)
		{
			string fullPath = PathUtils.GetFullPath(path);
			Uri uri;
			if (!Uri.TryCreate(fullPath, UriKind.Absolute, out uri))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilename, new object[]
				{
					fullPath
				}));
			}
			if (!uri.IsFile)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilename, new object[]
				{
					fullPath
				}));
			}
			return uri;
		}
		public static string GetAbsolutePath(Uri baseUri, string filename)
		{
			if (string.IsNullOrEmpty(filename))
			{
				throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilename, new object[]
				{
					filename
				}));
			}
			Uri uri;
			if (baseUri != null)
			{
				if (!Uri.TryCreate(baseUri, filename, out uri))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilename, new object[]
					{
						filename
					}));
				}
			}
			else
			{
				if (!Uri.TryCreate(filename, UriKind.Absolute, out uri))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilename, new object[]
					{
						filename
					}));
				}
			}
			if (!uri.IsAbsoluteUri)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.FilenameNotAbsolute, new object[]
				{
					uri
				}));
			}
			if (!uri.IsFile)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilename, new object[]
				{
					uri
				}));
			}
			return uri.LocalPath;
		}
		public static string GetRelativePath(Uri baseUri, string filename)
		{
			Uri uri;
			if (!Uri.TryCreate(filename, UriKind.Absolute, out uri))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilename, new object[]
				{
					uri
				}));
			}
			if (!uri.IsFile)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.InvalidFilename, new object[]
				{
					uri
				}));
			}
			if (baseUri != null)
			{
				uri = baseUri.MakeRelativeUri(uri);
			}
			string text = Uri.UnescapeDataString(uri.ToString());
			string[] array = new string[]
			{
				"file:///",
				"file:"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				if (text.StartsWith(text2, StringComparison.OrdinalIgnoreCase))
				{
					text = text.Substring(text2.Length);
					break;
				}
			}
			return text.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		}
		public static DateTime GetTimestamp(string filename)
		{
			try
			{
				FileInfo fileInfo = new FileInfo(filename);
				if (fileInfo.Exists)
				{
					return fileInfo.LastWriteTime;
				}
			}
			catch
			{
			}
			return DateTime.MinValue;
		}
		public static void SafeDeleteFile(string filename)
		{
			try
			{
				File.Delete(filename);
			}
			catch
			{
			}
		}
		public static void SafeDeleteFileAndDirectories(string filename, params string[] rootDirectories)
		{
			try
			{
				File.Delete(filename);
				while (true)
				{
					filename = Path.GetDirectoryName(filename);
					if (string.IsNullOrEmpty(filename))
					{
						break;
					}
					for (int i = 0; i < rootDirectories.Length; i++)
					{
						string text = rootDirectories[i];
						if (text.StartsWith(filename, StringComparison.OrdinalIgnoreCase))
						{
							goto Block_3;
						}
					}
					if (Directory.GetFileSystemEntries(filename).Length != 0)
					{
						break;
					}
					Directory.Delete(filename);
				}
				Block_3:;
			}
			catch
			{
			}
		}
		public static void CreateDirectory(string directory)
		{
			try
			{
				Directory.CreateDirectory(directory);
			}
			catch (Exception innerException)
			{
				throw new PipelineException(string.Format(CultureInfo.CurrentCulture, Resources.CantCreateDirectory, new object[]
				{
					directory
				}), innerException);
			}
		}
		public static string GetFullDirectoryName(string path)
		{
			string fullPath;
			try
			{
				fullPath = PathUtils.GetFullPath(path + Path.DirectorySeparatorChar);
			}
			catch (ArgumentException ex)
			{
				throw new PipelineException(ex.Message);
			}
			return fullPath;
		}
	}
}
