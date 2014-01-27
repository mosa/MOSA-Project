/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Metadata.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	///
	/// </summary>
	public class MosaTypeLoader
	{
		#region Data members

		/// <summary>
		/// The table count
		/// </summary>
		private const int TableCount = ((int)TableType.GenericParamConstraint >> 24) + 1;

		/// <summary>
		/// The resolver
		/// </summary>
		private readonly MosaTypeResolver resolver;

		/// <summary>
		/// Holds the metadata provider
		/// </summary>
		private IMetadataProvider metadataProvider;

		/// <summary>
		/// Holds the metadata module
		/// </summary>
		private IMetadataModule metadataModule;

		/// <summary>
		/// The signatures
		/// </summary>
		private Dictionary<HeapIndexToken, Signature> signatures;

		/// <summary>
		/// The map generic parameter
		/// </summary>
		private Dictionary<Token, MosaGenericParameter> mapGenericParam;

		/// <summary>
		/// The generic types
		/// </summary>
		private List<MosaType> genericTypes;

		/// <summary>
		/// The internal assembly
		/// </summary>
		private MosaAssembly internalAssembly = null;

		/// <summary>
		/// The table rows
		/// </summary>
		private int[] tableRows = new int[TableCount];

		/// <summary>
		/// The assembly
		/// </summary>
		private MosaAssembly assembly;

		/// <summary>
		/// Gets the entry method.
		/// </summary>
		/// <value>
		/// The entry method.
		/// </value>
		public MosaMethod EntryMethod { get; internal set; }

		/// <summary>
		/// The strings
		/// </summary>
		private Dictionary<HeapIndexToken, string> strings;

		/// <summary>
		/// The user strings
		/// </summary>
		private Dictionary<int, string> userStrings;

		/// <summary>
		/// The type infos
		/// </summary>
		private List<TypeInfo> typeInfos;

		/// <summary>
		/// The delayed generic
		/// </summary>
		private Queue<MosaType> delayedGeneric;

		#endregion Data members

		#region Constants

		/// <summary>
		/// The prologue id for a custom attribute blob.
		/// </summary>
		private const ushort ATTRIBUTE_BLOB_PROLOGUE = 0x0001;

		/// <summary>
		/// Length of a null string in a custom attribute blob.
		/// </summary>
		private const byte ATTRIBUTE_NULL_STRING_LEN = 0xFF;

		/// <summary>
		/// Length of an empty string in a custom attribute blob.
		/// </summary>
		private const byte ATTRIBUTE_EMPTY_STRING_LEN = 0x00;

		#endregion Constants

		private MosaTypeLoader(MosaTypeResolver resolver)
		{
			this.resolver = resolver;
		}

		public static MosaMethod Load(IMetadataModule metadataModule, MosaTypeResolver resolver)
		{
			var loader = new MosaTypeLoader(resolver);
			loader.Load(metadataModule);
			return loader.EntryMethod;
		}

		private void Load(IMetadataModule metadataModule)
		{
			this.metadataModule = metadataModule;
			this.metadataProvider = metadataModule.Metadata;

			RetrieveAllTableSizes();

			this.signatures = new Dictionary<HeapIndexToken, Signature>();
			this.userStrings = new Dictionary<int, string>();
			this.mapGenericParam = new Dictionary<Token, MosaGenericParameter>();
			this.genericTypes = new List<MosaType>();
			this.strings = new Dictionary<HeapIndexToken, string>();
			this.typeInfos = new List<TypeInfo>(GetTableRows(TableType.TypeDef));
			this.delayedGeneric = new Queue<MosaType>();

			assembly = new MosaAssembly();
			assembly.Name = metadataModule.Name;

			resolver.AddAssembly(assembly);
			resolver.AddUserStringHeap(assembly, metadataProvider.GetHeap(HeapType.UserString) as UserStringHeap);

			// Resolve all type refernces
			LoadTypeReferences();

			// Get type info - speeds up processing
			GetTypeInfo();

			// Loads all types (excluding generic instances)
			LoadTypes();

			// Loads generic parameters for types
			LoadGenericParams(TableType.TypeDef);

			// Loads all methods
			LoadMethods();

			// Loads generic parameters for methods
			LoadGenericParams(TableType.MethodDef);

			// Loads all fields
			LoadFields();
			LoadFieldLayout();
			LoadFieldData();

			// Load generic types
			LoadTypeSpecs();

			// Loads interfaces
			LoadInterfaces();

			// Load method types
			LoadMethodSpecs();

			// Resolve Delayed Generics
			ResolveDelayedGenerics();

			// Resolves all member references
			LoadMemberReferences();

			LoadGenericParamContraints();
			LoadCustomAttributes();
			LoadMethodImplementation();
			LoadExternals();

			if (metadataModule.ModuleType == ModuleType.Executable)
			{
				if (metadataModule.EntryPoint.RID != 0)
				{
					EntryMethod = resolver.GetMethodByToken(assembly, metadataModule.EntryPoint);
				}
			}

			// release
			this.metadataModule = null;
			this.metadataProvider = null;
			this.signatures = null;
			this.userStrings = null;
			this.mapGenericParam = null;
			this.internalAssembly = null;
			this.genericTypes = null;
			this.strings = null;
			this.typeInfos = null;
		}

		#region Internals

		/// <summary>
		///	Computes the size of each table and stores it for faster lookup
		/// </summary>
		private void RetrieveAllTableSizes()
		{
			for (int i = 0; i < TableCount; i++)
			{
				tableRows[i] = metadataProvider.GetRowCount((TableType)(i << 24));
			}
		}

		/// <summary>
		/// Gets the table rows.
		/// </summary>
		/// <param name="tokenType">Type of the token.</param>
		/// <returns></returns>
		private int GetTableRows(TableType tokenType)
		{
			return tableRows[(int)(tokenType) >> 24];
		}

		private HeapIndexToken GetMaxTokenValue(HeapIndexToken tokenType)
		{
			return (HeapIndexToken)(tableRows[(int)(tokenType) >> 24]) | (tokenType & HeapIndexToken.TableMask);
		}

		/// <summary>
		/// Get the last token for the table
		/// </summary>
		/// <param name="table">The table to lookup the token for</param>
		/// <returns>The last token inside the table</returns>
		private Token GetMaxTokenValue(TableType table)
		{
			return new Token(table, tableRows[(int)(table) >> 24]);
		}

		/// <summary>
		/// Loads the string from the heap table for stringIdx
		/// </summary>
		/// <param name="token">The string's index</param>
		/// <returns>The string at heap[stringIdx]</returns>
		private string GetString(HeapIndexToken token)
		{
			string value;

			if (strings.TryGetValue(token, out value))
			{
				return value;
			}

			value = metadataProvider.ReadString(token);
			strings.Add(token, value);

			return value;
		}

		private T RetrieveSignature<T>(HeapIndexToken token) where T : Signature
		{
			Signature signature;

			if (signatures.TryGetValue(token, out signature))
				return signature as T;
			else
				return (T)null;
		}

		private T StoreSignature<T>(HeapIndexToken token, T signature) where T : Signature
		{
			signatures.Add(token, signature);
			return signature;
		}

		private MethodSignature GetMethodSignature(HeapIndexToken token)
		{
			return (RetrieveSignature<MethodSignature>(token) ?? StoreSignature<MethodSignature>(token, new MethodSignature(metadataProvider, token)));
		}

		private FieldSignature GetFieldSignature(HeapIndexToken token)
		{
			return (RetrieveSignature<FieldSignature>(token) ?? StoreSignature<FieldSignature>(token, new FieldSignature(metadataProvider, token)));
		}

		private TypeSpecSignature GetTypeSpecSignature(HeapIndexToken token)
		{
			return (RetrieveSignature<TypeSpecSignature>(token) ?? StoreSignature<TypeSpecSignature>(token, new TypeSpecSignature(metadataProvider, token)));
		}

		private MethodSpecSignature GetMethodSpecSignature(HeapIndexToken token)
		{
			return (RetrieveSignature<MethodSpecSignature>(token) ?? StoreSignature<MethodSpecSignature>(token, new MethodSpecSignature(metadataProvider, token)));
		}

		private Signature GetMemberRefSignature(HeapIndexToken token)
		{
			return (RetrieveSignature<Signature>(token) ?? StoreSignature<Signature>(token, Signature.GetSignatureFromMemberRef(metadataProvider, token)));
		}

		private struct TypeInfo
		{
			public TypeDefRow TypeDefRow;
			public Token NestedClass;
			public Token EnclosingClass;
			public Token EndMethod;
			public Token EndField;
			public Token StartInterface;
			public Token EndInterface;
			public int Size;
			public short PackingSize;
		}

		#endregion Internals

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		private void LoadTypeReferences()
		{
			var maxToken = GetMaxTokenValue(TableType.TypeRef);
			foreach (var token in new Token(TableType.TypeRef, 1).Upto(maxToken))
			{
				MosaType type = null;

				var row = metadataProvider.ReadTypeRefRow(token);
				var typeName = GetString(row.TypeName);

				switch (row.ResolutionScope.Table)
				{
					case TableType.Module: goto case TableType.TypeRef;
					case TableType.TypeDef: throw new AssemblyLoadException();
					case TableType.TypeRef:
						{
							var row2 = metadataProvider.ReadTypeRefRow(row.ResolutionScope);
							var typeName2 = GetString(row2.TypeName);
							var @namespace = GetString(row2.TypeNamespace) + "." + typeName2;
							var asmRefRow = metadataProvider.ReadAssemblyRefRow(row2.ResolutionScope);
							var sourceAssemblyName = GetString(asmRefRow.Name);
							var sourceAssembly = resolver.GetAssemblyByName(sourceAssemblyName);
							type = resolver.GetTypeByName(sourceAssembly, @namespace, typeName);
							break;
						}
					case TableType.TypeSpec: throw new AssemblyLoadException();
					case TableType.ModuleRef: throw new AssemblyLoadException();
					case TableType.AssemblyRef:
						{
							var @namespace = GetString(row.TypeNamespace);
							var asmRefRow = metadataProvider.ReadAssemblyRefRow(row.ResolutionScope);
							var sourceAssemblyName = GetString(asmRefRow.Name);
							var sourceAssembly = resolver.GetAssemblyByName(sourceAssemblyName);
							type = resolver.GetTypeByName(sourceAssembly, @namespace, typeName);
							break;
						}
					default: throw new AssemblyLoadException();
				}

				if (type == null)
				{
					throw new AssemblyLoadException("Could not find genericBaseType: " + typeName);
				}

				resolver.AddType(assembly, token, type);
			}
		}

		private void GetTypeInfo()
		{
			var maxTypeDef = GetMaxTokenValue(TableType.TypeDef);
			var maxMethod = GetMaxTokenValue(TableType.MethodDef);
			var maxField = GetMaxTokenValue(TableType.Field);
			var maxLayout = GetMaxTokenValue(TableType.ClassLayout);
			var maxNestedClass = GetMaxTokenValue(TableType.NestedClass);
			var maxInterface = GetMaxTokenValue(TableType.InterfaceImpl);

			var classLayoutToken = new Token(TableType.ClassLayout, 1);
			ClassLayoutRow layoutClassRow = (maxLayout.RID != 0) ? metadataProvider.ReadClassLayoutRow(classLayoutToken) : null;

			var nestedtoken = new Token(TableType.NestedClass, 1);
			NestedClassRow nestedRow = (maxNestedClass.RID != 0) ? metadataProvider.ReadNestedClassRow(nestedtoken) : null;

			var interfaceToken = new Token(TableType.InterfaceImpl, 1);
			InterfaceImplRow interfaceRow = (maxInterface.RID != 0) ? metadataProvider.ReadInterfaceImplRow(interfaceToken) : null;

			TypeDefRow nextTypeDefRow = null;
			var typeDefRow = metadataProvider.ReadTypeDefRow(new Token(TableType.TypeDef, 1));

			foreach (var token in new Token(TableType.TypeDef, 1).Upto(maxTypeDef))
			{
				var info = new TypeInfo();

				info.TypeDefRow = typeDefRow;
				info.NestedClass = (nestedRow != null && nestedRow.NestedClass == token) ? nestedRow.NestedClass : Token.Zero;
				info.EnclosingClass = (nestedRow != null && nestedRow.NestedClass == token) ? nestedRow.EnclosingClass : Token.Zero;
				info.Size = (layoutClassRow != null && layoutClassRow.Parent == token) ? layoutClassRow.ClassSize : 0;
				info.PackingSize = (layoutClassRow != null && layoutClassRow.Parent == token) ? layoutClassRow.PackingSize : (short)0;
				info.StartInterface = (interfaceRow != null && interfaceRow.Class == token) ? interfaceToken : Token.Zero;
				info.EndInterface = info.StartInterface;

				if (token.RID < maxTypeDef.RID)
				{
					nextTypeDefRow = metadataProvider.ReadTypeDefRow(token.NextRow);

					info.EndField = nextTypeDefRow.FieldList;
					info.EndMethod = nextTypeDefRow.MethodList;

					if (info.EndMethod.RID > maxMethod.RID)
						info.EndMethod = maxMethod.NextRow;

					if (info.EndField.RID > maxField.RID)
						info.EndField = maxField.NextRow;
				}
				else
				{
					info.EndMethod = maxMethod.NextRow;
					info.EndField = maxField.NextRow;
				}

				typeInfos.Add(info);

				if (nestedRow != null && nestedRow.NestedClass == token)
				{
					nestedtoken = nestedtoken.NextRow;
					if (nestedtoken.RID <= maxNestedClass.RID)
						nestedRow = metadataProvider.ReadNestedClassRow(nestedtoken);
				}

				if (layoutClassRow != null && layoutClassRow.Parent == token)
				{
					classLayoutToken = classLayoutToken.NextRow;
					if (classLayoutToken.RID <= maxLayout.RID)
						layoutClassRow = metadataProvider.ReadClassLayoutRow(classLayoutToken);
				}

				while (interfaceRow != null && interfaceRow.Class == token)
				{
					info.EndInterface = interfaceToken;

					interfaceToken = interfaceToken.NextRow;
					if (interfaceToken.RID <= maxInterface.RID)
					{
						interfaceRow = metadataProvider.ReadInterfaceImplRow(interfaceToken);
					}
					else
					{
						interfaceRow = null;
					}
				}

				typeDefRow = nextTypeDefRow;
			}
		}

		private void LoadTypes()
		{
			var maxTypeDef = GetMaxTokenValue(TableType.TypeDef);

			foreach (var token in new Token(TableType.TypeDef, 1).Upto(maxTypeDef))
			{
				LoadType(token);
			}
		}

		private void LoadMethods()
		{
			var maxTypeDef = GetMaxTokenValue(TableType.TypeDef);

			foreach (var token in new Token(TableType.TypeDef, 1).Upto(maxTypeDef))
			{
				var type = resolver.GetTypeByToken(assembly, token);

				var info = typeInfos[(int)token.RID - 1];

				LoadMethods(type, info.TypeDefRow.MethodList, info.EndMethod);
			}
		}

		private void LoadFields()
		{
			var maxTypeDef = GetMaxTokenValue(TableType.TypeDef);

			foreach (var token in new Token(TableType.TypeDef, 1).Upto(maxTypeDef))
			{
				var type = resolver.GetTypeByToken(assembly, token);

				var info = typeInfos[(int)token.RID - 1];

				LoadFields(type, info.TypeDefRow.FieldList, info.EndField);
			}
		}

		private void LoadInterfaces()
		{
			var maxTypeDef = GetMaxTokenValue(TableType.TypeDef);

			foreach (var token in new Token(TableType.TypeDef, 1).Upto(maxTypeDef))
			{
				var type = resolver.GetTypeByToken(assembly, token);

				var info = typeInfos[(int)token.RID - 1];

				LoadInterfaces(type, info.StartInterface, info.EndInterface);
			}
		}

		private void LoadType(Token token)
		{
			if (resolver.CheckTypeExists(assembly, token))
				return;

			var info = typeInfos[(int)token.RID - 1];

			if (info.TypeDefRow.Extends.RID != 0)
			{
				switch (info.TypeDefRow.Extends.Table)
				{
					case TableType.TypeDef: LoadType(info.TypeDefRow.Extends); break;
					case TableType.TypeSpec: LoadTypeSpec(info.TypeDefRow.Extends); break;
					case TableType.TypeRef: break;
					default: throw new AssemblyLoadException("unexpected token genericBaseType.");
				}
			}

			var enclosingType = (info.NestedClass == token) ? resolver.GetTypeByToken(assembly, info.EnclosingClass) : null;

			MosaType type = new MosaType(assembly);
			type.Name = GetString(info.TypeDefRow.TypeName);
			type.BaseType = (info.TypeDefRow.Extends.RID == 0) ? null : resolver.GetTypeByToken(assembly, info.TypeDefRow.Extends);
			type.EnclosingType = enclosingType;
			type.IsNested =
				(info.TypeDefRow.Flags & TypeAttributes.NestedPublic) == TypeAttributes.NestedPublic ||
				(info.TypeDefRow.Flags & TypeAttributes.NestedPrivate) == TypeAttributes.NestedPrivate ||
				(info.TypeDefRow.Flags & TypeAttributes.NestedFamily) == TypeAttributes.NestedFamily;
			type.Namespace = type.IsNested ? enclosingType.FullName : GetString(info.TypeDefRow.TypeNamespace);
			type.FullName = type.Namespace + "." + type.Name;
			type.IsInterface = (info.TypeDefRow.Flags & TypeAttributes.Interface) == TypeAttributes.Interface;
			type.IsExplicitLayout = (info.TypeDefRow.Flags & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;
			type.PackingSize = info.PackingSize;
			type.Size = info.Size;

			type.SetFlags();
			type.SetOpenGeneric();

			resolver.AddType(assembly, token, type);
		}

		private void LoadMethods(MosaType declaringType, Token first, Token last)
		{
			if (declaringType.AreMethodsAssigned)
				return;

			if (first.RID >= last.RID)
			{
				declaringType.AreMethodsAssigned = true;
				return;
			}

			var maxMethod = GetMaxTokenValue(TableType.MethodDef);
			var methodDef = metadataProvider.ReadMethodDefRow(first);
			MethodDefRow nextMethodDef = null;

			foreach (var token in first.Upto(last.PreviousRow))
			{
				Token maxParam;

				if (token.RID < maxMethod.RID)
				{
					nextMethodDef = metadataProvider.ReadMethodDefRow(token.NextRow);
					maxParam = nextMethodDef.ParamList;
				}
				else
				{
					maxParam = GetMaxTokenValue(TableType.Param).NextRow;
				}

				var signature = GetMethodSignature(methodDef.SignatureBlob);

				var method = new MosaMethod();
				method.Name = GetString(methodDef.NameString);
				method.DeclaringType = declaringType;
				method.ReturnType = GetMosaType(signature.ReturnType);
				method.HasThis = signature.HasThis;
				method.HasExplicitThis = signature.HasExplicitThis;
				method.IsInternal = (methodDef.ImplFlags & MethodImplAttributes.InternalCall) == MethodImplAttributes.InternalCall;
				method.IsNoInlining = (methodDef.ImplFlags & MethodImplAttributes.NoInlining) == MethodImplAttributes.NoInlining;
				method.IsAbstract = (methodDef.Flags & MethodAttributes.Abstract) == MethodAttributes.Abstract;
				method.IsStatic = (methodDef.Flags & MethodAttributes.Static) == MethodAttributes.Static;
				method.IsSpecialName = (methodDef.Flags & MethodAttributes.SpecialName) == MethodAttributes.SpecialName;
				method.IsRTSpecialName = (methodDef.Flags & MethodAttributes.RTSpecialName) == MethodAttributes.RTSpecialName;
				method.IsVirtual = ((methodDef.Flags & MethodAttributes.Virtual) == MethodAttributes.Virtual);
				method.IsPInvokeImpl = ((methodDef.Flags & MethodAttributes.PInvokeImpl) == MethodAttributes.PInvokeImpl);
				method.IsNewSlot = ((methodDef.Flags & MethodAttributes.NewSlot) == MethodAttributes.NewSlot);
				method.IsFinal = ((methodDef.Flags & MethodAttributes.Final) == MethodAttributes.Final);
				method.IsCILGenerated = true;
				method.Rva = methodDef.Rva;

				if (methodDef.Rva != 0)
				{
					var code = metadataModule.GetInstructionStream((long)methodDef.Rva);
					var codeReader = new EndianAwareBinaryReader(code, Endianness.Little);
					var header = new MethodHeader(codeReader);

					method.Code = codeReader.ReadBytes(header.CodeSize);
					method.CodeAssembly = assembly;

					if (header.LocalVarSigTok.RID != 0)
					{
						var standAlongSigRow = metadataProvider.ReadStandAloneSigRow(header.LocalVarSigTok);
						var localVariables = new LocalVariableSignature(metadataProvider, standAlongSigRow.SignatureBlob);

						foreach (var local in localVariables.Locals)
						{
							var type = GetMosaType(local.Type);

							method.LocalVariables.Add(type);
						}
					}

					foreach (var handler in header.Clauses)
					{
						ExceptionBlock block = new ExceptionBlock();

						block.TryOffset = handler.TryOffset;
						block.TryLength = handler.TryLength;
						block.HandlerOffset = handler.HandlerOffset;
						block.HandlerLength = handler.HandlerLength;
						block.FilterOffset = handler.FilterOffset;

						block.Type = (handler.ClassToken.Value != 0) ? resolver.GetTypeByToken(assembly, handler.ClassToken) : null;

						switch (handler.ExceptionHandlerType)
						{
							case ExceptionHandlerType.Exception: block.ExceptionHandler = ExceptionBlockType.Exception; break;
							case ExceptionHandlerType.Fault: block.ExceptionHandler = ExceptionBlockType.Fault; break;
							case ExceptionHandlerType.Filter: block.ExceptionHandler = ExceptionBlockType.Filter; break;
							case ExceptionHandlerType.Finally: block.ExceptionHandler = ExceptionBlockType.Finally; break;
						}

						method.ExceptionBlocks.Add(block);
					}
				}

				LoadParameters(method, methodDef.ParamList, maxParam, signature);

				method.SetName();
				method.SetOpenGeneric();

				declaringType.Methods.Add(method);
				resolver.AddMethod(assembly, token, method);

				methodDef = nextMethodDef;
			}

			declaringType.AreMethodsAssigned = true;
		}

		private void LoadFields(MosaType declaringType, Token first, Token last)
		{
			if (declaringType.AreFieldsAssigned)
				return;

			declaringType.AreFieldsAssigned = true;

			foreach (var token in first.Upto(last.PreviousRow))
			{
				var fieldRow = metadataProvider.ReadFieldRow(token);

				if ((fieldRow.Flags & FieldAttributes.HasDefault) == FieldAttributes.HasDefault)
				{
					// FIXME!: Has a default value.
					//Debug.Assert(false);
				}

				var field = new MosaField();
				field.Name = GetString(fieldRow.Name);
				field.FullName = declaringType.FullName + "." + field.Name;
				field.DeclaringType = declaringType;
				field.IsLiteralField = (fieldRow.Flags & FieldAttributes.Literal) == FieldAttributes.Literal;
				field.IsStaticField = (fieldRow.Flags & FieldAttributes.Static) == FieldAttributes.Static;
				field.HasRVA = (fieldRow.Flags & FieldAttributes.HasFieldRVA) == FieldAttributes.HasFieldRVA;
				field.HasRVA = (fieldRow.Flags & FieldAttributes.HasDefault) == FieldAttributes.HasDefault;
				field.Type = GetMosaType(GetFieldSignature(fieldRow.Signature).Type);

				declaringType.Fields.Add(field);
				resolver.AddField(assembly, token, field);
			}
		}

		private MosaType GetMosaType(SigType sigType)
		{
			switch (sigType.Type)
			{
				case CilElementType.Class: return resolver.GetTypeByToken(assembly, (sigType as TypeSigType).Token);
				case CilElementType.ValueType: goto case CilElementType.Class;
				case CilElementType.Var: return resolver.GetVarType((sigType as VarSigType).Index);
				case CilElementType.MVar: return resolver.GetMVarType((sigType as MVarSigType).Index);
				case CilElementType.Ptr: return resolver.GetUnmanagedPointerType(GetMosaType((sigType as PtrSigType).ElementType));
				case CilElementType.ByRef: return resolver.GetManagedPointerType(GetMosaType((sigType as RefSigType).ElementType));
				case CilElementType.Array: return resolver.GetArrayType(GetMosaType((sigType as ArraySigType).ElementType));
				case CilElementType.SZArray: return resolver.GetArrayType(GetMosaType((sigType as SZArraySigType).ElementType));
				case CilElementType.GenericInst: return CreateGenericInstance(sigType as GenericInstSigType, Token.Zero);
				default: break;
			}

			var builtInSigType = sigType as BuiltInSigType;
			if (builtInSigType != null)
			{
				return resolver.GetTypeByElementType(builtInSigType.Type);
			}

			throw new NotImplementedException(String.Format("SigType of CilElementType.{0} is not supported.", sigType.Type));
		}

		/// <summary>
		/// Loads the generic param constraints.
		/// </summary>
		private void LoadGenericParams(TableType table)
		{
			var maxToken = GetMaxTokenValue(TableType.GenericParam);
			foreach (var token in new Token(TableType.GenericParam, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadGenericParamRow(token);

				if (row.Owner.Table != table)
					continue;

				var genericParameter = new MosaGenericParameter();
				genericParameter.Name = GetString(row.NameString);
				genericParameter.Index = row.Number;
				genericParameter.IsCovariant = (row.Flags & GenericParameterAttributes.Covariant) == GenericParameterAttributes.Covariant;
				genericParameter.IsContravariant = (row.Flags & GenericParameterAttributes.Contravariant) == GenericParameterAttributes.Contravariant;
				genericParameter.IsNonVariant = (row.Flags & GenericParameterAttributes.NonVariant) == GenericParameterAttributes.NonVariant;

				mapGenericParam.Add(token, genericParameter);

				switch (row.Owner.Table)
				{
					case TableType.TypeDef:
						var type = resolver.GetTypeByToken(assembly, row.Owner);
						type.GenericParameters.Add(genericParameter);
						type.IsBaseGeneric = true;
						break;

					case TableType.MethodDef:
						var method = resolver.GetMethodByToken(assembly, row.Owner);
						method.GenericParameters.Add(genericParameter);
						method.IsBaseGeneric = true;
						break;

					default: throw new NotImplementedException();
				}
			}
		}

		private void LoadGenericParamContraints()
		{
			var maxToken = GetMaxTokenValue(TableType.GenericParamConstraint);
			foreach (var token in new Token(TableType.GenericParamConstraint, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadGenericParamConstraintRow(token);

				var genericParam = mapGenericParam[row.Owner];

				var type = resolver.GetTypeByToken(assembly, row.Constraint);

				genericParam.Constraints.Add(type);
			}
		}

		private void LoadParameters(MosaMethod method, Token first, Token max, MethodSignature signature)
		{
			int index = 0;

			foreach (var token in first.Upto(max.PreviousRow))
			{
				var paramDef = metadataProvider.ReadParamRow(token);
				var parameter = new MosaParameter();

				parameter.Name = GetString(paramDef.Name);
				parameter.IsIn = (paramDef.Flags & ParameterAttributes.In) == ParameterAttributes.In;
				parameter.IsOut = (paramDef.Flags & ParameterAttributes.Out) == ParameterAttributes.Out;
				parameter.Position = paramDef.Sequence;
				parameter.Type = GetMosaType(signature.Parameters[index++]);

				method.Parameters.Add(parameter);
			}
		}

		private void LoadTypeSpecs()
		{
			var maxToken = GetMaxTokenValue(TableType.TypeSpec);
			foreach (var token in new Token(TableType.TypeSpec, 1).Upto(maxToken))
			{
				LoadTypeSpec(token);
			}
		}

		private void LoadTypeSpec(Token token)
		{
			if (resolver.CheckTypeExists(assembly, token))
				return;

			var row = metadataProvider.ReadTypeSpecRow(token);
			var signature = GetTypeSpecSignature(row.SignatureBlob);

			var genericSig = signature.Type as GenericInstSigType;
			if (genericSig == null)
			{
				var type = GetMosaType(signature.Type);
				resolver.AddType(assembly, token, type);
				return;
			}

			CreateGenericInstance(genericSig, token);
		}

		private MosaType CreateGenericInstance(GenericInstSigType genericSig, Token token)
		{
			var genericBaseType = GetMosaType(genericSig.BaseType);

			List<MosaType> genericArguments = new List<MosaType>(genericSig.GenericArguments.Length);

			foreach (var genericParam in genericSig.GenericArguments)
			{
				var genericParamType = GetMosaType(genericParam);
				genericArguments.Add(genericParamType);
			}

			var genericType = resolver.ResolveGenericType(genericBaseType, genericArguments);

			if (!token.IsZero)
			{
				resolver.AddType(assembly, token, genericType);
			}

			if (!genericTypes.Contains(genericType))
			{
				genericTypes.Add(genericType);

				if (!genericType.AreMethodsAssigned || !genericType.AreFieldsAssigned || !genericType.AreInterfacesAssigned)
				{
					delayedGeneric.Enqueue(genericType);
				}
			}

			return genericType;
		}

		private void ResolveDelayedGenerics()
		{
			while (delayedGeneric.Count != 0)
			{
				var genericType = delayedGeneric.Dequeue();

				resolver.AddGenericMethods(genericType);
				resolver.AddGenericFields(genericType);
				resolver.AddGenericInterfaces(genericType);

				if (!genericType.AreMethodsAssigned || !genericType.AreFieldsAssigned || !genericType.AreInterfacesAssigned)
				{
					delayedGeneric.Enqueue(genericType);
				}
			}
		}

		private void LoadMethodSpecs()
		{
			var maxToken = GetMaxTokenValue(TableType.MethodSpec);
			foreach (var token in new Token(TableType.MethodSpec, 1).Upto(maxToken))
			{
				LoadMethodSpec(token);
			}
		}

		private void LoadMethodSpec(Token token)
		{
			if (resolver.CheckMethodExists(assembly, token))
				return;

			var row = metadataProvider.ReadMethodSpecRow(token);
			var signature = GetMethodSpecSignature(row.InstantiationBlob);
			var method = resolver.GetMethodByToken(assembly, row.Method);

			List<MosaType> genericParamTypes = new List<MosaType>();

			foreach (var genericParam in signature.Types)
			{
				var genericParamType = GetMosaType(genericParam);
				genericParamTypes.Add(genericParamType);
			}

			var genericMethod = resolver.ResolveGenericMethod(method, genericParamTypes);

			resolver.AddMethod(assembly, token, genericMethod);
		}

		private void LoadMemberReferences()
		{
			var maxToken = GetMaxTokenValue(TableType.MemberRef);
			foreach (var token in new Token(TableType.MemberRef, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadMemberRefRow(token);
				var name = GetString(row.NameString);

				MosaType ownerType = null;

				if (row.Class.Table == TableType.TypeDef || row.Class.Table == TableType.TypeRef || row.Class.Table == TableType.TypeSpec)
				{
					ownerType = resolver.GetTypeByToken(assembly, row.Class);
				}
				else
				{
					throw new TypeLoadException(String.Format("Failed to retrieve owner genericBaseType for Token {0:x} (Table {1})", row.Class, row.Class.Table));
				}

				var signature = GetMemberRefSignature(row.SignatureBlob);

				if (signature is FieldSignature)
				{
					foreach (var field in ownerType.Fields)
					{
						if (field.Name == name)
						{
							resolver.AddField(assembly, token, field);
							break;
						}
					}
				}
				else
				{
					MethodSignature methodSignature = signature as MethodSignature;
					Debug.Assert(signature is MethodSignature);

					List<MosaType> typeSignature = new List<MosaType>(methodSignature.Parameters.Length);

					for (int index = 0; index < methodSignature.Parameters.Length; index++)
					{
						var parameter = methodSignature.Parameters[index];

						var parameterType = GetMosaType(parameter);

						if (parameterType.IsVarFlag)
						{
							parameterType = ownerType.GenericArguments[parameterType.VarOrMVarIndex];
						}
						else if (parameterType.IsMVarFlag)
						{
						}

						typeSignature.Add(parameterType);
					}

					MosaMethod targetMethod = null;

					foreach (var method in ownerType.Methods)
					{
						if (method.Name == name)
						{
							if (method.Matches(typeSignature))
							{
								targetMethod = method;
								break;
							}
						}
					}

					// Special case: string.get_Chars is same as string.get_Item
					if (name == "get_Chars" && ownerType.FullName == "System.String")
					{
						name = "get_Item";

						foreach (var method in ownerType.Methods)
						{
							if (method.Matches(typeSignature))
							{
								targetMethod = method;
								break;
							}
						}
					}

					if (targetMethod == null)
					{
						throw new TypeLoadException(String.Format("Failed to retrieve method for Token {0:x} (Table {1})", row.Class, row.Class.Table));
					}

					resolver.AddMethod(assembly, token, targetMethod);
				}
			}
		}

		private void LoadInterfaces(MosaType declaringType, Token first, Token last)
		{
			if (declaringType.AreInterfacesAssigned)
				return;

			declaringType.AreInterfacesAssigned = true;

			if (first.RID == 0)
				return;

			foreach (var token in first.Upto(last))
			{
				var row = metadataProvider.ReadInterfaceImplRow(token);

				Debug.Assert(resolver.GetTypeByToken(assembly, row.Class) == declaringType);

				var interfaceType = resolver.GetTypeByToken(assembly, row.Interface);

				declaringType.Interfaces.Add(interfaceType);
			}
		}

		/// <summary>
		/// Loads all custom attributes from the assembly.
		/// </summary>
		private void LoadCustomAttributes()
		{
			var maxToken = GetMaxTokenValue(TableType.CustomAttribute);
			foreach (var token in new Token(TableType.CustomAttribute, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadCustomAttributeRow(token);

				var attribute = new MosaAttribute();
				var ctor = resolver.GetMethodByToken(assembly, row.Type);
				var blob = metadataProvider.ReadBlob(row.Value);

				attribute.CtorMethod = ctor;

				// Create a binary reader for the blob
				using (var reader = new BinaryReader(new MemoryStream(blob), Encoding.UTF8))
				{
					var prologue = reader.ReadUInt16();

					if (prologue != ATTRIBUTE_BLOB_PROLOGUE)
					{
						throw new InvalidMetadataException(@"Invalid custom attribute blob.");
					}

					var parameters = ctor.Parameters.Count;

					for (int index = 0; index < parameters; index++)
					{
						var type = ctor.Parameters[index].Type;

						object value = null;

						if (type.IsBoolean) value = reader.ReadByte() == 1;
						else if (type.IsChar) value = (char)reader.ReadUInt16();
						else if (type.IsSignedByte) value = reader.ReadSByte();
						else if (type.IsSignedShort) value = reader.ReadInt16();
						else if (type.IsSignedInt) value = reader.ReadInt32();
						else if (type.IsSignedLong) value = reader.ReadInt64();
						else if (type.IsUnsignedByte) value = reader.ReadByte();
						else if (type.IsUnsignedShort) value = reader.ReadUInt16();
						else if (type.IsUnsignedInt) value = reader.ReadUInt32();
						else if (type.IsUnsignedLong) value = reader.ReadUInt64();
						else if (type.IsSingle) value = reader.ReadSingle();
						else if (type.IsDouble) value = reader.ReadDouble();
						else if (type.IsString) value = ParseString(reader);

						//FIXME! Add more

						attribute.Values.Add(value);
					}
				}

				// The following switch matches the AttributeTargets enumeration against
				// metadata tables, which make valid targets for an attribute.
				switch (row.Parent.Table)
				{
					case TableType.Assembly:
						// AttributeTargets.Assembly
						break;

					case TableType.AssemblyRef:
						break;

					case TableType.TypeDef:
						// AttributeTargets.Class
						// AttributeTargets.Delegate
						// AttributeTargets.Enum
						// AttributeTargets.Interface
						// AttributeTargets.Struct
						resolver.GetTypeByToken(assembly, row.Parent).CustomAttributes.Add(attribute);
						break;

					case TableType.MethodDef:
						// AttributeTargets.Constructor
						// AttributeTargets.Method
						resolver.GetMethodByToken(assembly, row.Parent).CustomAttributes.Add(attribute);
						break;

					case TableType.Event:
						// AttributeTargets.Event
						break;

					case TableType.Field:
						// AttributeTargets.Field
						resolver.GetFieldByToken(assembly, row.Parent).CustomAttributes.Add(attribute);
						break;

					case TableType.GenericParam:
						// AttributeTargets.GenericParameter
						break;

					case TableType.Module:
						// AttributeTargets.Module
						break;

					case TableType.Param:
						// AttributeTargets.Parameter
						// AttributeTargets.ReturnValue
						break;

					case TableType.Property:
						// AttributeTargets.StackFrameIndex
						break;

					default: throw new AssemblyLoadException();
				}
			}
		}

		/// <summary>
		/// Parses a string definition for an attribute field, parameter or property definition.
		/// </summary>
		/// <param name="reader">The binary reader to read From.</param>
		/// <returns>A string, which represents the value read.</returns>
		private static string ParseString(BinaryReader reader)
		{
			// Read the length
			int packedLen = DecodePackedLen(reader);

			if (ATTRIBUTE_NULL_STRING_LEN == packedLen)
				return null;

			if (ATTRIBUTE_EMPTY_STRING_LEN == packedLen)
				return String.Empty;

			// Read the string
			var buffer = reader.ReadChars(packedLen);

			return new String(buffer);
		}

		private static int DecodePackedLen(BinaryReader reader)
		{
			int result, offset;
			byte value = reader.ReadByte();

			if (0xC0 == (value & 0xC0))
			{
				// A 4 byte length...
				result = ((value & 0x1F) << 24);
				offset = 16;
			}
			else if (0x80 == (value & 0x80))
			{
				// A 2 byte length...
				result = ((value & 0x3F) << 8);
				offset = 0;
			}
			else
			{
				result = value & 0x7F;
				offset = -8;
			}

			while (offset != -8)
			{
				result |= (reader.ReadByte() << offset);
				offset -= 8;
			}

			return result;
		}

		private void LoadMethodImplementation()
		{
			var maxToken = GetMaxTokenValue(TableType.MethodImpl);
			foreach (var token in new Token(TableType.MethodImpl, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadMethodImplRow(token);

				var type = resolver.GetTypeByToken(assembly, row.Class);
				var body = resolver.GetMethodByToken(assembly, row.MethodBody);
				var declaration = resolver.GetMethodByToken(assembly, row.MethodDeclaration);

				type.InheritanceOveride.Add(declaration, body);
			}
		}

		/// <summary>
		/// Loads the externals.
		/// </summary>
		private void LoadExternals()
		{
			var maxToken = GetMaxTokenValue(TableType.ImplMap);
			foreach (var token in new Token(TableType.ImplMap, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadImplMapRow(token);

				var moduleRow = metadataProvider.ReadModuleRefRow(row.ImportScopeTable);
				var external = GetString(moduleRow.NameString);

				var method = resolver.GetMethodByToken(assembly, row.MemberForwarded);

				method.ExternalReference = external;
			}
		}

		private void LoadFieldData()
		{
			var maxToken = GetMaxTokenValue(TableType.FieldRVA);
			foreach (var token in new Token(TableType.FieldRVA, 1).Upto(maxToken))
			{
				var fieldRVARow = metadataProvider.ReadFieldRVARow(token);
				var field = resolver.GetFieldByToken(assembly, fieldRVARow.Field);

				field.HasRVA = true;
				field.RVA = fieldRVARow.Rva;

				int? size = DetermineFieldSize(field);

				if (!size.HasValue)
				{
					throw new InvalidMetadataException("Unable to determine field Size");
				}

				var data = metadataModule.GetDataSection((long)field.RVA);
				var dataReader = new EndianAwareBinaryReader(data, Endianness.Little);
				field.Data = dataReader.ReadBytes(size.Value);
			}
		}

		private int? DetermineFieldSize(MosaField field)
		{
			if (field.Type.IsBuiltInType)
			{
				return field.Type.FixedSize;
			}
			else if (field.Type.IsValueType)
			{
				int size = 0;

				foreach (var valueField in field.Type.Fields)
				{
					int? valueSize = DetermineFieldSize(valueField);

					if (!valueSize.HasValue)
					{
						return null;
					}

					size = size + valueSize.Value;
				}

				return size;
			}

			return null;
		}

		private void LoadFieldLayout()
		{
			var maxToken = GetMaxTokenValue(TableType.FieldLayout);
			foreach (var token in new Token(TableType.FieldLayout, 1).Upto(maxToken))
			{
				var fieldLayoutRow = metadataProvider.ReadFieldLayoutRow(token);
				var field = resolver.GetFieldByToken(assembly, fieldLayoutRow.Field);

				field.Offset = fieldLayoutRow.Offset;
			}
		}
	}
}