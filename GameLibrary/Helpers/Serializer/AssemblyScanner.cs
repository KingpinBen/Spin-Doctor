using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace GameLibrary.Helpers.Serializer
{
	internal class AssemblyScanner : IEnumerable<Assembly>, IEnumerable
	{
		private bool includeAllAssemblies;
		private static List<Assembly> loadedAssemblies = new List<Assembly>(new Assembly[]
		{
			typeof(AssemblyScanner).Assembly
		});
		public AssemblyScanner(bool includeAllAssemblies)
		{
			this.includeAllAssemblies = includeAllAssemblies;
		}
		public static void LoadAssembly(string assembly, bool includeInSearch)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			Assembly item;
			if (File.Exists(assembly))
			{
				if (Path.IsPathRooted(assembly) && new Uri(assembly).IsUnc)
				{
					throw new PipelineException(string.Format(CultureInfo.CurrentCulture, Resources.CantLoadPipelineAssemblyOffShare, new object[]
					{
						assembly
					}));
				}
				item = Assembly.LoadFrom(assembly);
			}
			else
			{
				item = Assembly.Load(assembly);
			}
			if (includeInSearch && !AssemblyScanner.loadedAssemblies.Contains(item))
			{
				AssemblyScanner.loadedAssemblies.Add(item);
			}
		}
		public static IEnumerable<Type> GetSearchAssemblyTypes()
		{
			List<Type> list = new List<Type>();
			foreach (Assembly current in AssemblyScanner.loadedAssemblies)
			{
				try
				{
					list.AddRange(current.GetTypes());
				}
				catch (ReflectionTypeLoadException)
				{
					throw new PipelineException(string.Format(CultureInfo.CurrentCulture, Resources.CantLoadPipelineAssembly, new object[]
					{
						current.FullName
					}));
				}
			}
			return list;
		}
		public IEnumerator<Assembly> GetEnumerator()
		{
			Stack<Assembly> stack = new Stack<Assembly>();
			Dictionary<Assembly, bool> dictionary = new Dictionary<Assembly, bool>();
			IEnumerable<Assembly> assemblies;
			if (this.includeAllAssemblies)
			{
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
			}
			else
			{
				assemblies = AssemblyScanner.loadedAssemblies;
			}
			using (IEnumerator<Assembly> enumerator = assemblies.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Assembly current = enumerator.Current;
					if (!this.IsSystemAssembly(current.GetName().Name))
					{
						stack.Push(current);
						dictionary[current] = true;
					}
				}
				goto IL_15B;
			}
			IL_C4:
			Assembly assembly = stack.Pop();
			yield return assembly;
			AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
			for (int i = 0; i < referencedAssemblies.Length; i++)
			{
				AssemblyName assemblyName = referencedAssemblies[i];
				if (!this.IsSystemAssembly(assemblyName.Name))
				{
					try
					{
						Assembly assembly2 = Assembly.Load(assemblyName);
						if (!dictionary.ContainsKey(assembly2))
						{
							stack.Push(assembly2);
							dictionary[assembly2] = true;
						}
					}
					catch
					{
					}
				}
			}
			IL_15B:
			if (stack.Count <= 0)
			{
				yield break;
			}
			goto IL_C4;
		}
		private bool IsSystemAssembly(string name)
		{
			return !this.includeAllAssemblies && (name == "mscorlib" || name == "System" || name.StartsWith("System.") || name.StartsWith("Microsoft.Build.") || name == "Microsoft.VisualC" || (name.StartsWith("Microsoft.Xna.Framework") && !name.StartsWith("Microsoft.Xna.Framework.Content.Pipeline")));
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
