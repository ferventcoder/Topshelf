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
namespace Topshelf.Model.Isolated
{
    using System;

    //this class is remoted accross, and should delegate actions to the real service controller
    public class IsolatedServiceControllerProxy :
        IServiceController
    {
        readonly IsolatedAppDomainManager _manager;
        AppDomain _domain;

        public IsolatedServiceControllerProxy(AppDomain domain, IsolatedAppDomainManager manager)
        {
            _domain = domain;
            _manager = manager;
        }

        #region IServiceController Members

        public void Initialize()
        {
            _manager.Initialize();
        }

        public void Dispose()
        {
            _manager.Dispose();
        }

        public Type ServiceType
        {
            get { return _manager.ServiceType; }
        }

        public string Name
        {
            get { return _manager.Name; }
            set { _manager.Name = value; }
        }

        public ServiceState State
        {
            get { return _manager.State; }
        }

        public void Start()
        {
            _manager.Start();
        }

        public void Stop()
        {
            _manager.Stop();
        }

        public void Pause()
        {
            _manager.Pause();
        }

        public void Continue()
        {
            _manager.Continue();
        }

        public ServiceBuilder BuildService
        {
            get { return _manager.BuildService; }
        }

        #endregion
    }
}