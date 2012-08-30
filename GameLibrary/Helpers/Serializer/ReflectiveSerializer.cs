using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework.Content;

namespace GameLibrary.Helpers.Serializer
{
	internal class ReflectiveSerializer : ContentTypeSerializer
	{
		private ContentTypeSerializer baseSerializer;
		private List<ReflectiveSerializerMemberHelper> memberHelpers = new List<ReflectiveSerializerMemberHelper>();
		private CollectionHelper collectionHelper;
        private ReflectionEmitUtils.ConstructorMethod instanceConstructor;
		private string collectionItemName;
		public override bool CanDeserializeIntoExistingObject
		{
			get
			{
				return base.TargetType.IsClass && ((this.baseSerializer != null && this.baseSerializer.CanDeserializeIntoExistingObject) || this.memberHelpers.Count > 0 || this.collectionHelper != null);
			}
		}
		public ReflectiveSerializer(Type targetType) : base(targetType)
		{
			this.instanceConstructor = ReflectionEmitUtils.GenerateConstructor(targetType);
		}
		protected internal override void Initialize(IntermediateSerializer serializer)
		{
			if (base.TargetType.BaseType != null)
			{
				this.baseSerializer = serializer.GetTypeSerializer(base.TargetType.BaseType);
			}
			MemberHelperBase<IntermediateSerializer>.CreateMemberHelpers<ReflectiveSerializerMemberHelper>(serializer, base.TargetType, this.memberHelpers);
			if (CollectionUtils.IsCollection(base.TargetType, false))
			{
				this.collectionHelper = serializer.GetCollectionHelper(base.TargetType);
			}
			object[] customAttributes = base.TargetType.GetCustomAttributes(typeof(ContentSerializerCollectionItemNameAttribute), true);
			if (customAttributes.Length == 1)
			{
				this.collectionItemName = ((ContentSerializerCollectionItemNameAttribute)customAttributes[0]).CollectionItemName;
			}
		}
		protected internal override void Serialize(IntermediateWriter output, object value, ContentSerializerAttribute format)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!base.IsTargetType(value))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.WrongArgumentType, new object[]
				{
					base.TargetType,
					value.GetType()
				}));
			}
			format = this.ApplyCustomFormatting(format);
			if (this.baseSerializer != null)
			{
				this.baseSerializer.Serialize(output, value, format);
			}
			foreach (ReflectiveSerializerMemberHelper current in this.memberHelpers)
			{
				current.Serialize(output, value);
			}
			if (this.collectionHelper != null)
			{
				this.collectionHelper.Serialize(output, format, value);
			}
		}
		protected internal override object Deserialize(IntermediateReader input, ContentSerializerAttribute format, object existingInstance)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			format = this.ApplyCustomFormatting(format);
			object obj = existingInstance;
			if (obj == null)
			{
				if (this.instanceConstructor == null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.NoDefaultConstructor, new object[]
					{
						base.TargetType
					}));
				}
				obj = this.instanceConstructor();
			}
			if (this.baseSerializer != null)
			{
				object obj2 = this.baseSerializer.Deserialize(input, format, obj);
				if (obj2 != obj)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DeserializerConstructedNewInstance, new object[]
					{
						this.baseSerializer.GetType(),
						this.baseSerializer.TargetType
					}));
				}
			}
			foreach (ReflectiveSerializerMemberHelper current in this.memberHelpers)
			{
				current.Deserialize(input, obj);
			}
			if (this.collectionHelper != null)
			{
				this.collectionHelper.Deserialize(input, format, obj);
			}
			return obj;
		}
		private ContentSerializerAttribute ApplyCustomFormatting(ContentSerializerAttribute format)
		{
			if (!format.HasCollectionItemName && this.collectionItemName != null)
			{
				format = format.Clone();
				format.CollectionItemName = this.collectionItemName;
			}
			return format;
		}
		public override bool ObjectIsEmpty(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.baseSerializer != null && !this.baseSerializer.ObjectIsEmpty(value))
			{
				return false;
			}
			foreach (ReflectiveSerializerMemberHelper current in this.memberHelpers)
			{
				if (!current.ObjectIsEmpty(value))
				{
					return false;
				}
			}
			return this.collectionHelper == null || this.collectionHelper.ObjectIsEmpty(value);
		}
		protected internal override void ScanChildren(IntermediateSerializer serializer, ContentTypeSerializer.ChildCallback callback, object value)
		{
			if (this.baseSerializer != null)
			{
				this.baseSerializer.ScanChildren(serializer, callback, value);
			}
			foreach (ReflectiveSerializerMemberHelper current in this.memberHelpers)
			{
				current.ScanValue(callback, value);
			}
			if (this.collectionHelper != null)
			{
				this.collectionHelper.ScanElements(callback, value);
			}
		}
	}
}
