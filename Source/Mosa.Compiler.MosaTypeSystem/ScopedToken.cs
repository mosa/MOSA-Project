/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System;
using System.Diagnostics;
using dnlib.DotNet;

namespace Mosa.Compiler.MosaTypeSystem
{
	[DebuggerDisplay("[{Module.Name}] {Token.Table} {Token.Rid}")]
	public struct ScopedToken : IEquatable<ScopedToken>
	{
		public ScopedToken(ModuleDefMD module, MDToken token)
		{
			this.module = module;
			this.token = token;
		}

		private readonly ModuleDefMD module;
		public ModuleDefMD Module { get { return module; } }

		private readonly MDToken token;
		public MDToken Token { get { return token; } }

		public bool Equals(ScopedToken other)
		{
			return other.Module == this.Module && other.Token == this.Token;
		}

		public override bool Equals(object obj)
		{
			return obj is ScopedToken && this.Equals((ScopedToken)obj);
		}

		public override int GetHashCode()
		{
			return Module.GetHashCode() * 7 + (int)Token.Raw;
		}

		public override string ToString()
		{
			return string.Format("[{0}] 0x{1:x8}", Module.Name, Token);
		}
	}
}
