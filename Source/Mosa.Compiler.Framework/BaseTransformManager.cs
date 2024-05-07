// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Base Transform Manager
/// </summary>
public abstract class BaseTransformManager
{
	public virtual void Initialize(Compiler compiler)
	{ }

	public virtual void Setup(MethodCompiler methodCompiler)
	{
		Reset();
	}

	public virtual void Reset()
	{ }
}
