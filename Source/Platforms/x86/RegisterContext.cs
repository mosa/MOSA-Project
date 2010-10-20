/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
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
using System.Runtime.InteropServices;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// 
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack=1)]
	public class RegisterContext
	{
		/// <summary>
		/// 
		/// </summary>
		public uint Eax;
		/// <summary>
		/// 
		/// </summary>
		public uint Ebx;
		/// <summary>
		/// 
		/// </summary>
		public uint Ecx;
		/// <summary>
		/// 
		/// </summary>
		public uint Edx;
		/// <summary>
		/// 
		/// </summary>
		public uint Esi;
		/// <summary>
		/// 
		/// </summary>
		public uint Edi;
		/// <summary>
		/// 
		/// </summary>
		public uint Ebp;
		/// <summary>
		/// 
		/// </summary>
		public uint Eip;
		/// <summary>
		/// 
		/// </summary>
		public uint Esp;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="eax"></param>
		/// <param name="ebx"></param>
		/// <param name="ecx"></param>
		/// <param name="edx"></param>
		/// <param name="esi"></param>
		/// <param name="edi"></param>
		/// <param name="ebp"></param>
		/// <param name="eip"></param>
		/// <param name="esp"></param>
		public RegisterContext(uint eax, uint ebx, uint ecx, uint edx, uint esi, uint edi, uint ebp, uint eip, uint esp)
		{
			Eax = eax;
			Ebx = ebx;
			Ecx = ecx;
			Edx = edx;
			Esi = esi;
			Edi = edi;
			Ebp = ebp;
			Eip = eip;
			Esp = esp;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="registerContext"></param>
		public RegisterContext(RegisterContext registerContext)
		{
			this.Update(registerContext);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="registerContext"></param>
		public void Update(RegisterContext registerContext)
		{
			Eax = registerContext.Eax;
			Ebx = registerContext.Ebx;
			Ecx = registerContext.Ecx;
			Edx = registerContext.Edx;
			Esi = registerContext.Esi;
			Edi = registerContext.Edi;
			Ebp = registerContext.Ebp;
			Eip = registerContext.Eip;
			Esp = registerContext.Esp;
		}
	}
}
