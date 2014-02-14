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
using System.IO;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Performs memory layout of a type for compilation.
	/// </summary>
	public sealed class TypeLayoutStage : BaseCompilerStage, ICompilerStage
	{
		#region ICompilerStage members

		void ICompilerStage.Run()
		{
			foreach (var type in typeSystem.AllTypes)
			{
				if (type.IsModule || type.IsBuiltInType)
					continue;

				if (type.IsBaseGeneric || type.IsOpenGenericType)
					continue;

				if (!(type.IsObject || type.IsValueType || type.IsEnum || type.IsString || type.IsInterface || type.IsLinkerGenerated))
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

		#endregion ICompilerStage members

		private void BuildTypeInterfaceSlots(MosaType type)
		{
			if (type.Interfaces.Count == 0)
				return;

			var slots = new List<string>(typeLayout.Interfaces.Count);

			foreach (MosaType interfaceType in typeLayout.Interfaces)
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

			var bitmap = new byte[(((typeLayout.Interfaces.Count - 1) / 8) + 1)];

			int at = 0;
			byte bit = 0;
			foreach (var interfaceType in typeLayout.Interfaces)
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
			var methodTable = typeLayout.GetInterfaceTable(type, interfaceType);

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

			var methodTable = typeLayout.GetMethodTable(type);
			AskLinkerToCreateMethodTable(type.FullName + @"$mtable", methodTable, headerlinks);
		}

		private void AskLinkerToCreateMethodTable(string methodTableName, IList<MosaMethod> methodTable, IList<string> headerlinks)
		{
			int methodTableSize = ((headerlinks == null ? 0 : headerlinks.Count) + (methodTable == null ? 0 : methodTable.Count)) * typeLayout.NativePointerSize;

			using (Stream stream = compiler.Linker.Allocate(methodTableName, SectionKind.ROData, methodTableSize, typeLayout.NativePointerAlignment))
			{
				stream.Position = methodTableSize;
			}

			int offset = 0;

			if (headerlinks != null)
			{
				foreach (string link in headerlinks)
				{
					if (!string.IsNullOrEmpty(link))
					{
						compiler.Linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuiltInPatch.I4, methodTableName, offset, 0, link, 0);
					}

					offset += typeLayout.NativePointerSize;
				}
			}

			if (methodTable == null)
				return;

			foreach (var method in methodTable)
			{
				if (!method.IsAbstract)
				{
					compiler.Linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuiltInPatch.I4, methodTableName, offset, 0, method.MethodName, 0);
				}
				offset += typeLayout.NativePointerSize;
			}
		}

		private void AskLinkerToCreateArray(string tableName, byte[] array)
		{
			int size = array.Length;

			//FIXME: change  SectionKind.Text to SectionKind.ROData
			using (var stream = compiler.Linker.Allocate(tableName, SectionKind.Text, size, typeLayout.NativePointerAlignment))
			{
				foreach (byte b in array)
					stream.WriteByte(b);

				stream.Position = size;
			}
		}

		private void AllocateStaticFields(MosaType type)
		{
			foreach (var field in type.Fields)
			{
				if (field.IsStaticField && !field.IsLiteralField)
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
			architecture.GetTypeRequirements(field.Type, out size, out alignment);

			size = (int)typeLayout.GetFieldSize(field);

			// The linker section to move this field into
			SectionKind section = field.HasRVA ? section = SectionKind.Data : section = SectionKind.BSS;

			AllocateSpace(field, section, size, alignment);
		}

		private void AllocateSpace(MosaField field, SectionKind section, int size, int alignment)
		{
			using (var stream = compiler.Linker.Allocate(field.FullName, section, size, alignment))
			{
				if (field.HasRVA)
				{
					stream.Write(field.Data, 0, size);
				}
				else
				{
					stream.WriteZeroBytes(size);
				}
			}
		}
	}
}