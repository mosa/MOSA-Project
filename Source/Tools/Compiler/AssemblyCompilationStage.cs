/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Loader;
using Mosa.Tools.Compiler.Symbols.Pdb;
using Mosa.Tools.Compiler.TypeInitializers;

namespace Mosa.Tools.Compiler
{
    public class AssemblyCompilationStage : IAssemblyCompilerStage
    {
        private readonly List<string> inputFileNames;

        private AssemblyCompiler outputAssemblyCompiler;

        private IAssemblyLinker linker;

        private ITypeInitializerSchedulerStage typeInitializerSchedulerStage;

        public AssemblyCompilationStage(IEnumerable<string> inputFileNames)
        {
            this.inputFileNames = new List<string>(inputFileNames);
        }

        public string Name
        {
            get
            {
                return @"AssemblyCompilationStage";
            }
        }

        public void Run()
        {
            foreach (string assemblyFileName in this.inputFileNames)
            {
                IMetadataModule assembly = this.LoadAssembly(RuntimeBase.Instance, assemblyFileName);

                this.CompileAssembly(assembly);
            }
        }

        public void Setup(AssemblyCompiler compiler)
        {
            this.outputAssemblyCompiler = compiler;
            this.typeInitializerSchedulerStage = compiler.Pipeline.FindFirst<ITypeInitializerSchedulerStage>();
            this.linker = compiler.Pipeline.FindFirst<IAssemblyLinker>();
        }

        private IMetadataModule LoadAssembly(RuntimeBase runtime, string assemblyFileName)
        {
            try
            {
                IMetadataModule assemblyModule = runtime.AssemblyLoader.Load(assemblyFileName);

                // Try to load debug information for the compilation
                this.LoadAssemblyDebugInfo(assemblyFileName);

                return assemblyModule;
            }
            catch (BadImageFormatException bife)
            {
                throw new CompilationException(String.Format("Couldn't load input file {0} (invalid format).", assemblyFileName), bife);
            }
        }

        private void LoadAssemblyDebugInfo(string assemblyFileName)
        {
            string dbgFile;
            dbgFile = Path.Combine(Path.GetDirectoryName(assemblyFileName), Path.GetFileNameWithoutExtension(assemblyFileName) + ".pdb") + "!!";
            if (File.Exists(dbgFile))
            {
                using (FileStream fileStream = new FileStream(dbgFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (PdbReader reader = new PdbReader(fileStream))
                    {
                        Debug.WriteLine(@"Global symbols:");
                        foreach (CvSymbol symbol in reader.GlobalSymbols)
                        {
                            Debug.WriteLine("\t" + symbol.ToString());
                        }

                        Debug.WriteLine(@"Types:");
                        foreach (PdbType type in reader.Types)
                        {
                            Debug.WriteLine("\t" + type.Name);
                            Debug.WriteLine("\t\tSymbols:");
                            foreach (CvSymbol symbol in type.Symbols)
                            {
                                Debug.WriteLine("\t\t\t" + symbol.ToString());
                            }

                            Debug.WriteLine("\t\tLines:");
                            foreach (CvLine line in type.LineNumbers)
                            {
                                Debug.WriteLine("\t\t\t" + line.ToString());
                            }
                        }
                    }
                }
            }
        }

        private void CompileAssembly(IMetadataModule assembly)
        {
            using (AotAssemblyCompiler assemblyCompiler = new AotAssemblyCompiler(this.outputAssemblyCompiler.Architecture, assembly, this.typeInitializerSchedulerStage, this.linker))
            {
                assemblyCompiler.Run();
            }
        }
    }
}
