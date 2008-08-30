/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler
{
    public class AotLinkerStage : IAssemblyCompilerStage, IAssemblyLinker
    {
        ObjectFileBuilderBase _objectFileBuilder;

        public AotLinkerStage(ObjectFileBuilderBase objectFileBuilder)
        {
            this._objectFileBuilder = objectFileBuilder;
        }

        public string Name { get { return "AOT Linker"; } }

        public void Run(AssemblyCompiler compiler) { }

        #region IAssemblyLinker Members

        public long Link(LinkType linkType, RuntimeMethod method, int methodOffset, int methodRelativeBase, RuntimeMember target)
        {
            return _objectFileBuilder.Link(
                linkType,
                method,
                methodOffset,
                methodRelativeBase,
                target
            );
        }

        #endregion
    }
}
