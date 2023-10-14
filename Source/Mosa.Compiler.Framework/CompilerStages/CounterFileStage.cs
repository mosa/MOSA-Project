// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics.Metrics;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// An compilation stage which generates a file with all the counters.
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class CounterFileStage : BaseCompilerStage
{
	protected override void Initialization()
	{
	}

	protected override void Finalization()
	{
		if (string.IsNullOrEmpty(MosaSettings.CounterFile))
			return;

		var counters = Compiler.GlobalCounters.GetCounters();

		counters.Sort(delegate (Counter x, Counter y)
		{
			return x.Name.CompareTo(y.Name);
		});

		var filter = MosaSettings.CounterFilter;

		using var writer = new StreamWriter(MosaSettings.CounterFile);

		writer.WriteLine($"Counter Name\tCount");

		foreach (var counter in counters)
		{
			if (!string.IsNullOrWhiteSpace(filter) && !counter.Name.Contains(filter))
				continue;

			writer.WriteLine($"{counter.Name}\t{counter.Count}");
		}

		writer.Close();
	}
}
