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

namespace PlugGen.Read
{
    public class AssemblyReader 
    {
        internal AssemblyDefinition AssemblyDefinition { get; private set; }
        internal IR.LoadedAssembly Assembly { get; private set; }

        private AssemblyReader() { }
        internal AssemblyReader(AssemblyDefinition assemblyDefinition)
        {
            if (assemblyDefinition == null)
                throw new ArgumentNullException("assemblyDefinition");
            this.AssemblyDefinition = assemblyDefinition;
            this.Assembly = new IR.LoadedAssembly();
        }

        public static IR.LoadedAssembly ReadAssembly(string fileName)
        {
            var assemblyReader = new AssemblyReader(Mono.Cecil.AssemblyFactory.GetAssembly(fileName));
            return assemblyReader.Read();
        }

        public IR.LoadedAssembly Read()
        {
            this.AssemblyDefinition.Modules.Cast<ModuleDefinition>().ToList().ForEach(Process_FindExterns);
            if (this.Assembly.Externs.Count > 0)
            {
                Process_RegisterReferencedTypeDefinitionMappings(this.AssemblyDefinition);
                this.AssemblyDefinition.Modules.Cast<ModuleDefinition>().ToList().ForEach(Process_RegisterTypeDefinitionMappings);
                this.Assembly.GetAllDefinedTypes().ToList().ForEach(Process_RecordIntimacyExposure);
                this.Assembly.Externs.Keys.ToList().ForEach(Process_FindIntimacyRequirements);
            }
            return this.Assembly;
        }

        private void Process_RegisterReferencedTypeDefinitionMappings(AssemblyDefinition ad)
        {
            foreach (var md in ad.Modules.Cast<ModuleDefinition>())
            {
                foreach (var ran in md.AssemblyReferences.Cast<AssemblyNameReference>())
                {
                    var rad = ad.Resolver.Resolve(ran);
                    if (rad == null)
                        throw new InvalidOperationException("Unable to resolve referenced assembly");
                    foreach (var lramd in rad.Modules.Cast<ModuleDefinition>())
                    {
                        lramd.Types.Cast<TypeDefinition>().ToList().ForEach(Process_RegisterReferencedTypeDefinitionMappings);
                    }
                }
            }
        }

        private void Process_RegisterReferencedTypeDefinitionMappings(TypeDefinition td)
        {
            var canonicalName = GetCanonicalName(td);
            if(!this.Assembly.ReferencedTypeDefinitionMappings.ContainsKey(canonicalName))
                this.Assembly.ReferencedTypeDefinitionMappings.Add(canonicalName, td);
            td.NestedTypes.Cast<TypeDefinition>().ToList().ForEach(Process_RegisterReferencedTypeDefinitionMappings);
        }

        private void Process_FindIntimacyRequirements(TypeDefinition td)
        {
            List<TypeDefinition> typesToAdd = new List<TypeDefinition>();
            //var thisTypesIntimacyExposure = this.Assembly.TypeIntimacyExposureSummaries[td];
            
            //if(thisTypesIntimacyExposure.HasInternalStaticMembers
            //    ||  thisTypesIntimacyExposure.HasNestedTypes
            //    ||  thisTypesIntimacyExposure.IsNestedType )
            //{
            //    typesToAdd.Add(td);
            //}
            
            //this.
            // TODO: Flesh this method out properly
            typesToAdd.Add(td);

            var finalTypesToAdd = typesToAdd.Distinct();
            finalTypesToAdd = finalTypesToAdd.Except(this.Assembly.IntimacyRequirements);
            this.Assembly.IntimacyRequirements.AddRange(finalTypesToAdd);
        }

        private void Process_RecordIntimacyExposure(TypeDefinition td)
        {
            if (this.Assembly.TypeIntimacyExposureSummaries.ContainsKey(td))
                return;
            
            var result = new IR.TypeIntimacyExposureSummary();
            if (td.DeclaringType != null)
                result.IsNestedType = true;
            if (td.NestedTypes.Count > 0)
                result.HasNestedTypes = true;
            result.HasInternalInstanceMembers =
                GetAllMethods(td).Where(mp => !mp.Key.IsStatic && mp.Key.IsAssembly).Count() > 0
                ||
                td.Fields.Cast<FieldDefinition>().Where(fd => (!fd.IsStatic && fd.IsAssembly)).Count() > 0;
            result.HasInternalStaticMembers =
                GetAllMethods(td).Where(mp => mp.Key.IsStatic && mp.Key.IsAssembly).Count() > 0
                ||
                td.Fields.Cast<FieldDefinition>().Where(fd => (fd.IsStatic && fd.IsAssembly)).Count() > 0;
            result.IsInternalType = td.IsNotPublic || td.IsNestedAssembly;
            
            this.Assembly.TypeIntimacyExposureSummaries.Add(td, result);
        }

        private void AccumulateInheritedTypes(TypeReference tr, List<TypeDefinition> accumulationStore)
        {
            AccumulateInheritedTypes(GetTypeDefinition(tr), accumulationStore);
        }
        
        private void AccumulateInheritedTypes(TypeDefinition td, List<TypeDefinition> accumulationStore)
        {
            foreach (var i in td.Interfaces.Cast<TypeReference>())
            {
                var iTd = GetTypeDefinition(i);
                if (iTd != null && !accumulationStore.Contains(iTd))
                {
                    accumulationStore.Add(iTd);
                    AccumulateInheritedTypes(iTd, accumulationStore);
                }
            }
            if (td.BaseType != null && !accumulationStore.Contains(td))
            {
                accumulationStore.Add(td);
                AccumulateInheritedTypes(td, accumulationStore);
            }
        }

        private void Process_RegisterTypeDefinitionMappings(ModuleDefinition md)
        {
            md.Types.Cast<TypeDefinition>().ToList().ForEach(Process_RegisterTypeDefinitionMappings);
        }

        private void Process_RegisterTypeDefinitionMappings(TypeDefinition td)
        {
            string canonicalName = GetCanonicalName(td);
            if (!this.Assembly.TypeDefinitionMappings.ContainsKey(canonicalName))
                this.Assembly.TypeDefinitionMappings.Add(canonicalName, td);
            td.NestedTypes.Cast<TypeDefinition>().ToList().ForEach(Process_RegisterTypeDefinitionMappings);
        }

        public TypeDefinition GetTypeDefinition(TypeReference tr)
        {
            string canonicalName = GetCanonicalName(tr);
            TypeDefinition result = null;
            bool lookupSuccess = this.Assembly.TypeDefinitionMappings.TryGetValue(canonicalName,out result);
            return result;
        }

        #region Find Externs

        private void Process_FindExterns(ModuleDefinition md)
        {
            md.Types.Cast<TypeDefinition>().ToList().ForEach(td => Process_FindExterns(td));   
        }

        private void Process_FindExterns(TypeDefinition td)
        {
            if (td.Name == "<Module>")
                return;
            if (td.Name == "<PrivateImplementationDetails>")
                return;

            var externs = GetAllExterns(td);

            if (externs.Count() > 0)
            {
                if(!this.Assembly.Externs.ContainsKey(td))
                    this.Assembly.Externs.Add(td, externs);
            }
            td.NestedTypes.Cast<TypeDefinition>().ToList().ForEach(ntd => Process_FindExterns(ntd));    
        }

        public static IEnumerable<KeyValuePair<MethodDefinition, MemberReference>> GetAllExterns(TypeDefinition td)
        {
            var allMethods = GetAllMethods(td);
            var externMethods = allMethods.Where(mdkvp => IsExterned(mdkvp.Key));
            return externMethods;
        }

        public static IEnumerable<KeyValuePair<MethodDefinition, MemberReference>> GetAllMethods(TypeDefinition td)
        {
            
            var standardMethods = td.Methods.Cast<MethodDefinition>()
                        .Select(md=>new KeyValuePair<MethodDefinition,MemberReference>(md,null));

            var constructorMethods = td.Constructors.Cast<MethodDefinition>()
                        .Select(md => new KeyValuePair<MethodDefinition, MemberReference>(md, null));

            var properties = td.Properties.Cast<PropertyDefinition>();
            var propertyMethods =
                    properties.Select(pd => new List<KeyValuePair<MethodDefinition, MemberReference>>{
                           new KeyValuePair<MethodDefinition,MemberReference>(pd.SetMethod,pd),
                           new KeyValuePair<MethodDefinition,MemberReference>(pd.GetMethod,pd),
                    }).SelectMany(led => led);

            var events = td.Events.Cast<EventDefinition>();
            var eventMethods =
                    events.Select(ed => new List<KeyValuePair<MethodDefinition, MemberReference>>{
                           new KeyValuePair<MethodDefinition,MemberReference>(ed.AddMethod,ed),
                           new KeyValuePair<MethodDefinition,MemberReference>(ed.InvokeMethod,ed),
                           new KeyValuePair<MethodDefinition,MemberReference>(ed.RemoveMethod,ed)
                    }).SelectMany(led => led);

            var allMethods =
                constructorMethods
                .Union(standardMethods)
                .Union(constructorMethods)
                .Union(propertyMethods)
                .Union(eventMethods);

            return allMethods.Where(mdi=>mdi.Key!=null).Distinct();
        }

        internal static bool IsExterned(MethodDefinition md)
        {
            return md.IsInternalCall || md.IsPInvokeImpl;
        }
        internal static bool IsInternal(MethodDefinition md)
        {
            return md.IsFamily;
        }
        internal static bool IsInternal(FieldDefinition fd)
        {
            return fd.IsFamily;
        }
        #endregion

        #region Canonical Naming
        public static string GetCanonicalName(TypeReference td)
        {
            StringBuilder sb = new StringBuilder();
            //if (td.DeclaringType != null)
            //{
            //    sb.Append(GetCanonicalName(td.DeclaringType));
            //    sb.Append('+');
            //}
            //else if(!String.IsNullOrEmpty(td.Namespace))
            //{
            //    sb.Append(td.Namespace);
            //    sb.Append('.');
            //}
            //sb.Append(td.Name);
            //if (td.GenericParameters != null && td.GenericParameters.Count > 0)
            //{
            //    sb.Append(GetCanonicalNamePart(td.GenericParameters));
            //}
            string cecilName = td.FullName;
            //if (td.DeclaringType != null)
            //{
            //    string dtCecilName = td.DeclaringType.Name;
            //    if (!cecilName.StartsWith(dtCecilName))
            //    {
            //        if (td.DeclaringType != null)
            //        {
            //            sb.Append(GetCanonicalName(td.DeclaringType));
            //            sb.Append('+');
            //        }
            //        else if (!String.IsNullOrEmpty(td.Namespace))
            //        {
            //            sb.Append(td.Namespace);
            //            sb.Append('.');
            //        }
            //        sb.Append(td.Name);
            //        if (td.GenericParameters != null && td.GenericParameters.Count > 0)
            //        {
            //            sb.Append(GetCanonicalNamePart(td.GenericParameters));
            //        }
            //    }
            //}
            //else
                sb.Append(cecilName);
            return sb.ToString();
        }

        public static string GetCanonicalName(MemberReference mr)
        {
            if (mr is MethodReference)
                return GetCanonicalName(mr as MethodReference);
            else if (mr is FieldReference)
                return GetCanonicalName(mr as FieldReference);
            else if (mr is PropertyReference)
                return GetCanonicalName(mr as PropertyReference);
            else
                throw new NotImplementedException();
        }

        public static string GetCanonicalName(MethodReference mr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetCanonicalName(mr.DeclaringType));
            sb.Append('.');
            sb.Append(mr.Name);
            if (mr.GenericParameters != null && mr.GenericParameters.Count > 0)
                sb.Append(GetCanonicalNamePart(mr.GenericParameters));
            if (mr.Parameters != null)
                sb.Append(GetCanonicalNamePart(mr.Parameters));
            return sb.ToString();
        }
        public static string GetCanonicalName(FieldReference fr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetCanonicalName(fr.DeclaringType));
            sb.Append('.');
            sb.Append(fr.Name);
            return sb.ToString();
        }
        public static string GetCanonicalName(PropertyReference pr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetCanonicalName(pr.DeclaringType));
            sb.Append('.');
            sb.Append(pr.Name);
            if (pr.Parameters != null && pr.Parameters.Count > 0)
                sb.Append(GetCanonicalNamePart(pr.Parameters));
            return sb.ToString();
        }

        internal static string GetCanonicalNamePart(GenericParameterCollection gpc)
        {
            if (gpc.Count <= 0)
                return String.Empty;
            return String.Format("`{0}", gpc.Count);
            //return gpc.Cast<GenericParameter>()
            //    .Aggregate<GenericParameter, StringBuilder, string>(
            //        new StringBuilder('<'),
            //        (sbi, gp) =>
            //        {
            //            sbi.Append((sbi.Length > 0) ? "," : String.Empty);
            //            sbi.Append(gp.
            //            return sbi;
            //        },
            //        (sbi) =>
            //        {
            //            sbi.Append('>');
            //            return sbi.ToString();
            //        }
            //);
        }

        internal static string GetCanonicalNamePart(ParameterDefinitionCollection pdc)
        {
            if (pdc.Count <= 0)
                return String.Empty;
            return pdc.Cast<ParameterDefinition>()
                .Aggregate<ParameterDefinition, StringBuilder, string>(
                    new StringBuilder('('),
                    (sbi, pd) =>
                    {
                        sbi.Append((sbi.Length > 0) ? "," : String.Empty);
                        sbi.Append(GetCanonicalNamePart(pd));
                        return sbi;
                    },
                    (sbi) =>
                    {
                        sbi.Append(')');
                        return sbi.ToString();
                    }
            );
        }

        internal static string GetCanonicalNamePart(ParameterDefinition pd)
        {
            StringBuilder sb = new StringBuilder();

            if (pd.IsOut && !pd.IsIn)
                sb.Append("out ");
            else if (pd.IsIn && pd.IsOut)
                sb.Append("ref ");
            sb.Append(GetCanonicalName(pd.ParameterType));

            return sb.ToString();
        }
        #endregion
    }
}
