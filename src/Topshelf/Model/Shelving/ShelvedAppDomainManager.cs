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
namespace Topshelf.Model.Shelving
{
    using System;

    [Serializable]
    public class ShelvedAppDomainManager
    {
        readonly IServiceController _controller;

        public ShelvedAppDomainManager()
        {
            Bootstrapper bootstrapper = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if(typeof(Bootstrapper).IsAssignableFrom(type))
                    {
                        bootstrapper = (Bootstrapper)Activator.CreateInstance(type);
                        break;
                    }
                }
            }

            _controller = (IServiceController)typeof(ServiceController<>).MakeGenericType(bootstrapper.ServiceType);
            
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

        public string Name
        {
            get { return _controller.Name; }
        }

        public ServiceState State
        {
            get { return _controller.State; }
        }
    }

    public interface Bootstrapper
    {
        Type ServiceType { get; }
        object BuildService();
    }
}