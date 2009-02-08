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
    /// Contains the latencies for Intel Core Single / Core Duo CPU's.
    /// Older chipsets contain longer latencies, so by using shorter latencies
    /// we don't screw up things.
    /// </summary>
    public sealed class InstructionLatency
    {
        /// <summary>
        /// Contains the latencies for the usual instructions most widely used
        /// in a standard x86 program.
        /// The latencies are given in byte, as it seems a latency of > 255 should
        /// not occur in any x86 CPU.
        /// </summary>
        private static Dictionary<Type, byte> latencies = new Dictionary<Type, byte>()
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
        public static byte GetLatency(Instruction instruction)
        {
            if (latencies.ContainsKey(instruction.GetType()))
                return latencies[instruction.GetType()];
            throw new NotSupportedException("No known latency available.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        public static byte GetLatency(Type instruction)
        {
            if (latencies.ContainsKey(instruction))
                return latencies[instruction];
            throw new NotSupportedException("No known latency available.");
        }
    }
}
