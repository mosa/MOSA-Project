// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

public abstract class PlugSystem
{
	public abstract MosaMethod GetReplacement(MosaMethod targetMethod);

	public abstract void CreatePlug(MosaMethod targetMethod, MosaMethod newMethod);
}
