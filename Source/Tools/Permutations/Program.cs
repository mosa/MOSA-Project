using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mosa.Tools.Permutations
{

	class Program
	{

		public static int Main(string[] args)
		{
			Console.WriteLine();
			Console.WriteLine("Permutations v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2010. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();

			if (args.Length == 0)
			{
				Console.WriteLine("Usage: Permutations <destination directory>");
				Console.Error.WriteLine("ERROR: Missing argument");
				return -1;
			}

			string directory = args[0];

			WritePermuationsToFile(directory, "Int64Fixture", I8.GetPermutationResults());
			WritePermuationsToFile(directory, "UInt64Fixture", U8.GetPermutationResults());
			WritePermuationsToFile(directory, "Int32Fixture", I4.GetPermutationResults());
			WritePermuationsToFile(directory, "UInt32Fixture", U4.GetPermutationResults());
			WritePermuationsToFile(directory, "Int16Fixture", I2.GetPermutationResults());
			WritePermuationsToFile(directory, "UInt16Fixture", U2.GetPermutationResults());
			WritePermuationsToFile(directory, "SByteFixture", I1.GetPermutationResults());
			WritePermuationsToFile(directory, "ByteFixture", U1.GetPermutationResults());
			WritePermuationsToFile(directory, "CharFixture", Char.GetPermutationResults());
			WritePermuationsToFile(directory, "DoubleFixture", Double.GetPermutationResults());
			WritePermuationsToFile(directory, "FloatFixture", Float.GetPermutationResults());

			return 0;
		}

		private static void WritePermuationsToFile(string directory, string classname, IList<string> permutations)
		{
			File.WriteAllLines(Path.Combine(directory, classname + ".Partial.cs"), FormatCode.Format(ClassTemplate.AddClass(classname, permutations)));
		}

	}
}
