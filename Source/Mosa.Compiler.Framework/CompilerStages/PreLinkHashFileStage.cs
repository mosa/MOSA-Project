// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Mosa.Compiler.Framework.Linker;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public class PreLinkHashFileStage : BaseCompilerStage
{
	#region Data Members

	/// <summary>
	/// Holds the text writer used to emit the map file.
	/// </summary>
	private TextWriter writer;

	#endregion Data Members

	private class HashInfo
	{
		public string Name;
		public string Hash;
	}

	protected override void Initialization()
	{
	}

	protected override void Finalization()
	{
		Generate(MosaSettings.PreLinkHashFile);
	}

	protected void Generate(string filename)
	{
		if (string.IsNullOrEmpty(filename))
			return;

		using (writer = new StreamWriter(filename))
		{
			writer.WriteLine("Symbol Name\tHash");

			var info = GetAndSortSymbolHashData();

			foreach (var data in info)
			{
				writer.WriteLine($"{data.Name}\t{data.Hash}");
			}
		}
	}

	#region Internals

	private List<HashInfo> GetAndSortSymbolHashData()
	{
		var info = new List<HashInfo>();

		foreach (var kind in MosaLinker.SectionKinds)
		{
			foreach (var symbol in Linker.Symbols)
			{
				if (symbol.SectionKind != kind)
					continue;

				info.Add(new HashInfo
				{
					Name = symbol.Name,
					Hash = symbol.Stream.Length == 0 ? "-ZERO-" : ComputeHash(symbol.Stream)
				});
			}
		}

		info.Sort((HashInfo x, HashInfo y) => string.Compare(y.Name, x.Name));

		return info;
	}

	public static string ComputeHash(Stream stream)
	{
		stream.Position = 0;

		using var sha = SHA256.Create();
		var hash = sha.ComputeHash(stream);
		return BitConverter.ToString(hash);
	}

	#endregion Internals
}
