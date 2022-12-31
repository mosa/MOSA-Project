// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Quote from 0xd4d, https://github.com/0xd4d/dnlib/issues/230:
// "I don't plan on making that type public ever again. If you need to use it,
// you can just copy the source code to your project, it's not a lot of code."

namespace dnlib.DotNet
{
	internal readonly struct GenericArgumentsStack
	{
		private readonly List<IList<TypeSig?>> argsStack;
		private readonly bool isTypeVar;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="isTypeVar"><c>true</c> if it's for generic types, <c>false</c> if generic methods</param>
		public GenericArgumentsStack(bool isTypeVar)
		{
			argsStack = new List<IList<TypeSig>>();
			this.isTypeVar = isTypeVar;
		}

		/// <summary>
		/// Pushes generic arguments
		/// </summary>
		/// <param name="args">The generic arguments</param>
		public void Push(IList<TypeSig?> args) => argsStack.Add(args);

		/// <summary>
		/// Pops generic arguments
		/// </summary>
		/// <returns>The popped generic arguments</returns>
		public IList<TypeSig> Pop()
		{
			var index = argsStack.Count - 1;
			var result = argsStack[index];
			argsStack.RemoveAt(index);
			return result;
		}

		/// <summary>
		/// Resolves a generic argument
		/// </summary>
		/// <param name="number">Generic variable number</param>
		/// <returns>A <see cref="TypeSig"/> or <c>null</c> if none was found</returns>
		public TypeSig? Resolve(uint number)
		{
			TypeSig? result = null;
			for (var i = argsStack.Count - 1; i >= 0; i--)
			{
				var args = argsStack[i];
				if (number >= args.Count)
					return null;
				var typeSig = args[(int)number];
				if (typeSig is not GenericSig genericSig || genericSig.IsTypeVar != isTypeVar)
					return typeSig;
				result = genericSig;
				number = genericSig.Number;
			}
			return result;
		}
	}

	/// <summary>
	/// Replaces generic type/method var with its generic argument
	/// </summary>
	internal sealed class GenericArguments
	{
		private GenericArgumentsStack typeArgsStack = new(true);
		private GenericArgumentsStack methodArgsStack = new(false);

		/// <summary>
		/// Pushes generic arguments
		/// </summary>
		/// <param name="typeArgs">The generic arguments</param>
		public void PushTypeArgs(IList<TypeSig?> typeArgs) => typeArgsStack.Push(typeArgs);

		/// <summary>
		/// Pops generic arguments
		/// </summary>
		/// <returns>The popped generic arguments</returns>
		public IList<TypeSig> PopTypeArgs() => typeArgsStack.Pop();

		/// <summary>
		/// Pushes generic arguments
		/// </summary>
		/// <param name="methodArgs">The generic arguments</param>
		public void PushMethodArgs(IList<TypeSig?> methodArgs) => methodArgsStack.Push(methodArgs);

		/// <summary>
		/// Pops generic arguments
		/// </summary>
		/// <returns>The popped generic arguments</returns>
		public IList<TypeSig> PopMethodArgs() => methodArgsStack.Pop();

		/// <summary>
		/// Replaces a generic type/method var with its generic argument (if any). If
		/// <paramref name="typeSig"/> isn't a generic type/method var or if it can't
		/// be resolved, it itself is returned. Else the resolved type is returned.
		/// </summary>
		/// <param name="typeSig">Type signature</param>
		/// <returns>New <see cref="TypeSig"/> which is never <c>null</c> unless
		/// <paramref name="typeSig"/> is <c>null</c></returns>
		public TypeSig? Resolve(TypeSig? typeSig)
		{
			switch (typeSig)
			{
				case GenericMVar genericMVar:
				{
					var newSig = methodArgsStack.Resolve(genericMVar.Number);
					if (newSig == null || newSig == typeSig)
						return typeSig;
					return newSig;
				}
				case GenericVar genericVar:
				{
					var newSig = typeArgsStack.Resolve(genericVar.Number);
					if (newSig == null || newSig == typeSig)
						return typeSig;
					return newSig;
				}
				default:
				{
					return typeSig;
				}
			}
		}
	}
}
