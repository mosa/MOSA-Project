// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Linker;

/// <summary>
/// Linker Symbol
/// </summary>
public sealed class LinkerSymbol
{
	public string Name { get; }

	public SectionKind SectionKind { get; internal set; }

	public Stream Stream { get; internal set; }

	public uint Alignment { get; internal set; }

	public bool IsDataAvailable => Stream != null;

	public uint Size => Stream != null ? (uint)Stream.Length : 0;

	public bool IsResolved => VirtualAddress != 0;

	public string ExternalSymbolName { get; internal set; }

	public bool IsExternalSymbol { get; set; }

	public uint SectionOffset { get; internal set; }

	public ulong VirtualAddress { get; internal set; }

	public bool IsReplaced { get; internal set; }

	public List<LinkRequest> LinkRequests { get; }

	private readonly object _lock = new object();

	public Compiler Compiler { get; internal set; }

	//public int Version { get; set; } // for debugging

	public MosaMethod MosaMethod { get; set; } // for debugging

	public MethodData MethodData { get; set; } // for debugging

	internal LinkerSymbol(string name, uint alignment = 0, SectionKind kind = SectionKind.Unknown)
	{
		Name = name;
		Alignment = alignment;
		SectionKind = kind;
		LinkRequests = new List<LinkRequest>();
		IsExternalSymbol = false;
		IsReplaced = false;
	}

	public void SetData(MemoryStream stream)
	{
		Stream = stream;
	}

	public void SetData(byte[] data)
	{
		SetData(new MemoryStream(data));
	}

	public void SetReplacementStatus(bool replaced)
	{
		IsReplaced = replaced;
	}

	public void AddPatch(LinkRequest linkRequest)
	{
		var lockTimer = Stopwatch.StartNew();
		lock (_lock)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, _lock, "LinkerSymbol");

			LinkRequests.Add(linkRequest);
		}
	}

	public void RemovePatches()
	{
		var lockTimer = Stopwatch.StartNew();
		lock (_lock)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, _lock, "LinkerSymbol");

			LinkRequests.Clear();
		}
	}

	public LinkRequest[] GetLinkRequests()
	{
		var lockTimer = Stopwatch.StartNew();
		lock (_lock)
		{
			Compiler.LockMonitor.RecordLockWait(lockTimer, _lock, "LinkerSymbol");

			return LinkRequests.ToArray();
		}
	}

	public void ApplyPatch(long offset, ulong value, byte patchSize, byte shift)
	{
		Stream.Position = offset;

		value >>= shift;

		// Apply the patch
		switch (patchSize)
		{
			case 8: Stream.WriteByte((byte)value); return;
			case 24: Stream.Write16((ushort)value); Stream.Write8((byte)(value >> 16)); return;
			case 16: Stream.Write16((ushort)value); return;
			case 32: Stream.Write32((uint)value); return;
			case 64: Stream.Write64(value); return;
		}
	}

	public override string ToString()
	{
		return $"[{SectionKind}] {Name}";
	}
}
