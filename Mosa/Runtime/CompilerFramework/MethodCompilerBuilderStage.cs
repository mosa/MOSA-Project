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
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Builds and schedules method compilers for a type.
    /// </summary>
    public sealed class MethodCompilerBuilderStage : IAssemblyCompilerStage
    {
        #region IAssemblyCompilerStage members

        string IAssemblyCompilerStage.Name
        {
            get { return @"Method Compiler Builder"; }
        }

        void IAssemblyCompilerStage.Run(AssemblyCompiler compiler)
        {
			// Retrieve the provider provider
			IMetadataProvider metadata = compiler.Assembly.Metadata;
			// FIXME: Get the type from a previous stage
			RuntimeType type = compiler.CurrentType;
			
			// Iterate all methods in the type
            foreach (RuntimeMethod method in type.Methods)
            {
                //Debug.WriteLine("Checking method: " + type.ToString() + "." + method.Name);

                // Is this a generic method?
                if (true == method.IsGeneric)
                    continue;

                // Is this a native method?
                if (0 == method.Rva)
                {
                    Debug.WriteLine("Skipping native method: " + type.ToString() + "." + method.Name);
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
				compiler.ScheduleMethodForCompilation(type, method);
			}
        }

        #endregion // IAssemblyCompilerStage members
    }
}
