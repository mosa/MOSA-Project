/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;
using Mosa.TinyCPUSimulator.Adaptor;

namespace Mosa.TinyCPUSimulator.TestSystem
{
	public abstract class BasePlatform
	{
		public string Name { get; set; }

		public BasePlatform(string platform)
		{
			Name = platform;
		}

		public abstract IArchitecture CreateArchitecture();

		public abstract ISimAdapter CreateSimAdaptor();

		public abstract void InitializeSimulation(ISimAdapter simAdapter);

		public abstract void ResetSimulation(ISimAdapter simAdapter);

		public abstract void PopulateStack(ISimAdapter simAdapter, object parameter);

		public abstract void PopulateStack(ISimAdapter simAdapter, params object[] parameters);

		public abstract void PrepareToExecuteMethod(ISimAdapter simAdapter, ulong address);

		//public abstract T GetResult<T>(ISimAdapter simAdapter);

		public abstract object GetResult(ISimAdapter simAdapter, CilElementType cilElementType);
	}
}