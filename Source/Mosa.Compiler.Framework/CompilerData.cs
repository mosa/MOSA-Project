// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Compiler Data
	/// </summary>
	public sealed class CompilerData
	{
		#region Data Members

		private readonly Dictionary<MosaType, TypeData> types = new Dictionary<MosaType, TypeData>();

		private readonly Dictionary<MosaMethod, MethodData> methods = new Dictionary<MosaMethod, MethodData>();

		#endregion Data Members

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
			lock (types)
			{
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
			lock (methods)
			{
				if (!methods.TryGetValue(method, out MethodData methodData))
				{
					methodData = new MethodData(method)
					{
						HasProtectedRegions = method.ExceptionHandlers.Count != 0,
						IsCompilerGenerated = method.IsCompilerGenerated,
						HasDoNotInlineAttribute = method.IsNoInlining,
						HasAggressiveInliningAttribute = method.IsAggressiveInlining
					};

					methods.Add(method, methodData);
				}

				return methodData;
			}
		}

		#endregion Methods
	}
}
