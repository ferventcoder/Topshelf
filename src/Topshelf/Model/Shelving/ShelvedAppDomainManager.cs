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
	using System.Linq;
	using System.Reflection;

	public class ShelvedAppDomainManager :
		AppDomainManager,
		IServiceController
	{
		readonly Bootstrapper _bootstrapper;
		readonly IServiceController _controller;

		public ShelvedAppDomainManager(AssemblyName servicePath)
		{
			var assembly = AppDomain.CurrentDomain.Load(servicePath);

			Type type = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => x.IsInterface == false)
				.Where(x => typeof(Bootstrapper).IsAssignableFrom(x))
				.FirstOrDefault();

			if (type == null)
				throw new InvalidOperationException("The bootstrapper was not found.");

			_bootstrapper = Activator.CreateInstance(type) as Bootstrapper;
			if (_bootstrapper == null)
				throw new InvalidOperationException("Unable to create the bootstrapper");

			Type controllerType = typeof(ServiceController<>).MakeGenericType(_bootstrapper.ServiceType);

			_controller = Activator.CreateInstance(controllerType) as IServiceController;
		}

		public string Name
		{
			get { return _controller.Name; }
		}


		public Type ServiceType
		{
			get { return _controller.ServiceType; }
		}

		string IServiceController.Name { get; set; }

		public ServiceState State
		{
			get { return _controller.State; }
		}

		public ServiceBuilder BuildService
		{
			get { return _controller.BuildService; }
		}

		public void Initialize()
		{
			_controller.Initialize();
		}

		public void Start()
		{
			_controller.Start();
		}

		public void Stop()
		{
			_controller.Stop();
		}

		public void Pause()
		{
			_controller.Pause();
		}

		public void Continue()
		{
			_controller.Continue();
		}

		public void Dispose()
		{
		}
	}
}