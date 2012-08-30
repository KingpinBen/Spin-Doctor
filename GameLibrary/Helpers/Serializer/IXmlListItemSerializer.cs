using System;
namespace GameLibrary.Helpers.Serializer
{
	internal interface IXmlListItemSerializer
	{
		object Deserialize(XmlListReader list);
	}
}
