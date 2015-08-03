// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Emits method lookup table
	/// </summary>
	public class MethodLookupTableStage : BaseCompilerStage
	{
		#region Data members

		private PatchType NativePatchType;

		#endregion Data members

		protected override void Setup()
		{
			if (TypeLayout.NativePointerSize == 4)
				NativePatchType = BuiltInPatch.I4;
			else
				NativePatchType = BuiltInPatch.I8;
		}

		protected override void Run()
		{
			CreateMethodLookupTable();
		}

		protected void CreateMethodLookupTable()
		{
			// Emit assembly list
			var methodLookupTable = Linker.CreateSymbol(Metadata.MethodLookupTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
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

						// 1. Pointer to Method
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodLookupTable, (int)writer.Position, 0, method.FullName, SectionKind.Text, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);

						// 2. Size of Method
						Linker.Link(LinkType.Size, NativePatchType, methodLookupTable, (int)writer.Position, 0, method.FullName, SectionKind.Text, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);

						// 3. Pointer to Method Definition
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodLookupTable, (int)writer.Position, 0, method.FullName + Metadata.MethodDefinition, SectionKind.ROData, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);
					}
				}
			}
		}
	}
}