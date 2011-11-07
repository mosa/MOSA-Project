/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Compiler.Verifier;

namespace Mosa.Tools.Verifier
{
	static class ParseOptions
	{
		public static bool Parse(VerifierOptions options, string[] args)
		{
			foreach (string arg in args)
			{
				switch (arg)
				{
					case "/ML":
					case "/ml":
						options.MetadataValidation = true; break;
					case "/IL":
					case "/il":
						options.ILValidation = true; break;
					default:
						options.InputFile = arg; break;
				}
			}
			return true;
		}
	}
}
