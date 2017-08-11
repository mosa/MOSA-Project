// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Base class for instructions, which invoke other functions.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.BaseCILInstruction" />
	public abstract class InvokeInstruction : BaseCILInstruction
	{
		#region Types

		/// <summary>
		/// Specifies a set of flags used to control invocation target metadata decoding.
		/// </summary>
		[Flags]
		protected enum InvokeSupportFlags
		{
			None = 0,

			/// <summary>
			/// Specifies that the invoke instruction supports member references.
			/// </summary>
			MemberRef = 1,

			/// <summary>
			/// Specifies that the invoke instruction supports member definitions.
			/// </summary>
			MethodDef = 2,

			/// <summary>
			/// Specifies that the invoke instruction supports member specifications.
			/// </summary>
			MethodSpec = 4,

			/// <summary>
			/// Specifies that the invoke instruction supports call site invocations.
			/// </summary>
			CallSite = 8,

			/// <summary>
			/// Specifies support for all method invocation targets.
			/// </summary>
			All = MemberRef | MethodDef | MethodSpec | CallSite
		}

		#endregion Types

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InvokeInstruction" /> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		protected InvokeInstruction(OpCode opcode)
			: base(opcode, 0)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Determines flow behavior of this instruction.
		/// </summary>
		/// <value></value>
		/// <remarks>
		/// Knowledge of control flow is required for correct basic block
		/// building. Any instruction that alters the control flow must override
		/// this property and correctly identify its control flow modifications.
		/// </remarks>
		public override FlowControl FlowControl { get { return FlowControl.Call; } }

		/// <summary>
		/// Gets the supported immediate metadata tokens in the instruction.
		/// </summary>
		protected abstract InvokeSupportFlags InvokeSupport { get; }

		#endregion Properties

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(InstructionNode node, IInstructionDecoder decoder)
		{
			DecodeInvocationTarget(node, decoder);
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Resolve(Context context, BaseMethodCompiler compiler)
		{
			base.Resolve(context, compiler);
		}

		/// <summary>
		/// Decodes the invocation target.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The IL decoder, which provides decoding functionality.</param>
		/// <returns></returns>
		protected static MosaMethod DecodeInvocationTarget(InstructionNode ctx, IInstructionDecoder decoder)
		{
			var method = (MosaMethod)decoder.Instruction.Operand;

			decoder.Compiler.Scheduler.TrackMethodInvoked(method);

			SetInvokeTarget(ctx, decoder.Compiler, method);

			return method;
		}

		/// <summary>
		/// Sets the invoke target.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="compiler">The compiler.</param>
		/// <param name="method">The method.</param>
		private static void SetInvokeTarget(InstructionNode context, BaseMethodCompiler compiler, MosaMethod method)
		{
			context.InvokeMethod = method;

			// Fix the parameter list
			int paramCount = method.Signature.Parameters.Count;

			if (method.HasThis && !method.HasExplicitThis)
				paramCount++;

			// Setup operands for parameters and the return value
			if (!method.Signature.ReturnType.IsVoid)
			{
				context.ResultCount = 1;

				if (MosaTypeLayout.IsStoredOnStack(method.Signature.ReturnType))
				{
					context.Result = AllocateVirtualRegisterOrStackSlot(compiler, method.Signature.ReturnType);
				}
				else
				{
					context.Result = compiler.CreateVirtualRegister(method.Signature.ReturnType.GetStackType());
				}
			}
			else
			{
				context.ResultCount = 0;
			}

			context.OperandCount = (byte)paramCount;
		}

		#endregion Methods
	}
}
