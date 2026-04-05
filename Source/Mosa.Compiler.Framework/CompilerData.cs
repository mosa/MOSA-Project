// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Compiler Data
/// </summary>
public sealed class CompilerData
{
	#region Data Members

	private readonly Compiler Compiler;

	private readonly Dictionary<MosaType, TypeData> types = new Dictionary<MosaType, TypeData>();

	private readonly Dictionary<MosaMethod, MethodData> methods = new Dictionary<MosaMethod, MethodData>();

	#endregion Data Members

	public CompilerData(Compiler compiler)
	{
		Compiler = compiler;
	}

	public IEnumerable<MethodData> MethodData
	{
		get
		{
			foreach (var method in methods)
			{
				yield return method.Value;
			}
		}
	}

	#region Methods

	public TypeData GetTypeData(MosaType type)
	{
		var lockTimer = Stopwatch.StartNew();
		lock (types)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, types, "CompilerData.types");

			if (!types.TryGetValue(type, out TypeData compilerType))
			{
				compilerType = new TypeData(type);
				types.Add(type, compilerType);
			}

			return compilerType;
		}
	}

	public MethodData GetMethodData(MosaMethod method)
	{
		var lockTimer = Stopwatch.StartNew();
		lock (methods)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, methods, "CompilerData.methods");

			if (!methods.TryGetValue(method, out MethodData methodData))
			{
				var code = method.Code;

				methodData = new MethodData(method, Compiler)
				{
					HasProtectedRegions = method.ExceptionHandlers.Count != 0,
					IsCompilerGenerated = method.IsCompilerGenerated,
					HasDoNotInlineAttribute = method.IsNoInlining,
					HasAggressiveInliningAttribute = method.IsAggressiveInlining,
					VirtualCodeSize = code == null ? 0 : code.Count
				};

				methods.Add(method, methodData);
			}

			return methodData;
		}
	}

	#endregion Methods
}
