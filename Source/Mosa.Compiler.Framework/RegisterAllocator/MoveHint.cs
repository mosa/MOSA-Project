// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator;

public sealed class MoveHint
{
	public readonly SlotIndex Slot;

	public readonly Register From;
	public readonly Register To;

	public readonly int Bonus;

	public LiveInterval FromInterval { get; private set; }

	public LiveInterval ToInterval { get; private set; }

	public PhysicalRegister FromRegister
	{
		get
		{
			if (From.IsPhysicalRegister)
				return From.PhysicalRegister;

			if (FromInterval == null)
				return null;

			// lazy updates are allowed, so if interval doesn't touch move slot (anymore), return null
			if (FromInterval.Start != Slot)
				return null;

			return FromInterval.AssignedPhysicalRegister;
		}
	}

	public PhysicalRegister ToRegister
	{
		get
		{
			if (To.IsPhysicalRegister)
				return To.PhysicalRegister;

			if (ToInterval == null)
				return null;

			// lazy updates are allowed, so if interval doesn't touch move slot (anymore), return null
			if (ToInterval.End != Slot)
				return null;

			return ToInterval.AssignedPhysicalRegister;
		}
	}

	public MoveHint(SlotIndex slot, Register from, Register to, int bonus)
	{
		Slot = slot;
		From = from;
		To = to;
		Bonus = bonus;
	}

	public void Update(LiveInterval interval)
	{
		var updateInterval = interval.AssignedPhysicalRegister == null ? null : interval;

		if (interval.VirtualRegister == From)
		{
			FromInterval = updateInterval;
		}

		if (interval.VirtualRegister == To)
		{
			ToInterval = updateInterval;
		}
	}

	public override string ToString()
	{
		var sb = new StringBuilder();

		sb.Append(Slot.ToString());

		sb.Append(" FROM: ");

		if (From.IsPhysicalRegister)
		{
			sb.Append(From.PhysicalRegister);
		}
		else
		{
			sb.AppendFormat("v{0}", From.RegisterOperand.Index);

			if (FromRegister != null)
			{
				sb.Append($" [{FromRegister.ToString()}]");
			}
		}

		sb.Append(" TO: ");
		if (To.IsPhysicalRegister)
		{
			sb.Append(To.PhysicalRegister);
		}
		else
		{
			sb.AppendFormat("v{0}", To.RegisterOperand.Index);

			if (ToRegister != null)
			{
				sb.Append($" [{ToRegister.ToString()}]");
			}
		}

		return sb.ToString();
	}
}
