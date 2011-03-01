// Copyright 2007-2010 The Apache Software Foundation.
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
namespace Topshelf.Specs.ServiceCoordinator
{
	using Magnum.Extensions;
	using Magnum.TestFramework;
	using Messages;
	using Model;
	using NUnit.Framework;
	using Stact;
	using TestObject;


	[TestFixture]
	[Slow]
	public class ServiceCoordinator_Singles_Specs
	{
		[SetUp]
		public void EstablishContext()
		{
			_service = new TestService();
			_service2 = new TestService2();

			_serviceCoordinator = new ServiceCoordinator(new PoolFiber(), x => { }, x => { }, x => { }, 1.Minutes());
			_serviceCoordinator.CreateService("test",
			                                  (inbox,coordinator) =>
			                                  new LocalServiceController<TestService>("test", inbox, coordinator,
			                                                                     x => x.Start(),
			                                                                     x => x.Stop(),
			                                                                     x => x.Pause(),
			                                                                     x => x.Continue(),
			                                                                     (x, c) => _service));
			_serviceCoordinator.CreateService("test2",
											  (inbox, coordinator) =>
											  new LocalServiceController<TestService>("test2", inbox, coordinator,
			                                                                     x => x.Start(),
			                                                                     x => x.Stop(),
			                                                                     x => x.Pause(),
			                                                                     x =>
			                                                                     x.Continue(),
			                                                                     (x, c) =>
			                                                                     _service2));
			_serviceCoordinator.Start();

			_service.Running.WaitUntilCompleted(10.Seconds());
			_service2.Running.WaitUntilCompleted(10.Seconds());
		}

		[TearDown]
		public void CleanUp()
		{
			_serviceCoordinator.Dispose();
		}

		[Test, Explicit("Not Yet Implemented")]
		public void Continue_individual_service()
		{
            _serviceCoordinator.Send(new PauseService("test"));
            _service.Paused.WaitUntilCompleted(2.Seconds()).ShouldBeTrue();
            _serviceCoordinator.Send(new ContinueService("test"));
            _service.WasContinued.WaitUntilCompleted(2.Seconds()).ShouldBeTrue();

            _service2.Running.IsCompleted.ShouldBeTrue();
		}

        [Test, Explicit("Not Yet Implemented")]
		public void Pause_individual_service()
		{
			_serviceCoordinator.Send(new PauseService("test"));

			_service.Running.IsCompleted.ShouldBeTrue();
			_service.Paused.IsCompleted.ShouldBeTrue();

			_service2.Running.IsCompleted.ShouldBeTrue();
		}

        [Test, Explicit("Not Yet Implemented")]
		public void Pause_should_pause_all_services_and_continue_only_the_named_service()
		{
			_serviceCoordinator.Send(new PauseService("test"));
			_serviceCoordinator.Send(new PauseService("test2"));
			_serviceCoordinator.Send(new ContinueService("test"));

			_service.Running.IsCompleted.ShouldBeTrue();
			_service.Paused.IsCompleted.ShouldBeTrue();
			_service.WasContinued.IsCompleted.ShouldBeTrue();

			_service2.Running.IsCompleted.ShouldBeTrue();
			_service2.Paused.IsCompleted.ShouldBeTrue();
		}

        [Ignore("Failing")]
        [Test]
		public void Stop_individual_service()
		{
			_serviceCoordinator.Send(new StopService("test"));

			_service.Running.IsCompleted.ShouldBeTrue();
			_service.Stopped.WaitUntilCompleted(5.Seconds()).ShouldBeTrue();

			_service2.Running.IsCompleted.ShouldBeTrue();
			_service2.Stopped.IsCompleted.ShouldBeFalse();
		}

		TestService _service;
		TestService2 _service2;
		ServiceCoordinator _serviceCoordinator;
	}
}