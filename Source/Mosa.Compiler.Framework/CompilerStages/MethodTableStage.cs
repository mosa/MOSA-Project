// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.IO;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// Emits method lookup table
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public class MethodTableStage : BaseCompilerStage
	{
		#region Data Members

		private PatchType NativePatchType;

		#endregion Data Members

		protected override void Initialization()
		{
			NativePatchType = (TypeLayout.NativePointerSize == 4) ? PatchType.I32 : NativePatchType = PatchType.I64;
		}

		protected override void Finalization()
		{
			// Emit assembly list
			var methodLookupTable = Linker.DefineSymbol(Metadata.MethodLookupTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new BinaryWriter(methodLookupTable.Stream);

			// 1. Number of methods
			int count = 0;
			writer.Write(0);

			foreach (var module in TypeSystem.Modules)
			{
				foreach (var type in module.Types.Values)
				{
					if (type.IsModule)
						continue;

					if (!Compiler.MethodScanner.IsTypeAllocated(type))
						continue;

					var methodList = TypeLayout.GetMethodTable(type);

					if (methodList == null)
						continue;

					foreach (var method in methodList)
					{
						var targetMethodData = GetTargetMethodData(method);

						if (!targetMethodData.HasCode)
							continue;

						// 1. Pointer to Method
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodLookupTable, writer.GetPosition(), targetMethodData.Method.FullName, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);

						// 2. Size of Method
						Linker.Link(LinkType.Size, NativePatchType, methodLookupTable, writer.GetPosition(), targetMethodData.Method.FullName, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);

						// 3. Pointer to Method Definition
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodLookupTable, writer.GetPosition(), Metadata.MethodDefinition + method.FullName, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);

						count++;
					}
				}
			}

			writer.SetPosition(0);
			writer.Write(count);
		}

		private MethodData GetTargetMethodData(MosaMethod method)
		{
			var methodData = Compiler.GetMethodData(method);

			if (methodData.ReplacedBy == null)
				return methodData;

			return Compiler.GetMethodData(methodData.ReplacedBy);
		}
	}
}
