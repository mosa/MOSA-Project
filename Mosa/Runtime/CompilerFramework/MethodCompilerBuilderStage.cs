/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Builds and schedules method compilers for a type.
    /// </summary>
    public class MethodCompilerBuilderStage : IAssemblyCompilerStage, IMethodCompilerBuilder
    {
        #region Data members

        private readonly List<MethodCompilerBase> _methodCompilers = new List<MethodCompilerBase>();

        #endregion

        #region IAssemblyCompilerStage members

        string IAssemblyCompilerStage.Name
        {
            get { return @"Method Compiler Builder"; }
        }

        void IAssemblyCompilerStage.Run(AssemblyCompiler compiler)
        {
            // Retrieve the provider provider
            ReadOnlyRuntimeTypeListView types = RuntimeBase.Instance.TypeLoader.GetTypesFromModule(compiler.Assembly);
            foreach (RuntimeType type in types)
            {
                // Do not compile generic types
                if (type.IsGeneric)
                    continue;

                foreach (RuntimeMethod method in type.Methods)
                {
                    if (method.IsGeneric)
                        continue;

                    if (method.IsNative)
                    {
                        Debug.WriteLine("Skipping native method: " + type + "." + method.Name);
                        Debug.WriteLine("Method will not be available in compiled image.");
                        continue;
                    }

                    // FIXME: Create a method implementation for this method...
                    //MethodImplementation methodImpl = provider.GetRow<MethodImplementation>(method);
                    //methodImpl.OwnerType = type;
                    //Debug.WriteLine("\tMethod: " + method.ToString());

                    // Schedule the method for compilation...
                    // FIXME: Do we really want to do it this way? Shouldn't we use some compilation service for this?
                    // REFACTOR out of the AssemblyCompiler class
                    MethodCompilerBase mcb = compiler.CreateMethodCompiler(type, method);
                    ScheduleMethod(mcb);
                }
            }
        }

        private void ScheduleMethod(MethodCompilerBase mcb)
        {
            _methodCompilers.Add(mcb);
        }

        #endregion

        IEnumerable<MethodCompilerBase> IMethodCompilerBuilder.Scheduled
        {
            get { return _methodCompilers; }
        }
    }
}
