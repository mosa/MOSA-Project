/*
 * (c) 20010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Internal
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExceptionEngine
	{

		// HACK: Add compiler architecture to the runtime
		private const uint nativeIntSize = 4;

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
		public unsafe static void ThrowException(uint edi, uint esi, uint ebp, uint esp, uint ebx, uint edx, uint ecx, uint eax, uint exception)
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
			Native.RestoreContext(context.Ebp, context.Esp, context.Eip);
			//Native.RestoreContext(context.edi, context.esi, context.ebp, context.esp, context.ebx, context.edx, context.ecx, context.eax);
		}

		/// <summary>
		/// Tries to handle the exception by searching the method header for a suitable exception handler/filter.
		/// </summary>
		/// <param name="registerContext">The register status right before the throw</param>
		/// <param name="exception">The thrown exception</param>
		/// <param name="eip">EIP to return to when handled</param>
		private unsafe static void HandleException(RegisterContext registerContext, uint exception, uint eip)
		{
			// Get the method lookup entry
			uint methodLookupEntry = GetMethodLookupEntry(registerContext.Eip);

			if (methodLookupEntry == 0)
			{
				// We're not in a compiled method - where are we?

				// Go panic!
			}

			// Find the method description
			uint methodDescription = GetMethodDescription(methodLookupEntry);

			// Get the protected block table
			uint exceptionHandlerTable = GetExceptionHandlerTable(methodDescription);

			if (exceptionHandlerTable == 0)
			{
				// Method does not have any protected blocks

				// At-the-moment: All methods have this table, even if the method doesn't have any exceptions.

				// Go panic!

				// TODO
				// 1. Unwind stack 
				//   A. If at top of stack then notify kernel to terminate this thread
				// 2. Update EIP of calling method 
				// 3. Re-execute this method to look for a protected block
			}

			// Get the protected block
			uint protectedBlock = GetProtectedBlock(exceptionHandlerTable, eip, 0);

			if (protectedBlock == 0)
			{
				// No protected block

				// TODO
				// 1. Unwind stack 
				//   A. If at top of stack then notify kernel to terminate this thread
				// 2. Update EIP of calling method 
				// 3. Re-execute this method to look for a protected block
			}

			uint* entry = (uint*)protectedBlock;
			uint exceptionType = entry[0];
			uint handlerOffset = entry[3];

			uint methodStart = GetMethodStartAddress(methodLookupEntry);

			uint handler = methodStart + handlerOffset;

			// TODO:

			if (exceptionType == 0) // exception handler type
			{
				// TODO
				// Set next EIP to exception handler
				// Place exception object in EDX
				// Restore Context (e.g. execute return)

			}
			else if (exceptionType == 2) // finally handler type
			{
				// TODO
				// Set next EIP to exception handler
				// Call finally handler (with return address on stack to resume search for next protected block or exception)
			}
			else
			{
				// Go panic!
			}

		}

		public unsafe static uint GetMethodLookupEntry(uint methodLookupTable)
		{
			uint* entry = (uint*)Native.GetMethodLookupTable(methodLookupTable);

			while (entry[0] != 0)
			{
				if (entry[0] >= methodLookupTable)
				{
					if ((entry[0] + entry[1]) < methodLookupTable)
					{
						return (uint)entry;
					}
				}

				entry = entry + (nativeIntSize * 3);
			}

			return 0;
		}

		public unsafe static uint GetMethodDescription(uint methodLookupEntry)
		{
			return ((uint*)methodLookupEntry)[2];
		}

		public unsafe static uint GetMethodStartAddress(uint methodLookupEntry)
		{
			return ((uint*)methodLookupEntry)[0];
		}

		public unsafe static uint GetExceptionHandlerTable(uint methodDscr)
		{
			return ((uint*)GetMethodDescription(methodDscr))[0];
		}

		public unsafe static uint GetProtectedBlock(uint protectedBlockTable, uint eip, uint exception)
		{
			uint* entry = (uint*)protectedBlockTable;

			while (true)
			{
				uint start = entry[1];

				// check end of table
				if (start == 0)
					return 0;

				uint length = entry[2];

				if (eip >= start && eip < start + length)
				{
					uint exceptionMethod = entry[4];

					if (exception == 0)
					{
						return (uint)entry;
					}

					if (Runtime.IsInstanceOfType(exceptionMethod, exception) != 0)
					{
						return (uint)entry;
					}
				}

				entry = entry + (nativeIntSize * 6);
			}

		}

		/// <summary>
		/// Identify the appropriate handler for the thrown exception
		/// </summary>
		/// <param name="registerContext">The register status right before the throw</param>
		/// <param name="exception">The thrown exception</param>
		/// <param name="eip">EIP to return to when handled</param>
		private static void HandlerIdentificationPass(RegisterContext registerContext, uint exception, uint eip)
		{
		}

		/// <summary>
		/// Call the appropriate handler or rethrow the exception
		/// </summary>
		/// <param name="registerContext">The register status right before the throw</param>
		/// <param name="exception">The thrown exception</param>
		/// <param name="eip">EIP to return to when handled</param>
		private static void HandlerInvocationPass(RegisterContext registerContext, uint exception, uint eip)
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
