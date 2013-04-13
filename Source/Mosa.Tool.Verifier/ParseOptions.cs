/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Verifier;

namespace Mosa.Tool.Verifier
{
	internal static class ParseOptions
	{
		public static bool Parse(VerificationOptions options, string[] args)
		{
			foreach (string arg in args)
			{
				switch (arg.ToLower())
				{
					case "/ml":
						options.MetadataValidation = true; break;
					case "/il":
						options.ILValidation = true; break;
					case "/warnings":
					case "/w":
						options.IncludingWarnings = true; break;
					default:
						options.InputFile = arg; break;
				}
			}
			return true;
		}
	}
}