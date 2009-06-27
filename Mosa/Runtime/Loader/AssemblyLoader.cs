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
using System.IO;
using System.Text;

using Mosa.Runtime.Loader.PE;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Loader
{
    /// <summary>
    /// Provides a default implementation of the IAssemblyLoader interface.
    /// </summary>
    public class AssemblyLoader : IAssemblyLoader
    {
        #region Data members

        private string[] _searchPath;
        private List<string> _privatePaths = new List<string>();
        private List<IMetadataModule> _loadedImages = new List<IMetadataModule>();
        private ITypeSystem _typeLoader;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
        /// </summary>
        /// <param name="typeLoader">The type loader.</param>
        public AssemblyLoader(ITypeSystem typeLoader)
        {
            // HACK: I can't figure out an easier way to get the framework dir right now...
            string frameworkDir = Path.GetDirectoryName(typeof(System.Object).Assembly.Location);

            _searchPath = new string[] {
                AppDomain.CurrentDomain.BaseDirectory,
                frameworkDir
            };

            _typeLoader = typeLoader;
        }

        #endregion // Construction

        #region IAssemblyLoader Members

        IEnumerable<IMetadataModule> IAssemblyLoader.Modules
        {
            get { return _loadedImages; }
        }

        void IAssemblyLoader.AppendPrivatePath(string path)
        {
            _privatePaths.Add(path);
        }

        IMetadataModule IAssemblyLoader.Resolve(IMetadataProvider provider, AssemblyRefRow assemblyRef)
        {
            IMetadataModule result = null;
            string name;
            provider.Read(assemblyRef.NameIdx, out name);

            result = GetLoadedAssembly(name);
            if (null == result)
                result = DoLoadAssembly(name + ".dll");
            if (null == result)
                throw new TypeLoadException();

            return result;
        }

        IMetadataModule IAssemblyLoader.Load(string file)
        {
            IMetadataModule result = null;

            if (!File.Exists(file))
            {
                if (!Path.IsPathRooted(file))
                    file = String.Format(@"{0}{1}{2}", Environment.CurrentDirectory, Path.DirectorySeparatorChar, file);
                else
                    return null;
            }

            if (File.Exists(file))
            {
                result = PortableExecutableImage.Load(_loadedImages.Count, new FileStream(file, FileMode.Open, FileAccess.Read));
                if (null != result)
                {
                    lock (_loadedImages)
                    {
                        _loadedImages.Add(result);
                    }

                    _typeLoader.AssemblyLoaded(result);
                }
            }

            return result;
        }

        void IAssemblyLoader.Unload(IMetadataModule module)
        {
            IDisposable disp = module as IDisposable;
            if (null != disp)
                disp.Dispose();
        }

        #endregion // IAssemblyLoader Members

        #region Internals

        private IMetadataModule GetLoadedAssembly(string name)
        {
            IMetadataModule result = null;
            foreach (IMetadataModule image in _loadedImages)
            {
                if (true == name.Equals(image.Name))
                {
                    result = image;
                    break;
                }
            }
            return result;
        }

        private IMetadataModule DoLoadAssembly(string name)
        {
            IMetadataModule result = DoLoadAssemblyFromPaths(name, _privatePaths);
            if (null == result)
                result = DoLoadAssemblyFromPaths(name, _searchPath);

            return result;
        }

        private IMetadataModule DoLoadAssemblyFromPaths(string name, IEnumerable<string> paths)
        {
            IMetadataModule result = null;
            string fullName;

            foreach (string path in paths)
            {
                fullName = Path.Combine(path, name);
                try
                {
                    result = (this as IAssemblyLoader).Load(fullName);
                    if (null != result)
                        break;
                }
                catch
                {
                    /* Failed to load assembly there... */
                }
            }

            return result;
        }

        #endregion // Internals
    }
}
