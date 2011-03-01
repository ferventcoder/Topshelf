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
	using System.Diagnostics;
	using Magnum.Channels.Configuration;
	using Magnum.Extensions;


	public static class AddressRegistry
	{
		public static Uri ServiceCoordinatorAddress
		{
			get { return GetServiceUri().AppendPath("Coordinator"); }
		}


		public static string ServiceCoordinatorPipeName
		{
			get { return "{0}/ServiceCoordinator".FormatWith(GetPid()); }
		}

		static int GetPid()
		{
			return Process.GetCurrentProcess().Id;
		}

		static Uri GetServiceUri()
		{
			return new Uri("net.pipe://localhost/topshelf/{0}".FormatWith(GetPid()));
		}

		public static Uri GetShelfServiceAddress(AppDomain appDomain)
		{
			return GetServiceUri().AppendPath("Shelf").AppendPath(appDomain.FriendlyName);
		}

		public static Uri GetServiceAddress(string serviceName)
		{
			return GetServiceUri().AppendPath("Service").AppendPath(serviceName);
		}

		public static string GetShelfServicePipeName(AppDomain appDomain)
		{
			return "{0}/Shelf/{1}".FormatWith(GetPid(), appDomain.FriendlyName);
		}

		public static string GetServicePipeName(string serviceName)
		{
			return "{0}/Shelf/{1}".FormatWith(GetPid(), serviceName);
		}

		public static OutboundChannel GetOutboundCoordinatorChannel()
		{
			return new OutboundChannel(ServiceCoordinatorAddress, ServiceCoordinatorPipeName);
		}

		public static InboundChannel GetInboundServiceCoordinatorChannel(Action<ConnectionConfigurator> cfg)
		{
			return new InboundChannel(ServiceCoordinatorAddress, ServiceCoordinatorPipeName, cfg);
		}

		public static InboundChannel GetInboundServiceChannel(AppDomain appDomain, Action<ConnectionConfigurator> cfg)
		{
			return new InboundChannel(GetShelfServiceAddress(appDomain), GetShelfServicePipeName(appDomain), cfg);
		}

		public static InboundChannel GetInboundServiceChannel(string serviceName, Action<ConnectionConfigurator> cfg)
		{
			return new InboundChannel(GetServiceAddress(serviceName), GetServicePipeName(serviceName), cfg);
		}
	}
}