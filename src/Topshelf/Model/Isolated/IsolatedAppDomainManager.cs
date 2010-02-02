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
    using ApplicationDomain;

    public class IsolatedAppDomainManager :
        MarshalByRefObject,
        IServiceController
    {
        SerializableActions<object> _actions;
        object _service;

        public IsolatedAppDomainManager(SerializableActions<object> actions)
        {
            _actions = actions;
        }

        public void Dispose()
        {

        }

        public string Name
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public ServiceState State
        {
            get { throw new NotImplementedException(); }
        }

        public Type ServiceType
        {
            get { throw new NotImplementedException(); }
        }

        public ServiceBuilder BuildService
        {
            get { return _actions.BuildAction; }
        }

        public void Initialize()
        {
            _service = BuildService(Name);
        }

        public void Start()
        {
            _actions.StartAction(_service);
        }

        public void Stop()
        {
            _actions.StopAction(_service);
        }

        public void Pause()
        {
            _actions.PauseAction(_service);
        }

        public void Continue()
        {
            _actions.ContinueAction(_service);
        }
    }
}