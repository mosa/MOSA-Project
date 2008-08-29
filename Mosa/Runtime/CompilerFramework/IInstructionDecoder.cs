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
    public interface IInstructionDecoder
    {
        MethodCompilerBase Compiler { get; }
        RuntimeMethod Method { get; }
        //IMetadataProvider Metadata { get; }
        //IArchitecture Architecture { get; }
        //AssemblyCompiler AssemblyCompiler { get; }

        Operand GetLocalOperand(int idx);
        Operand GetParameterOperand(int idx);

        byte DecodeByte();
        sbyte DecodeSByte();

        short DecodeInt16();
        ushort DecodeUInt16();

        int  DecodeInt32();
        uint DecodeUInt32();

        long DecodeInt64();

        float DecodeSingle();
        double DecodeDouble();

        TokenTypes DecodeToken();
    }
}
