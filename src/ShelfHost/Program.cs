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
	using System.IO;
	using System.Linq;
	using Commands;
	using Magnum;
	using Magnum.Pipeline;
	using Magnum.Pipeline.Segments;
	using Topshelf.Events;
	using Topshelf.Model.Shelving;

	internal class Program
	{
		static IHostConfiguration _configuration;

		public static Pipe EventAggregator { get; private set; }

		static void Main(string[] args)
		{
			EventAggregator = PipeSegment.New();

			_configuration = new HostConfiguration();

			var coordinator = new ShelvedServiceCoordinator(EventAggregator, _configuration);
			coordinator.Start();

			Console.WriteLine("Starting up...");

			ShelvedServiceInfo[] services = GetHostedServices();
			services.Each(x =>
				{
					var message = new NewServiceFoundImpl
						{
							ServicePath = x.FullPath
						};

					EventAggregator.Send(message);
				});

			// TODO file system watcher for .config changes

			Console.WriteLine("Hit it...");
			Console.ReadLine();

			Console.WriteLine("Shutting down...");
			EventAggregator.Send(new ShutdownHostImpl
				{
					Reason = "Service Host Exiting",
				});

			coordinator.Dispose();
			coordinator = null;

			Console.WriteLine("Program complete.");
			Console.ReadLine();
		}


		static ShelvedServiceInfo[] GetHostedServices()
		{
			string servicePath = _configuration.ServicesPath;
			Console.WriteLine("Opening service folder: " + servicePath);

			if (!Directory.Exists(servicePath))
			{
				Console.WriteLine("No service folder found");
				return new ShelvedServiceInfo[] {};
			}

			return Directory.GetDirectories(servicePath)
				.Select(path =>
					{
						Console.WriteLine("Service found at: " + path);

						return new ShelvedServiceInfo(path);
					})
				.ToArray();
		}
	}


	public interface ILogger
	{
		bool IsDebugEnabled { get; }
		void Debug(string message);
	}


	public interface ILogProvider
	{
		ILogger GetLogger(string name);
	}

	public enum LogLevel
	{
		Debug,
		Info,
		Warn,
		Error,
		Fatal,
	}

	public class Logger
	{
		readonly ILogger _logger;
		static readonly ILogProvider _logProvider;

		public Logger(string name)
		{
			_logger = _logProvider.GetLogger(name);
		}

		public void Debug(Action<ILogger> logAction)
		{
			if (_logger.IsDebugEnabled)
				logAction(_logger);
		}

		public static Logger GetLogger(string name)
		{
			return new Logger(name);
		}

		public void Debug(string message)
		{
			if (_logger.IsDebugEnabled)
				_logger.Debug(message);
		}
	}
}