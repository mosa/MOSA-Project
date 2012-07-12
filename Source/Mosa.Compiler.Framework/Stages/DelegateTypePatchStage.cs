/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.TypeSystem.Cil;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Emits metadata for assemblies and types
	/// </summary>
	public sealed class DelegateTypePatchStage : BaseCompilerStage, ICompilerStage
	{
		
		#region ICompilerStage members
		
		void ICompilerStage.Run()
		{
			DelegateTypePatcher delegateTypePatcher = new DelegateTypePatcher(typeSystem, architecture.PlatformName);

			foreach (var type in typeSystem.GetAllTypes())
			{
				if (type.IsDelegate && type.FullName != "System.Delegate" && type.FullName != "System.MulticastDelegate")
				{
					delegateTypePatcher.PatchType(type);

					//compiler.Scheduler.TrackTypeAllocated(type);
					foreach (var method in type.Methods)
					{
						compiler.Scheduler.TrackMethodInvoked(method);
					}
				}
			}
		}

		#endregion // ICompilerStage members

	}
}
