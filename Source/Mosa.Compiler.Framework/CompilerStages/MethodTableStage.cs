// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;

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
			NativePatchType = (TypeLayout.NativePointerSize == 4) ? PatchType.I4 : NativePatchType = PatchType.I8;
		}

		protected override void Finalization()
		{
			// Emit assembly list
			var methodLookupTable = Linker.DefineSymbol(Metadata.MethodLookupTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(methodLookupTable.Stream, Architecture.Endianness);

			// 1. Number of methods
			int count = 0;
			writer.Write((int)0);

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
						if ((!method.HasImplementation && method.IsAbstract) || method.HasOpenGenericParams || method.DeclaringType.HasOpenGenericParams)
							continue;

						if (!Compiler.MethodScanner.IsMethodInvoked(method))
							continue;

						// 1. Pointer to Method
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodLookupTable, writer.Position, method.FullName, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);

						// 2. Size of Method
						Linker.Link(LinkType.Size, NativePatchType, methodLookupTable, writer.Position, method.FullName, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);

						// 3. Pointer to Method Definition
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodLookupTable, writer.Position, Metadata.MethodDefinition + method.FullName, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);

						count++;
					}
				}
			}

			writer.Position = 0;
			writer.Write(count);
		}
	}
}
