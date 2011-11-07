using System;

namespace Mosa.Compiler.Verifier
{
	public class VerificationError
	{
		public string Section { get; private set; }
		public string Error { get; private set; }
		public string Data { get; private set; }

		public VerificationError(string section, string error, string data)
		{
			this.Section = section;
			this.Error = error;
			this.Data = data;
		}

		public VerificationError(string section, string error)
			: this(section, error, null)
		{
		}

		public override string ToString()
		{
			return "[" + Section + "] " + Error + (string.IsNullOrEmpty(Data) ? string.Empty : ": " + Data);
		}
	}
}
