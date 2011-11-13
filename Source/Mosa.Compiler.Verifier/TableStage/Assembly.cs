
using System.Collections.Generic;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Tables;

namespace Mosa.Compiler.Verifier.TableStage
{
	public class Assembly : BaseTableVerificationStage
	{

		protected override void Run()
		{
			int rows = metadata.GetRowCount(TableType.Assembly);

			if (rows == 0)
				return;

			// 1. The Assembly table shall contain zero or one row
			if (rows > 1)
			{
				AddSpecificationError("22.2-1", "The Assembly table shall contain zero or one row", "Multiple rows found");
			}

			Token token = new Token(TableType.Assembly, 1);
			AssemblyRow row = metadata.ReadAssemblyRow(token);

			// 2. HashAlgId shall be one of the specified values
			// Note: Microsoft treats this as a WARNING rather than an error
			if (!IsValidHashAlgID((int)row.HashAlgId))
			{
				AddSpecificationError("22.2-2", "HashAlgId shall be one of the specified values", "Invalid Hash Algorithm ID", token);
			}

			// 4. Flags shall have only those values set that are specified 
			if (!IsAssemblyFlags((int)row.Flags))
			{
				AddSpecificationError("22.2-4", "Flags shall have only those values set that are specified", "Invalid Hash Algorithm ID", token);
			}

			// 6. Name shall index a non-empty string in the String heap
			switch (CheckName(row.Name))
			{
				case 0: break;
				case 1: AddSpecificationError("22.2-6", "Name shall index a non-empty string, in the String heap", "Empty name", token); break;
				case 2: AddSpecificationError("22.2-6", "Name shall index a non-empty string, in the String heap", "Invalid index", token); break;
				case 3: AddSpecificationError("22.2-6", "Name shall index a non-empty string, in the String heap", "Empty name", token); break;
			}

			// 9. If Culture is non-null, it shall index a single string from the list specified
			switch (CheckCulture(row.Culture))
			{
				case 0: break;
				case 1: AddSpecificationError("22.2-9", "If Culture is non-null, it shall index a single string from the list specified", "Invalid or Missing Culture", token); break;
			}

		}

		protected static List<int> validHashAlgID = new List<int>() { 0x0000, 0x8003, 0x8004 };
		protected static List<int> validAssemblyFlags = new List<int>() { 0x0001, 0x0000, 0x0030, 0x0100, 0x8000, 0x4000 };

		protected static bool IsValidHashAlgID(int hash)
		{
			return validHashAlgID.Contains(hash);
		}

		protected static bool IsAssemblyFlags(int flag)
		{
			return validAssemblyFlags.Contains(flag);
		}

	}
}

