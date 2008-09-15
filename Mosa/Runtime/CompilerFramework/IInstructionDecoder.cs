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

using Mosa.Runtime.CompilerFramework.Ir;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Interface of instruction decoders.
    /// </summary>
    public interface IInstructionDecoder
    {
        /// <summary>
        /// Gets the compiler, that is currently executing.
        /// </summary>
        MethodCompilerBase Compiler { get; }

        /// <summary>
        /// Gets the RuntimeMethod being compiled.
        /// </summary>
        RuntimeMethod Method { get; }

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void Decode(out byte value);

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void Decode(out sbyte value);

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void Decode(out short value);

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void Decode(out ushort value);

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void Decode(out int value);

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void Decode(out uint value);

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void Decode(out long value);

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void Decode(out float value);

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void Decode(out double value);

        /// <summary>
        /// Decodes <paramref name="value"/> from the instruction stream.
        /// </summary>
        /// <param name="value">Receives the decoded value from the instruction stream.</param>
        void Decode(out TokenTypes value);
    }
}
