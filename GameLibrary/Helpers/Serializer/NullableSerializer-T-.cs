using System;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class NullableSerializer<T> : ContentTypeSerializer<T?> where T : struct
	{
		private ContentTypeSerializer underlyingTypeSerializer;
		private ContentSerializerAttribute underlyingFormat;
		internal override Type BoxedTargetType
		{
			get
			{
				return typeof(T);
			}
		}
		protected internal override void Initialize(IntermediateSerializer serializer)
		{
			this.underlyingTypeSerializer = serializer.GetTypeSerializer(typeof(T));
			this.underlyingFormat = new ContentSerializerAttribute();
			this.underlyingFormat.FlattenContent = true;
		}
		protected internal override void Serialize(IntermediateWriter output, T? value, ContentSerializerAttribute format)
		{
			output.WriteRawObject<T>(value.Value, this.underlyingFormat, this.underlyingTypeSerializer);
		}
		protected internal override T? Deserialize(IntermediateReader input, ContentSerializerAttribute format, T? existingInstance)
		{
			return new T?(input.ReadRawObject<T>(this.underlyingFormat, this.underlyingTypeSerializer));
		}
	}
}
