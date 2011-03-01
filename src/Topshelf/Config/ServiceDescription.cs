// Copyright 2007-2011 The Apache Software Foundation.
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
namespace Topshelf
{
	public interface ServiceDescription
	{
		/// <summary>
		/// The name of the service as it is registered in the service control manager
		/// </summary>
		/// <param name="name"></param>
		string Name { get; }

		/// <summary>
		/// The name of the service as it should be displayed in the service control manager
		/// </summary>
		/// <param name="name"></param>
		string DisplayName { get; }

		/// <summary>
		/// The description of the service that is displayed in the service control manager
		/// </summary>
		/// <param name="description"></param>
		string Description { get; }

		/// <summary>
		/// The service instance name that should be used when the service is registered
		/// </summary>
		/// <param name="instanceName"></param>
		string InstanceName { get; }

		/// <summary>
		/// Returns the Windows service name, including the instance name, which is registered with the SCM
		/// Example: myservice$bob
		/// </summary>
		/// <returns></returns>
		string GetServiceName();
	}
}