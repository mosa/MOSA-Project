/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using Mosa.Platform.Internal.x86;
using System.Reflection;
using x86Runtime = Mosa.Platform.Internal.x86.Runtime;

namespace System
{
	public sealed unsafe class RuntimeType : Type
	{
		private MetadataTypeStruct* typeStruct;
		private string fullname;
		private RuntimeTypeHandle handle;

		public override string AssemblyQualifiedName
		{
			get { throw new NotImplementedException(); }
		}

		public override Type DeclaringType
		{
			get { throw new NotImplementedException(); }
		}

		public override string FullName
		{
			get { throw new NotImplementedException(); }
		}

		public override int GenericParameterPosition
		{
			get { throw new NotImplementedException(); }
		}

		public override Type[] GenericTypeArguments
		{
			get { throw new NotImplementedException(); }
		}

		public override bool IsConstructedGenericType
		{
			get { throw new NotImplementedException(); }
		}

		public override bool IsGenericParameter
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override string Namespace
		{
			get { throw new NotImplementedException(); }
		}

		internal RuntimeType(RuntimeTypeHandle handle)
		{
			this.handle = handle;
			this.typeStruct = (MetadataTypeStruct*)((uint**)&handle)[0];
			this.fullname = x86Runtime.InitializeMetadataString((*this.typeStruct).Name);
		}

		public override int GetArrayRank()
		{
			throw new NotImplementedException();
		}

		public override Type GetElementType()
		{
			throw new NotImplementedException();
		}

		public override Type[] GetGenericTypeDefinition()
		{
			throw new NotImplementedException();
		}

		protected override bool HasElementTypeImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsArrayImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsByRefImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsNestedImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsPointerImpl()
		{
			throw new NotImplementedException();
		}

		public override Type MakeArrayType()
		{
			throw new NotImplementedException();
		}

		public override Type MakeArrayType(int rank)
		{
			throw new NotImplementedException();
		}

		public override Type MakeByRefType()
		{
			throw new NotImplementedException();
		}

		public override Type MakeGenericType(params Type[] typeArguments)
		{
			throw new NotImplementedException();
		}

		public override Type MakePointerType()
		{
			throw new NotImplementedException();
		}
	}
}