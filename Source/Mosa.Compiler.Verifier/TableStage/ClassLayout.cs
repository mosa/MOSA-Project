/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Tables;

namespace Mosa.Compiler.Verifier.TableStage
{
	public class ClassLayout : BaseTableVerificationStage
	{
		protected static List<int> packingSizes = new List<int>() { 0, 1, 2, 4, 8, 16, 32, 64, 128 };

		protected override void Run()
		{
			int rows = metadata.GetRowCount(TableType.ClassLayout);

			//1. A ClassLayout table can contain zero or more rows
			if (rows == 0)
				return;

			var maxTypeDefToken = metadata.GetMaxTokenValue(TableType.TypeDef);

			foreach (var token in new Token(TableType.ClassLayout, 1).Upto(rows))
			{
				ClassLayoutRow row = metadata.ReadClassLayoutRow(token);

				//2. Parent shall index a valid row in the TypeDef table, corresponding to a Class or ValueType (but not to an Interface) [ERROR]
				if (row.Parent.RID >= maxTypeDefToken.RID)
				{
					AddSpecificationError("22.8-2", "Parent shall index a valid row in the TypeDef table, corresponding to a Class or ValueType (but not to an Interface)", "Invalid index", token);
					continue;
				}

				TypeDefRow typeDefRow = metadata.ReadTypeDefRow(row.Parent);

				// 2. Parent shall index a valid row in the TypeDef table, corresponding to a Class or ValueType (but not to an Interface) [ERROR]
				if (((typeDefRow.Flags & TypeAttributes.ClassSemanticsMask) == TypeAttributes.Interface)
					|| ((typeDefRow.Flags & TypeAttributes.ClassSemanticsMask) != TypeAttributes.Class))
				{
					AddSpecificationError("22.8-2", "Parent shall index a valid row in the TypeDef table, corresponding to a Class or ValueType (but not to an Interface)", "Not a Class or ValueType", token);
				}

				//3. The Class or ValueType indexed by Parent shall be SequentialLayout or ExplicitLayout (§23.1.15). (That is, AutoLayout types shall not own any rows in the ClassLayout table.) [ERROR]
				if (((typeDefRow.Flags & TypeAttributes.LayoutMask) == TypeAttributes.AutoLayout)
					|| ((typeDefRow.Flags & TypeAttributes.LayoutMask) != TypeAttributes.SequentialLayout
					&& (typeDefRow.Flags & TypeAttributes.LayoutMask) != TypeAttributes.ExplicitLayout))
				{
					AddSpecificationError("22.8-3", "Parent shall index a valid row in the TypeDef table, corresponding to a Class or ValueType (but not to an Interface)", "Not a Class or ValueType", token);
				}

				//4. If Parent indexes a SequentialLayout type, then:
				if ((typeDefRow.Flags & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout)
				{
					//	o PackingSize shall be one of {0, 1, 2, 4, 8, 16, 32, 64, 128}. (0 means use the default pack size for the platform on which the application is running.) [ERROR]
					if (!packingSizes.Contains(row.PackingSize))
					{
						AddSpecificationError("22.8-4", "PackingSize shall be one of {0, 1, 2, 4, 8, 16, 32, 64, 128}", "Invalid Packing size", token);
					}

					//	o If Parent indexes a ValueType, then ClassSize shall be less than 1 MByte (0x100000 bytes). [ERROR]
				}

				//5. If Parent indexes an ExplicitLayout type, then
				if ((typeDefRow.Flags & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout)
				{
					//	o if Parent indexes a ValueType, then ClassSize shall be less than 1 MByte (0x100000 bytes) [ERROR]
					
					//	o PackingSize shall be 0. (It makes no sense to provide explicit offsets for each field, as well as a packing size.) [ERROR]
					if (row.PackingSize != 0)
					{
						AddSpecificationError("22.8-6", "PackingSize shall be 0", "Packing size not zero", token);
					}
				}

				//6. Note that an ExplicitLayout type might result in a verifiable type, provided the layout does not create a type whose fields overlap.

				//7. Layout along the length of an inheritance chain shall follow the rules specified above (starting at ‘highest’ Type, with no ‘holes’, etc.) [ERROR]

			}
		}

	}
}

