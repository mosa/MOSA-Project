using System;
using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Platform.ARMv6.Stages
{
    /// <summary>
    /// Transforms IR instructions into their appropriate ARMv6.
    /// </summary>
    /// <remarks>
    /// This transformation stage transforms IR instructions into their equivalent ARMv6 sequences.
    /// </remarks>
    public sealed class IRTransformationStage// : BaseTransformationStage, IIRVisitor, IMethodCompilerStage
    {
        //private int stackSize;

        #region IMethodCompilerStage

        /// <summary>
        /// Setup stage specific processing on the compiler context.
        /// </summary>
        /// <param name="methodCompiler">The compiler context to perform processing in.</param>
        //void IMethodCompilerStage.Setup(BaseMethodCompiler methodCompiler)
        //{
        //    base.Setup(methodCompiler);

        //    stackSize = methodCompiler.StackLayout.StackSize;

        //    Debug.Assert((stackSize % 4) == 0, @"Stack size of method can't be divided by 4!!");
        //}

        #endregion IMethodCompilerStage

        //#region IIRVisitor

        ///// <summary>
        ///// Visitation function for AddSInstruction.
        ///// </summary>
        ///// <param name="context">The context.</param>
        //void IIRVisitor.AddSigned(Context context)
        //{
        //}
    }
}
