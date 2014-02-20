/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class CallvirtInstruction : InvokeInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CallvirtInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public CallvirtInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the supported immediate metadata tokens in the instruction.
		/// </summary>
		/// <value></value>
		protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
		{
			get { return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef | InvokeSupportFlags.MethodSpec; }
		}

		#endregion Properties

		#region Methods

		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			if (ctx.HasPrefix && ctx.Previous.Instruction is ConstrainedPrefixInstruction)
			{
				MosaMethod toBeReplaced = HandleConstrained(ctx, decoder, ctx.Previous);
				base.Decode(ctx, decoder);
				if (toBeReplaced != null)
				{
					ctx.MosaMethod = toBeReplaced;
					ctx.ReplaceInstructionOnly(CILInstruction.Instructions[(int)OpCode.Call]);
				}
			}
			else
				base.Decode(ctx, decoder);
		}

		MosaMethod HandleConstrained(Context ctx, IInstructionDecoder decoder, Context constrained)
		{
			var type = constrained.MosaType;
			var method = (MosaMethod)decoder.Instruction.Operand;

			ctx.MosaType = type;
			if (type.IsValueType)
			{
				MosaMethod implMethod = null;
				// HACK: Should use proper override searching
				// Note that although Partition III 2.1 stated that the third case will only occur in these types
				// MethodImpl can change the name of override method and should do a proper override searching
				if (method.DeclaringType.Module == decoder.TypeSystem.CorLib &&
					(method.DeclaringType.FullName == "System.Object" ||
					method.DeclaringType.FullName == "System.ValueType" ||
					method.DeclaringType.FullName == "System.Enum") &&
					(implMethod = type.FindMethodBySignature(method.Name, method.Signature)) == null)
				{
					// Third case: dereference and box

					constrained.SetInstruction(CILInstruction.Instructions[(int)OpCode.Ldobj]);
					constrained.Result = LoadInstruction.CreateResultOperand(decoder, type);
					constrained.MosaType = type;

					constrained.AppendInstruction(CILInstruction.Instructions[(int)OpCode.Box], decoder.Compiler.CreateVirtualRegister(decoder.TypeSystem.BuiltIn.Object));
					constrained.MosaType = type;
				}
				else if (implMethod != null)
				{
					// Second case: unmodified, change callvirt target method
					constrained.Delete(true);
					return implMethod;
				}
				else
					throw new CompilerException();
			}
			else
			{
				// First case: Dereference

				constrained.SetInstruction(CILInstruction.Instructions[(int)OpCode.Ldobj]);
				constrained.Result = LoadInstruction.CreateResultOperand(decoder, type);
				constrained.MosaType = type;
			}
			return null;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Callvirt(context);
		}

		#endregion Methods
	}
}