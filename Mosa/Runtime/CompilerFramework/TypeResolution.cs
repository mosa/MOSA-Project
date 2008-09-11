/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Loader.PE;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class TypeResolution
    {
        #region Data members

        //private Dictionary<int, IMetadataModule> _assemblies;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolution"/> class.
        /// </summary>
        public TypeResolution()
        {
        }

        #endregion // Construction

        #region Methods
/*
        public IMetadataModule LoadAssembly(string path)
        {
            // FIXME: Use framework lookup rules to load the referenced assembly.

            AssemblyRow assemblyRow;
            string assemblyName, assemblyRefName;
            IMetadataModule result = PortableExecutableImage.Load(new FileStream(path, FileMode.Open, FileAccess.Read));
            result.Metadata.Read(TokenTypes.Assembly + 1, out assemblyRow);
            result.Metadata.Read(assemblyRow.NameIdx, out assemblyName);
            Debug.WriteLine(@"Loaded " + assemblyName);
            Debug.WriteLine(@"References:");
            TokenTypes maxAssemblyRef = result.Metadata.GetMaxTokenValue(TokenTypes.AssemblyRef);
            for (TokenTypes token = TokenTypes.AssemblyRef + 1; token <= maxAssemblyRef; token++)
            {
                AssemblyRefRow assemblyRef;
                result.Metadata.Read(token, out assemblyRef);
                result.Metadata.Read(assemblyRef.NameIdx, out assemblyRefName);
                Debug.WriteLine("\t " + assemblyRefName);
            }

            return result;
        }
*/
        #endregion // Methods
    }
}
