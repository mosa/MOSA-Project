/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Metadata;

namespace Mosa.Compiler.Verifier
{
	public abstract class BaseTableVerificationStage : BaseVerificationStage
	{
		protected bool IsValidStringIndex(HeapIndexToken token)
		{
			// TODO:

			return true;
		}

		protected bool IsValidHeapIndex(HeapIndexToken token)
		{
			// TODO:

			return true;
		}

		protected int CheckName(HeapIndexToken nameIndex)
		{
			if (nameIndex == 0)
			{
				return 1;
			}
			else
			{
				if (!IsValidStringIndex(nameIndex))
				{
					return 2;
				}
				else
				{
					string name = metadata.ReadString(nameIndex);

					if (string.IsNullOrEmpty(name))
					{
						return 3;
					}
				}
			}

			return 0;
		}

		protected int CheckCulture(HeapIndexToken cultureIndex)
		{
			if (cultureIndex != 0)
			{
				string culture = metadata.ReadString(cultureIndex);

				if (!string.IsNullOrEmpty(culture))
				{
					if (IsValidCulture(culture))
					{
						return 1;
					}
				}
			}

			return 0;
		}

		protected static List<string> validCultures = new List<string> {
			"ar-SA","ar-IQ","ar-EG","ar-LY","ar-DZ","ar-MA","ar-TN","ar-OM","ar-YE","ar-SY","ar-JO","ar-LB",
			"ar-KW","ar-AE","ar-BH","ar-QA","bg-BG","ca-ES","zh-TW","zh-CN","zh-HK","zh-SG","zh-MO","cs-CZ",
			"da-DK","de-DE","de-CH","de-AT","de-LU","de-LI","el-GR","en-US","en-GB","en-AU","en-CA","en-NZ",
			"en-IE","en-ZA","en-JM","en-CB","en-BZ","en-TT","en-ZW","en-PH","es-ES-Ts","es-MX","es-ES-Is","es-GT",
			"es-CR","es-PA","es-DO","es-VE","es-CO","es-PE","es-AR","es-EC","es-CL","es-UY","es-PY","es-BO",
			"es-SV","es-HN","es-NI","es-PR","fi-FI","fr-FR","fr-BE","fr-CA","fr-CH","fr-LU","fr-MC","he-IL",
			"hu-HU","is-IS","it-IT","it-CH","ja-JP","ko-KR","nl-NL","nl-BE","nb-NO","nn-NO","pl-PL","pt-BR",
			"pt-PT","ro-RO","ru-RU","hr-HR","lt-sr-SP","cy-sr-SP","sk-SK","sq-AL","sv-SE","sv-FI","th-TH","tr-TR",
			"ur-PK","id-ID","uk-UA","be-BY","sl-SI"
		};

		protected static bool IsValidCulture(string culture)
		{
			foreach (string entry in validCultures)
				if (string.Equals(culture, entry, StringComparison.OrdinalIgnoreCase))
					return true;

			return false;
		}
	}
}