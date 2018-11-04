// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Workspace.Experiment.Debug
{
	internal static class Program
	{
		//private static void Test1()
		//{
		//	var map = new SymbolDictionary();

		//	map.Add(IRInstructionList.List);

		//	var match = new List<string> {
		//		"(IR.MulUnsigned 1 x)",
		//		"(MulUnsigned 1 x)",
		//		"(IR.AddUnsigned32(IR.MulUnsigned x y)(IR.MulUnsigned x z))",
		//		"(AddUnsigned32(MulUnsigned x y)(MulUnsigned x z))",
		//		"(MulUnsigned x (AddUnsigned32 y z))",
		//		"(MulUnsigned 1 x)",
		//		"x",
		//		"(MulUnsigned (Const c1) (Const c2))",
		//		"[c1 * c2]",
		//		"(MulUnsigned 1 2) ",
		//		"[1 * 2]"
		//	};

		//	var tokenized = new List<List<Token>>();

		//	foreach (var m in match)
		//	{
		//		tokenized.Add(Tokenizer.Parse(m, m.StartsWith("(") ? ParseType.Instructions : ParseType.Expression));
		//	}

		//	return;
		//}

		//private static void Test()
		//{
		//	//var tree = ExpressionTest.GetTestExpression2();
		//	//var basicBlocks = ExpressionTest.CreateBasicBlockInstructionSet();

		//	//var match = tree.Transform(basicBlocks[0].Last.Previous, null);

		//	//ExpressionTest.GetTestExpression5();
		//	//ExpressionTest.GetTestExpression4();
		//	//ExpressionTest.GetTestExpression3();
		//	//ExpressionTest.GetTestExpression2();

		//	return;
		//}

		private static void Main()
		{
			var files = Directory.GetFiles(@"X:\CF3\testsuite", "test1.c", SearchOption.AllDirectories);

			foreach (var file in files)
			{
				var testname = file.Replace(@"testsuite\EXP_", "Exp").Replace(@"\test", "Test").Replace("_", "Test");

				testname = Path.GetFileNameWithoutExtension(testname);

				var newfilename = @"X:\MOSA-Project-tgiphil\Source\Mosa.UnitTest.CF3\" + testname + ".cs";

				Console.WriteLine(file);

				Transform(file, newfilename, testname);
			}
		}

		private static void Transform(string file, string newfile, string testname)
		{
			var transforms = new List<(string, string)>
			{
				("uint8_t ", "byte "),
				("uint16_t ", "ushort "),
				("uint32_t ", "uint "),
				("uint64_t ", "ulong "),
				("int8_t ", "sbyte "),
				("int16_t ", "short "),
				("int32_t ", "int "),
				("int64_t ", "long "),
				("UINT8_MAX", "byte.MaxValue"),
				("UINT16_MAX", "ushort.MaxValue"),
				("UINT32_MAX", "uint.MaxValue"),
				("UINT64_MAX", "ulong.MaxValue"),
				("INT8_MIN", "sbyte.MinValue"),
				("INT16_MIN", "short.MinValue"),
				("INT32_MIN", "int.MinValue"),
				("INT64_MIN", "long.MinValue"),
				("INT8_MAX", "sbyte.MaxValue"),
				("INT16_MAX", "short.MaxValue"),
				("INT32_MAX", "int.MaxValue"),
				("INT64_MAX", "long.MaxValue"),
				("(void)", "()"),

				//("static ",""),
				("volatile ",""),

				//("private const static","private static"),

				("LLU;","UL;"),
				("LL;","L;"),
				("LLU)","UL)"),
				("LL)","L)"),

				("void ","public static bool "),
				("    ","\t"),
				("{ NG(); } else { ; }","{ return false; } else { return true; }"),
			};

			var lines = File.ReadAllLines(file);
			var newlines = new List<string>(lines.Length + 10);
			bool start = true;

			foreach (var line in lines)
			{
				var newline = line;

				if (start && newline.Contains("test1.h"))
				{
					newlines.Add("using System;");
					newlines.Add("");
					newlines.Add("namespace Mosa.UnitTest.Collection");
					newlines.Add("{");
					newlines.Add("\tpublic static class " + testname);
					newlines.Add("\t{");
					newlines.Add("");
					continue;
				}
				else if (newline.StartsWith("#include"))
				{
					continue;
				}
				else if (newline.Contains("int main(void)"))
				{
					break;
				}

				if (start && newline.Contains(" = "))
				{
					if (newline.Contains("static"))
					{
						newline = "\t\tprivate " + newline;
					}
					else
					{
						newline = "\t\tprivate const " + newline;
					}
				}

				if (!start && newline.Contains(" = "))
				{
					newline = newline.Replace("static ", "");
				}

				if (newline.Contains("(void)"))
				{
					start = false;
				}

				foreach (var t in transforms)
				{
					newline = newline.Replace(t.Item1, t.Item2);
				}

				if (newline.Contains("U;"))
				{
					if (newline.Contains("ushort"))
					{
						newline = newline.Replace("U;", ";");
					}
					if (newline.Contains("byte"))
					{
						newline = newline.Replace("U;", ";");
					}
				}
				newlines.Add(newline);
			}

			newlines.Add("\t}");
			newlines.Add("}");

			File.WriteAllLines(newfile, newlines);
		}
	}
}
