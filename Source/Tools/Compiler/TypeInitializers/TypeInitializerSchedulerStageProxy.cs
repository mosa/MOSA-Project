/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;

namespace Mosa.Tools.Compiler.TypeInitializers
{
    public class TypeInitializerSchedulerStageProxy : ITypeInitializerSchedulerStage, IAssemblyCompilerStage
    {
        private readonly ITypeInitializerSchedulerStage realStage;

        public TypeInitializerSchedulerStageProxy(ITypeInitializerSchedulerStage realStage)
        {
            this.realStage = realStage;
        }

        public CompilerGeneratedMethod Method
        {
            get
            {
                return this.realStage.Method;
            }
        }

        public void Schedule(RuntimeMethod method)
        {
            this.realStage.Schedule(method);
        }

        public string Name
        {
            get
            {
                return @"TypeInitializerSchedulerStageProxy";
            }
        }

        public void Run()
        {
        }

        public void Setup(AssemblyCompiler compiler)
        {
        }
    }
}