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
using Mosa.Runtime.CompilerFramework;
using Mosa.Platforms.x86.CPUx86;
using Mosa.Platforms.x86.CPUx86.Intrinsics;

namespace Mosa.Platforms.x86
{
    /// <summary>
    /// Latency table used by the InstructionSchedulingStage.
    /// Contains the Latencies for Intel Core Single / Core Duo CPU's.
    /// Older chipsets contain longer Latencies, so by using shorter Latencies
    /// we don't screw up things.
    /// </summary>
    public sealed class InstructionLatency
    {
        /// <summary>
        /// Contains the Latencies for the usual instructions most widely used
        /// in a standard x86 program.
        /// The Latencies are given in byte, as it seems a latency of > 255 should
        /// not occur in any x86 CPU.
        /// </summary>
        private static readonly Dictionary<Type, sbyte> Latencies = new Dictionary<Type, sbyte>()
        {
            { typeof(AdcInstruction),  1 },
            { typeof(SbbInstruction),  1 },
            { typeof(AddInstruction),  1 },
            { typeof(SubInstruction),  2 },
            { typeof(CliInstruction), 11 },
            { typeof(MulInstruction),  4 },
            { typeof(DivInstruction), 22 },
            { typeof(PushInstruction), 3 },
            { typeof(XchgInstruction), 2 },
            { typeof(SetccInstruction), 1 },
            { typeof(DecInstruction), 1 },
            { typeof(IncInstruction), 1 },
            { typeof(CmpInstruction), 1 },
            { typeof(SseAddInstruction),   3 },
            { typeof(SseSubInstruction),   3 },
            { typeof(SseDivInstruction),  32 },
            { typeof(SseMulInstruction),   5 },
        };

        /// <summary>
        /// Returns the latency for the given instruction
        /// </summary>
        /// <param name="instruction">Instruction to retrieve latency for</param>
        /// <returns>The matching latency.</returns>
        public static sbyte GetLatency(LegacyInstruction instruction)
        {
            if (Latencies.ContainsKey(instruction.GetType()))
                return Latencies[instruction.GetType()];

        	return -1;
        }

		/// <summary>
		/// Gets the latency.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns></returns>
        public static sbyte GetLatency(Type instruction)
        {
            if (Latencies.ContainsKey(instruction))
                return Latencies[instruction];

            return -1;
        }
    }
}
