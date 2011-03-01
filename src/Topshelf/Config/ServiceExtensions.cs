﻿// Copyright 2007-2011 The Apache Software Foundation.
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
	using System;
	using HostConfigurators;
	using Model;
	using ServiceConfigurators;


	public static class ServiceExtensions
	{
		public static HostConfigurator Service<TService>(this HostConfigurator configurator,
		                                                 Action<ServiceConfigurator<TService>> callback)
			where TService : class
		{
			var serviceConfigurator = new ServiceConfiguratorImpl<TService>();

			callback(serviceConfigurator);

			configurator.AddConfigurator(serviceConfigurator);

			return configurator;
		}

		public static ServiceConfigurator<T> ConstructUsing<T>(this ServiceConfigurator<T> configurator, Func<T> factory)
			where T : class
		{
			configurator.ConstructUsing((d, name, coordinator) => factory());

			return configurator;
		}

		public static ServiceConfigurator<T> ConstructUsing<T>(this ServiceConfigurator<T> configurator,
		                                                       Func<string, T> factory)
			where T : class
		{
			configurator.ConstructUsing((d, name, coordinator) => factory(name));

			return configurator;
		}

		public static ServiceConfigurator<T> ConstructUsing<T>(this ServiceConfigurator<T> configurator,
		                                                       InternalServiceFactory<T> factory)
			where T : class
		{
			configurator.ConstructUsing((d, name, coordinator) => factory(name, coordinator));

			return configurator;
		}
	}
}