using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace GameLibrary.Helpers.Serializer
{
	/// <summary>Thrown when errors are encountered in content during processing.</summary>
	[Serializable]
	public class InvalidContentException : Exception
	{
		private ContentIdentity contentIdentity;
		/// <summary>Gets or sets the identity of the content item that caused the exception.</summary>
		public ContentIdentity ContentIdentity
		{
			get
			{
				return this.contentIdentity;
			}
			set
			{
				this.contentIdentity = value;
			}
		}
		/// <summary>Initializes a new instance of the InvalidContentException class</summary>
		public InvalidContentException()
		{
		}
		/// <summary>Initializes a new instance of the InvalidContentException class with the specified error message.</summary>
		/// <param name="message">A message that describes the error.</param>
		public InvalidContentException(string message) : base(message)
		{
		}
		/// <summary>Initializes a new instance of the InvalidContentException class with the specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If innerException is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
		public InvalidContentException(string message, Exception innerException) : base(message, innerException)
		{
		}
		/// <summary>Initializes a new instance of the InvalidContentException class with the specified error message and the identity of the content throwing the exception.</summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="contentIdentity">Information about the content item that caused this error, including the file name. In some cases, a location within the file (of the problem) is specified.</param>
		public InvalidContentException(string message, ContentIdentity contentIdentity) : base(message)
		{
			this.contentIdentity = contentIdentity;
		}
		/// <summary>Initializes a new instance of the InvalidContentException class with the specified error message, the identity of the content throwing the exception, and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="contentIdentity">Information about the content item that caused this error, including the file name. In some cases, a location within the file (of the problem) is specified.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If innerException is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
		public InvalidContentException(string message, ContentIdentity contentIdentity, Exception innerException) : base(message, innerException)
		{
			this.contentIdentity = contentIdentity;
		}
		/// <summary>Initializes a new instance of the InvalidContentException class with information on serialization and streaming context for the related content item.</summary>
		/// <param name="serializationInfo">Information necessary for serialization and deserialization of the content item.</param>
		/// <param name="streamingContext">Information necessary for the source and destination of a given serialized stream. Also provides an additional caller-defined context.</param>
		protected InvalidContentException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
		{
			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			this.contentIdentity = (ContentIdentity)serializationInfo.GetValue("ContentIdentity", typeof(ContentIdentity));
		}
		/// <summary>When overridden in a derived class, returns information about the exception.</summary>
		/// <param name="info">Information necessary for serialization and deserialization of the content item.</param>
		/// <param name="context">Information necessary for the source and destination of a given serialized stream. Also provides an additional caller-defined context.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("ContentIdentity", this.contentIdentity, typeof(ContentIdentity));
		}
	}
}
