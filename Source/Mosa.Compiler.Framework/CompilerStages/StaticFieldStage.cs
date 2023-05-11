// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// Emits metadata for assemblies and types
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class StaticFieldStage : BaseCompilerStage
{
	protected override void Finalization()
	{
		foreach (var type in TypeSystem.AllTypes)
		{
			if (type.HasOpenGenericParams)
				continue;

			foreach (var field in type.Fields)
			{
				if (!field.IsStatic || field.HasOpenGenericParams)
					continue;

				if (!Compiler.MethodScanner.IsFieldAccessed(field))
					continue;

				var section = field.Data != null ? SectionKind.ROData : SectionKind.BSS;
				var size = TypeLayout.GetFieldSize(field);

				var symbol = Compiler.Linker.DefineSymbol(field.FullName, section, Architecture.NativeAlignment, size);

				if (field.Data != null)
				{
					symbol.Stream.Write(field.Data, 0, (int)size);
				}
			}
		}
	}
}
