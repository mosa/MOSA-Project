// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
using dnlib.DotNet;

namespace Mosa.Compiler.MosaTypeSystem
{
	[DebuggerDisplay("[{Module.Name}] {Token.Table} {Token.Rid}")]
	public struct ScopedToken : IEquatable<ScopedToken>
	{
		public ScopedToken(ModuleDef module, MDToken token)
		{
			this.Module = module;
			this.Token = token;
		}

		public ModuleDef Module { get; }

		public MDToken Token { get; }

		public bool Equals(ScopedToken other)
		{
			return other.Module == Module && other.Token == Token;
		}

		public override bool Equals(object obj)
		{
			return obj is ScopedToken && Equals((ScopedToken)obj);
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
