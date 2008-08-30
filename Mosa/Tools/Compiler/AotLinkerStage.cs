using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.ObjectFiles;
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
