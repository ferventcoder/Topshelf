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
namespace Topshelf.Configuration.Dsl
{
    using System;
    using System.IO;
    using Model;
    using Model.Shelving;

    public class ShelvedServiceConfigurator :
        IShelvedServiceConfigurator
    {

         string _name;
        string _pathToPrivateBin;
        string _pathToConfigurationFile;
        string[] _args;

        #region IShelvedServiceConfigurator Members

        public void PathToPrivateBin(string pathToPrivateBin)
        {
            _pathToPrivateBin = pathToPrivateBin;
            _pathToConfigurationFile = Path.Combine(_pathToPrivateBin, "{0}.config".FormatWith(_name));
        }

        public void Named(string name)
        {
            _name = name;
        }

        public void CommandLineArguments(string[] args)
        {
            _args = args;
        }

        #endregion

        public IServiceController Create()
        {
            IServiceController serviceController = new ShelvedServiceController
                                                   {
                                                       Name = _name,
                                                       PathToConfigurationFile = _pathToConfigurationFile,
                                                       Args = _args
                                                   };

            return serviceController;
        
        }
    }
}