using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	[ContentTypeSerializer]
	internal class BoundingFrustumSerializer : ContentTypeSerializer<BoundingFrustum>
	{
		private static ContentSerializerAttribute matrixFormat;
		static BoundingFrustumSerializer()
		{
			BoundingFrustumSerializer.matrixFormat = new ContentSerializerAttribute();
			BoundingFrustumSerializer.matrixFormat.ElementName = "Matrix";
		}
		protected internal override void Serialize(IntermediateWriter output, BoundingFrustum value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.WriteObject<Matrix>(value.Matrix, BoundingFrustumSerializer.matrixFormat);
		}
		protected internal override BoundingFrustum Deserialize(IntermediateReader input, ContentSerializerAttribute format, BoundingFrustum existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			return new BoundingFrustum(input.ReadObject<Matrix>(BoundingFrustumSerializer.matrixFormat));
		}
	}
}
