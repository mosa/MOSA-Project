// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.BareMetal.HelloWorld;

public interface IApp
{
	string Name { get; }

	string Description { get; }

	void Execute();
}
