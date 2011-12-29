/*
 * (c) 20010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExceptionEngine
	{
		/// <summary>
		/// Saves the context and handles the exceptions.
		/// </summary>
		/// <param name="edi">The edi.</param>
		/// <param name="esi">The esi.</param>
		/// <param name="ebp">The ebp.</param>
		/// <param name="esp">The esp.</param>
		/// <param name="ebx">The ebx.</param>
		/// <param name="edx">The edx.</param>
		/// <param name="ecx">The ecx.</param>
		/// <param name="eax">The eax.</param>
		public unsafe static void ThrowException(uint edi, uint esi, uint ebp, uint esp, uint ebx, uint edx, uint ecx, uint eax, Exception exception)
		{
			// EIP is on the stack

			// Two ways to get this:
			// 1. As an offset from the ESP stored on the stack; offset is -36 (just after the 9 x 32-bit values that the pushad operand pushed)
			uint eip = (uint)((uint*)(esp - (9 * 4)));

			// 2. As an offset from GetEBP(); offset is +4 (the EIP stored by the CALL operand)
			uint eip2 = (uint)((uint*)(Native.GetEBP() + 4));

			// Create context
			RegisterContext registerContext = new RegisterContext(eax, ebx, ecx, edx, esi, edi, ebp, eip, esp);

			// Try to handle the exception
			HandleException(registerContext, exception, eip);

			// Return after exception has been handled
			RestoreContext(registerContext);
		}

		/// <summary>
		/// Restores the context by loading the values from the given context.
		/// </summary>
		/// <param name="context">The register context to restore the state from</param>
		private static void RestoreContext(RegisterContext context)
		{
			//Native.RestoreContext();
		}

		/// <summary>
		/// Tries to handle the exception by searching the method header for a suitable exception handler/filter.
		/// </summary>
		/// <param name="registerContext">The register status right before the throw</param>
		/// <param name="exception">The thrown exception</param>
		/// <param name="eip">EIP to return to when handled</param>
		private static void HandleException(RegisterContext registerContext, Exception exception, uint eip)
		{
			HandlerIdentificationPass(registerContext, exception, eip);
			HandlerInvocationPass(registerContext, exception, eip);
		}

		/// <summary>
		/// Identify the appropriate handler for the thrown exception
		/// </summary>
		/// <param name="registerContext">The register status right before the throw</param>
		/// <param name="exception">The thrown exception</param>
		/// <param name="eip">EIP to return to when handled</param>
		private static void HandlerIdentificationPass(RegisterContext registerContext, Exception exception, uint eip)
		{




		}

		/// <summary>
		/// Call the appropriate handler or rethrow the exception
		/// </summary>
		/// <param name="registerContext">The register status right before the throw</param>
		/// <param name="exception">The thrown exception</param>
		/// <param name="eip">EIP to return to when handled</param>
		private static void HandlerInvocationPass(RegisterContext registerContext, Exception exception, uint eip)
		{
		}

		/// <summary>
		/// Calls the given filter to handle the exception
		/// </summary>1
		/// <param name="registerContext">The register status right before the throw</param>
		/// <param name="exceptionInformation">The exception handler information</param>
		private static void CallFilter(RegisterContext registerContext, object exceptionInformation)
		{
			//Native.CallFilter();
		}
	}
}
