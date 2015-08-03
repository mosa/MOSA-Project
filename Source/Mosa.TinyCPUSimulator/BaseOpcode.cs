// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator
{
	public enum OpcodeFlowType { Normal, Call, Jump, Branch, Return };

	public class BaseOpcode
	{
		public virtual void Execute(SimCPU cpu, SimInstruction instruction)
		{
		}

		public override string ToString()
		{
			return this.GetType().Name;
		}

		public virtual OpcodeFlowType FlowType { get { return OpcodeFlowType.Normal; } }
	}
}