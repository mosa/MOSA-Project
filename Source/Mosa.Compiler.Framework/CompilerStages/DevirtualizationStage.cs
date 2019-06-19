// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// Devirtualization Stage
	/// </remarks>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class DevirtualizationStage : BaseCompilerStage
	{
		private Counter DevirtualizedMethodsCount = new Counter("DevirtualizationStage.DevirtualizedMethodsCount");

		#region Overrides

		protected override void Setup()
		{
			foreach (var type in TypeSystem.AllTypes)
			{
				foreach (var method in type.Methods)
				{
					if (method.IsStatic || !method.IsVirtual)
						continue;

					if (!method.HasImplementation && method.IsAbstract)
						continue;

					if (TypeLayout.IsMethodOverridden(method))
						continue;

					var methodData = Compiler.GetMethodData(method);

					methodData.IsDevirtualized = true;
					DevirtualizedMethodsCount++;
				}
			}
		}

		protected override void Finalization()
		{
			if (CompilerOptions.EnableStatistics)
			{
				//var log = new TraceLog(TraceType.MethodCounters, null, string.Empty);
				//log.Log(MethodData.Counters.Export());
				//CompilerTrace.PostTraceLog(log);
			}
		}

		#endregion Overrides
	}
}
