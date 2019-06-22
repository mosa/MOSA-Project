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
				// If type has an interface - don't consider either type for devirtualization
				// FUTURE: be more specific and check each method
				if (HasInterface(type))
					continue;

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

		private bool HasInterface(MosaType type)
		{
			var baseType = type;

			while (baseType != null)
			{
				foreach (var interfaceType in baseType.Interfaces)
				{
					return true;
				}

				baseType = baseType.BaseType;
			}

			return false;
		}
	}
}
