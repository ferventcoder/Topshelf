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
    using System.Reflection;
    using Isolated;
    using Shelving;

    public class AppDomainFactory
    {
        public static AppDomainBundle CreateNewAppDomain(ShelvedServiceInfo info)
        {
            var setup = AppDomain.CurrentDomain.SetupInformation;
            setup.PrivateBinPath = info.FullPath;
            setup.ApplicationBase = info.FullPath;

            setup.ShadowCopyFiles = "true";
            setup.ShadowCopyDirectories = "true";

            var domain = AppDomain.CreateDomain(info.InferredName, null, setup);
            var mgr = (TopshelfAppDomainManager)domain.CreateInstanceAndUnwrap("", "");
            return new AppDomainBundle(domain, mgr);
        }

        public static AppDomainBundle CreateNewAppDomain(IsolatedServiceInfo info)
        {
            var setup = AppDomain.CurrentDomain.SetupInformation;

            setup.ShadowCopyFiles = "true";

            if (!string.IsNullOrEmpty(info.PathToConfigurationFile))
            {
                setup.ConfigurationFile = info.PathToConfigurationFile;
            }

            if (info.Args != null)
                setup.AppDomainInitializerArguments = info.Args;
            if (info.ConfigureArgsAction != null)
                setup.AppDomainInitializer = info.ConfigureArgsAction();

            var domain = AppDomain.CreateDomain(info.Name, null, setup);
            var args = new object[] {info.Type, info.Actions};
            var mgr = (TopshelfAppDomainManager)domain.CreateInstanceAndUnwrap("", "", true, BindingFlags.Public, null, args,null, null, null);
            return new AppDomainBundle(domain, mgr);
        }
    }
}