// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// An compilation stage, which generates a map file of the built binary file.
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class DebugFileStage : BaseCompilerStage
{
	#region Data Members

	/// <summary>
	/// Holds the text writer used to emit the map file.
	/// </summary>
	private TextWriter writer;

	private readonly Dictionary<string, int> SourceFiles = new Dictionary<string, int>();

	#endregion Data Members

	protected override void Finalization()
	{
		if (string.IsNullOrEmpty(MosaSettings.DebugFile))
			return;

		using (writer = new StreamWriter(MosaSettings.DebugFile))
		{
			EmitSections();
			EmitSymbols();
			EmitEntryPoint();
			EmitTypes();
			EmitMethods();
			EmitParameters();
			EmitFields();
			EmitSourceLabels();
			EmitSourceFileInformation();
			EmitSourceInformation();
		}
	}

	#region Internals

	private void EmitSections()
	{
		writer.WriteLine("[Sections]");
		writer.WriteLine("Address\tOffset\tSize\tKind\tName");
		foreach (var linkerSection in Linker.Sections)
		{
			writer.WriteLine(
				"{0:x8}\t{1}\t{2}\t{3}\t{4}",
				linkerSection.VirtualAddress,
				0, //linkerSection.FileOffset,
				linkerSection.Size,
				linkerSection.SectionKind.ToString(),
				linkerSection.Name);
		}
	}

	private void EmitSymbols()
	{
		writer.WriteLine("[Symbols]");
		writer.WriteLine("Address\tOffset\tSize\tType\tName");

		foreach (var symbol in Linker.Symbols)
		{
			writer.WriteLine(
				"{0:x8}\t{1}\t{2}\t{3}\t{4}",
				symbol.VirtualAddress,
				symbol.SectionOffset,
				symbol.Size,
				symbol.SectionKind.ToString(),
				symbol.Name);
		}
	}

	private void EmitEntryPoint()
	{
		if (Linker.EntryPoint != null)
		{
			writer.WriteLine("[EntryPoint]");
			writer.WriteLine("Address\tName");
			writer.WriteLine(
				"{0:x8}\t{1}",
				Linker.EntryPoint.VirtualAddress,
				Linker.EntryPoint.Name);
		}
	}

	private void EmitTypes()
	{
		writer.WriteLine("[Types]");
		writer.WriteLine("TypeID\tTypeDef\tSize\tFullName\tBaseTypeID\tDeclaringTypeID\tElementTypeID");
		foreach (var type in TypeSystem.AllTypes)
		{
			if (type.IsModule)
				continue;

			//if (!Linker.IsSymbolDefined(type.FullName))
			//	continue;

			var size =
				type.HasOpenGenericParams
				|| type.IsInterface
				|| type.IsVoid
				|| (!type.IsReferenceType && !type.IsValueType)
				? 0 : TypeLayout.GetTypeLayoutSize(type);

			writer.WriteLine(
				"{0}\t{1:x8}\t{2}\t{3}\t{4}\t{5}\t{6}",
				type.ID,
				Linker.GetSymbol(type.FullName).VirtualAddress,
				size,
				type.FullName,
				type.BaseType != null ? type.BaseType.ID : 0,
				type.DeclaringType != null ? type.DeclaringType.ID : 0,
				type.ElementType != null ? type.ElementType.ID : 0
			);
		}
	}

	private void EmitMethods()
	{
		writer.WriteLine("[Methods]");
		writer.WriteLine("MethodID\tAddress\tSize\tMethodDef\tFullName\tTypeID\tReturnTypeID\tStackSize\tParameterStackSize\tAttributes");

		foreach (var type in TypeSystem.AllTypes)
		{
			if (type.IsModule)
				continue;

			foreach (var method in type.Methods)
			{
				if (!Linker.IsSymbolDefined(method.FullName))
					continue;

				var symbol = Linker.GetSymbol(method.FullName);
				var methodData = Compiler.GetMethodData(method);

				writer.WriteLine(
					"{0}\t{1:x8}\t{2}\t{3:x8}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}",
					method.ID,
					symbol.VirtualAddress,
					symbol.Size,
					Linker.GetSymbol(Metadata.MethodDefinition + method.FullName).VirtualAddress,
					method.FullName,
					method.Signature.ReturnType.ID,
					methodData.LocalMethodStackSize,
					methodData.ParameterStackSize,
					(int)method.MethodAttributes,
					type.ID
				);
			}
		}
	}

	private void EmitParameters()
	{
		writer.WriteLine("[Parameters]");
		writer.WriteLine("MethodID\tIndex\tOffset\tName\tFullName\tParameterTypeID\tAttributes\tSize");

		foreach (var type in TypeSystem.AllTypes)
		{
			if (type.IsModule)
				continue;

			foreach (var method in type.Methods)
			{
				var index = 0;

				var methodData = Compiler.GetMethodData(method);

				foreach (var parameter in method.Signature.Parameters)
				{
					writer.WriteLine(
						"{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}",
						method.ID,
						index,
						methodData == null || methodData.ParameterOffsets == null ? 0 : methodData.ParameterOffsets[index],
						parameter.Name,
						parameter.FullName,
						parameter.ParameterType.ID,
						(int)parameter.ParameterAttributes,
						methodData == null || methodData.ParameterSizes == null ? 0 : methodData.ParameterSizes[index]
					);

					index++;
				}
			}
		}
	}

	private void EmitFields()
	{
		writer.WriteLine("[Fields]");
		writer.WriteLine("TypeID\tIndex\tFullName\tName\tFieldTypeID\tAddress\tAttributes\tOffset\tDataLength\tDataAddress");

		foreach (var type in TypeSystem.AllTypes)
		{
			if (type.IsModule)
				continue;

			var index = 0;

			foreach (var field in type.Fields)
			{
				var symbolName = Metadata.FieldDefinition + field.FullName;

				if (!Linker.IsSymbolDefined(symbolName))
					continue;

				var symbol = Linker.GetSymbol(symbolName);

				//var datasection = (field.Data != null) ? SectionKind.ROData : SectionKind.BSS; // not used yet
				var offset = field.HasOpenGenericParams || field.IsStatic ? 0 : TypeLayout.GetFieldOffset(field);

				writer.WriteLine(
					"{0}\t{1}\t{2}\t{3}\t{4}\t{5:x8}\t{6}\t{7}\t{8}\t{9:x8}",
					type.ID,
					index++,
					field.FullName,
					field.Name,
					field.FieldType.ID,
					symbol?.VirtualAddress ?? 0,
					(int)field.FieldAttributes,
					field.IsStatic && !field.IsLiteral ? 0 : offset,  // todo: missing first offset
					field.Data?.Length ?? 0,
					0 // todo: DataAddress
				);
			}
		}
	}

	private void EmitSourceLabels()
	{
		writer.WriteLine("[SourceLabels]");
		writer.WriteLine("MethodID\tLabel\tStart\tLength");

		foreach (var methodData in Compiler.CompilerData.MethodData)
		{
			if (methodData == null || methodData.LabelRegions.Count == 0)
				continue;

			var first = methodData.LabelRegions[0];

			if (first.Start != 0)
			{
				// special case which maps any prologue instructions to the first label
				writer.WriteLine(
					"{0}\t{1}\t{2}\t{3}",
					methodData.Method.ID,
					0,
					0,
					first.Start
				);
			}

			foreach (var labelRegion in methodData.LabelRegions)
			{
				writer.WriteLine(
					"{0}\t{1}\t{2}\t{3}",
					methodData.Method.ID,
					labelRegion.Label,
					labelRegion.Start,
					labelRegion.Length
				);
			}
		}
	}

	private void EmitSourceFileInformation()
	{
		var filenames = new List<string>();
		var hashset = new HashSet<string>();

		var last = string.Empty;

		foreach (var type in TypeSystem.AllTypes)
		{
			if (type.IsModule)
				continue;

			foreach (var method in type.Methods)
			{
				if (method.Code == null)
					continue;

				foreach (var instruction in method.Code)
				{
					var filename = instruction.Document;

					if (filename == null)
						continue;

					if (last == filename)
						continue;

					if (!hashset.Contains(filename))
					{
						filenames.Add(filename);
						hashset.Add(filename);
						last = filename;
					}
				}
			}
		}

		writer.WriteLine("[SourceFile]");
		writer.WriteLine("SourceFileID\tFileName");

		var index = 0;

		foreach (var filename in filenames)
		{
			writer.WriteLine(
				"{0}\t{1}",
				++index,
				filename
			);

			SourceFiles.Add(filename, index);
		}
	}

	private void EmitSourceInformation()
	{
		writer.WriteLine("[Source]");
		writer.WriteLine("MethodID\tLabel\tStartLine\tStartColumn\tEndLine\tEndColumn\tSourceFileID");

		foreach (var type in TypeSystem.AllTypes)
		{
			if (type.IsModule)
				continue;

			foreach (var method in type.Methods)
			{
				if (method.Code == null)
					continue;

				foreach (var instruction in method.Code)
				{
					if (instruction.Document == null)
						continue;

					if (instruction.StartLine == 0xFEEFEE)
						continue;

					writer.WriteLine(
						"{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
						method.ID,
						instruction.Offset,
						instruction.StartLine,
						instruction.StartColumn,
						instruction.EndLine,
						instruction.EndColumn,
						SourceFiles[instruction.Document]
					);
				}
			}
		}
	}

	#endregion Internals
}
