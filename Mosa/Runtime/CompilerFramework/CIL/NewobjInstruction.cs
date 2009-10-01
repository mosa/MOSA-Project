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

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// Intermediate representation of the newobj IL instruction.
	/// </summary>
	/// <remarks>
	/// Actually this is a waste. Newobj is a compound of at least three instructions:
	///   - pop ctor-args
	///   - push type
	///   - push type-size
	///   - call allocator.new
	///   - dup
	///   - push ctor-args
	///   - call ctor
	/// Note that processing this instruction does require extensive call stack rewriting in order
	/// to insert the this reference in front of all other ctor arguments, even though it is pushed
	/// *after* calling allocator new as seen above. Additionally note that after executing the ctor
	/// call another reference to this is on the stack in order to be able to use the constructed object.
	/// Note that this is very similar to arrays (newarr), except there's no ctor to call.
	/// I don't want to have runtime helpers for newarr and newobj, so we unite both by using a common
	/// allocator, which receives the type and memory size as parameters. This also fixes string
	/// issues for us, which vary in size and thus can't be allocated by a plain newobj.
	/// <para/>
	/// These details are automatically processed by the Expand function, which expands this highlevel
	/// opcode into its parts as described above. The exception is that Expand is not stack based anymore
	/// and uses virtual registers to implement two calls:
	/// - this-vreg = allocator-new(type-vreg, type-size-vreg)
	/// - ctor(this-vreg[, args])
	/// <para/>
	/// Those calls are ultimately processed by further expansion and inlining, except that allocator-new
	/// is a kernel call and can't be inlined - even by the jit.
	/// <para/>
	/// The expansion essentially adds a dependency to mosacor, which provides the allocator and gc.
	/// </remarks>
	public sealed class NewobjInstruction : InvokeInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NewobjInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public NewobjInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the supported immediate metadata tokens in the instruction.
		/// </summary>
		/// <value></value>
		protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
		{
			get
			{
				return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef;
			}
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
			// Decode base classes first
			base.Decode(ctx, decoder);

			/*
             * HACK: We need to remove the this parameter From the operand list, as it
             * is not available yet. It is implicitly created by newobj and appropriately
             * passed. So we do as if it doesn't exist. Upon instruction expansion a call
             * to the allocator is inserted and its result is the this pointer passed. This
             * must be done by expansion though...
             * 
             */

			// Remove the this parameter
			// FIXME: _operands = new Operand[_operands.Length-1];

			// Set the return value, even though constructors return void
			throw new NotImplementedException();
			//SetResult(0, null);
			//CreateResultOperand(_invokeTarget.DeclaringType)	
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(Context ctx, IMethodCompiler compiler)
		{
			base.Validate(ctx, compiler);

			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			// HACK: Don't validate the base class - it still assumes a method call without the this ptr required
			// for constructors.
			//base.Validate(compiler);

			// Validate the operands...
			Debug.Assert(ctx.OperandCount == ctx.InvokeTarget.Parameters.Count - 1, @"Operand count doesn't match parameter count.");
			for (int i = 0; i < ctx.OperandCount; i++) {
				/* FIXME: Check implicit conversions
					if (ops[i] != null) {
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
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Newobj(context);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context ctx)
		{
			StringBuilder builder = new StringBuilder();
			//            int opIdx = 0;

			// Output the result...
			builder.AppendFormat("{0} = newobj ", ctx.Result);

			// Output the call name
			builder.Append(ctx.InvokeTarget);
			/*
				if (opIdx < _operands.Length)
				{
					builder.Append('(');

					while (opIdx < _operands.Length)
					{
						builder.AppendFormat("{0}, ", _operands[opIdx++]);
					}

					builder.Remove(builder.Length - 2, 2);
					builder.Append(')');
				}
				else
					builder.Append("()");
			*/
			return builder.ToString();
		}

		#endregion Methods


	}
}
