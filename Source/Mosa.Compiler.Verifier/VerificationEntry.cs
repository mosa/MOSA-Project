/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;

namespace Mosa.Compiler.Verifier
{
	public enum VerificationType { Error, Warning, Other };

	public class VerificationEntry
	{
		public VerificationType Type { get; private set; }
		public string Section { get; private set; }
		public string Rule { get; private set; }
		public string Description { get; private set; }
		public Token Location { get; private set; }

		public VerificationEntry(VerificationType type, string section, string error, string description, Token token)
		{
			this.Type = type;
			this.Section = section;
			this.Rule = error;
			this.Description = description;
			this.Location = token;
		}

		public VerificationEntry(VerificationType type, string section, string error, string description)
			: this(type, section, error, description, Token.Zero)
		{
		}

		public VerificationEntry(VerificationType type, string section, string error)
			: this(type, section, error, null, Token.Zero)
		{
		}

		public override string ToString()
		{
			return "[" + this.Type.ToString() + "] " + Rule + " (" + Section + ")." + (string.IsNullOrEmpty(Description) ? string.Empty : " " + Description + (Location == Token.Zero ? string.Empty : " at " + Location.ToString()));

		}
	}
}
