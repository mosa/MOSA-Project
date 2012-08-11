/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;

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
			foreach (RuntimeType type in typeSystem.GetAllTypes())
			{
				if (type.ContainsOpenGenericParameters)
					continue;

				if (type.IsModule || type.IsGeneric)
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

		#endregion // ICompilerStage members

		private void BuildTypeInterfaceSlots(RuntimeType type)
		{
			if (type.Interfaces.Count == 0)
				return;

			List<string> slots = new List<string>(typeLayout.Interfaces.Count);

			foreach (RuntimeType interfaceType in typeLayout.Interfaces)
			{
				if (type.Interfaces.Contains(interfaceType))
					slots.Add(type.FullName + @"$mtable$" + interfaceType.FullName);
				else
					slots.Add(null);
			}

			AskLinkerToCreateMethodTable(type.FullName + @"$itable", null, slots);
		}

		private void BuildTypeInterfaceBitmap(RuntimeType type)
		{
			if (type.Interfaces.Count == 0)
				return;

			byte[] bitmap = new byte[(((typeLayout.Interfaces.Count - 1) / 8) + 1)];

			int at = 0;
			byte bit = 0;
			foreach (RuntimeType interfaceType in typeLayout.Interfaces)
			{
				if (type.ImplementsInterface(interfaceType))
					bitmap[at] = (byte)(bitmap[at] | (byte)(1 << bit));

				bit++;
				if (bit == 8)
				{
					bit = 0;
					at++;
				}
			}

			AskLinkerToCreateArray(type.FullName + @"$ibitmap", bitmap);
		}

		private void BuildTypeInterfaceTables(RuntimeType type)
		{
			foreach (RuntimeType interfaceType in type.Interfaces)
			{
				BuildInterfaceTable(type, interfaceType);
			}
		}

		private void BuildInterfaceTable(RuntimeType type, RuntimeType interfaceType)
		{
			RuntimeMethod[] methodTable = typeLayout.GetInterfaceTable(type, interfaceType);

			if (methodTable == null)
				return;

			AskLinkerToCreateMethodTable(type.FullName + @"$mtable$" + interfaceType.FullName, methodTable, null);
		}

		/// <summary>
		/// Builds the method table.
		/// </summary>
		/// <param name="type">The type.</param>
		private void BuildMethodTable(RuntimeType type)
		{
			// The method table is offset by a four pointers:
			// 1. interface dispatch table pointer
			// 2. type pointer - contains the type information pointer, used to realize object.GetType().
			// 3. interface implementation bitmap
			// 4. parent type (if any)
			// 5. type metadata
			List<string> headerlinks = new List<string>();

			// 1. interface dispatch table pointer
			if (type.Interfaces.Count == 0)
				headerlinks.Add(null);
			else
				headerlinks.Add(type.FullName + @"$itable");

			// 2. type pointer - contains the type information pointer, used to realize object.GetType().
			headerlinks.Add(null); // TODO: GetType()

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

			// 5. Type Metadata
			if (!type.IsModule && !(type.Module is InternalTypeModule))
				headerlinks.Add(type.FullName + @"$dtable");
			else
				headerlinks.Add(null);

			IList<RuntimeMethod> methodTable = typeLayout.GetMethodTable(type);
			AskLinkerToCreateMethodTable(type.FullName + @"$mtable", methodTable, headerlinks);
		}

		private void AskLinkerToCreateMethodTable(string methodTableName, IList<RuntimeMethod> methodTable, IList<string> headerlinks)
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
						compiler.Linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, methodTableName, offset, 0, link, IntPtr.Zero);
					}

					offset += typeLayout.NativePointerSize;
				}
			}

			if (methodTable == null)
				return;

			foreach (RuntimeMethod method in methodTable)
			{
				if (!method.IsAbstract)
				{
					//if (compiler.Scheduler.IsMethodScheduled(method))
					{
						compiler.Linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, methodTableName, offset, 0, method.FullName, IntPtr.Zero);
					}
				}
				offset += typeLayout.NativePointerSize;
			}

		}

		private void AskLinkerToCreateArray(string tableName, byte[] array)
		{
			int size = array.Length;

			//FIXME: change  SectionKind.Text to SectionKind.ROData
			using (Stream stream = compiler.Linker.Allocate(tableName, SectionKind.Text, size, typeLayout.NativePointerAlignment))
			{
				foreach (byte b in array)
					stream.WriteByte(b);

				stream.Position = size;
			}
		}

		private void AllocateStaticFields(RuntimeType type)
		{
			foreach (RuntimeField field in type.Fields)
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
		private void CreateStaticField(RuntimeField field)
		{
			Debug.Assert(field != null, @"No field given.");

			// Determine the size of the type & alignment requirements
			int size, alignment;
			architecture.GetTypeRequirements(field.SignatureType, out size, out alignment);

			size = (int)typeLayout.GetFieldSize(field);

			// The linker section to move this field into
			SectionKind section;
			// Does this field have an RVA?
			if (field.RVA != 0)
			{
				// FIXME: Move a static field into ROData, if it is read-only and can be initialized
				// using static analysis
				section = SectionKind.Data;
			}
			else
			{
				section = SectionKind.BSS;
			}

			AllocateSpace(field, section, size, alignment);
		}

		private void AllocateSpace(RuntimeField field, SectionKind section, int size, int alignment)
		{
			using (Stream stream = compiler.Linker.Allocate(field.ToString(), section, size, alignment))
			{
				if (field.RVA != 0)
				{
					InitializeStaticValueFromRVA(stream, size, field);
				}
				else
				{
					stream.WriteZeroBytes(size);
				}
			}
		}

		private void InitializeStaticValueFromRVA(Stream stream, int size, RuntimeField field)
		{
			using (Stream source = field.Module.MetadataModule.GetDataSection((long)field.RVA))
			{
				byte[] data = new byte[size];
				int wrote = source.Read(data, 0, size);

				if (wrote != size)
					throw new InvalidDataException(); // FIXME

				stream.Write(data, 0, size);
			}
		}

	}
}
