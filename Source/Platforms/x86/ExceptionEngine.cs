/*
 * (c) 20010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExceptionEngine
	{
		/// <summary>
		/// Saves the context and handles the excpetion.
		/// </summary>
		/// <param name="eax">Current status of eax</param>
		/// <param name="ebx">Current status of ebx</param>
		/// <param name="ecx">Current status of ecx</param>
		/// <param name="edx">Current status of edx</param>
		/// <param name="esi">Current status of esi</param>
		/// <param name="edi">Current status of edi</param>
		/// <param name="ebp">Current status of ebp</param>
		/// <param name="exception">The exception object</param>
		/// <param name="eip">Current status of eip</param>
		/// <param name="esp">Current status of esp</param>
		public static void ThrowException(uint eax, uint ebx, uint ecx, uint edx, uint esi, uint edi, uint ebp, Exception exception, uint eip, uint esp)
		{
			// Read callee's EIP from method header
			eip = Native.GetEip();
			// Create context
			RegisterContext registerContext = new RegisterContext(eax, ebx, ecx, edx, esi, edi, ebp, eip, esp + 40);

			// Return after exception has been handled
			RestoreContext(registerContext);
		}

		/// <summary>
		/// Restores the context by loading the values from the given context.
		/// </summary>
		/// <param name="context">The register context to restore the state from</param>
		public static void RestoreContext(RegisterContext context)
		{
			Native.RestoreContext();
		}
	}
}
