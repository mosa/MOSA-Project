/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.CompilerFramework
{
  /// <summary>
  /// Interface provided by stack layout stages to provide code generation with stack reservation knowledge.
  /// </summary>
  public interface IStackLayoutProvider
  {
	/// <summary>
	/// Retrieves the total size of local variables on the stack in bytes as used by the compiled method.
	/// </summary>
	int LocalsSize { get; }
  }
}

