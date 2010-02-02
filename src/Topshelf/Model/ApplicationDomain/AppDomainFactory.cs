// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Topshelf.Model.ApplicationDomain
{
	using System;
	using System.IO;
	using System.Reflection;
	using System.Security.Policy;
	using Isolated;
	using Shelving;

	public class AppDomainFactory
	{
		public static AppDomainBundle CreateNewShelvedAppDomain(ShelvedServiceInfo info, string cachePath)
		{
			AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;

			setup.PrivateBinPath = info.FullPath;
			setup.ApplicationBase = info.FullPath;
			setup.ApplicationName = info.InferredName;
			setup.ConfigurationFile = Path.Combine(info.FullPath, info.InferredName + ".dll.config");

			setup.ShadowCopyFiles = true.ToString();
			setup.ShadowCopyDirectories = info.FullPath;
			setup.CachePath = Path.Combine(cachePath, info.InferredName);

			string domainName = info.InferredName + "AppDomain";
			var evidence = new Evidence(AppDomain.CurrentDomain.Evidence);

			AppDomain domain = AppDomain.CreateDomain(domainName, null, setup);

			var args = new object[] {info.AssemblyName};
			var manager = (ShelvedAppDomainManager)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().GetName().ToString(),
			                                	typeof(ShelvedAppDomainManager).FullName, true,
			                                	BindingFlags.Public | BindingFlags.Instance,
			                                	null, args, null, null, null);

			return new AppDomainBundle(domain, manager);
		}

		public static AppDomainBundle CreateNewIsolatedAppDomain(IsolatedServiceInfo info, string cachePath)
		{
			AppDomainSetup setup = AppDomain.CurrentDomain.SetupInformation;
            
            //private bin path : no
            //application base : no
		    setup.ApplicationName = info.Name;

            if (!string.IsNullOrEmpty(info.PathToConfigurationFile))
		        setup.ConfigurationFile = info.PathToConfigurationFile;

			setup.ShadowCopyFiles = true.ToString();
            //shadow copy directories
		    setup.CachePath = Path.Combine(cachePath, info.Name);


			if (info.Args != null)
				setup.AppDomainInitializerArguments = info.Args;

			if (info.ConfigureArgsAction != null)
				setup.AppDomainInitializer = info.ConfigureArgsAction();

			AppDomain domain = AppDomain.CreateDomain(info.Name, null, setup);
			var args = new object[] {info.Actions, info.ServiceType};

            var mgr = (IsolatedAppDomainManager)domain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().GetName().ToString(),
                typeof(IsolatedAppDomainManager).FullName, true,
                BindingFlags.Public | BindingFlags.Instance, 
                null, args, null, null, null);

			return new AppDomainBundle(domain, mgr);
		}
	}
}