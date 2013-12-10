/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.TypeSystem.Cil;
using System;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.CIL
{
	/* FIXME:
	 * - Schedule compilation of invocation target
	 * - Scheduling puts the target method on the jit or aot compilers work list
	 * - This allows the jit to run async ahead of time in the same process
	 * - This may not turn the jit into a full aot, but is used to prepare MethodCompilers and jit stubs
	 *   for invoked methods.
	 */

	/// <summary>
	/// Base class for instructions, which invoke other functions.
	/// </summary>
	public abstract class InvokeInstruction : BaseCILInstruction
	{
		#region Types

		/// <summary>
		/// Specifies a set of flags used to control invocation target metadata decoding.
		/// </summary>
		[Flags]
		protected enum InvokeSupportFlags
		{
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
		/// Initializes a new instance of the <see cref="InvokeInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public InvokeInstruction(OpCode opcode)
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
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			DecodeInvocationTarget(ctx, decoder, InvokeSupport);
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Resolve(Context ctx, BaseMethodCompiler compiler)
		{
			base.Resolve(ctx, compiler);
		}

		/// <summary>
		/// Decodes the invocation target.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The IL decoder, which provides decoding functionality.</param>
		/// <param name="flags">Flags, which control the</param>
		/// <returns></returns>
		protected static Token DecodeInvocationTarget(Context ctx, IInstructionDecoder decoder, InvokeSupportFlags flags)
		{
			// Retrieve the immediate argument - it contains the token
			// of the methoddef, methodref, methodspec or callsite to call.
			Token callTarget = decoder.DecodeTokenType();

			if (!IsCallTargetSupported(callTarget.Table, flags))
				throw new InvalidOperationException(@"Invalid IL call target specification.");

			ITypeModule module = decoder.Method.Module;
			RuntimeMethod method = null;

			switch (callTarget.Table)
			{
				case TableType.MethodDef:
					method = module.GetMethod(callTarget);
					decoder.Compiler.Scheduler.TrackMethodInvoked(method);
					break;

				case TableType.MemberRef:
					method = module.GetMethod(callTarget, decoder.Method.DeclaringType);
					if (method.DeclaringType.IsGeneric)
						decoder.Compiler.Scheduler.TrackMethodInvoked(method);
					break;

				case TableType.MethodSpec:
					method = module.GetMethod(callTarget);
					decoder.Compiler.Scheduler.TrackMethodInvoked(method);
					break;

				default:
					Debug.Assert(false, @"Should never reach this!");
					break;
			}

			if (method.DeclaringType.ContainsOpenGenericParameters)
			{
				method = decoder.GenericTypePatcher.PatchMethod(method.DeclaringType.Module, decoder.Method.DeclaringType as CilGenericType, method);
				decoder.Compiler.Scheduler.TrackMethodInvoked(method);
			}

			SetInvokeTarget(ctx, decoder.Compiler, method);

			return callTarget;
		}

		/// <summary>
		/// Sets the invoke target.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="compiler">The compiler.</param>
		/// <param name="method">The method.</param>
		private static void SetInvokeTarget(Context context, BaseMethodCompiler compiler, RuntimeMethod method)
		{
			if (method == null)
				throw new ArgumentNullException(@"method");

			context.InvokeMethod = method;

			// Fix the parameter list
			int paramCount = method.SigParameters.Length;

			if (method.HasThis && !method.HasExplicitThis)
				paramCount++;

			// Setup operands for parameters and the return value
			if (!method.ReturnType.IsVoid)
			{
				context.ResultCount = 1;
				context.Result = compiler.CreateVirtualRegister(Operand.NormalizeSigType(method.ReturnType));
			}
			else
				context.ResultCount = 0;

			context.OperandCount = (byte)paramCount;
		}

		/// <summary>
		/// Determines whether [is call target supported] [the specified target type].
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="flags">The flags.</param>
		/// <returns>
		/// 	<c>true</c> if [is call target supported] [the specified target type]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsCallTargetSupported(TableType targetType, InvokeSupportFlags flags)
		{
			bool result = false;

			if (targetType == TableType.MethodDef && InvokeSupportFlags.MethodDef == (flags & InvokeSupportFlags.MethodDef))
				result = true;
			else if (targetType == TableType.MemberRef && InvokeSupportFlags.MemberRef == (flags & InvokeSupportFlags.MemberRef))
				result = true;
			else if (targetType == TableType.MethodSpec && InvokeSupportFlags.MethodSpec == (flags & InvokeSupportFlags.MethodSpec))
				result = true;

			return result;
		}

		#endregion Methods
	}
}