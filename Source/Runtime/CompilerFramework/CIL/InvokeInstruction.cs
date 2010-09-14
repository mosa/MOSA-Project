/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
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
	public abstract class InvokeInstruction : BaseInstruction
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

		#endregion // Types

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InvokeInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public InvokeInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

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
		public override FlowControl FlowControl
		{
			get { return FlowControl.Call; }
		}

		/// <summary>
		/// Gets the supported immediate metadata tokens in the instruction.
		/// </summary>
		protected abstract InvokeSupportFlags InvokeSupport
		{
			get;
		}

		#endregion // Properties

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
		public override void Validate(Context ctx, IMethodCompiler compiler)
		{
			base.Validate(ctx, compiler);

			int paramCount = ctx.InvokeTarget.Signature.Parameters.Length;

			if (ctx.InvokeTarget.Signature.HasThis && !ctx.InvokeTarget.Signature.HasExplicitThis)
				paramCount++;

			// Validate the operands...
			Debug.Assert(ctx.OperandCount == paramCount, @"Operand count doesn't match parameter count.");

			for (int i = 0; i < ctx.OperandCount; i++)
			{
				/* FIXME: Check implicit conversions
				// if (ops[i] != null) {
					Debug.Assert(_operands[i].Type == _parameterTypes[i]);
					if (_operands[i].Type != _parameterTypes[i])
					{
						// FIXME: Determine if we can do an implicit conversion
						throw new ExecutionEngineException(@"Invalid operand types.");
					}
				*/
			}
		}

		/// <summary>
		/// Decodes the invocation target.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The IL decoder, which provides decoding functionality.</param>
		/// <param name="flags">Flags, which control the</param>
		/// <returns></returns>
		protected static TokenTypes DecodeInvocationTarget(Context ctx, IInstructionDecoder decoder, InvokeSupportFlags flags)
		{
			// Retrieve the immediate argument - it contains the token
			// of the methoddef, methodref, methodspec or callsite to call.
			TokenTypes callTarget = decoder.DecodeTokenType();
			TokenTypes targetType = (TokenTypes.TableMask & callTarget);

			if (!IsCallTargetSupported(targetType, flags))
				throw new InvalidOperationException(@"Invalid IL call target specification.");

			RuntimeMethod method = null;

			switch (targetType)
			{
				case TokenTypes.MethodDef:
					method = decoder.ModuleTypeSystem.GetMethod(decoder.Method, callTarget);
					break;

				case TokenTypes.MemberRef:
					method = decoder.ModuleTypeSystem.GetMethod(decoder.Method, callTarget);
					if (method.DeclaringType.IsGeneric == true)
					{
						ScheduleMethodForCompilation(decoder, method);
					}
					break;

				case TokenTypes.MethodSpec:
					method = DecodeMethodSpecification(decoder, callTarget);
					break;

				default:
					Debug.Assert(false, @"Should never reach this!");
					break;
			}

			SetInvokeTarget(ctx, decoder.Compiler, method);

			return callTarget;
		}

		private static RuntimeMethod DecodeMethodSpecification(IInstructionDecoder decoder, TokenTypes callTarget)
		{
			RuntimeMethod method = decoder.ModuleTypeSystem.GetMethod(decoder.Method, callTarget);

			ScheduleMethodForCompilation(decoder, method);

			return method;
		}

		private static void ScheduleMethodForCompilation(IInstructionDecoder decoder, RuntimeMethod method)
		{
			ICompilationSchedulerStage compilationScheduler = decoder.Compiler.Scheduler;
			compilationScheduler.ScheduleMethodForCompilation(method);

		}

		/// <summary>
		/// Sets the invoke target.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		/// <param name="method">The method.</param>
		private static void SetInvokeTarget(Context ctx, IMethodCompiler compiler, RuntimeMethod method)
		{
			if (method == null)
				throw new ArgumentNullException(@"method");

			// Signature of the call target
			// Number of parameters required for the call

			ctx.InvokeTarget = method;

			// Retrieve the target signature
			MethodSignature signature = ctx.InvokeTarget.Signature;

			// Fix the parameter list
			byte paramCount = (byte)signature.Parameters.Length;
			if (signature.HasThis && !signature.HasExplicitThis)
				paramCount++;

			// Setup operands for parameters and the return value
			if (signature.ReturnType.Type != CilElementType.Void)
			{
				ctx.ResultCount = 1;
				ctx.Result = compiler.CreateTemporary(signature.ReturnType);
			}
			else
				ctx.ResultCount = 0;

			ctx.OperandCount = paramCount;
		}

		/// <summary>
		/// Determines whether [is call target supported] [the specified target type].
		/// </summary>
		/// <param name="targetType">Type of the target.</param>
		/// <param name="flags">The flags.</param>
		/// <returns>
		/// 	<c>true</c> if [is call target supported] [the specified target type]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsCallTargetSupported(TokenTypes targetType, InvokeSupportFlags flags)
		{
			bool result = false;

			if (TokenTypes.MethodDef == targetType && InvokeSupportFlags.MethodDef == (flags & InvokeSupportFlags.MethodDef))
				result = true;
			else if (TokenTypes.MemberRef == targetType && InvokeSupportFlags.MemberRef == (flags & InvokeSupportFlags.MemberRef))
				result = true;
			else if (TokenTypes.MethodSpec == targetType && InvokeSupportFlags.MethodSpec == (flags & InvokeSupportFlags.MethodSpec))
				result = true;

			return result;
		}

		/// <summary>
		/// Finds the invoke overload.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="ownerType">Type of the owner.</param>
		/// <param name="nameIdx">The name idx.</param>
		/// <param name="signatureIdx">The signature idx.</param>
		/// <returns></returns>
		private object FindInvokeOverload(IMetadataProvider metadata, SigType ownerType, TokenTypes nameIdx, TokenTypes signatureIdx)
		{
			throw new NotImplementedException();
			/*
				MethodDefinition result = null;
				TypeDefinition elementTypeDef = ownerType.ElementType as TypeDefinition;
				string name;
				Debug.Assert(null != elementTypeDef, @"Cross assembly type resolution not supported yet.");
				if (null == elementTypeDef)
				{
					// FIXME: Resolve the reference using all referenced assemblies
					throw new InvalidOperationException(@"Cross assembly type resolution not supported yet.");
				}

				metadata.Read(nameIdx, out name);

				foreach (MethodDefinition methodDef in elementTypeDef.Methods)
				{
					if (methodDef.Name.Equals(name))
					{
						// FIXME: Check the signatures...
						if (IsSameSignature(metadata, methodDef.SignatureIdx, signatureIdx))
						{
							// We've found the method
							//result = temp;
							//result.OwnerType = ownerType;
							result = methodDef;
							break;
						}

					}
				}
	
				return result;
			 */
		}

		/// <summary>
		/// Determines whether [is same signature] [the specified metadata].
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="sig1">The sig1.</param>
		/// <param name="sig2">The sig2.</param>
		/// <returns>
		/// 	<c>true</c> if [is same signature] [the specified metadata]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsSameSignature(IMetadataProvider metadata, TokenTypes sig1, TokenTypes sig2)
		{
			byte[] src = metadata.ReadBlob(sig1);
			byte[] dst = metadata.ReadBlob(sig2);
			bool result = (src.Length == dst.Length);
			if (result)
			{
				for (int i = 0; result && i < src.Length; i++)
					result = (src[i] == dst[i]);
			}
			return result;
		}

		#endregion // Methods
	}
}
