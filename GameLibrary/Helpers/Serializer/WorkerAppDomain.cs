using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;

namespace GameLibrary.Helpers.Serializer
{
	internal class WorkerAppDomain : IDisposable
	{
		private class RemoteProxy : MarshalByRefObject
		{
			private ISponsor clientSponsor;
			private ILease lease;
			public RemoteProxy()
			{
				WorkerAppDomain.InsideWorkerDomain = true;
			}
			public override object InitializeLifetimeService()
			{
				if (this.lease != null)
				{
					return this.lease;
				}
				this.lease = (ILease)base.InitializeLifetimeService();
				this.clientSponsor = new ClientSponsor();
				this.lease.Register(this.clientSponsor);
				return this.lease;
			}
			public void Unregister()
			{
				if (this.lease != null)
				{
					this.lease.Unregister(this.clientSponsor);
					this.lease = null;
				}
			}
			public void LoadAssemblies(string[] searchAssemblies, string[] dependencies)
			{
				WorkerAppDomain.RemoteProxy.LoadAssemblies(dependencies, false);
				WorkerAppDomain.RemoteProxy.LoadAssemblies(searchAssemblies, true);
				try
				{
					AssemblyScanner.GetSearchAssemblyTypes();
				}
				catch (TypeLoadException ex)
				{
					throw new PipelineException(string.Format(CultureInfo.CurrentCulture, Resources.CantLoadPipelineAssembly, new object[]
					{
						ex.TypeName
					}));
				}
				this.VerifyVersionOfReferenceAssemblies();
			}
			private void VerifyVersionOfReferenceAssemblies()
			{
				Version version = base.GetType().Assembly.GetName().Version;
				foreach (Assembly current in new AssemblyScanner(false))
				{
					string name = current.GetName().Name;
					AssemblyName[] referencedAssemblies = current.GetReferencedAssemblies();
					for (int i = 0; i < referencedAssemblies.Length; i++)
					{
						AssemblyName assemblyName = referencedAssemblies[i];
						string name2 = assemblyName.Name;
						if (this.StringMatchesXnaAssembly(name2))
						{
							Version version2 = assemblyName.Version;
							if (version2 != version)
							{
								throw new PipelineException(string.Format(CultureInfo.CurrentCulture, Resources.AssemblyReferenceWrongVersion, new object[]
								{
									name,
									name2,
									version2.ToString(),
									version.ToString()
								}));
							}
						}
					}
				}
			}
			private bool StringMatchesXnaAssembly(string assemblyName)
			{
				switch (assemblyName)
				{
				case "Microsoft.Xna.Framework":
				case "Microsoft.Xna.Framework.Game":
				case "Microsoft.Xna.Framework.GamerServices":
				case "Microsoft.Xna.Framework.Graphics":
				case "Microsoft.Xna.Framework.Xact":
				case "Microsoft.Xna.Framework.Net":
				case "Microsoft.Xna.Framework.Storage":
				case "Microsoft.Xna.Framework.Input.Touch":
				case "Microsoft.Xna.Framework.Video":
				case "Microsoft.Xna.Framework.Avatar":
				case "Microsoft.Xna.Content.Pipeline":
					return true;
				}
				return false;
			}
			private static void LoadAssemblies(string[] assemblies, bool includeInSearch)
			{
				if (assemblies != null)
				{
					for (int i = 0; i < assemblies.Length; i++)
					{
						string text = assemblies[i];
						try
						{
							AssemblyScanner.LoadAssembly(text, includeInSearch);
						}
						catch
						{
                            throw new Exception();
						}
					}
				}
			}
		}
		private AppDomain appDomain;
		private WorkerAppDomain.RemoteProxy remoteProxy;
		public static bool InsideWorkerDomain;
		public WorkerAppDomain()
		{
			Assembly assembly = typeof(WorkerAppDomain.RemoteProxy).Assembly;
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			appDomainSetup.ApplicationBase = Path.GetDirectoryName(assembly.Location);
			this.appDomain = AppDomain.CreateDomain(assembly.GetName().Name, null, appDomainSetup);
			try
			{
				this.remoteProxy = this.Create<WorkerAppDomain.RemoteProxy>();
			}
			catch
			{
				this.Dispose();
				throw;
			}
		}
		public void Dispose()
		{
			if (this.remoteProxy != null)
			{
				this.remoteProxy.Unregister();
				this.remoteProxy = null;
			}
			if (this.appDomain != null)
			{
				AppDomain.Unload(this.appDomain);
				this.appDomain = null;
			}
		}
		public T Create<T>()
		{
			return (T)((object)this.appDomain.CreateInstanceAndUnwrap(typeof(T).Assembly.FullName, typeof(T).FullName));
		}
		public void LoadAssemblies(string[] searchAssemblies, string[] dependencies)
		{
			this.remoteProxy.LoadAssemblies(searchAssemblies, dependencies);
		}
	}
}
