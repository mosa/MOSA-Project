/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Bruce Markham       < illuminus86@gmail.com >
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;

namespace PlugGen.Emit
{
    public interface ICodeDomEmitter
        : IEmitter
    {
        CodeDomProvider CodeDomProvider { get; }
    }
}
