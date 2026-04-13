// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Compiler Method Data
/// </summary>
public sealed class MethodData
{
	#region Properties

	public MosaMethod Method { get; }

	public Compiler Compiler { get; internal set; }

	public LinkerSymbol Symbol { get; set; }

	public Counters Counters { get; }

	public bool IsCompilerGenerated { get; set; }

	public bool StackFrameRequired { get; set; }

	public bool HasProtectedRegions { get; set; }

	public bool HasDoNotInlineAttribute { get; set; }

	public bool HasAggressiveInliningAttribute { get; set; }

	public bool AggressiveInlineRequested { get; set; }

	public bool IsMethodImplementationReplaced { get; set; }

	public bool HasLoops { get; set; }

	public bool IsSelfReferenced { get; set; }

	public bool HasEpilogue { get; set; }

	public bool HasReturnValue { get; set; }

	public bool HasAddressOfInstruction { get; set; }

	public int IRInstructionCount { get; set; }

	public int IRStackParameterInstructionCount { get; set; }

	public int NonIRInstructionCount { get; set; }

	public bool IsDevirtualized { get; set; }

	public int Version { get; set; }

	public int ParameterStackSize { get; set; }

	public List<uint> ParameterSizes { get; set; }

	public List<uint> ParameterOffsets { get; set; }

	public int ReturnSize { get; set; }

	public bool ReturnInRegister { get; set; }

	public int LocalMethodStackSize { get; set; }

	public List<LabelRegion> LabelRegions { get; }

	public long ElapsedTicks { get; set; }

	public long TotalElapsedTicks { get; set; }

	public bool DoNotInline { get; set; }

	public bool IsReferenced { get; set; }

	public bool HasCode { get; set; }

	public List<SafePointEntry> SafePointEntries { get; }

	public List<GCStackEntry> GCStackEntries { get; }

	public bool Inlined
	{
		get
		{
			var lockTimer = Stopwatch.StartNew();
			lock (_lock)
			{
				Compiler.LockMonitor.RecordLockWait(lockTimer, _lock, this.ToString());
				return InlineMethodData.IsInlined;
			}
		}
	}

	public MosaMethod ReplacedBy { get; set; }

	public int VirtualCodeSize { get; set; }

	public bool IsInvoked { get; set; }

	public bool IsUnitTest { get; set; }

	#endregion Properties

	#region Data Members

	private readonly object _lock = new object();

	private InlineMethodData InlineMethodData;

	#endregion Data Members

	public MethodData(MosaMethod mosaMethod, Compiler compiler)
	{
		Method = mosaMethod;
		Compiler = compiler;

		Counters = new Counters(compiler, $"Counters: {mosaMethod.FullName}");
		LabelRegions = new List<LabelRegion>();
		SafePointEntries = new List<SafePointEntry>();
		GCStackEntries = new List<GCStackEntry>();

		Version = 0;
		DoNotInline = false;
		InlineMethodData = new InlineMethodData(null, 0);
		IsDevirtualized = false;
		IsReferenced = false;
		HasCode = false;
		IsInvoked = false;
		AggressiveInlineRequested = false;
		StackFrameRequired = true;
		IsSelfReferenced = false;
		IsUnitTest = false;
	}

	public override string ToString()
	{
		return Method.FullName;
	}

	#region Methods

	public void AddLabelRegion(int label, int start, int length)
	{
		LabelRegions.Add(new LabelRegion
		{
			Label = label,
			Start = start,
			Length = length,
		});
	}

	public InlineMethodData GetInlineMethodDataForUseBy(MosaMethod method)
	{
		var lockTimer = Stopwatch.StartNew();
		lock (_lock)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, _lock, this.ToString());

			InlineMethodData.AddReference(method);
			return InlineMethodData;
		}
	}

	public InlineMethodData SwapInlineMethodData(BasicBlocks basicBlocks)
	{
		var lockTimer = Stopwatch.StartNew();
		lock (_lock)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, _lock, this.ToString());

			var tmp = InlineMethodData;

			InlineMethodData = new InlineMethodData(basicBlocks, Version);

			return tmp;
		}
	}

	public InlineMethodData GetInlineMethodData()
	{
		var lockTimer = Stopwatch.StartNew();
		lock (_lock)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, _lock, this.ToString());

			return InlineMethodData;
		}
	}

	#endregion Methods
}
