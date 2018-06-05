// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// Emits method lookup table
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public class MethodExceptionLookupTableStage : BaseCompilerStage
	{
		#region Data Members

		private PatchType NativePatchType;

		#endregion Data Members

		protected override void Setup()
		{
			if (TypeLayout.NativePointerSize == 4)
				NativePatchType = PatchType.I4;
			else
				NativePatchType = PatchType.I8;
		}

		protected override void RunPostCompile()
		{
			CreateMethodExceptionLookupTable();
		}

		protected void CreateMethodExceptionLookupTable()
		{
			// Emit assembly list
			var methodLookupTable = Linker.CreateSymbol(Metadata.MethodExceptionLookupTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(methodLookupTable.Stream, Architecture.Endianness);

			// 1. Number of methods
			int count = 0;

			foreach (var module in TypeSystem.Modules)
			{
				foreach (var type in module.Types.Values)
				{
					if (type.IsModule)
						continue;

					var methodList = TypeLayout.GetMethodTable(type);

					if (methodList == null)
						continue;

					foreach (var method in methodList)
					{
						if (method.IsAbstract)
							continue;

						if (method.ExceptionHandlers.Count == 0)
							continue;

						count++;
					}
				}
			}
			writer.Write(count);

			foreach (var module in TypeSystem.Modules)
			{
				foreach (var type in module.Types.Values)
				{
					if (type.IsModule)
						continue;

					var methodList = TypeLayout.GetMethodTable(type);

					if (methodList == null)
						continue;

					foreach (var method in methodList)
					{
						if (method.IsAbstract)
							continue;

						if (method.ExceptionHandlers.Count == 0)
							continue;

						// 1. Pointer to Method
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodLookupTable, (int)writer.Position, SectionKind.Text, method.FullName, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);

						// 2. Size of Method
						Linker.Link(LinkType.Size, NativePatchType, methodLookupTable, (int)writer.Position, SectionKind.Text, method.FullName, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);

						// 3. Pointer to Method Definition
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodLookupTable, (int)writer.Position, SectionKind.ROData, method.FullName + Metadata.MethodDefinition, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);
					}
				}
			}
		}
	}
}
