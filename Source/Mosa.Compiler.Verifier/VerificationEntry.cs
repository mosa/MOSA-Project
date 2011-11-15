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
		public string Assembly { get; private set; }
		public VerificationType Type { get; private set; }
		public string Section { get; private set; }
		public string Rule { get; private set; }
		public string Description { get; private set; }
		public Token Location { get; private set; }

		public VerificationEntry(string assembly, VerificationType type, string section, string error, string description, Token token)
		{
			this.Assembly = assembly;
			this.Type = type;
			this.Section = section;
			this.Rule = error;
			this.Description = description;
			this.Location = token;
		}

		public VerificationEntry(string assembly, VerificationType type, string section, string error, string description)
			: this(assembly, type, section, error, description, Token.Zero)
		{
		}

		public override string ToString()
		{
			return
				"[" + this.Type.ToString().ToUpper() + "] "
				+ Description
				+ (string.IsNullOrEmpty(Assembly) ? string.Empty : " in " + Assembly)
				+ (Location == Token.Zero ? string.Empty : " at " + Location.ToString())
				+ ". (" + Section + ") " 
				+ Rule;
		}
	}
}
