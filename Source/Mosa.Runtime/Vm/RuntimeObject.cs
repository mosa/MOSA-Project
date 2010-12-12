/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Metadata.Loader;

namespace Mosa.Runtime.Vm
{
	/// <summary>
	/// Base class of all runtime type system objects.
	/// </summary>
	public abstract class RuntimeObject
	{
		#region Data members

		/// <summary>
		/// Holds the token of the object.
		/// </summary>
		private int token;

		/// <summary>
		/// Holds the module, which owns the member.
		/// </summary>
		protected IModuleTypeSystem moduleTypeSystem;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="RuntimeObject"/>.
		/// </summary>
		/// <param name="token">The runtime token of this metadata.</param>
		protected RuntimeObject(IModuleTypeSystem moduleTypeSystem, int token)
		{
			this.token = token;
			this.moduleTypeSystem = moduleTypeSystem;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the token of the object.
		/// </summary>
		public int Token
		{
			get { return token; }
		}

		/// <summary>
		/// Retrieves the module, which holds this member.
		/// </summary>
		public IMetadataModule MetadataModule
		{
			get { return moduleTypeSystem.MetadataModule; }
		}

		/// <summary>
		/// Retrieves the module, which holds this member.
		/// </summary>
		public IModuleTypeSystem ModuleTypeSystem
		{
			get { return moduleTypeSystem; }
		}

		#endregion // Properties
	}
}
