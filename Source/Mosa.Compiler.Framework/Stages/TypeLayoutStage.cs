/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Performs memory layout of a type for compilation.
	/// </summary>
	public sealed class TypeLayoutStage : BaseCompilerStage
	{
		protected override void Run()
		{
			foreach (var type in TypeSystem.AllTypes)
			{
				if (type.IsModule)
					continue;

				if (type.HasOpenGenericParams)
					continue;

				if (type.BaseType == null && !type.IsInterface && type.FullName != "System.Object")   // ghost types like generic params, function ptr, etc.
					continue;

				if (!type.IsInterface)
				{
					BuildMethodTable(type);
					BuildTypeInterfaceSlots(type);
					BuildTypeInterfaceBitmap(type);
					BuildTypeInterfaceTables(type);
				}

				AllocateStaticFields(type);
			}
		}

		private void BuildTypeInterfaceSlots(MosaType type)
		{
			if (type.Interfaces.Count == 0)
				return;

			var slots = new List<string>(TypeLayout.Interfaces.Count);

			foreach (MosaType interfaceType in TypeLayout.Interfaces)
			{
				if (type.Interfaces.Contains(interfaceType))
					slots.Add(type.FullName + @"$mtable$" + interfaceType.FullName);
				else
					slots.Add(null);
			}

			AskLinkerToCreateMethodTable(type.FullName + @"$itable", null, slots);
		}

		private void BuildTypeInterfaceBitmap(MosaType type)
		{
			if (type.Interfaces.Count == 0)
				return;

			var bitmap = new byte[(((TypeLayout.Interfaces.Count - 1) / 8) + 1)];

			int at = 0;
			byte bit = 0;
			foreach (var interfaceType in TypeLayout.Interfaces)
			{
				if (type.Interfaces.Contains(interfaceType))
				{
					bitmap[at] = (byte)(bitmap[at] | (byte)(1 << bit));
				}

				bit++;
				if (bit == 8)
				{
					bit = 0;
					at++;
				}
			}

			AskLinkerToCreateArray(type.FullName + @"$ibitmap", bitmap);
		}

		private void BuildTypeInterfaceTables(MosaType type)
		{
			foreach (var interfaceType in type.Interfaces)
			{
				BuildInterfaceTable(type, interfaceType);
			}
		}

		private void BuildInterfaceTable(MosaType type, MosaType interfaceType)
		{
			var methodTable = TypeLayout.GetInterfaceTable(type, interfaceType);

			if (methodTable == null)
				return;

			AskLinkerToCreateMethodTable(type.FullName + @"$mtable$" + interfaceType.FullName, methodTable, null);
		}

		/// <summary>
		/// Builds the method table.
		/// </summary>
		/// <param name="type">The type.</param>
		private void BuildMethodTable(MosaType type)
		{
			// The method table is offset by a four pointers:
			// 1. interface dispatch table pointer
			// 2. type metadata pointer - contains the type metadata pointer, used to realize object.GetType().
			// 3. interface implementation bitmap
			// 4. parent type (if any)
			var headerlinks = new List<string>();

			// 1. interface dispatch table pointer
			if (type.Interfaces.Count == 0)
				headerlinks.Add(null);
			else
				headerlinks.Add(type.FullName + @"$itable");

			// 2. type metadata pointer - contains the type metadata pointer, used to realize object.GetType().
			if (!type.IsModule)
				headerlinks.Add(type.FullName + @"$dtable");
			else
				headerlinks.Add(null);

			// 3. interface bitmap
			if (type.Interfaces.Count == 0)
				headerlinks.Add(null);
			else
				headerlinks.Add(type.FullName + @"$ibitmap");

			// 4. parent type (if any)
			if (type.BaseType == null)
				headerlinks.Add(null);
			else
				headerlinks.Add(type.BaseType + @"$mtable");

			var methodTable = TypeLayout.GetMethodTable(type);
			AskLinkerToCreateMethodTable(type.FullName + @"$mtable", methodTable, headerlinks);
		}

		private void AskLinkerToCreateMethodTable(string methodTableName, IList<MosaMethod> methodTable, IList<string> headerlinks)
		{
			int methodTableSize = ((headerlinks == null ? 0 : headerlinks.Count) + (methodTable == null ? 0 : methodTable.Count)) * TypeLayout.NativePointerSize;

			var methodtable = Compiler.Linker.CreateSymbol(methodTableName, SectionKind.ROData, TypeLayout.NativePointerAlignment, methodTableSize);

			int offset = 0;

			if (headerlinks != null)
			{
				foreach (string link in headerlinks)
				{
					if (!string.IsNullOrEmpty(link))
					{
						Compiler.Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, methodtable, offset, 0, link, SectionKind.ROData, 0);
					}

					offset += TypeLayout.NativePointerSize;
				}
			}

			if (methodTable == null)
				return;

			foreach (var method in methodTable)
			{
				if (!method.IsAbstract)
				{
					Compiler.Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, methodtable, offset, 0, method.FullName, SectionKind.Text, 0);
				}
				offset += TypeLayout.NativePointerSize;
			}
		}

		private void AskLinkerToCreateArray(string tableName, byte[] array)
		{
			var symbol = Compiler.Linker.CreateSymbol(tableName, SectionKind.ROData, TypeLayout.NativePointerAlignment, array.Length);
			symbol.Stream.Write(array);
		}

		private void AllocateStaticFields(MosaType type)
		{
			foreach (var field in type.Fields)
			{
				// TODO: Inline literal field constants
				if (field.IsStatic)
				{
					// Assign a memory slot to the static & initialize it, if there's initial data set
					CreateStaticField(field);
				}
			}
		}

		/// <summary>
		/// Allocates memory for the static field and initializes it.
		/// </summary>
		/// <param name="field">The field.</param>
		private void CreateStaticField(MosaField field)
		{
			// Determine the size of the type & alignment requirements
			int size, alignment;
			Architecture.GetTypeRequirements(TypeLayout, field.FieldType, out size, out alignment);

			size = (int)TypeLayout.GetFieldSize(field);

			// The linker section to move this field into
			SectionKind section = field.Data != null ? section = SectionKind.Data : section = SectionKind.BSS;

			AllocateSpace(field, section, size, alignment);
		}

		private void AllocateSpace(MosaField field, SectionKind section, int size, int alignment)
		{
			var symbol = Compiler.Linker.CreateSymbol(field.FullName, section, alignment, size);

			if (field.Data != null)
			{
				symbol.Stream.Write(field.Data, 0, size);
			}
		}
	}
}