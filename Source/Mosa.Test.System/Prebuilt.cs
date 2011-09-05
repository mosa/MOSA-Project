using System;
using System.Reflection;

namespace Mosa.Test.System
{
	public static class Prebuilt
	{
		private static Assembly prebuilt = LoadPrebuiltDelegateAssembly();

		private static Assembly LoadPrebuiltDelegateAssembly()
		{
			if (prebuilt == null)
			{
				prebuilt = Assembly.LoadFrom("Mosa.Test.Prebuilt.dll");
			}

			return prebuilt;
		}

		public static Type GetType(string name)
		{
			return prebuilt.GetType(name);
		}
	}
}
