﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using System.Text;
using System.Text.Json;

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildBaseTemplate
{
	public string JsonFile { get; }
	public string DestinationPath { get; }
	protected string DestinationFile { get; set; }

	protected StringBuilder Lines { get; } = new StringBuilder();

	protected dynamic Entries { get; set; }

	public BuildBaseTemplate(string jsonFile, string destinationPath, string destinationFile = null)
	{
		JsonFile = jsonFile;
		DestinationPath = destinationPath;
		DestinationFile = destinationFile;
	}

	public void Execute()
	{
		ReadJSON();
		Iterator();
	}

	protected virtual void Iterator()
	{
		Generate();
	}

	protected void Generate()
	{
		AddSourceHeader();
		Body();
		Save();
	}

	protected void AddSourceHeader()
	{
		Lines.AppendLine("// Copyright (c) MOSA Project. Licensed under the New BSD License.");
		Lines.AppendLine();
		Lines.AppendLine("// This code was generated by an automated template.");
		Lines.AppendLine();
	}

	protected void Save()
	{
		var destination = Path.Combine(DestinationPath, DestinationFile);
		var lines = Lines.ToString();

		if (File.Exists(destination))
		{
			var existinglines = File.ReadAllText(destination);

			if (lines == existinglines)
				return;
		}

		var path = Path.GetDirectoryName(destination);

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		File.WriteAllText(destination, lines);
	}

	protected void ReadJSON()
	{
		if (JsonFile == null)
			return;

		using var jsonTextReader = File.OpenText(JsonFile);

		var jsonData = jsonTextReader.ReadToEnd();
		var jsonDeserialized = JsonSerializer.Deserialize<JsonElement>(jsonData);

		Entries = new DynamicJsonObject(jsonDeserialized);
	}

	protected virtual void Body()
	{
	}

	protected virtual void Body(dynamic node = null)
	{
	}
}
