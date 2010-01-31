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
    using System.IO;

    public class ShelvedServiceInfo
    {
        readonly string _fullPath;
        readonly string _inferredName;

        public ShelvedServiceInfo(string path)
        {
            _inferredName = new DirectoryInfo(path).Name;
            _fullPath = new DirectoryInfo(path).FullName;
        }

        public string InferredName
        {
            get { return _inferredName; }
        }

        public string FullPath
        {
            get
            {
                return _fullPath;
            }
        }
    }
}