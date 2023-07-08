// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// An compilation stage, which generates a map file of the built binary file.
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public class InlinedFileStage : BaseCompilerStage
{
	protected override void Finalization()
	{
		if (string.IsNullOrEmpty(MosaSettings.InlinedFile))
			return;

		var methods = GetAndSortMethodData();

		using (var writer = new StreamWriter(MosaSettings.InlinedFile))
		{
			writer.WriteLine("Method\tCompilations");

			foreach (var data in methods)
			{
				writer.WriteLine($"{InlinedSetupStage.RemoveReturnValue(data.Method.FullName)}");
			}
		}
	}

	protected List<MethodData> GetAndSortMethodData()
	{
		var methods = new List<MethodData>();

		foreach (var type in TypeSystem.AllTypes)
		{
			foreach (var method in type.Methods)
			{
				var data = Compiler.GetMethodData(method);

				if (data.Inlined)
				{
					methods.Add(data);
				}
			}
		}

		methods.Sort((MethodData x, MethodData y) => String.Compare(x.Method.FullName, y.Method.FullName));

		return methods;
	}
}
