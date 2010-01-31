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
    using System.Diagnostics;

    [DebuggerDisplay("Shelved")]
    public class ShelvedServiceController :
        IServiceController
    {
        //this should be newed up in the remote app domain
        readonly ShelvedAppDomainManager _manager = new ShelvedAppDomainManager();
        public string PathToConfigurationFile { get; set; }

        public string[] Args { get; set; }

        #region IServiceController Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            //no-op
        }

        public Type ServiceType
        {
            get { throw new NotImplementedException(); }
        }

        public string Name { get; set; }

        public ServiceState State
        {
            get { return _manager.State; }
        }

        public ServiceBuilder BuildService
        {
            get { throw new NotImplementedException(); }
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

        #endregion
    }
}