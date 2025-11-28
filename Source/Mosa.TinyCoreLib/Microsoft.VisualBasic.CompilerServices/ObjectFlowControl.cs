using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualBasic.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ObjectFlowControl
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class ForLoopControl
	{
		internal ForLoopControl()
		{
		}

		[RequiresUnreferencedCode("The types of the parameters cannot be statically analyzed and may be trimmed")]
		public static bool ForLoopInitObj(object Counter, object Start, object Limit, object StepValue, ref object LoopForResult, ref object CounterResult)
		{
			throw null;
		}

		public static bool ForNextCheckDec(decimal count, decimal limit, decimal StepValue)
		{
			throw null;
		}

		[RequiresUnreferencedCode("The types of the parameters cannot be statically analyzed and may be trimmed")]
		public static bool ForNextCheckObj(object Counter, object LoopObj, ref object CounterResult)
		{
			throw null;
		}

		public static bool ForNextCheckR4(float count, float limit, float StepValue)
		{
			throw null;
		}

		public static bool ForNextCheckR8(double count, double limit, double StepValue)
		{
			throw null;
		}
	}

	internal ObjectFlowControl()
	{
	}

	public static void CheckForSyncLockOnValueType(object? Expression)
	{
	}
}
