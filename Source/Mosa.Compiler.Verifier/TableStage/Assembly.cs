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
using Mosa.Compiler.Metadata.Tables;

namespace Mosa.Compiler.Verifier.TableStage
{
	public class Assembly : BaseVerificationStage
	{

		protected override void Run()
		{
			int rows = metadata.GetRowCount(TableType.Assembly);

			if (rows == 0)
				return;

			// 1. The Assembly table shall contain zero or one row
			if (rows == 0 || rows == 1)
			{
				AddSpecificationError("22.2-1", "AssemblyTable/Entry: The Assembly table shall contain zero or one row", "Multiple rows found");
				return;
			}

			if (rows == 0)
				return;

			AssemblyRow row = metadata.ReadAssemblyRow(new Token(TableType.Assembly, 1));

			// 2. HashAlgId shall be one of the specified values
			// Note: Microsoft treats this as a WARNING rather than an error
			if (!IsValidHashAlgID((int)row.HashAlgId))
			{
				AddSpecificationError("22.2-2", "AssemblyTable/Entry: HashAlgId shall be one of the specified values", "Invalid Hash Algorithm ID");
				return;
			}

			// 4. Flags shall have only those values set that are specified 
			if (!IsAssemblyFlags((int)row.Flags))
			{
				AddSpecificationError("22.2-4", "AssemblyTable/Entry: Flags shall have only those values set that are specified", "Invalid Hash Algorithm ID");
				return;
			}

			// 6. Name shall index a non-empty string in the String heap
			if (row.Name == 0)
			{
				AddSpecificationError("22.2-6", "AssemblyTable/Entry: Name shall index a non-empty string in the String heap", "Empty name");
				return;
			}

			string name = metadata.ReadString(row.Name);

			if (string.IsNullOrEmpty(name))
			{
				AddSpecificationError("22.2-6", "AssemblyTable/Entry: Name shall index a non-empty string in the String heap", "Empty name");
				return;
			}

			// 9. If Culture is non-null, it shall index a single string from the list specified
			if (row.Culture != 0)
			{
				string culture = metadata.ReadString(row.Culture);

				if (!string.IsNullOrEmpty(culture))
				{
					if (IsValidCulture(culture))
					{
						AddSpecificationError("22.2-9", "AssemblyTable/Entry: If Culture is non-null, it shall index a single string from the list specified", "Invalid or Missing Culture");
						return;
					}
				}
			}

		}

		protected static List<int> validHashAlgID = new List<int>() { 0x0000, 0x8003, 0x8004 };
		protected static List<int> validAssemblyFlags = new List<int>() { 0x0001, 0x0000, 0x0030, 0x0100, 0x8000, 0x4000 };
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

		protected static bool IsValidHashAlgID(int hash)
		{
			return validHashAlgID.Contains(hash);
		}

		protected static bool IsAssemblyFlags(int flag)
		{
			return validAssemblyFlags.Contains(flag);
		}

		protected static bool IsValidCulture(string culture)
		{
			foreach (string entry in validCultures)
				if (string.Equals(culture, entry, StringComparison.OrdinalIgnoreCase))
					return true;

			return false;
		}
	}
}

