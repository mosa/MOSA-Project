/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Verifier
{
	public enum VerificationType { Error, Warning, Other };

	public class VerificationEntry
	{
		public VerificationType Type { get; private set; }
		public string Section { get; private set; }
		public string Rule { get; private set; }
		public string Description { get; private set; }

		public VerificationEntry(VerificationType type, string section, string error, string description)
		{
			this.Type = type;
			this.Section = section;
			this.Rule = error;
			this.Description = description;
		}

		public VerificationEntry(VerificationType type, string section, string error)
			: this(type, section, error, null)
		{
		}

		public override string ToString()
		{
			return this.Type.ToString() + "[" + Section + "] " + Rule + (string.IsNullOrEmpty(Description) ? string.Empty : ": " + Description);
		}
	}
}
