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
using IR = Mosa.Runtime.CompilerFramework.IR;
using IL = Mosa.Runtime.CompilerFramework.IL;

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
            { typeof(x86.Instructions.AdcInstruction),  1 },
            { typeof(x86.Instructions.SbbInstruction),  1 },
            { typeof(x86.Instructions.AddInstruction),  1 },
            { typeof(x86.Instructions.SubInstruction),  2 },
            { typeof(x86.Instructions.Intrinsics.CliInstruction), 11 },
            { typeof(x86.Instructions.MulInstruction),  4 },
            { typeof(x86.Instructions.DivInstruction), 22 },
            { typeof(x86.Instructions.Intrinsics.PushInstruction), 3 },
            { typeof(x86.Instructions.Intrinsics.XchgInstruction), 2 },
            { typeof(x86.Instructions.SetccInstruction), 1 },
            { typeof(x86.Instructions.DecInstruction), 1 },
            { typeof(x86.Instructions.IncInstruction), 1 },
            { typeof(x86.Instructions.CmpInstruction), 1 },
            { typeof(x86.Instructions.SseAddInstruction),   3 },
            { typeof(x86.Instructions.SseSubInstruction),   3 },
            { typeof(x86.Instructions.SseDivInstruction),  32 },
            { typeof(x86.Instructions.SseMulInstruction),   5 },
        };

        /// <summary>
        /// Returns the latency for the given instruction
        /// </summary>
        /// <param name="instruction">Instruction to retrieve latency for</param>
        /// <returns>The matching latency.</returns>
        public static sbyte GetLatency(Instruction instruction)
        {
            if (Latencies.ContainsKey(instruction.GetType()))
                return Latencies[instruction.GetType()];

        	return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        public static sbyte GetLatency(Type instruction)
        {
            if (Latencies.ContainsKey(instruction))
                return Latencies[instruction];
            return -1;
        }
    }
}
