// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common.Exceptions;

[Serializable]
public class CompilerException : Exception
{
	public string Stage { get; set; }

	public string Method { get; set; }

	public string BaseMessage { get; protected set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="CompilerException" /> class.
	/// </summary>
	public CompilerException() : base()
	{
		BaseMessage = "Exception";
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CompilerException" /> class.
	/// </summary>
	/// <param name="message">The message that describes the error.</param>
	public CompilerException(string message) : base(message)
	{
		BaseMessage = message;
	}

	public CompilerException(string stage, string method, string message)
	{
		Stage = stage;
		Method = method;
		BaseMessage = message;
	}

	public override string Message => BuildMessage();

	protected string BuildMessage()
	{
		if (Stage != null && Method != null)
			return $"Stage: {Stage} - Method: {Method} -> {BaseMessage}";

		if (Method != null)
			return $"Method: {Method} -> {BaseMessage}";

		if (Stage != null)
			return $"Stage: {Stage} -> {BaseMessage}";

		return BaseMessage;
	}
}
