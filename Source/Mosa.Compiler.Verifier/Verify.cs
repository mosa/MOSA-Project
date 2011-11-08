
using System;
using System.Collections.Generic;
using System.IO;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Tables;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Loader.PE;

namespace Mosa.Compiler.Verifier
{
	public class Verify
	{

		private VerifierOptions options;
		private List<VerificationEntry> entries;

		/// <summary>
		/// Gets a value indicating whether [any errors].
		/// </summary>
		/// <value><c>true</c> if [any errors]; otherwise, <c>false</c>.</value>
		public bool HasErrors
		{
			get
			{
				foreach (VerificationEntry entry in entries)
					if (entry.Type == VerificationType.Error)
						return true;

				return false;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Verify"/> class.
		/// </summary>
		/// <param name="options">The options.</param>
		public Verify(VerifierOptions options)
		{
			this.options = options;
			entries = new List<VerificationEntry>();
		}

		/// <summary>
		/// Runs this instance.
		/// </summary>
		/// <returns></returns>
		public bool Run()
		{
			string file = options.InputFile;

			try
			{
				if (!File.Exists(file))
				{
					AddNonSpecificationError("File not found");
					return false;
				}

				IMetadataModule module = LoadPE(file);

				VerifyMetadata(module.Metadata);

			}
			catch (Exception e)
			{
				AddNonSpecificationError("Exception thrown", e.ToString());
			}

			return !HasErrors;
		}

		#region Verification Helpers

		protected void AddNonSpecificationError(string error)
		{
			entries.Add(new VerificationEntry(VerificationType.Error, "0", error));
		}

		protected void AddNonSpecificationError(string error, string data)
		{
			entries.Add(new VerificationEntry(VerificationType.Error, "0", error, data));
		}

		protected void AddSpecificationError(string section, string error, string data)
		{
			entries.Add(new VerificationEntry(VerificationType.Error, section, error, data));
		}

		protected void AddSpecificationError(string section, string error)
		{
			entries.Add(new VerificationEntry(VerificationType.Error, section, error));
		}

		#endregion

		protected PortableExecutableImage LoadPE(string file)
		{
			try
			{
				Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
				PortableExecutableImage peImage = new PortableExecutableImage(stream);

				return peImage;
			}
			catch
			{
				AddNonSpecificationError("Unable to load PE image", file);
				throw;
			}
		}

		protected void VerifyMetadata(IMetadataProvider metadata)
		{
			VerifyAssemblyTable(metadata);
		}

		protected void VerifyAssemblyTable(IMetadataProvider metadata)
		{
			int rows = metadata.GetRowCount(TableType.Assembly);

			if (rows == 0)
				return;

			// 1. The Assembly table shall contain zero or one row
			if (rows > 1)
			{
				AddSpecificationError("22.2-1", "AssemblyTable/Entry: The Assembly table shall contain zero or one row", "Multiple rows found");
				return;
			}

			AssemblyRow row = metadata.ReadAssemblyRow(new Token(TableType.Assembly, 0));
			
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
			string name = metadata.ReadString(row.Name);

			if (string.IsNullOrEmpty(name))
			{
				AddSpecificationError("22.2-6", "AssemblyTable/Entry: Name shall index a non-empty string in the String heap", "Empty Name");
				return;
			}

			// 9. If Culture is non-null, it shall index a single string from the list specified (§23.1.3) 
			string culture = metadata.ReadString(row.Culture);

			if (IsValidCulture(culture))
			{
				AddSpecificationError("22.2-6", "AssemblyTable/Entry: If Culture is non-null, it shall index a single string from the list specified", "Invalid or Missing Culture");
				return;
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
			if (string.IsNullOrEmpty(culture))
				return false;

			foreach (string entry in validCultures)
				if (string.Equals(culture, entry, StringComparison.OrdinalIgnoreCase))
					return true;

			return false;
		}
	}
}

