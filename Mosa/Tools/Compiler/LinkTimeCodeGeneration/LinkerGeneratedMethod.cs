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
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Loader;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Represents compiler generated methods.
    /// </summary>
    public sealed class CompilerGeneratedMethod : RuntimeMethod
    {
        #region Data Members

        /// <summary>
        /// Holds the name of the compiler generated method.
        /// </summary>
        private string name;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerGeneratedMethod"/> class.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="declaringType">Type of the declaring.</param>
        public CompilerGeneratedMethod(IMetadataModule module, string name, RuntimeType declaringType) :
            base(0, module, declaringType)
        {
            if (name == null)
                throw new ArgumentNullException(@"name");

            this.name = name;
            this.Parameters = new List<RuntimeParameter>();
        }

        #endregion // Construction

        #region RuntimeMethod Overrides

        /// <summary>
        /// Called to retrieve the name of the type.
        /// </summary>
        /// <returns>The name of the type.</returns>
        protected override string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Gets the method signature.
        /// </summary>
        /// <returns>The method signature.</returns>
        protected override MethodSignature GetMethodSignature()
        {
            return new MethodSignature(new SigType(CilElementType.Void), new SigType[0]);
        }

        #endregion // RuntimeMethod Overrides
    }
}
