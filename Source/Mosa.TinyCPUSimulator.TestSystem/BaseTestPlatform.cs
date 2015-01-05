/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.TinyCPUSimulator.Adaptor;

namespace Mosa.TinyCPUSimulator.TestSystem
{
	public abstract class BaseTestPlatform
	{
		public string Name { get; set; }

		public BaseTestPlatform(string platform)
		{
			Name = platform;
		}

		public abstract BaseArchitecture CreateArchitecture();

		public abstract ISimAdapter CreateSimAdaptor();

		public abstract void InitializeSimulation(ISimAdapter simAdapter);

		public abstract void ResetSimulation(ISimAdapter simAdapter);

		public abstract void PrepareToExecuteMethod(ISimAdapter simAdapter, ulong address, params object[] parameters);

		public abstract object GetResult(ISimAdapter simAdapter, MosaType type);
	}
}
