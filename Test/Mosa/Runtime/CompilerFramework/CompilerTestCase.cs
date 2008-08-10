/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using MbUnit.Framework;
using Mosa.Runtime.Loader;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Vm;
using System.Runtime.InteropServices;
using Mosa.Runtime;


namespace cltester
{
    /// <summary>
    /// XML serializable type, that represents a single executable test case.
    /// </summary>
    public sealed class CompilerTestCase : ITestCase
    {
        #region Data members

        /// <summary>
        /// Holds the author of the compiler test case.
        /// </summary>
        private string _author;

        /// <summary>
        /// Holds the contact mail address of the author.
        /// </summary>
        private string _contact;

        /// <summary>
        /// Holds a description of the test case.
        /// </summary>
        private string _description;

        /// <summary>
        /// Holds the language of the source files.
        /// </summary>
        private string _language;

        /// <summary>
        /// Holds the name of the test case.
        /// </summary>
        private string _name;

        /// <summary>
        /// Holds the source files of the test case.
        /// </summary>
        private string[] _sources;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="CompilerTestCase"/>.
        /// </summary>
        public CompilerTestCase()
        {
            _language = @"C#";
            _sources = new string[0];
            _name = _description = _author = _contact = @"";
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the author of the test case.
        /// </summary>
        [XmlAttribute(@"author")]
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        /// <summary>
        /// Gets or sets the contact of the test case.
        /// </summary>
        [XmlAttribute(@"contact")]
        public string Contact
        {
            get { return _contact; }
            set { _contact = value; }
        }

        /// <summary>
        /// Gets or sets the description of the test case.
        /// </summary>
        [XmlElement(@"Description")]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the programming language of the test case.
        /// </summary>
        [XmlAttribute(@"language")]
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// Gets or sets the name of the test case.
        /// </summary>
        [XmlAttribute(@"name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets a list of source filenames of the test case.
        /// </summary>
        [XmlElement(@"Source")]
        public string[] Sources
        {
            get { return _sources; }
            set { _sources = value; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Invokes the test case.
        /// </summary>
        /// <param name="o">?</param>
        /// <param name="args">List of arguments to pass to the test case.</param>
        /// <returns>?</returns>
        public object Invoke(object o, System.Collections.IList args)
        {
            Console.WriteLine("Executing {0} compiler...", _language);
            CompilerResults compilerResults = ExecuteCompiler();

            using (TestRuntime rt = new TestRuntime())
            {
                Console.WriteLine("Executing MOSA compiler...");
                IMetadataModule module = ExecuteMosaCompiler(compilerResults.PathToAssembly);

                Console.WriteLine("Executing test functions...");
                foreach (RuntimeType type in rt.TypeLoader.GetTypesFromModule(module))
                {
                    foreach (RuntimeMethod method in type.Methods)
                    {
                        if (true == method.Name.StartsWith("Test_"))
                        {
                            Console.WriteLine("Running {0}...", method.Name.Substring(5));
                            int exitCode;
                            NativeCode fn = (NativeCode)Marshal.GetDelegateForFunctionPointer(method.Address, typeof(NativeCode));
                            exitCode = fn();

                            /*
                            IntPtr hDll = GetModuleHandle(@"kernel32.dll");
                            IntPtr ptrGetTickCount = GetProcAddress(hDll, @"GetTickCount");
                            NativeCode fn = (NativeCode)Marshal.GetDelegateForFunctionPointer(ptrGetTickCount, typeof(NativeCode));
                            exitCode = fn();
                             */

                            Console.WriteLine("Done. (Exit code: {0})", exitCode);
                        }
                    }
                }
                Console.WriteLine("Test functions executed");
            }

            return null;
        }

        /// <summary>
        /// Executes the MOSA JIT compiler on the assembly.
        /// </summary>
        /// <param name="assemblyPath">The path to the assembly to compile.</param>
        /// <returns>A metadata module representing the given assembly.</returns>
        private IMetadataModule ExecuteMosaCompiler(string assemblyPath)
        {
            IMetadataModule module = RuntimeBase.Instance.AssemblyLoader.Load(assemblyPath);
            TestCaseAssemblyCompiler.Compile(module);
            return module;
        }

        /// <summary>
        /// Executes the source code compiler.
        /// </summary>
        /// <returns>The compiler results.</returns>
        private CompilerResults ExecuteCompiler()
        {
            CompilerResults result;
            using (CodeDomProvider provider = CodeDomProvider.CreateProvider(_language))
            {
                CompilerParameters options = new CompilerParameters();
                options.GenerateInMemory = false;
                options.OutputAssembly = "Test.dll";

                ICodeCompiler compiler = provider.CreateCompiler();
                result = compiler.CompileAssemblyFromFileBatch(options, _sources);
                Assert.IsEmpty(result.Errors);
            }
            return result;
        }

        /// <summary>
        /// Delegate, which represents the signature of the test functions in native code.
        /// </summary>
        /// <returns>The test result. This result should be non-zero on success.</returns>
        private delegate int NativeCode();

        /// <summary>
        /// Publishes the test case and allow associated invocations in the given test suite.
        /// </summary>
        /// <param name="suite">The test suite to publish the test case in.</param>
        public void Publish(TestSuite suite)
        {
            suite.Add(this);
        }

        #endregion // Methods

        #region Interop Test

        [DllImport(@"kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        private static extern IntPtr GetModuleHandle(string module);

        [DllImport(@"kernel32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string functionName);

        #endregion // Interop Test
    }
}
