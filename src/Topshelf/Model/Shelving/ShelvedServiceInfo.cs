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
	using System.IO;
	using System.Reflection;

	[Serializable]
	public class ShelvedServiceInfo
	{
		public ShelvedServiceInfo(string path)
			: this(new DirectoryInfo(path))
		{
		}

		public ShelvedServiceInfo(FileSystemInfo info)
		{
			InferredName = info.Name;
			FullPath = info.FullName;
			AssemblyName = AssemblyName.GetAssemblyName(Path.Combine(FullPath, InferredName + ".dll"));
		}

		public AssemblyName AssemblyName { get; private set; }

		public string InferredName { get; private set; }

		public string FullPath { get; private set; }
	}
}