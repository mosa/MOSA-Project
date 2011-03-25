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
using System.Text;
using System.IO;
using System.Collections;

using Mono.Cecil;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Tools.MetadataExplorer.Tables
{

	public static class Resolver
	{
		public static TableRow GetTableRow(IMetadataProvider metadata, MetadataToken metadataToken)
		{
			TokenType tokenType = metadataToken.TokenType;

			switch (tokenType)
			{
				case TokenType.File: return new FileRowExt(metadata, metadata.ReadFileRow((TokenTypes)metadataToken.ToInt32()));
				case TokenType.TypeDef: return new TypeDefRowExt(metadata, metadata.ReadTypeDefRow((TokenTypes)metadataToken.ToInt32()));
				case TokenType.TypeSpec: return new TypeSpecRowExt(metadata, metadata.ReadTypeSpecRow((TokenTypes)metadataToken.ToInt32()));
				case TokenType.TypeRef: return new TypeRefRowExt(metadata, metadata.ReadTypeRefRow((TokenTypes)metadataToken.ToInt32()));
				case TokenType.Field: return new FieldRowExt(metadata, metadata.ReadFieldRow((TokenTypes)metadataToken.ToInt32()));
				case TokenType.Method: return new MethodDefRowExt(metadata, metadata.ReadMethodDefRow((TokenTypes)metadataToken.ToInt32()));
					
				default: return null;
			}
		}
	}
}
