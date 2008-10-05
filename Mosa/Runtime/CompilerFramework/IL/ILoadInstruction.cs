using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.IL
{
    /// <summary>
    /// Interface used to tag an IL load instructions.
    /// </summary>
    /// <remarks>
    /// This interface is used by <see cref="CilToIrTransformationStage"/> to drop 
    /// load instructions from the instruction stream. It uses the interface to determine
    /// the appropriate operands to replace/remove.
    /// </remarks>
    public interface ILoadInstruction
    {
        /// <summary>
        /// Gets the load destination operand.
        /// </summary>
        Operand Destination { get; }

        /// <summary>
        /// Gets the load source operand.
        /// </summary>
        Operand Source { get; }
    }
}
