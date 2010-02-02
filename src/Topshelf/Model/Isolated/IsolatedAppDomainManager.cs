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
        readonly SerializableActions<object> _actions;

        readonly IServiceController _controller;

        object _service;

        public IsolatedAppDomainManager(SerializableActions<object> actions, Type serviceType)
        {
            _actions = actions;

            //crazy stuff here

            Type controllerType = typeof(ServiceController<>).MakeGenericType(serviceType);

            _controller = Activator.CreateInstance(controllerType, actions.BuildAction) as IServiceController;
        }


        public string Name
        {
            get
            {
                return _controller.Name;
            }
            set
            {
                _controller.Name = value;
            }
        }

        public ServiceState State
        {
            get { return _controller.State; }
        }

        public Type ServiceType
        {
            get { return _controller.ServiceType; }
        }

        public ServiceBuilder BuildService
        {
            get { return _controller.BuildService; }
        }

        public void Initialize()
        {
           _controller.Initialize();
        }

        public void Start()
        {
            _controller.Start();
        }

        public void Stop()
        {
            _controller.Stop();
        }

        public void Pause()
        {
            _controller.Pause();
        }

        public void Continue()
        {
            _controller.Continue();
        }

        public void Dispose()
        {
        }
    }
}