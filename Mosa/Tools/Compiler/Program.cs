/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.IO;

using Mosa.Runtime.Loader.PE;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Loader;
using Mosa.Platforms.x86;
using Mosa.Runtime.CompilerFramework.ObjectFiles;

namespace Mosa.Tools.Compiler {
	public static class Program {
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Main(string[] args)
		{
            //CompilerScheduler.Setup(2);

            string assembly = args[0], path;

            IArchitecture architecture = Architecture.CreateArchitecture(ArchitectureFeatureFlags.AutoDetect);
            ObjectFileBuilderBase objfile = architecture.GetObjectFileBuilders()[0];
            using (CompilationRuntime cr = new CompilationRuntime())
            {
                path = Path.GetDirectoryName(assembly);
                cr.AssemblyLoader.AppendPrivatePath(path);
                AotCompiler.Compile(architecture, assembly, objfile);
            }

            //CompilerScheduler.Wait();
		}

/*
		private static void DumpMethods(Assembly peAssembly, string ns, string type)
		{
			TypeDefRow typeDefRow = new TypeDefRow(), typeDefRow2 = new TypeDefRow();
			MethodDefRow methodDefRow = new MethodDefRow();
			StringHeap strings = (StringHeap)peAssembly.Metadata.GetMetadataHeap(HeapType.UserString);
			TableHeap heap = (TableHeap)peAssembly.Metadata.GetMetadataHeap(HeapType.Tables);
			int rowCount = heap.GetRowCount(TokenTypes.TypeDef);
			Console.WriteLine("Dumping {0} type defs:", rowCount);
			for (int rowid = 0, i = 1; rowid < rowCount; rowid++, i++)
			{
				heap.GetRow((int)TokenTypes.TypeDef | rowid, ref typeDefRow);
				if (true == strings[typeDefRow.TypeNamespaceStringIdx].Equals(ns) && true == strings[typeDefRow.TypeNameStringIdx].Equals(type))
				{
					heap.GetRow((int)TokenTypes.TypeDef | (rowid+1), ref typeDefRow2);

					Console.WriteLine("Dumping type {1}.{2} with rowid {0}", rowid, ns, type);
					for (int methodid = (int)typeDefRow.MethodList; methodid < (int)typeDefRow2.MethodList; methodid++)
					{
						heap.GetRow(methodid, ref methodDefRow);
						Console.WriteLine("\t{0} at 0x{1:X08}", strings[methodDefRow.NameStringIdx], methodDefRow._rva);

						if (0 != methodDefRow._rva)
						{
							InstructionStream stream = peAssembly.GetInstructionStream(methodDefRow);
							while (false == stream.EOF)
							{
								Console.Write("\t\t{0:X08}: ", stream.Position);
								for (int j = 0; false == stream.EOF && j < 16; j++)
								{
									Console.Write("{0:X02} ", stream.ReadByte());
								}
								Console.Write("\n");
							}
						}
					}
					break;
				}
			}
		}

		private static void DumpTypeDefs(Assembly peAssembly)
		{
			TypeDefRow typeDefRow = new TypeDefRow();
			StringHeap strings = (StringHeap)peAssembly.Metadata.GetMetadataHeap(HeapType.UserString);
			TableHeap heap = (TableHeap)peAssembly.Metadata.GetMetadataHeap(HeapType.Tables);
			int rowCount = heap.GetRowCount(TokenTypes.TypeDef);
			Console.WriteLine("Dumping {0} type defs:", rowCount);
			for (int rowid = 0, i=1; rowid < rowCount; rowid++, i++)
			{
				heap.GetRow((int)TokenTypes.TypeDef | rowid, ref typeDefRow);
				Console.WriteLine("{0}: {1}.{2}", rowid, strings[typeDefRow.TypeNamespaceStringIdx], strings[typeDefRow.TypeNameStringIdx]);
				if (0 == i % 25)
					Console.ReadLine();
			}
		}

		private static void DumpStringHeap(Assembly peAssembly)
		{
			StringHeap heap = (StringHeap)peAssembly.Metadata.GetMetadataHeap(HeapType.UserString);
			string value;

			for (int pos = 0, i=0; pos < heap.Size; pos++, i++)
			{
				value = heap[pos];
				Console.WriteLine("0x{0:X08}: {1}", pos, value);
				pos += value.Length;
				//if (0 == i % 25)
				//	Console.ReadLine();

			}
		}

		private static void DumpUserStringHeap(Assembly peAssembly)
		{
			UserStringHeap heap = (UserStringHeap)peAssembly.Metadata.GetMetadataHeap(HeapType.UserString);
			string value;

			for (int pos = 1, i = 1; pos < heap.Size; i++)
			{
				value = heap[pos];
				Console.WriteLine("0x{0:X08}: {1}", pos, value);
				int length = (value.Length*2)+1;
				pos += length;
				if (length < 128)
				{
					pos++;
				}
				else if (length < 16384)
				{
					pos += 2;
				}
				else
				{
					pos += 4;
				}

				if (0 == i % 25)
					Console.ReadLine();
			}
		}
*/
	}
}