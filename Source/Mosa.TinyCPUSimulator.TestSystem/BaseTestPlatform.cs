// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.TinyCPUSimulator.Adaptor;

namespace Mosa.TinyCPUSimulator.TestSystem
{
	public abstract class BaseTestPlatform
	{
		public string Name { get; private set; }

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
