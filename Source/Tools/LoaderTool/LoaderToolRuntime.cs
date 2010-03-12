
using System;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;
using Mosa.Runtime;
using Mosa.Runtime.Memory;

namespace Mosa.Tools.LoaderTool
{
	internal class LoaderToolRuntime : RuntimeBase
    {
        private ITypeSystem typeSystem;
        
        private IAssemblyLoader assemblyLoader;
    
        public LoaderToolRuntime()
        {
            this.typeSystem = new DefaultTypeSystem();
            this.assemblyLoader = new AssemblyLoader(this.typeSystem);
        }
		
		public override IMemoryPageManager MemoryManager 
		{
			get 
			{
				throw new System.NotImplementedException ();
			}
		}
		
		public override IAssemblyLoader AssemblyLoader 
		{
			get 
			{
				return this.assemblyLoader;
			}
		}


		public override IJitService JitService 
		{
			get 
			{
				throw new NotImplementedException();
			}
		}
		
		public override ITypeSystem TypeLoader 
		{
			get 
			{
				return this.typeSystem;
			}
		}
    }
}
