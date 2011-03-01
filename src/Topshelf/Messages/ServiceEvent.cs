﻿// Copyright 2007-2010 The Apache Software Foundation.
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
namespace Topshelf.Messages
{
	/// <summary>
	/// A service event, published by a service as the state changes
	/// </summary>
	public abstract class ServiceEvent
	{
		protected ServiceEvent(string serviceName)
		{
			ServiceName = serviceName;
		}

		protected ServiceEvent()
		{
		}

		/// <summary>
		/// The name of the service that sourced this event
		/// </summary>
		public string ServiceName { get; protected set; }

		/// <summary>
		/// The event type that was published
		/// </summary>
		public ServiceEventType EventType { get; protected set; }
	}
}