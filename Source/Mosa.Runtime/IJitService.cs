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

using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime
{
	/// <summary>
	/// Allows the loader parts of the runtime to access specific jit compiler services.
	/// </summary>
	public interface IJitService
	{
		/// <summary>
		/// Sets up the jit service for the given runtime method representation.
		/// </summary>
		/// <param name="method">The method to setup jitting for.</param>
		void SetupJit(RuntimeMethod method);
	}
}
