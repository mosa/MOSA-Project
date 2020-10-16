// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// De-virtualization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class DevirtualizationStage : BaseCompilerStage
	{
		private Counter DevirtualizedMethodsCount = new Counter("DevirtualizationStage.DevirtualizedMethodsCount");

		#region Overrides

		protected override void Setup()
		{
			foreach (var type in TypeSystem.AllTypes)
			{
				// If type has an interface - don't consider either type for de-virtualization
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
			//if (CompilerOptions.Statistics)
			//{
			//	//var log = new TraceLog(TraceType.MethodCounters, null, string.Empty);
			//	//log.Log(MethodData.Counters.Export());
			//	//CompilerTrace.PostTraceLog(log);
			//}
		}

		#endregion Overrides

		private bool HasInterface(MosaType type)
		{
			return CheckBaseTypesForInterface(type) || CheckDerivedTypesForInterface(type);
		}

		private bool CheckBaseTypesForInterface(MosaType type)
		{
			var baseType = type;

			while (baseType != null)
			{
				if (baseType.Interfaces.Count >= 1)
					return true;

				baseType = baseType.BaseType;
			}

			return false;
		}

		private bool CheckDerivedTypesForInterface(MosaType type)
		{
			var derivedTypes = TypeLayout.GetDerivedTypes(type);

			if (derivedTypes == null || derivedTypes.Length == 0)
				return false;

			foreach (var dervided in derivedTypes)
			{
				if (dervided.Interfaces.Count >= 1)
					return true;

				if (CheckDerivedTypesForInterface(dervided))
					return true;
			}

			return false;
		}
	}
}
