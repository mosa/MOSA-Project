// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;

namespace Mosa.Compiler.Framework.Analysis;

/// <summary>
/// Analyzes method parameters to determine which are read-only (not written to).
/// </summary>
/// <remarks>
/// A parameter is considered read-only if it is never the target of a ParameterStore instruction.
/// This includes checking the Low and High parts for 64-bit values on 32-bit platforms.
/// </remarks>
public sealed class ParameterReadOnlyAnalysis
{
	private readonly BitArray paramReadOnly;
	private readonly int parameterCount;
	private readonly Parameters parameters;

	/// <summary>
	/// Gets the number of parameters analyzed.
	/// </summary>
	public int ParameterCount => parameterCount;

	/// <summary>
	/// Initializes a new instance of the <see cref="ParameterReadOnlyAnalysis"/> class.
	/// </summary>
	/// <param name="parameters">The method parameters to analyze.</param>
	public ParameterReadOnlyAnalysis(Parameters parameters)
	{
		this.parameters = parameters;

		// Count parameters first
		parameterCount = parameters.Count;

		if (parameterCount == 0)
			return;

		paramReadOnly = new BitArray(parameterCount, false);

		foreach (var operand in parameters)
		{
			var write = HasParameterStore(operand);

			if (!write && operand.Low != null)
			{
				write = HasParameterStore(operand.Low);
			}

			if (!write && operand.High != null)
			{
				write = HasParameterStore(operand.High);
			}

			paramReadOnly[operand.Index] = !write;
		}
	}

	/// <summary>
	/// Determines whether the specified parameter is read-only.
	/// </summary>
	/// <param name="parameterIndex">The zero-based index of the parameter.</param>
	/// <returns>
	/// <c>true</c> if the parameter is read-only (never written to); otherwise, <c>false</c>.
	/// </returns>
	public bool IsReadOnly(int parameterIndex)
	{
		if (parameterIndex < 0 || parameterIndex >= parameterCount)
			return false;

		return paramReadOnly.Get(parameterIndex);
	}

	/// <summary>
	/// Determines whether the specified parameter operand is read-only.
	/// </summary>
	/// <param name="parameter">The parameter operand to check.</param>
	/// <returns>
	/// <c>true</c> if the parameter is read-only (never written to); otherwise, <c>false</c>.
	/// </returns>
	public bool IsReadOnly(Operand parameter)
	{
		if (parameter == null || !parameter.IsParameter)
			return false;

		return IsReadOnly(parameter.Index);
	}

	/// <summary>
	/// Traces the read-only status of all parameters to the provided trace log.
	/// </summary>
	/// <param name="traceLog">The trace log to write to, or null to skip tracing.</param>
	public void Trace(TraceLog traceLog)
	{
		if (traceLog == null || parameterCount == 0)
			return;

		foreach (var operand in parameters)
		{
			var isReadOnly = IsReadOnly(operand);

			traceLog.Log($"{operand}: {(isReadOnly ? "ReadOnly" : "Writable")}");
		}
	}

	private static bool HasParameterStore(Operand operand)
	{
		foreach (var use in operand.Uses)
		{
			if (use.Instruction.IsParameterStore)
			{
				return true;
			}
		}

		return false;
	}
}
