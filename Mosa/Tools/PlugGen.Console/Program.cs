/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Bruce Markham       < illuminus86@gmail.com >
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using PlugGen.Emit;

namespace PlugGen.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramOptions programOptions = new ProgramOptions(args);
            programOptions.ShowBanner();

            var codeDomProviderTypes = new Type[]
            {
                typeof(Microsoft.CSharp.CSharpCodeProvider),
                typeof(Microsoft.VisualBasic.VBCodeProvider),
                //typeof(Microsoft.VisualC.VSCodeProvider)
            };

            System.Console.Write("Loading assembly...");
            var loadedAssembly = PlugGen.Read.AssemblyReader.ReadAssembly(System.IO.Path.Combine(System.Environment.CurrentDirectory, programOptions.InputDll));
            System.Console.WriteLine("DONE.");

            foreach (var codeDomProviderType in codeDomProviderTypes)
            {
                System.Console.Write("Emitting with {0}...", codeDomProviderType.FullName);

                //var emitterCreator = GetCodeDomEmitterCreator(programOptions.GetCodeDomProviderType());
                var emitterCreator = GetCodeDomEmitterCreator(codeDomProviderType);

                var emitter = emitterCreator(new PlugGen.IR.LoadedAssembly[] { loadedAssembly }, programOptions.TranslatedNamespacePrefix) as ICodeDomEmitter;

                string baseFolder = System.IO.Path.Combine(System.Environment.CurrentDirectory, programOptions.OutputDirectory);
                baseFolder = System.IO.Path.Combine(baseFolder, @".\" + emitter.CodeDomProvider.FileExtension);
                emitter.EmitAtBaseFolder(baseFolder);

                System.Console.WriteLine("DONE.");
            }
        }

        internal static Func<IEnumerable<PlugGen.IR.LoadedAssembly>, string, IEmitter> GetCodeDomEmitterCreator(System.Type codeDomType)
        {
            if (codeDomType == null)
                throw new ArgumentNullException("codeDomType");
            if (!typeof(System.CodeDom.Compiler.CodeDomProvider).IsAssignableFrom(codeDomType))
                throw new ArgumentOutOfRangeException("codeDomType");

            System.Type baseEmitterType = typeof(CodeDomEmitter<>);
            System.Type emitterType = baseEmitterType.MakeGenericType(codeDomType);

            Func<IEnumerable<PlugGen.IR.LoadedAssembly>, string, IEmitter> createEmitter =
                delegate(IEnumerable<PlugGen.IR.LoadedAssembly> loadedAssemblies, string translatedNamespacePrefix)
                {
                    return (emitterType.Assembly.CreateInstance(
                        emitterType.FullName,
                        false,
                        System.Reflection.BindingFlags.Default,
                        null,
                        new object[] { loadedAssemblies, translatedNamespacePrefix }
                        , System.Globalization.CultureInfo.CurrentCulture,
                        new object[] { }
                        )) as IEmitter;
                };
            return createEmitter;
        }
    }
}
