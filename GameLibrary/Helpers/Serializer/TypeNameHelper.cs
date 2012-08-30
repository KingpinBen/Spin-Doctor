using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace GameLibrary.Helpers.Serializer
{
	internal class TypeNameHelper
	{
		private const string ArraySuffix = "[]";
		private const string NestedTypeSeparator = "+";
		private const string GenericArgumentStart = "[";
		private const string GenericArgumentSeparator = ",";
		private const string GenericArgumentClose = "]";
		private const string ClrGenericMarker = "`";
		private const char ClrNamespaceSeparator = '.';
		private const char XmlNamespaceSeparator = ':';
		private readonly char[] XmlNamespaceBeginnings = new char[]
		{
			'[',
			','
		};
		private Dictionary<Type, string> typeToXmlName = new Dictionary<Type, string>();
		private Dictionary<string, Type> xmlNameToType = new Dictionary<string, Type>();
		public void WriteTypeName(XmlWriter output, Type type)
		{
			if (type.IsByRef || type.IsPointer)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.BadTypePointerOrReference, new object[]
				{
					type
				}));
			}
			if (type.ContainsGenericParameters)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.BadTypeOpenGeneric, new object[]
				{
					type
				}));
			}
			string text;
			if (this.typeToXmlName.TryGetValue(type, out text))
			{
				output.WriteString(text);
				return;
			}
			if (!type.IsArray)
			{
				this.WriteClrType(output, type);
				return;
			}
			if (type.GetArrayRank() != 1)
			{
				throw new RankException(Resources.CantSerializeMultidimensionalArrays);
			}
			this.WriteTypeName(output, type.GetElementType());
			output.WriteString("[]");
		}
		private void WriteClrType(XmlWriter output, Type type)
		{
			string text = type.Name;
			int num = text.IndexOf("`");
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
			Type declaringType = TypeNameHelper.GetDeclaringType(type);
			if (declaringType != null)
			{
				this.WriteClrType(output, declaringType);
				output.WriteString("+" + text);
			}
			else
			{
				TypeNameHelper.WriteTypeAndNamespace(output, text, type.Namespace);
			}
			this.WriteGenericArguments(output, type, declaringType);
		}
		private static void WriteTypeAndNamespace(XmlWriter output, string typeName, string clrNamespace)
		{
			if (string.IsNullOrEmpty(clrNamespace))
			{
				output.WriteString(typeName);
				return;
			}
			string text = output.LookupPrefix(clrNamespace);
			if (text != null)
			{
				output.WriteString(text + ':' + typeName);
				return;
			}
			int num = clrNamespace.LastIndexOf('.');
			typeName = clrNamespace.Substring(num + 1) + '.' + typeName;
			if (num > 0)
			{
				clrNamespace = clrNamespace.Substring(0, num);
				TypeNameHelper.WriteTypeAndNamespace(output, typeName, clrNamespace);
				return;
			}
			output.WriteString(typeName);
		}
		private void WriteGenericArguments(XmlWriter output, Type type, Type declaringType)
		{
			if (!type.IsGenericType)
			{
				return;
			}
			Type[] genericArguments = type.GetGenericArguments();
			int i = 0;
			if (declaringType != null && declaringType.IsGenericType)
			{
				i += declaringType.GetGenericArguments().Length;
			}
			if (i >= genericArguments.Length)
			{
				return;
			}
			output.WriteString("[");
			this.WriteTypeName(output, genericArguments[i++]);
			while (i < genericArguments.Length)
			{
				output.WriteString(",");
				this.WriteTypeName(output, genericArguments[i++]);
			}
			output.WriteString("]");
		}
		private static Type GetDeclaringType(Type type)
		{
			Type declaringType = type.DeclaringType;
			if (declaringType == null)
			{
				return null;
			}
			if (!declaringType.IsGenericTypeDefinition)
			{
				return declaringType;
			}
			int num = declaringType.GetGenericArguments().Length;
			Type[] array = new Type[num];
			Array.Copy(type.GetGenericArguments(), array, num);
			return declaringType.MakeGenericType(array);
		}
		public Type ParseTypeName(XmlReader input, string typeName)
		{
			typeName = this.ExpandXmlNamespaces(input, typeName);
			Type type;
			if (this.xmlNameToType.TryGetValue(typeName, out type))
			{
				return type;
			}
			if (typeName.EndsWith("[]"))
			{
				string typeName2 = typeName.Substring(0, typeName.Length - "[]".Length);
				Type type2 = this.ParseTypeName(input, typeName2);
				type = type2.MakeArrayType();
			}
			else
			{
				List<Type> list = new List<Type>();
				string clrTypeName = this.GetClrTypeName(input, typeName, list);
				type = TypeNameHelper.FindType(clrTypeName);
				if (list.Count > 0)
				{
					type = type.MakeGenericType(list.ToArray());
				}
			}
			this.xmlNameToType[typeName] = type;
			return type;
		}
		private string GetClrTypeName(XmlReader input, string typeName, List<Type> genericArguments)
		{
			int num = TypeNameHelper.IndexOfTypeSeparator(typeName);
			if (num > 0)
			{
				string text = typeName.Substring(0, num);
				string text2 = typeName.Substring(num + "+".Length);
				text = this.GetClrTypeName(input, text, genericArguments);
				text2 = this.GetClrTypeName(input, text2, genericArguments);
				typeName = text + "+" + text2;
			}
			else
			{
				if (typeName.EndsWith("]"))
				{
					typeName = this.ParseGenericArguments(input, typeName, genericArguments);
				}
			}
			return typeName;
		}
		private static int IndexOfTypeSeparator(string input)
		{
			int num = 0;
			for (int i = 0; i < input.Length; i++)
			{
				if (TypeNameHelper.SubstringMatches(input, i, "["))
				{
					num++;
				}
				else
				{
					if (TypeNameHelper.SubstringMatches(input, i, "]"))
					{
						num--;
					}
					else
					{
						if (TypeNameHelper.SubstringMatches(input, i, "+") && num == 0)
						{
							return i;
						}
					}
				}
			}
			return -1;
		}
		private string ParseGenericArguments(XmlReader input, string typeName, List<Type> genericArguments)
		{
			int num = typeName.IndexOf("[");
			if (num <= 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.BadTypeNameString, new object[]
				{
					typeName
				}));
			}
			int num2 = num + "[".Length;
			int length = typeName.Length - num2 - "]".Length;
			string input2 = typeName.Substring(num2, length);
			List<string> list = TypeNameHelper.SplitGenericArguments(input2);
			foreach (string current in list)
			{
				genericArguments.Add(this.ParseTypeName(input, current));
			}
			typeName = typeName.Substring(0, num);
			typeName = typeName + "`" + list.Count.ToString(CultureInfo.InvariantCulture);
			return typeName;
		}
		private static List<string> SplitGenericArguments(string input)
		{
			List<string> list = new List<string>();
			int num = 0;
			int i = 0;
			while (i < input.Length)
			{
				if (TypeNameHelper.SubstringMatches(input, i, ",") && num == 0)
				{
					list.Add(input.Substring(0, i));
					input = input.Substring(i + ",".Length);
					i = 0;
				}
				else
				{
					if (TypeNameHelper.SubstringMatches(input, i, "["))
					{
						num++;
					}
					else
					{
						if (TypeNameHelper.SubstringMatches(input, i, "]"))
						{
							num--;
						}
					}
					i++;
				}
			}
			if (!string.IsNullOrEmpty(input))
			{
				list.Add(input);
			}
			return list;
		}
		private static bool SubstringMatches(string input, int pos, string token)
		{
			if (token.Length == 1)
			{
				return input[pos] == token[0];
			}
			throw new NotSupportedException();
		}
		private string ExpandXmlNamespaces(XmlReader input, string typeName)
		{
			int startIndex = 0;
			int num;
			while ((num = typeName.IndexOf(':', startIndex)) > 0)
			{
				int num2 = typeName.LastIndexOfAny(this.XmlNamespaceBeginnings, num) + 1;
				string text = typeName.Substring(0, num2);
				string text2 = typeName.Substring(num2, num - num2);
				string text3 = typeName.Substring(num + 1);
				text2 = input.LookupNamespace(text2);
				typeName = string.Concat(new object[]
				{
					text,
					text2,
					'.',
					text3
				});
				startIndex = typeName.Length - text3.Length;
			}
			return typeName;
		}
		private static Type FindType(string typeName)
		{
			Type type = Type.GetType(typeName);
			if (type != null)
			{
				return type;
			}
			foreach (Assembly current in new AssemblyScanner(true))
			{
				type = current.GetType(typeName);
				if (type != null)
				{
					return type;
				}
			}
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.CantFindType, new object[]
			{
				typeName
			}));
		}
		public void AddXmlTypeName(ContentTypeSerializer serializer)
		{
			Type targetType = serializer.TargetType;
			string xmlTypeName = serializer.XmlTypeName;
			if (this.xmlNameToType.ContainsKey(xmlTypeName))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateXmlTypeName, new object[]
				{
					serializer.GetType().AssemblyQualifiedName,
					this.xmlNameToType[xmlTypeName].AssemblyQualifiedName,
					xmlTypeName
				}));
			}
			this.typeToXmlName.Add(targetType, xmlTypeName);
			this.xmlNameToType.Add(xmlTypeName, targetType);
		}
	}
}
