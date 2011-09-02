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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Mosa.Compiler.Linker;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Performs memory layout of a type for compilation.
	/// </summary>
	public sealed class TypeLayoutStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage
	{
		#region Data members

		private IAssemblyLinker linker;
		private HashSet<RuntimeType> processed = new HashSet<RuntimeType>();

		#endregion // Data members

		#region IPipelineStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"TypeLayoutStage2"; } }

		#endregion // IPipelineStage

		#region IAssemblyCompilerStage members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			linker = RetrieveAssemblyLinkerFromCompiler();
		}

		void IAssemblyCompilerStage.Run()
		{
			foreach (RuntimeType type in typeSystem.GetAllTypes())
			{
				if (processed.Contains(type))
					continue;

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
				processed.Add(type);
			}
		}

		#endregion // IAssemblyCompilerStage members

		public ITypeLayout TypeLayout { get { return typeLayout; } }

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

			byte[] bitmap = new byte[(((typeLayout.Interfaces.Count - 1) / sizeof(byte)) + 1)];

			int at = 0;
			byte bit = 0;
			foreach (RuntimeType interfaceType in typeLayout.Interfaces)
			{
				if (type.Interfaces.Contains(interfaceType))
					bitmap[at] = (byte)(bitmap[at] | (byte)(1 << bit));

				bit++;
				if (bit == sizeof(byte))
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
		public void BuildMethodTable(RuntimeType type)
		{

			// HINT: The method table is offset by a four pointers:
			// 1. interface dispatch table pointer
			// 2. type pointer - contains the type information pointer, used to realize object.GetType().
			// 3. interface bitmap
			// 4. parent type (if any)
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

			IList<RuntimeMethod> methodTable = typeLayout.GetMethodTable(type);
			AskLinkerToCreateMethodTable(type.FullName + @"$mtable", methodTable, headerlinks);
		}

		private void AskLinkerToCreateMethodTable(string methodTableName, IList<RuntimeMethod> methodTable, IList<string> headerlinks)
		{
			int methodTableSize = ((headerlinks == null ? 0 : headerlinks.Count) + (methodTable == null ? 0 : methodTable.Count)) * typeLayout.NativePointerSize;

			// Debug.WriteLine("Method Table: " + methodTableName);

			using (Stream stream = linker.Allocate(methodTableName, SectionKind.Text, methodTableSize, typeLayout.NativePointerAlignment))
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
						// Debug.WriteLine("  # " + (offset / typeLayout.NativePointerSize).ToString() + " " + link);
						linker.Link(LinkType.AbsoluteAddress | LinkType.I4, methodTableName, offset, 0, link, IntPtr.Zero);
					}
					else
					{
						// Debug.WriteLine("  # " + (offset / typeLayout.NativePointerSize).ToString() + " [null]");
					}
					offset += typeLayout.NativePointerSize;
				}
			}

			if (methodTable != null)
			{
				foreach (RuntimeMethod method in methodTable)
				{
					if (!method.IsAbstract)
					{
						// Debug.WriteLine("  # " + (offset / typeLayout.NativePointerSize).ToString() + " " + method.ToString());
						linker.Link(LinkType.AbsoluteAddress | LinkType.I4, methodTableName, offset, 0, method.ToString(), IntPtr.Zero);
					}
					else
					{
						// Debug.WriteLine("  # " + (offset / typeLayout.NativePointerSize).ToString() + " [null]");
					}
					offset += typeLayout.NativePointerSize;
				}
			}
		}

		private void AskLinkerToCreateArray(string tableName, byte[] array)
		{
			int size = array.Length;

			using (Stream stream = linker.Allocate(tableName, SectionKind.Text, size, typeLayout.NativePointerAlignment))
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

			this.AllocateSpace(field, section, size, alignment);
		}

		private void AllocateSpace(RuntimeField field, SectionKind section, int size, int alignment)
		{
			using (Stream stream = linker.Allocate(field.ToString(), section, size, alignment))
			{
				if (field.RVA != 0)
				{
					InitializeStaticValueFromRVA(stream, size, field);
				}
				else
				{
					WriteDummyBytes(stream, size);
				}
			}
		}

		private void InitializeStaticValueFromRVA(Stream stream, int size, RuntimeField field)
		{
			using (Stream source = field.Module.MetadataModule.GetDataSection((long)field.RVA))
			{
				byte[] data = new byte[size];
				source.Read(data, 0, size);
				stream.Write(data, 0, size);
			}
		}

		private static void WriteDummyBytes(Stream stream, int size)
		{
			for (int i = 0; i < size; i++)
				stream.WriteByte(0);
		}

	}
}
