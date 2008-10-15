/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;
using System.IO;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    public class AotLinkerStage : IAssemblyCompilerStage, IAssemblyLinker
    {
        /// <summary>
        /// 
        /// </summary>
        ObjectFileBuilderBase _objectFileBuilder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectFileBuilder"></param>
        public AotLinkerStage(ObjectFileBuilderBase objectFileBuilder)
        {
            this._objectFileBuilder = objectFileBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return "AOT Linker"; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compiler"></param>
        public void Run(AssemblyCompiler compiler) { }

        #region IAssemblyLinker Members

        /// <summary>
        /// Allocates the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="section">The section.</param>
        /// <param name="size">The size.</param>
        /// <param name="alignment">The alignment.</param>
        /// <returns></returns>
        public Stream Allocate(RuntimeMember member, LinkerSection section, int size, int alignment)
        {
            return _objectFileBuilder.Allocate(member, section, size, alignment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkType"></param>
        /// <param name="method"></param>
        /// <param name="methodOffset"></param>
        /// <param name="methodRelativeBase"></param>
        /// <param name="target"></param>
        /// <returns></returns>
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
