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
using Mono.Cecil;
using System.CodeDom;

namespace PlugGen.Emit
{
    public sealed class OutputClassFileItem
    {
        private OutputClassFileItem() { }
        public OutputClassFileItem(TypeDefinition originalTypeDefinition, IEnumerable<TypeDefinition> nestedTypes, CodeTypeDeclaration outputDefinition, string filePath)
        {
            if (originalTypeDefinition == null)
                throw new ArgumentNullException("originalTypeDefinition");
            if (nestedTypes == null)
                throw new ArgumentNullException("nestedTypes");
            if (outputDefinition == null)
                throw new ArgumentNullException("outputDefinition");
            if (String.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");

            this.OriginalTypeDefinition = originalTypeDefinition;
            this.NestedTypes = nestedTypes;
            this.OutputDefinition = outputDefinition;
            this.FilePath = filePath;
        }

        public TypeDefinition OriginalTypeDefinition { get; private set; }

        public IEnumerable<TypeDefinition> NestedTypes { get; private set; }

        public CodeTypeDeclaration OutputDefinition { get; private set; }

        public string FilePath { get; private set; }
    }
}
