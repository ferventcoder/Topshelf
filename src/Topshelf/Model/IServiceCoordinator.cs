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
namespace Topshelf.Model
{
	using System;
	using System.Collections.Generic;


	/// <summary>
	///   Interface to the service controller
	/// </summary>
	public interface IServiceCoordinator :
		IEnumerable<IServiceController>,
		IDisposable
	{
		/// <summary>
		///   The number of services managed by the coordinator
		/// </summary>
		int ServiceCount { get; }

		/// <summary>
		///   Returns the service by name
		/// </summary>
		/// <param name = "serviceName">The name of the service to retrieve</param>
		/// <returns></returns>
		IServiceController this[string serviceName] { get; }

		/// <summary>
		/// Sends a message to the coordinator
		/// </summary>
		/// <typeparam name="T">The message type</typeparam>
		/// <param name="message">The message</param>
		void Send<T>(T message);

		/// <summary>
		///   Start the services hosted by the coordinator and wait until they have completed starting
		///   before returning to the caller
		/// </summary>
		void Start();

		/// <summary>
		///   Stops the service coordinator and waits for the specified timeout until the services have stopped
		/// </summary>
		void Stop();

		/// <summary>
		/// Create the service using the factory method specified and add it to the coordinator
		/// </summary>
		/// <param name="serviceName">The name of the service to be created</param>
		/// <param name="serviceFactory">The factory method to use when the service is created</param>
		void CreateService(string serviceName, Func<IServiceCoordinator, ServiceStateMachine> serviceFactory);
	}
}