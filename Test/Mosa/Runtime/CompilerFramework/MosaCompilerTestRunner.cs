using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
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
    public abstract class MosaCompilerTestRunner : IDisposable
    {
        static Dictionary<string, CodeDomProvider> providerCache = new Dictionary<string, CodeDomProvider>();

        public MosaCompilerTestRunner()
        {
            Language = "C#";
            References = new string[0];
        }

        TempFileCollection temps = new TempFileCollection();
        bool _needCompile = true;
        string _language;
        string _codeFilename;
        string _codeSource;
        string[] _references;

        public string Language
        {
            get { return _language; }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    _needCompile = true;
                }
            }
        }
        public string CodeFilename
        {
            get { return _codeFilename; }
            set
            {
                if (_codeFilename != value)
                {
                    _codeFilename = value;
                    _needCompile = true;
                }
            }
        }
        public string CodeSource
        {
            get { return _codeSource; }
            set
            {
                if (_codeSource != value)
                {
                    _codeSource = value;
                    _needCompile = true;
                }
            }
        }
        public string[] References
        {
            get { return _references; }
            set
            {
                if (_references != value)
                {
                    _references = value;
                    _needCompile = true;
                }
            }
        }

        CompilerResults compileResults;
        TestRuntime runtime;
        IMetadataModule module;

        [FixtureSetUp]
        public void Begin()
        {
            Console.WriteLine("Building runtime...");
            runtime = new TestRuntime();
        }

        public object Run<TDelegate>(string ns, string type, string method, params object[] parameters)
        {
            if (_needCompile)
            {
                if (module != null)
                    RuntimeBase.Instance.AssemblyLoader.Unload(module);
                Console.WriteLine("Executing {0} compiler...", _language);
                compileResults = CompileCode();
                Console.WriteLine("Executing MOSA compiler...");
                module = CompileAssembly(compileResults.PathToAssembly);
                _needCompile = false;
            }
            RuntimeMethod runtimeMethod = FindMethod(
                ns,
                type,
                method
            );
            Delegate fn = Marshal.GetDelegateForFunctionPointer(
                runtimeMethod.Address,
                typeof(TDelegate)
            );
            return fn.DynamicInvoke(parameters);
        }

        private RuntimeMethod FindMethod(string ns, string type, string method)
        {
            RuntimeMethod runtimeMethod;
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

        [FixtureTearDown]
        public void End()
        {
            runtime.Dispose();
        }

        private CompilerResults CompileCode()
        {
            CodeDomProvider provider;
            if (!providerCache.TryGetValue(_language, out provider))
                provider = CodeDomProvider.CreateProvider(Language);
            if (null == provider)
                throw new NotSupportedException("The language '" + Language + "' is not supported on this machine.");
            CompilerResults compileResults;
            CompilerParameters parameters = new CompilerParameters(
                    References ?? new string[0],
                    Path.GetTempFileName()
                )
                {
                    GenerateInMemory = false
                };
            if (CodeSource != null)
            {
                Console.Write("From Source: ");
                Console.WriteLine(new string('-', 40 - 13));
                Console.WriteLine(_codeSource);
                Console.WriteLine(new string('-', 40));
                compileResults = provider.CompileAssemblyFromSource(
                    parameters,
                    _codeSource
                );
            }
            else if (CodeFilename != null)
            {
                Console.WriteLine("From File: {0}", _codeFilename);
                compileResults = provider.CompileAssemblyFromFile(
                    parameters,
                    _codeFilename
                );
            }
            else
                throw new NotSupportedException();
            if (compileResults.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Code compile errors:");
                foreach (CompilerError error in compileResults.Errors)
                {
                    sb.AppendLine(error.ToString());
                }
                throw new Exception(sb.ToString());
            }
            return compileResults;
        }

        private IMetadataModule CompileAssembly(string filename)
        {
            IMetadataModule module = RuntimeBase.Instance.AssemblyLoader.Load(
                compileResults.PathToAssembly
            );
            TestCaseAssemblyCompiler.Compile(module);
            return module;
        }

        void IDisposable.Dispose()
        {
            this.End();
        }
    }
}
