﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using System.Diagnostics;

namespace Mosa.Compiler.MosaTypeSystem.CLR.Utils
{
	[DebuggerDisplay("[{Module.Name}] {Token.Table} {Token.Rid}")]
	public readonly struct ScopedToken : IEquatable<ScopedToken>
	{
		public ScopedToken(ModuleDef? module, MDToken token)
		{
			Module = module;
			Token = token;
		}

		public ModuleDef? Module { get; }

		public MDToken Token { get; }

		public bool Equals(ScopedToken other)
		{
			return other.Module == Module && other.Token == Token;
		}

		public override bool Equals(object? obj)
		{
			return obj is ScopedToken token && Equals(token);
		}

		public override int GetHashCode()
		{
			return Module.GetHashCode() * 7 + (int)Token.Raw;
		}

		public override string ToString()
		{
			return $"[{Module?.Name}] 0x{Token:x8}";
		}
	}
}
