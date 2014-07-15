/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;
using System.Runtime.InteropServices;
using System.Reflection;

namespace System
{
	public sealed unsafe class _Type : Type
	{
		internal _Type(RuntimeTypeHandle handle)
			: base(handle)
		{

		}

		public override Assembly Assembly
		{
			get { throw new NotImplementedException(); }
		}

		public override Type BaseType
		{
			get { throw new NotImplementedException(); }
		}

		public override string FullName
		{
			get { throw new NotImplementedException(); }
		}

		public override string Namespace
		{
			get { throw new NotImplementedException(); }
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			throw new NotImplementedException();
		}

		public override Type GetElementType()
		{
			throw new NotImplementedException();
		}

		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotImplementedException();
		}

		public override Type[] GetInterfaces()
		{
			throw new NotImplementedException();
		}

		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		protected override bool HasElementTypeImpl()
		{
			throw new NotImplementedException();
		}

		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, Globalization.CultureInfo culture, string[] namedParameters)
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

		protected override bool IsPointerImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsPrimitiveImpl()
		{
			throw new NotImplementedException();
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}
	}
}