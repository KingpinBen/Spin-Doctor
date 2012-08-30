using System;
using System.Globalization;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
namespace GameLibrary.Helpers.Serializer
{
	internal class ReflectiveSerializerMemberHelper : MemberHelperBase<IntermediateSerializer>
	{
		private ContentTypeSerializer typeSerializer;
		private ReflectionEmitUtils.GetterMethod valueGetter;
		private ReflectionEmitUtils.SetterMethod valueSetter;
		private ContentSerializerAttribute formatAttribute;
		protected override void Initialize(IntermediateSerializer serializer, FieldInfo fieldInfo, bool canWrite)
		{
			this.valueGetter = ReflectionEmitUtils.GenerateGetter(fieldInfo);
			if (canWrite)
			{
				this.valueSetter = ReflectionEmitUtils.GenerateSetter(fieldInfo);
			}
			this.InitializeCommon(serializer, fieldInfo, fieldInfo.FieldType);
		}
		protected override void Initialize(IntermediateSerializer serializer, PropertyInfo propertyInfo)
		{
			this.valueGetter = ReflectionEmitUtils.GenerateGetter(propertyInfo);
			if (propertyInfo.CanWrite)
			{
				this.valueSetter = ReflectionEmitUtils.GenerateSetter(propertyInfo);
			}
			this.InitializeCommon(serializer, propertyInfo, propertyInfo.PropertyType);
		}
		private void InitializeCommon(IntermediateSerializer serializer, MemberInfo memberInfo, Type memberType)
		{
			this.typeSerializer = serializer.GetTypeSerializer(memberType);
			Attribute customAttribute = Attribute.GetCustomAttribute(memberInfo, typeof(ContentSerializerAttribute));
			if (customAttribute == null)
			{
				this.formatAttribute = new ContentSerializerAttribute();
				this.formatAttribute.ElementName = memberInfo.Name;
				return;
			}
			this.formatAttribute = (ContentSerializerAttribute)customAttribute;
			if (this.formatAttribute.ElementName == null)
			{
				this.formatAttribute = this.formatAttribute.Clone();
				this.formatAttribute.ElementName = memberInfo.Name;
			}
		}
		public void Serialize(IntermediateWriter output, object parentInstance)
		{
			object obj = this.valueGetter(parentInstance);
			if (obj == null && !this.formatAttribute.AllowNull)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.NullElementNotAllowed, new object[]
				{
					this.formatAttribute.ElementName
				}));
			}
			if (this.formatAttribute.Optional)
			{
				if (obj == null)
				{
					return;
				}
				if (this.valueSetter == null && this.typeSerializer.ObjectIsEmpty(obj))
				{
					return;
				}
			}
			if (this.formatAttribute.SharedResource)
			{
				output.WriteSharedResource<object>(obj, this.formatAttribute);
				return;
			}
			output.WriteObject<object>(obj, this.formatAttribute, this.typeSerializer);
		}
		public void Deserialize(IntermediateReader input, object parentInstance)
		{
			if (!this.formatAttribute.FlattenContent)
			{
				if (this.formatAttribute.Optional)
				{
					if (!input.MoveToElement(this.formatAttribute.ElementName))
					{
						return;
					}
				}
				else
				{
					input.Xml.MoveToContent();
				}
			}
			if (this.formatAttribute.SharedResource)
			{
				this.DeserializeSharedResource(input, parentInstance);
				return;
			}
			this.DeserializeRegularObject(input, parentInstance);
		}
		private void DeserializeRegularObject(IntermediateReader input, object parentInstance)
		{
			if (this.valueSetter != null)
			{
				object value = input.ReadObject<object>(this.formatAttribute, this.typeSerializer);
				this.valueSetter(parentInstance, value);
				return;
			}
			object obj = this.valueGetter(parentInstance);
			if (obj == null)
			{
				throw input.CreateInvalidContentException(Resources.CantSerializeReadOnlyNull, new object[]
				{
					this.formatAttribute.ElementName
				});
			}
			input.ReadObject<object>(this.formatAttribute, this.typeSerializer, obj);
		}
		private void DeserializeSharedResource(IntermediateReader input, object parentInstance)
		{
			if (this.valueSetter == null)
			{
				throw new InvalidOperationException(Resources.ReadOnlySharedResource);
			}
			Action<object> fixup = delegate(object value)
			{
				if (!this.typeSerializer.IsTargetType(value))
				{
					throw input.CreateInvalidContentException(Resources.WrongSharedResourceType, new object[]
					{
						this.typeSerializer.TargetType,
						value.GetType()
					});
				}
				this.valueSetter(parentInstance, value);
			};
			input.ReadSharedResource<object>(this.formatAttribute, fixup);
		}
		public bool ObjectIsEmpty(object parent)
		{
			object obj = this.valueGetter(parent);
			return obj == null || (!(obj.GetType() != this.typeSerializer.BoxedTargetType) && this.typeSerializer.ObjectIsEmpty(obj));
		}
		public void ScanValue(ContentTypeSerializer.ChildCallback callback, object parent)
		{
			object obj = this.valueGetter(parent);
			if (obj == null)
			{
				return;
			}
			if (this.formatAttribute.SharedResource)
			{
				callback(null, obj);
				return;
			}
			callback(this.typeSerializer, obj);
		}
		protected override bool CanDeserializeIntoExistingObject(IntermediateSerializer serializer, Type memberType)
		{
			return serializer.GetTypeSerializer(memberType).CanDeserializeIntoExistingObject;
		}
	}
}
