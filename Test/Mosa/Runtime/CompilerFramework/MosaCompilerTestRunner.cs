/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Loader;
using Mosa.Runtime;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Metadata.Signatures;
using MbUnit.Framework;
using System.Runtime.InteropServices;
using Test.Mosa.Runtime.CompilerFramework.BaseCode;
using System.IO;

namespace Test.Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Interfaceclass for MbUnit3 to run our testcases.
    /// </summary>
    public abstract class MosaCompilerTestRunner : IDisposable
    {
        #region Data members

        /// <summary>
        /// The filename of the assembly, which contains the test case.
        /// </summary>
        string assembly = null;

        /// <summary>
        /// Flag, which determines if the compiler needs to run.
        /// </summary>
        bool needCompile = true;

        /// <summary>
        /// An array of assembly references to include in the compilation.
        /// </summary>
        string[] references;

        /// <summary>
        /// The test runtime.
        /// </summary>
        TestRuntime runtime;

        /// <summary>
        /// The metadata module of the test case.
        /// </summary>
        IMetadataModule module;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaCompilerTestRunner"/> class.
        /// </summary>
        public MosaCompilerTestRunner()
        {
            this.references = new string[0];
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the test needs to be compiled.
        /// </summary>
        /// <value><c>true</c> if a compilation is needed; otherwise, <c>false</c>.</value>
        protected bool NeedCompile
        {
            get { return this.needCompile; }
            set { this.needCompile = value; }
        }

        /// <summary>
        /// Gets or sets the references.
        /// </summary>
        /// <value>The references.</value>
        public string[] References
        {
            get { return this.references; }
            set
            {
                if (this.references != value)
                {
                    this.references = value;
                    this.needCompile = true;
                }
            }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Builds the test runtime used to execute tests.
        /// </summary>
        [FixtureSetUp]
        public void Begin()
        {
            Console.WriteLine("Building runtime...");
            runtime = new TestRuntime();
        }

        /// <summary>
        /// Disposes the test runtime and deletes the compiled assembly.
        /// </summary>
        [FixtureTearDown]
        public void End()
        {
            // Dispose the test runtime.
            if (null != this.runtime)
            {
                this.runtime.Dispose();
                this.runtime = null;
            }

            // Try to delete the compiled assembly...
            if (null != this.assembly)
            {
                try
                {
                    File.Delete(this.assembly);
                }
                catch
                {
                }
            }
        }


        /// <summary>
        /// Runs a test case.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate used to run the test case.</typeparam>
        /// <param name="ns">The namespace of the test.</param>
        /// <param name="type">The type, which contains the test.</param>
        /// <param name="method">The name of the method of the test.</param>
        /// <param name="parameters">The parameters to pass to the test.</param>
        /// <returns>The result of the test.</returns>
        public object Run<TDelegate>(string ns, string type, string method, params object[] parameters)
        {
            // Do we need to compile the code?
            if (this.needCompile == true)
            {
                if (module != null)
                    RuntimeBase.Instance.AssemblyLoader.Unload(module);
                this.assembly = this.CompileTestCode<TDelegate>(ns, type, method);
                Console.WriteLine("Executing MOSA compiler...");
                module = RunMosaCompiler(this.assembly);
                this.needCompile = false;
            }

            // Find the test method to execute
            RuntimeMethod runtimeMethod = FindMethod(
                ns,
                type,
                method
            );

            // Create a delegate for the test method
            Delegate fn = Marshal.GetDelegateForFunctionPointer(
                runtimeMethod.Address,
                typeof(TDelegate)
            );

            // Execute the test method
            return fn.DynamicInvoke(parameters);
        }

        /// <summary>
        /// Finds a runtime method, which represents the requested method.
        /// </summary>
        /// <exception cref="MissingMethodException">The sought method is not found.</exception>
        /// <param name="ns">The namespace of the sought method.</param>
        /// <param name="type">The type, which contains the sought method.</param>
        /// <param name="method">The method to find.</param>
        /// <returns>An instance of <see cref="RuntimeMethod"/>.</returns>
        private RuntimeMethod FindMethod(string ns, string type, string method)
        {
            foreach (RuntimeType t in runtime.TypeLoader.GetTypesFromModule(module))
            {
                if (t.Namespace != ns || t.Name != type)
                    continue;
                foreach (RuntimeMethod m in t.Methods)
                {
                    if (m.Name == method)
                    {
                        return m;
                    }
                }
            }
            throw new MissingMethodException();
        }

        /// <summary>
        /// Compiles the test code.
        /// </summary>
        /// <param name="ns">The namespace of the test.</param>
        /// <param name="type">The type, which contains the test.</param>
        /// <param name="method">The name of the method of the test.</param>
        /// <exception cref="NotSupportedException">Compilation is not supported.</exception>
        /// <exception cref="Exception">A generic exception during compilation.</exception>
        /// <returns>The name of the compiled assembly file.</returns>
        protected abstract string CompileTestCode<TDelegate>(string ns, string type, string method);

        /// <summary>
        /// Loads the specified assembly into the mosa runtime and executes the mosa compiler.
        /// </summary>
        /// <param name="assemblyFile">The assembly file name.</param>
        /// <returns>The metadata module, which represents the loaded assembly.</returns>
        private IMetadataModule RunMosaCompiler(string assemblyFile)
        {
            IMetadataModule module = RuntimeBase.Instance.AssemblyLoader.Load(
                assemblyFile
            );
            TestCaseAssemblyCompiler.Compile(module);
            return module;
        }

        #endregion // Methods

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            this.End();
        }

        #endregion // IDisposable Members
    }
}
