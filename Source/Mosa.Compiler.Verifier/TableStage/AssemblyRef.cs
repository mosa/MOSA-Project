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

			foreach (var token in new Token(TableType.AssemblyRef, 1).Upto(rows))
			{
				AssemblyRefRow row = metadata.ReadAssemblyRefRow(token);

				// 2. Flags shall have only one bit set, the PublicKey bit (§23.1.2). All other bits shall be zero. [ERROR]
				if ((int)row.Flags != 0x0001)
				{
					AddSpecificationError("22.5-2", "Flags shall have only one bit set, the PublicKey bit (§23.1.2). All other bits shall be zero. ", "Empty name", token);
				}

				// 3. PublicKeyOrToken can be null, or non-null (note that the Flags.PublicKey bit specifies whether the 'blob' is a full public key, or the short hashed token)
				if ((int)row.PublicKeyOrToken != 0)
				{
					// 4. If non-null, then PublicKeyOrToken shall index a valid offset in the Blob heap [ERROR]
					if (!IsValidHeapIndex(row.PublicKeyOrToken))
					{
						AddSpecificationError("22.5-3", "PublicKeyOrToken shall index a valid offset in the Blob heap. ", "Invalid index", token);
					}
				}

				// 5. Name shall index a non-empty string, in the String heap 
				switch (CheckName(row.Name))
				{
					case 0: break;
					case 1: AddSpecificationError("22.5-5", "Name shall index a non-empty string, in the String heap", "Empty name", token); break;
					case 2: AddSpecificationError("22.5-5", "Name shall index a non-empty string, in the String heap", "Invalid index", token); break;
					case 3: AddSpecificationError("22.5-5", "Name shall index a non-empty string, in the String heap", "Empty name", token); break;
				}

				// 8. HashValue can be null or non-null
				// 9. If non-null, then HashValue shall index a non-empty 'blob' in the Blob heap [ERROR]
				if (row.HashValue != 0 && !IsValidHeapIndex(row.HashValue))
				{
					AddSpecificationError("22.5-9", "HashValue shall index a non-empty 'blob' in the Blob heap. ", "Invalid index", token);
				}

				// 6. Culture can be null or non-null.
				switch (CheckCulture(row.Culture))
				{
					case 0: break;
					case 1: AddSpecificationError("22.5-7", "If Culture is non-null, it shall index a single string from the list specified", "Invalid or Missing Culture", token); break;
				}

				// 10. The AssemblyRef table shall contain no duplicates (where duplicate rows are deemed to be those having the same MajorVersion, MinorVersion, BuildNumber, RevisionNumber, PublicKeyOrToken, Name, and Culture) [WARNING]
				foreach (var othertoken in new Token(TableType.AssemblyRef, 1).Upto(rows))
				{
					if (token == othertoken)
						continue;

					AssemblyRefRow otherrow = metadata.ReadAssemblyRefRow(othertoken);

					if (row.MajorVersion != row.MajorVersion ||
						row.MinorVersion != row.MinorVersion ||
						row.BuildNumber != row.BuildNumber ||
						row.MajorVersion != row.MajorVersion ||
						row.PublicKeyOrToken != row.PublicKeyOrToken)
						continue;

					if (string.CompareOrdinal(metadata.ReadString(row.Name), metadata.ReadString(row.Name)) != 0)
						continue;

					if (string.CompareOrdinal(metadata.ReadString(row.Culture), metadata.ReadString(row.Culture)) != 0)
						continue;

					AddSpecificationError("22.5-7", "The AssemblyRef table shall contain no duplicates", "Duplicate found", token);
				}

			}
		}

	}
}

