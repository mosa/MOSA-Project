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
	public sealed unsafe class RuntimeTypeInfo : TypeInfo
	{
		public override string AssemblyQualifiedName
		{
			get { throw new NotImplementedException(); }
		}

		public override Assembly Assembly
		{
			get { throw new NotImplementedException(); }
		}

		public override TypeAttributes Attributes
		{
			get { throw new NotImplementedException(); }
		}

		public override Type BaseType
		{
			get { throw new NotImplementedException(); }
		}

		public override bool ContainsGenericParameters
		{
			get { throw new NotImplementedException(); }
		}

		public override MethodBase DeclaringMethod
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

		public override bool IsEnum
		{
			get { throw new NotImplementedException(); }
		}

		public override bool IsGenericParameter
		{
			get { throw new NotImplementedException(); }
		}

		public override bool IsGenericType
		{
			get { throw new NotImplementedException(); }
		}

		public override bool IsGenericTypeDefinition
		{
			get { throw new NotImplementedException(); }
		}

		public override bool IsSerializable
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

		public RuntimeTypeInfo()
		{

		}

		public override int GetArrayRank()
		{
			throw new NotImplementedException();
		}

		public override Type GetElementType()
		{
			throw new NotImplementedException();
		}

		public override Type[] GetGenericParameterConstraints()
		{
			throw new NotImplementedException();
		}

		public override Type GetGenericTypeDefinition()
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

		protected override bool IsPrimitiveImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsValueTypeImpl()
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