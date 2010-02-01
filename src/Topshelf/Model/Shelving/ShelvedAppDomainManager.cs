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
		readonly Type _bootstrapperType;
		readonly IServiceController _controller;
		readonly AssemblyName _servicePath;
		Assembly _assembly;

		public ShelvedAppDomainManager(AssemblyName servicePath)
		{
			_servicePath = servicePath;

			LoadServiceAssemblyIntoAppDomain();

			_bootstrapperType = FindBootstrapperImplementation();

			_bootstrapper = Activator.CreateInstance(_bootstrapperType) as Bootstrapper;
			if (_bootstrapper == null)
				throw new InvalidOperationException("Unable to create the bootstrapper: " + _bootstrapperType.FullName);

			Type controllerType = typeof(ServiceController<>).MakeGenericType(_bootstrapper.ServiceType);

			ServiceBuilder builder = x =>
				{
					return _bootstrapper.BuildService();
				};

			_controller = Activator.CreateInstance(controllerType, builder) as IServiceController;
			if (_controller == null)
				throw new InvalidOperationException("Unable to create controller for service: " + _bootstrapper.ServiceType.FullName);

			_controller.Initialize();
		}

		public string Name
		{
			get { return _controller.Name; }
			set { _controller.Name = Name; }
		}

		public Type ServiceType
		{
			get { return _controller.ServiceType; }
		}

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

		void LoadServiceAssemblyIntoAppDomain()
		{
			_assembly = AppDomain.CurrentDomain.Load(_servicePath);
		}

		static Type FindBootstrapperImplementation()
		{
			Type type = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => x.IsInterface == false)
				.Where(x => typeof(Bootstrapper).IsAssignableFrom(x))
				.FirstOrDefault();

			if (type == null)
				throw new InvalidOperationException("The bootstrapper was not found.");
			return type;
		}
	}
}