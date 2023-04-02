// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

public abstract class MethodScanner
{
	public bool IsEnabled { get; set; }

	public abstract void Initialize();

	public abstract bool IsFieldAccessed(MosaField field);

	public abstract bool IsTypeAllocated(MosaType type);

	public abstract bool IsMethodInvoked(MosaMethod method);

	public abstract void MethodInvoked(MosaMethod method, MosaMethod source);

	public abstract void MethodDirectInvoked(MosaMethod method, MosaMethod source);

	public abstract void InterfaceMethodInvoked(MosaMethod method, MosaMethod source);

	public abstract void TypeAllocated(MosaType type, MosaMethod source);

	public abstract void AccessedField(MosaField field);

	public abstract void MoreLogInfo();

	public abstract void Complete();
}
