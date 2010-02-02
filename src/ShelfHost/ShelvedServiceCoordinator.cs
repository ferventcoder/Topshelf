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
	using System;
	using System.Collections.Generic;
	using Magnum;
	using Magnum.Pipeline;
	using Topshelf.Commands;
	using Topshelf.Events;
	using Topshelf.Model.ApplicationDomain;
	using Topshelf.Model.Shelving;

	public class ShelvedServiceCoordinator :
		IConsumer<NewServiceFound>,
		IConsumer<ShutdownHost>,
		IDisposable
	{
		readonly IHostConfiguration _configuration;
		readonly Pipe _eventAggregator;
		IDictionary<string, AppDomainBundle> _bundles = new Dictionary<string, AppDomainBundle>();
		bool _disposed;
		ISubscriptionScope _scope;

		public ShelvedServiceCoordinator(Pipe eventAggregator, IHostConfiguration configuration)
		{
			_eventAggregator = eventAggregator;
			_configuration = configuration;
		}

		public void Consume(NewServiceFound message)
		{
			var info = new ShelvedServiceInfo(message.ServicePath);

			AppDomainBundle bundle = AppDomainFactory.CreateNewShelvedAppDomain(info, _configuration.CachePath);

			bundle.Controller.Start();

			Console.WriteLine("Service was started: " + info.InferredName);

			_bundles.Add(info.InferredName, bundle);
		}

		public void Consume(ShutdownHost message)
		{
			_bundles.Values.Each(service => { service.Controller.Stop(); });
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Start()
		{
			_scope = _eventAggregator.NewSubscriptionScope();

			_scope.Subscribe(this);
		}

		void Dispose(bool disposing)
		{
			if (_disposed)
				return;
			if (disposing)
			{
				if (_scope != null)
				{
					_scope.Dispose();
					_scope = null;
				}

				_bundles.Values.Each(service =>
					{
						AppDomain.Unload(service.Domain);
					});
			}

			_disposed = true;
		}

		~ShelvedServiceCoordinator()
		{
			Dispose(false);
		}
	}
}