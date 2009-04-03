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

namespace PlugGen.Emit
{
    public class NamespaceToFolderTranslationStrategy
    {
        private static Dictionary<Predicate<Tuple<NamespaceFolderMappingPart, string>>, Func<Tuple<NamespaceFolderMappingPart, string>, NamespaceFolderMappingPart>> MappingTriggers;
        static NamespaceToFolderTranslationStrategy()
        {
            MappingTriggers = new Dictionary<Predicate<Tuple<NamespaceFolderMappingPart, string>>, Func<Tuple<NamespaceFolderMappingPart, string>, NamespaceFolderMappingPart>>();
            
            
            MappingTriggers.Add(
                (p) => !(p.A!=null)
                ,
                (p) => new NamespaceFolderMappingPart(p.B, @".\" + p.B, p.A)
            );

            MappingTriggers.Add(
                (p) => (p.A != null) && p.A.NamespaceNamePart == "System" && !(p.A.ParentMapping != null)
                ,
                (p) => new NamespaceFolderMappingPart(p.B, @"..\" + p.A.NamespaceNamePart + "." + p.B, p.A)
            );
            MappingTriggers.Add(
                (p) => (p.A != null) && p.A.NamespaceNamePart == "Microsoft" && !(p.A.ParentMapping != null)
                ,
                (p) => new NamespaceFolderMappingPart(p.B, @"..\" + p.A.NamespaceNamePart + "." + p.B, p.A)
            );
            MappingTriggers.Add(
                (p) => (p.A != null) && p.A.NamespaceNamePart == "Mono" && !(p.A.ParentMapping != null)
                ,
                (p) => new NamespaceFolderMappingPart(p.B, @"..\" + p.A.NamespaceNamePart + "." + p.B, p.A)
            );

            MappingTriggers.Add(
                (p) => true
                ,
                (p) => new NamespaceFolderMappingPart(p.B, @".\" + p.B, p.A)
            );
        }

        public string GetRelativeFolderPath(string fullNamespaceName)
        {
            const char namespacePartDelimitter = '.';
            
            string[] namespaceParts = fullNamespaceName.Split(namespacePartDelimitter);
            
            var aggregatedNamespaceMapping =
                namespaceParts.Aggregate<string,NamespaceFolderMappingPart,NamespaceFolderMappingPart>(
                    null,
                    (s,cv) => GetRelativeFolderPath(cv,s),
                    p => p
                );
            if (!(aggregatedNamespaceMapping != null))
                return null;
            else
                return aggregatedNamespaceMapping.AggregatedFolderPath;
        }
        private NamespaceFolderMappingPart GetRelativeFolderPath(string namespaceNamePart, NamespaceFolderMappingPart parent)
        {
            var allTriggerPredicates = MappingTriggers.Keys;
            var firstTrigger = allTriggerPredicates.FirstOrDefault(p => p(new Tuple<NamespaceFolderMappingPart, string>(parent, namespaceNamePart)));
            if (firstTrigger == null)
                throw new NotImplementedException();
            var translator = MappingTriggers[firstTrigger];
            var mappingPart = translator(new Tuple<NamespaceFolderMappingPart,string>(parent,namespaceNamePart));
            return mappingPart;
        }
    }

    internal class NamespaceFolderMappingPart
    {
        public NamespaceFolderMappingPart(string namespaceNamePart, string folderPathRelativeToParent, NamespaceFolderMappingPart parentMapping)
        {
            this.namespaceNamePart = namespaceNamePart;
            this.folderPathRelativeToParent = folderPathRelativeToParent;
            this.parentMapping = parentMapping;
            this.namespaceDepth = DetermineNamespaceDepth(this.ParentMapping) + 1;
            if (parentMapping!=null)
                this.aggregatedFolderPath = System.IO.Path.Combine(parentMapping.aggregatedFolderPath, folderPathRelativeToParent);
            else
                this.aggregatedFolderPath = folderPathRelativeToParent;
        }

        readonly string namespaceNamePart;
        public string NamespaceNamePart { get { return this.namespaceNamePart; } }
        readonly string folderPathRelativeToParent;
        public string FolderPathRelativeToParent { get { return this.folderPathRelativeToParent; } }
        readonly NamespaceFolderMappingPart parentMapping;
        public NamespaceFolderMappingPart ParentMapping { get { return this.parentMapping; } }
        readonly int namespaceDepth;
        public int NamespaceDepth { get { return this.NamespaceDepth; } }
        readonly string aggregatedFolderPath;
        public string AggregatedFolderPath { get { return this.aggregatedFolderPath; } }

        public static bool operator ==(NamespaceFolderMappingPart nfmp1, NamespaceFolderMappingPart nfmp2)
        {
            if (Object.ReferenceEquals(nfmp1, null) && Object.ReferenceEquals(nfmp2 , null))
                return true;
            else if (Object.ReferenceEquals(nfmp1 ,null) && !Object.ReferenceEquals(nfmp2 , null))
                return false;
            else if (!Object.ReferenceEquals(nfmp1 , null) && Object.ReferenceEquals(nfmp2 , null))
                return false;
            else
                return Object.Equals(nfmp1.namespaceNamePart, nfmp2.namespaceNamePart) && Object.Equals(nfmp1.folderPathRelativeToParent, nfmp2.folderPathRelativeToParent) && Object.Equals(nfmp1.parentMapping, nfmp2.parentMapping);
        }
        public static bool operator !=(NamespaceFolderMappingPart tup1, NamespaceFolderMappingPart tup2)
        {
            return !(tup1 == tup2);
        }
        public override bool Equals(object obj)
        {
            if (obj is NamespaceFolderMappingPart) 
                return this.Equals((NamespaceFolderMappingPart)obj);
            else 
                return false;
        }
        public bool Equals(NamespaceFolderMappingPart obj)
        {
            return Equals(this, obj);
        }

        public static NamespaceFolderMappingPart Create(string namespaceNamePart, string folderPathRelativeToParent, NamespaceFolderMappingPart parentMapping)
        {
            return new NamespaceFolderMappingPart(namespaceNamePart, folderPathRelativeToParent, parentMapping);
        }

        public override int GetHashCode()
        {
            return this.namespaceNamePart.GetHashCode() ^ this.folderPathRelativeToParent.GetHashCode() ^ this.parentMapping.GetHashCode();
        }

        private static int DetermineNamespaceDepth(NamespaceFolderMappingPart currentNamespacesParent)
        {
            if (currentNamespacesParent == null)
                return 0;
            else
                return DetermineNamespaceDepth(currentNamespacesParent.ParentMapping) + 1;
        }
    }
}
