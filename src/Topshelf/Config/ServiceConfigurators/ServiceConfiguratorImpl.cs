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
namespace Topshelf.ServiceConfigurators
{
	using System;
	using Builders;
	using HostConfigurators;
	using Magnum.Extensions;
	using Model;


	public class ServiceConfiguratorImpl<TService> :
		ServiceConfigurator<TService>,
		HostBuilderConfigurator
		where TService : class
	{
		Action<TService> _continue;
		DescriptionServiceFactory<TService> _factory;
		string _name;
		Action<TService> _pause;
		Action<TService> _start;
		Action<TService> _stop;

		public HostBuilder Configure(HostBuilder builder)
		{
			builder.Match<RunBuilder>(x =>
				{
					string name = _name.IsEmpty() ? typeof(TService).Name : _name;

					var serviceBuilder = new LocalServiceBuilder<TService>(x.Description, name, _factory, _start, _stop, _pause, _continue);

					x.AddServiceBuilder(serviceBuilder);
				});

			return builder;
		}

		public void Validate()
		{
			if (_start == null)
			{
				throw new HostConfigurationException(
					"A start action must be specified for the {0} service".FormatWith(typeof(TService)));
			}

			if (_stop == null)
			{
				throw new HostConfigurationException(
					"A stop action must be specified for the {0} service".FormatWith(typeof(TService)));
			}

			if (_factory == null)
			{
				throw new HostConfigurationException(
					"A service factory must be specified for the {0} service".FormatWith(typeof(TService)));
			}
		}

		public void SetServiceName(string name)
		{
			_name = name;
		}

		public void ConstructUsing(DescriptionServiceFactory<TService> serviceFactory)
		{
			_factory = serviceFactory;
		}

		public void WhenStarted(Action<TService> startAction)
		{
			_start = startAction;
		}

		public void WhenStopped(Action<TService> stopAction)
		{
			_stop = stopAction;
		}

		public void WhenPaused(Action<TService> pauseAction)
		{
			_pause = pauseAction;
		}

		public void WhenContinued(Action<TService> continueAction)
		{
			_continue = continueAction;
		}
	}
}