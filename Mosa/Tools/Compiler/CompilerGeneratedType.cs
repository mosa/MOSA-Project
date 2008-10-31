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
using Mosa.Runtime.Loader;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// Represents a compiler generated type.
    /// </summary>
    public sealed class CompilerGeneratedType : RuntimeType
    {
        #region Data Members

        /// <summary>
        /// Holds all methods of the compiler generated type.
        /// </summary>
        private List<CompilerGeneratedMethod> methods;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerGeneratedType"/> class.
        /// </summary>
        /// <param name="module">The metadata module owning the type.</param>
        /// <param name="namespace">The namespace.</param>
        /// <param name="name">The name.</param>
        public CompilerGeneratedType(IMetadataModule module, string @namespace, string name) : 
            base(0, module) 
        {
            if (null == @namespace)
                throw new ArgumentNullException(@"namespace");
            if (null == name)
                throw new ArgumentNullException(@"name");

            this.methods = new List<CompilerGeneratedMethod>();
            
            base.Namespace = @namespace;
            base.Name = name;
            base.Methods = (IList<RuntimeMethod>)this.methods;
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Gets the methods.
        /// </summary>
        /// <returns>A list, which contains all methods of the compiler generated type.</returns>
        public IList<CompilerGeneratedMethod> GetMethods()
        {
            return this.methods;
        }

        #endregion // Methods

        #region RuntimeType Overrides

        /// <summary>
        /// Gets the base type.
        /// </summary>
        /// <returns>The base type.</returns>
        protected override RuntimeType GetBaseType()
        {
            // Compiler generated types don't have a base type.
            return null;
        }

        /// <summary>
        /// Called to retrieve the name of the type.
        /// </summary>
        /// <returns>The name of the type.</returns>
        protected override string GetName()
        {
            return this.Name;
        }

        /// <summary>
        /// Called to retrieve the namespace of the type.
        /// </summary>
        /// <returns>The namespace of the type.</returns>
        protected override string GetNamespace()
        {
            return this.Namespace;
        }

        #endregion // RuntimeType Overrides
    }
}
