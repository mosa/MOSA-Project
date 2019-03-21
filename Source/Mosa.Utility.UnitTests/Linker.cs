// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Utility.UnitTests
{
	public static class Linker
	{
		public static LinkerMethodInfo GetMethodInfo(TypeSystem typeSystem, MosaLinker linker, UnitTestInfo unitTestInfo)
		{
			string fullMethodName = unitTestInfo.FullMethodName;

			int first = fullMethodName.LastIndexOf(".");
			int second = fullMethodName.LastIndexOf(".", first - 1);

			var methodNamespaceName = fullMethodName.Substring(0, second);
			var methodTypeName = fullMethodName.Substring(second + 1, first - second - 1);
			var methodName = fullMethodName.Substring(first + 1);

			var method = FindMosaMethod(
				typeSystem,
				methodNamespaceName,
				methodTypeName,
				methodName,
				unitTestInfo.Values);

			var address = GetMethodAddress(method, linker);

			var methodInfo = new LinkerMethodInfo
			{
				MethodNamespaceName = methodNamespaceName,
				MethodTypeName = methodTypeName,
				MethodName = methodName,
				MosaMethod = method,
				MosaMethodAddress = address
			};

			return methodInfo;
		}

		private static MosaMethod FindMosaMethod(TypeSystem typeSystem, string ns, string type, string method, params object[] parameters)
		{
			foreach (var t in typeSystem.AllTypes)
			{
				if (t.Name != type)
					continue;

				if (!string.IsNullOrEmpty(ns) && t.Namespace != ns)
					continue;

				foreach (var m in t.Methods)
				{
					if (m.Name == method && m.Signature.Parameters.Count == parameters.Length)
					{
						return m;
					}
				}

				break;
			}

			return null;
		}

		private static IntPtr GetMethodAddress(MosaMethod method, MosaLinker linker)
		{
			var symbol = linker.GetSymbol(method.FullName);

			if (symbol.VirtualAddress == 0)
			{
				return IntPtr.Zero;

				//Console.WriteLine(method.FullName);
			}

			return new IntPtr((long)symbol.VirtualAddress);
		}
	}
}
