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
        MarshalByRefObject,
        IServiceController
    {
        readonly IServiceController _wrappedServiceController;

        public IsolatedServiceControllerProxy(Type type, SerializableActions<object> actions)
        {
            Actions = actions;
            _wrappedServiceController = (IServiceController)typeof(ServiceController<>).MakeGenericType(type);
            //TODO: how to set actions on wrapped service controller
        }

        public SerializableActions<object> Actions { get; set; }
        #region IServiceController Members

        public void Initialize()
        {
            _wrappedServiceController.Initialize();
        }

        public void Dispose()
        {
            _wrappedServiceController.Dispose();
        }

        public Type ServiceType
        {
            get { return _wrappedServiceController.ServiceType; }
        }

        public string Name
        {
            get { return _wrappedServiceController.Name; }
            set { _wrappedServiceController.Name = value; }
        }

        public ServiceState State
        {
            get { return _wrappedServiceController.State; }
        }

        public void Start()
        {
            _wrappedServiceController.Start();
        }

        public void Stop()
        {
            _wrappedServiceController.Stop();
        }

        public void Pause()
        {
            _wrappedServiceController.Pause();
        }

        public void Continue()
        {
            _wrappedServiceController.Continue();
        }

        public ServiceBuilder BuildService
        {
            get { return _wrappedServiceController.BuildService; }
        }

        #endregion
    }
}