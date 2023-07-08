// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// An compilation stage, which generates a map file of the built binary file.
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public class MethodCompileTimeStage : BaseCompilerStage
{
	protected override void Finalization()
	{
		if (string.IsNullOrEmpty(MosaSettings.CompileTimeFile))
			return;

		var methods = GetAndSortMethodData();

		using (var writer = new StreamWriter(MosaSettings.CompileTimeFile))
		{
			writer.WriteLine("Ticks\tMilliseconds\tCompiler Count\tMethod");

			foreach (var data in methods)
			{
				writer.WriteLine($"{data.ElapsedTicks}{'\t'}{data.ElapsedTicks / TimeSpan.TicksPerMillisecond}{'\t'}{data.Version}{'\t'}{data.Method.FullName}");
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

				if (data == null)
					continue;

				methods.Add(data);
			}
		}

		methods.Sort((MethodData x, MethodData y) => (int)(y.ElapsedTicks - x.ElapsedTicks));

		return methods;
	}
}
