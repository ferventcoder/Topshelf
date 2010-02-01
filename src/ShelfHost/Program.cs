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
	using System.Reflection;
	using Magnum;
	using Topshelf.Model.ApplicationDomain;
	using Topshelf.Model.Shelving;

	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Starting up...");

			ShelvedServiceInfo[] services = GetHostedServices();
			services.Each(x =>
				{
					Console.WriteLine("STarting service:" + x.InferredName);
					StartService(x);
				});


			Console.WriteLine("Hit it...");
			Console.ReadLine();
		}

		static void StartService(ShelvedServiceInfo info)
		{
			var bundle = AppDomainFactory.CreateNewAppDomain(info, GetCachePath());

			bundle.Controller.Start();

			Console.WriteLine("Service was started: " + info.InferredName);

			bundle.Controller.Stop();

			Console.WriteLine("Service was stopped: " + info.InferredName);
		}

		static ShelvedServiceInfo[] GetHostedServices()
		{
			string servicePath = GetServicesPath();
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

		static string GetServicesPath()
		{
			string hostPath = GetHostPath();

			string serviceFolderName = "Services";
			return Path.Combine(Path.GetDirectoryName(hostPath), serviceFolderName);
		}

		static string GetCachePath()
		{
			string hostPath = GetHostPath();

			string cacheFolderName = "AssemblyCache";
			return Path.Combine(Path.GetDirectoryName(hostPath), cacheFolderName);
		}

		static string GetHostPath()
		{
			string hostPath = Assembly.GetExecutingAssembly().Location;
			Console.WriteLine("Host Path: " + hostPath);
			return hostPath;
		}
	}

	public class HostedService
	{
		public HostedService(string path)
		{
			Console.WriteLine("Service found at: " + path);
		}
	}
}