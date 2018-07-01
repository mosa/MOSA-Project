// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace Mosa.Workspace.Generator.Debug
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var dataPath = @"..\Source\Data";
			var targetPath = @"..\Source\";

			new BuildIRInstruction(
				Path.Combine(dataPath, @"IRInstructions.json"),
				Path.Combine(targetPath, @"Mosa.Compiler.Framework\IR\"),
				"IRInstruction.cs"
			);

			new BuildIRInstructionMap(
				Path.Combine(dataPath, @"IRInstructions.json"),
				Path.Combine(targetPath, @"Mosa.Compiler.Framework\IR\"),
				"IRInstructionMap.cs"
			);

			new BuildIRInstructions(
				Path.Combine(dataPath, @"IRInstructions.json"),
				Path.Combine(targetPath, @"Mosa.Compiler.Framework\IR\")
			);
		}
	}
}
