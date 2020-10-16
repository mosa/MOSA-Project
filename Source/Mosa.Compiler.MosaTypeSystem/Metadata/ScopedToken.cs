// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using System;
using System.Diagnostics;

namespace Mosa.Compiler.MosaTypeSystem
{
	[DebuggerDisplay("[{Module.Name}] {Token.Table} {Token.Rid}")]
	public struct ScopedToken : IEquatable<ScopedToken>
	{
		public ScopedToken(ModuleDef module, MDToken token)
		{
			this.module = module;
			this.token = token;
		}

		private readonly ModuleDef module;

		public ModuleDef Module { get { return module; } }

		private readonly MDToken token;

		public MDToken Token { get { return token; } }

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
