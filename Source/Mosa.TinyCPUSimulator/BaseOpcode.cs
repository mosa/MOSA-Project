/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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