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
namespace Topshelf.FileSystem
{
	using System;
	using System.IO;
	using System.Linq;
	using Magnum.Extensions;
	using Magnum.FileSystem.Events;
	using Messages;
	using Model;
	using Stact;
	using Stact.Internal;


	public class DirectoryMonitor :
		IDisposable
	{
		readonly string _baseDirectory;
		readonly PoolFiber _fiber;
		ChannelAdapter _channel;
		ChannelConnection _connection;
		bool _disposed;
		PollingFileSystemEventProducer _producer;
		Scheduler _scheduler;
		IServiceChannel _serviceChannel;

		public DirectoryMonitor(string directory, IServiceChannel serviceChannel)
		{
			_baseDirectory = directory;
			_serviceChannel = serviceChannel;
			_fiber = new PoolFiber();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~DirectoryMonitor()
		{
			Dispose(false);
		}

		void Dispose(bool disposing)
		{
			if (_disposed)
				return;
			if (disposing)
			{
				if (_scheduler != null)
					_scheduler.Stop(30.Seconds());

				_fiber.Shutdown(30.Seconds());

				if (_producer != null)
					_producer.Dispose();

				_serviceChannel = null;
			}

			_disposed = true;
		}

		public void Start()
		{
			// file system watcher will fail if directory isn't there, ensure it is
			if (!Directory.Exists(_baseDirectory))
				Directory.CreateDirectory(_baseDirectory);

			_scheduler = new TimerScheduler(new PoolFiber());
			_channel = new ChannelAdapter();

			_producer = new PollingFileSystemEventProducer(_baseDirectory, _channel, _scheduler, new PoolFiber(),
			                                               2.Minutes());

			_connection = _channel.Connect(config =>
				{
					config
						.AddConsumerOf<FileSystemEvent>()
						.BufferFor(3.Seconds())
						.UseScheduler(_scheduler)
						.Distinct(fsEvent => GetChangedDirectory(fsEvent.Path))
						.UsingConsumer(fsEvents =>
							{
								fsEvents.Keys.Distinct().Each(key =>
									{
										if (key == _baseDirectory)
											return;

										_serviceChannel.Send(new ServiceFolderChanged(key));
									});
							})
						.HandleOnFiber(_fiber);
				});
		}

		public void Stop()
		{
			if (_producer != null)
			{
				_producer.Dispose();
				_producer = null;
			}

			if (_scheduler != null)
			{
				_scheduler.Stop(30.Seconds());
				_scheduler = null;
			}

			if (_connection != null)
			{
				_connection.Dispose();
				_connection = null;
			}

			_channel = null;
		}

		/// <summary>
		///   Normalize the source of the event; we only care about the directory in question
		/// </summary>
		string GetChangedDirectory(string eventItem)
		{
			return eventItem.Substring(_baseDirectory.Length)
				.Split(Path.DirectorySeparatorChar)
				.Where(x => x.Length > 0)
				.FirstOrDefault();
		}
	}
}