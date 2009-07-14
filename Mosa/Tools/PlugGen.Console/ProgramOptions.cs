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

using Mono;
using Mono.GetOptions;
using System.CodeDom.Compiler;
using PlugGen;

[assembly: Author("Managed Operating System Alliance")]
[assembly: ReportBugsTo("Bruce Markham <illuminus86@gmail.com>")]

namespace PlugGen.Console
{
    public class ProgramOptions
        : Mono.GetOptions.Options
    {
        public ProgramOptions(string[] args)
            : base(args)
        {
            this.TranslatedNamespacePrefix = "MOSA.Internal";
        }

        [Mono.GetOptions.Option("The base class library DLL to read in",'i')]
        public string InputDll
        { get; protected set; }

        [Mono.GetOptions.Option("The base output directory to write class files to",'o')]
        public string OutputDirectory
        { get; protected set; }

        [Mono.GetOptions.Option("The namespace prefix that is prepended to translated types", 'n')]
        public string TranslatedNamespacePrefix
        { get; protected set; }

        [Mono.GetOptions.Option("The code language to use for output", 'l')]
        public string Language
        { get; protected set; }

        public System.Type GetCodeDomProviderType()
        {
            if (String.IsNullOrEmpty(this.Language))
                return null;

            string langId = this.Language;

            foreach (var lcdp in RecognizedLanguages)
            {
                if (String.Equals(langId, lcdp.A.FileExtension, StringComparison.OrdinalIgnoreCase))
                    return lcdp.A.GetType();
                if (String.Equals(langId, lcdp.A.GetType().Name, StringComparison.OrdinalIgnoreCase))
                    return lcdp.A.GetType();
                if (String.Equals(langId, lcdp.A.GetType().FullName, StringComparison.OrdinalIgnoreCase))
                    return lcdp.A.GetType();
                if (lcdp.B.Contains(langId, StringComparer.OrdinalIgnoreCase))
                    return lcdp.A.GetType();
            }

            return null;
        }

        private static readonly IEnumerable<Tuple<CodeDomProvider,string[]>> RecognizedLanguages = new List<Tuple<CodeDomProvider,string[]>>()
        {
            new Tuple<CodeDomProvider,string[]> ( new Microsoft.CSharp.CSharpCodeProvider(),
                                                    new string[] { "cs","c#","csharp","c-sharp" } ),
            new Tuple<CodeDomProvider,string[]> ( new Microsoft.VisualBasic.VBCodeProvider(),
                                                    new string[] { "vb", "vb.net", "visualbasic" } )
        };
    }
}
