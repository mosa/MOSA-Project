/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using IL = Mosa.Runtime.CompilerFramework.IL;
using IR = Mosa.Runtime.CompilerFramework.IR;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// x86 specific implementation of the prologue instruction.
    /// </summary>
    sealed class PrologueInstruction : IR.PrologueInstruction
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="PrologueInstruction"/>.
        /// </summary>
        /// <param name="stackSize">The stack size requirements of the method.</param>
        public PrologueInstruction(int stackSize) :
            base(stackSize)
        {
        }

        #endregion // Construction

        #region IR.PrologueInstruction Overrides

        public override object Expand(MethodCompilerBase methodCompiler)
        {
            IArchitecture architecture = methodCompiler.Architecture;

            SigType I = new SigType(CilElementType.I);
            RegisterOperand ebp = new RegisterOperand(I, GeneralPurposeRegister.EBP);
            RegisterOperand esp = new RegisterOperand(I, GeneralPurposeRegister.ESP);

            return new Instruction[] {
                /* If you want to stop at the header of an emitted function, just uncomment
                 * the following line. It will issue a breakpoint instruction. Note that if
                 * you debug using visual studio you must enable unmanaged code debugging, 
                 * otherwise the function will never return and the breakpoint will never
                 * appear.
                 */
                // int 3
                //architecture.CreateInstruction(typeof(IL.BreakInstruction), IL.OpCode.Break),
                // push ebp
                architecture.CreateInstruction(typeof(IR.PushInstruction), ebp),
                // mov ebp, esp
                architecture.CreateInstruction(typeof(IR.MoveInstruction), ebp, esp),
                // sub esp, localsSize
                architecture.CreateInstruction(typeof(IL.SubInstruction), IL.OpCode.Sub, esp, new ConstantOperand(I, -this.StackSize)),

                /*
                 * This move adds the runtime method identification token onto the stack. This
                 * allows us to perform call stack identification and gives the garbage collector 
                 * the possibility to identify roots into the managed heap. 
                 */
                // mov [ebp-4], token
                architecture.CreateInstruction(typeof(IR.MoveInstruction), new MemoryOperand(I, GeneralPurposeRegister.EBP, new IntPtr(-4)), new ConstantOperand(I, methodCompiler.Method.Token))
            };
        }

        #endregion // IR.PrologueInstruction Overrides
    }
}
