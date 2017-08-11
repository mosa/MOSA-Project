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

		private readonly Dictionary<MosaType, CompilerTypeData> types = new Dictionary<MosaType, CompilerTypeData>();

		private readonly Dictionary<MosaMethod, CompilerMethodData> methods = new Dictionary<MosaMethod, CompilerMethodData>();

		#endregion Data Members

		public IEnumerable<CompilerMethodData> MethodData
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

		public CompilerTypeData GetCompilerTypeData(MosaType type)
		{
			lock (types)
			{
				if (!types.TryGetValue(type, out CompilerTypeData compilerType))
				{
					compilerType = new CompilerTypeData(type);
					types.Add(type, compilerType);
				}

				return compilerType;
			}
		}

		public CompilerMethodData GetCompilerMethodData(MosaMethod method)
		{
			lock (methods)
			{
				if (!methods.TryGetValue(method, out CompilerMethodData compilerMethod))
				{
					compilerMethod = new CompilerMethodData(method);
					methods.Add(method, compilerMethod);
				}

				return compilerMethod;
			}
		}

		#endregion Methods
	}
}
