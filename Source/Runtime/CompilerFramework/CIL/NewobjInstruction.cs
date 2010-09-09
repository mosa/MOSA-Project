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
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata.Tables;

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
		protected override InvokeSupportFlags InvokeSupport
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
		/// <param name="typeSystem">The type system.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder, ITypeSystem typeSystem)
		{
			TokenTypes ctor = DecodeInvocationTarget(ctx, decoder, InvokeSupport, typeSystem);

			/*
			 * HACK: We need to remove the this parameter from the operand list, as it
			 * is not available yet. It is implicitly created by newobj and appropriately
			 * passed. So we do as if it doesn't exist. Upon instruction expansion a call
			 * to the allocator is inserted and its result is the this pointer passed. This
			 * must be done by expansion though...
			 * 
			 */

			// Remove the this argument from the invocation, it's not on the stack yet.
			ctx.OperandCount--;

			// Get the type to allocate
			SigType sigType = this.CreateSignatureTypeFor(decoder.Compiler.Assembly, ctor, ctx.InvokeTarget.DeclaringType);

			// Set a return value according to the type of the object allocated
			ctx.Result = decoder.Compiler.CreateTemporary(sigType);
			ctx.ResultCount = 1;
		}

		private SigType CreateSignatureTypeFor(IMetadataModule module, TokenTypes ctorToken, RuntimeType declaringType)
		{
			TokenTypes typeToken = (TokenTypes)declaringType.Token;
			if (IsMemberRef(ctorToken) == true)
			{
				typeToken = GetLocalTypeRefToken(module, ctorToken);
			}

			RuntimeType baseType = declaringType.BaseType;
			if (baseType != null && baseType.FullName == @"System.ValueType")
			{
				return new ValueTypeSigType(typeToken);
			}

			return new ClassSigType(typeToken);
		}

		private static TokenTypes GetLocalTypeRefToken(IMetadataModule module, TokenTypes ctorToken)
		{
			MemberRefRow memberRef = module.Metadata.ReadMemberRefRow(ctorToken);
			return memberRef.ClassTableIdx;
		}

		private static bool IsMemberRef(TokenTypes ctorToken)
		{
			return (ctorToken & TokenTypes.MemberRef) == TokenTypes.MemberRef;
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(Context ctx, IMethodCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			// Validate the operands...
			int offset = (ctx.InvokeTarget.Signature.HasExplicitThis ? 1 : 0);
			Debug.Assert(ctx.OperandCount == ctx.InvokeTarget.Parameters.Count - offset, @"Operand count doesn't match parameter count.");
			for (int i = 0; i < ctx.OperandCount; i++)
			{
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

		#endregion Methods


	}
}
