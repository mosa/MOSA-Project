/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Tables;

namespace Mosa.Compiler.Verifier.TableStage
{
	public class AssemblyRef : BaseTableVerificationStage
	{

		protected override void Run()
		{
			int rows = metadata.GetRowCount(TableType.AssemblyRef);

			if (rows == 0)
				return;

			// 8. HashValue can be null or non-null
			// 9. If non-null, then HashValue shall index a non-empty 'blob' in the Blob heap [ERROR]
			// 10. The AssemblyRef table shall contain no duplicates (where duplicate rows are deemed to be those having the same MajorVersion, MinorVersion, BuildNumber, RevisionNumber, PublicKeyOrToken, Name, and Culture) [WARNING]

			foreach (var token in new Token(TableType.AssemblyRef, 1).Upto(rows))
			{
				AssemblyRow row = metadata.ReadAssemblyRow(token);

				// 2. Flags shall have only one bit set, the PublicKey bit (§23.1.2). All other bits shall be zero. [ERROR]
				if ((int)row.Flags != 0x0001)
				{
					AddSpecificationError("22.5-5", "AssemblyRefTable/Entry: Flags shall have only one bit set, the PublicKey bit (§23.1.2). All other bits shall be zero. ", "Empty name");
				}

				// 3. PublicKeyOrToken can be null, or non-null (note that the Flags.PublicKey bit specifies whether the 'blob' is a full public key, or the short hashed token)
				if ((int)row.PublicKey != 0)
				{
					// 4. If non-null, then PublicKeyOrToken shall index a valid offset in the Blob heap [ERROR]
					if (!IsValidHeapIndex(row.PublicKey))
					{
						AddSpecificationError("22.5-5", "AssemblyRefTable/Entry: PublicKeyOrToken shall index a valid offset in the Blob heap. ", "Invalid index");
					}
				}

				// 5. Name shall index a non-empty string, in the String heap 
				if (row.Name == 0)
				{
					AddSpecificationError("22.5-5", "AssemblyRefTable/Entry: Name shall index a non-empty string, in the String heap", "Empty name");
				}
				else
				{
					if (!IsValidString(row.Name))
					{
						AddSpecificationError("22.5-5", "AssemblyRefTable/Entry: Name shall index a non-empty string, in the String heap", "Invalid index");
					}
					else
					{
						string name = metadata.ReadString(row.Name);

						if (string.IsNullOrEmpty(name))
						{
							AddSpecificationError("22.5-5", "AssemblyRefTable/Entry: Name shall index a non-empty string, in the String heap", "Empty name");
						}
					}
				}

				// 6. Culture can be null or non-null.
				if (row.Culture != 0)
				{
					string culture = metadata.ReadString(row.Culture);

					// 7. If non-null, it shall index a single string from the list specified 
					if (!string.IsNullOrEmpty(culture))
					{
						if (IsValidCulture(culture))
						{
							AddSpecificationError("22.5-7", "AssemblyRefTable/Entry: If Culture is non-null, it shall index a single string from the list specified", "Invalid or Missing Culture");
						}
					}
				}
			}
		}

	}
}

