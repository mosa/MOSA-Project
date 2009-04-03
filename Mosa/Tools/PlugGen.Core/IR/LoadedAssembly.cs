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

namespace PlugGen.IR
{
    public class LoadedAssembly
    {
        public LoadedAssembly() { }

        public readonly Dictionary<TypeDefinition, IEnumerable<KeyValuePair<MethodDefinition, MemberReference>>> Externs = new Dictionary<TypeDefinition, IEnumerable<KeyValuePair<MethodDefinition, MemberReference>>>();
        public readonly List<TypeDefinition> IntimacyRequirements = new List<TypeDefinition>();
        public readonly Dictionary<TypeDefinition, TypeIntimacyExposureSummary> TypeIntimacyExposureSummaries = new Dictionary<TypeDefinition, TypeIntimacyExposureSummary>();

        public readonly Dictionary<string, TypeDefinition> TypeDefinitionMappings = new Dictionary<string, TypeDefinition>();

        public readonly List<AssemblyDefinition> LoadedReferenceAssemblies = new List<AssemblyDefinition>();
        public readonly Dictionary<string, TypeDefinition> ReferencedTypeDefinitionMappings = new Dictionary<string, TypeDefinition>();

        public IEnumerable<TypeDefinition> GetTypesRequiringEmission()
        {
            return this.Externs.Keys
                    .Union(this.IntimacyRequirements);
        }

        public IEnumerable<TypeDefinition> GetAllDefinedTypes()
        {
            return this.TypeDefinitionMappings.Values;
        }
    }
}
