// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	public static class PlatformRegistry
	{
		private static Dictionary<string, BaseArchitecture> Registry = new Dictionary<string, BaseArchitecture>();

		public static void Register()
		{
			Registry = new Dictionary<string, BaseArchitecture>();

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.GetTypes())
				{
					if (!type.IsAbstract && typeof(BaseArchitecture).IsAssignableFrom(type))
					{
						var platform = (BaseArchitecture)Activator.CreateInstance(type);
						Registry.Add(platform.PlatformName, platform);
					}
				}
			}
		}

		public static void Add(BaseArchitecture platform)
		{
			Registry.Add(platform.PlatformName, platform);
		}

		public static BaseArchitecture GetPlatform(string platformName)
		{
			Registry.TryGetValue(platformName, out BaseArchitecture platform);

			return platform;
		}
	}
}
