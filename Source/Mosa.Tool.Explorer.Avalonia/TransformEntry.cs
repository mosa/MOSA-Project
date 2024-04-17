// Copyright (c) MOSA Project. Licensed under the New BSD License.
namespace Mosa.Tool.Explorer.Avalonia;

public class TransformEntry
{
	public int ID { get; set; }

	public string Name { get; set; }

	public string Before { get; set; }

	public string After { get; set; }

	public string Block { get; set; }

	public int Pass { get; set; }
}
