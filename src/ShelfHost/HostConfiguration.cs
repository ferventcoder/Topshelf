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
namespace ShelfHost
{
	using System.IO;
	using System.Reflection;

	public class HostConfiguration :
		IHostConfiguration
	{
		const string CacheFolderName = "AssemblyCache";
		const string ServiceFolderName = "Services";

		public string HostPath
		{
			get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
		}

		public string CachePath
		{
			get { return Path.Combine(HostPath, CacheFolderName); }
		}

		public string ServicesPath
		{
			get { return Path.Combine(HostPath, ServiceFolderName); }
		}
	}
}