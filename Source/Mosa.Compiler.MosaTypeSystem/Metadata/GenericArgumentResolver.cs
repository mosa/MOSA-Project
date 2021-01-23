// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem.Metadata
{
	/// <summary>
	/// Resolves generic arguments
	/// </summary>
	internal class GenericArgumentResolver
	{
		private readonly GenericArguments genericArguments;
		private RecursionCounter recursionCounter;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericArgumentResolver"/> class.
		/// </summary>
		public GenericArgumentResolver()
		{
			genericArguments = new GenericArguments();
			recursionCounter = new RecursionCounter();
		}

		/// <summary>
		/// Pushes the type generic arguments into resolver stack.
		/// </summary>
		/// <param name="genericArgs">The generic arguments.</param>
		public void PushTypeGenericArguments(IList<TypeSig> genericArgs)
		{
			genericArguments.PushTypeArgs(genericArgs);
		}

		/// <summary>
		/// Pushes the method generic arguments into resolver stack.
		/// </summary>
		/// <param name="genericArgs">The generic arguments.</param>
		public void PushMethodGenericArguments(IList<TypeSig> genericArgs)
		{
			genericArguments.PushMethodArgs(genericArgs);
		}

		/// <summary>
		/// Pop a set of type generic arguments out of resolver stack.
		/// </summary>
		public void PopTypeGenericArguments()
		{
			genericArguments.PopTypeArgs();
		}

		/// <summary>
		/// Pop a set of method generic arguments out of resolver stack.
		/// </summary>
		public void PopMethodGenericArguments()
		{
			genericArguments.PopMethodArgs();
		}

		/// <summary>
		/// Resolves the generic parameters in the specified type signature.
		/// </summary>
		/// <param name="typeSig">The type signature.</param>
		/// <returns>Resolved type signature.</returns>
		public TypeSig Resolve(TypeSig typeSig)
		{
			return ResolveGenericArgs(typeSig);
		}

		/// <summary>
		/// Resolves the generic parameters in the specified method signature.
		/// </summary>
		/// <param name="methodSig">The method signature.</param>
		/// <returns>Resolved method signature.</returns>
		public MethodSig Resolve(MethodSig methodSig)
		{
			return ResolveGenericArgs(methodSig);
		}

		private bool ReplaceGenericArg(ref TypeSig typeSig)
		{
			if (genericArguments == null)
				return false;
			var newTypeSig = genericArguments.Resolve(typeSig);
			if (newTypeSig != typeSig)
			{
				typeSig = newTypeSig;
				return true;
			}
			return false;
		}

		private MethodSig ResolveGenericArgs(MethodSig sig)
		{
			if (sig == null)
				return null;

			if (!recursionCounter.Increment())
				return null;

			var result = ResolveGenericArgs(new MethodSig(sig.GetCallingConvention()), sig);

			recursionCounter.Decrement();
			return result;
		}

		private MethodSig ResolveGenericArgs(MethodSig sig, MethodSig old)
		{
			sig.RetType = ResolveGenericArgs(old.RetType);

			foreach (var p in old.Params)
				sig.Params.Add(ResolveGenericArgs(p));

			if (sig.ParamsAfterSentinel != null)
			{
				foreach (var p in old.ParamsAfterSentinel)
				{
					sig.ParamsAfterSentinel.Add(ResolveGenericArgs(p));
				}
			}

			return sig;
		}

		private TypeSig ResolveGenericArgs(TypeSig typeSig)
		{
			if (!recursionCounter.Increment())
				return null;

			if (ReplaceGenericArg(ref typeSig))
			{
				recursionCounter.Decrement();
				return typeSig;
			}

			TypeSig result;
			switch (typeSig.ElementType)
			{
				case ElementType.Ptr: result = new PtrSig(ResolveGenericArgs(typeSig.Next)); break;
				case ElementType.ByRef: result = new ByRefSig(ResolveGenericArgs(typeSig.Next)); break;
				case ElementType.Var: result = new GenericVar((typeSig as GenericVar).Number); break;
				case ElementType.ValueArray: result = new ValueArraySig(ResolveGenericArgs(typeSig.Next), (typeSig as ValueArraySig).Size); break;
				case ElementType.SZArray: result = new SZArraySig(ResolveGenericArgs(typeSig.Next)); break;
				case ElementType.MVar: result = new GenericMVar((typeSig as GenericMVar).Number); break;
				case ElementType.CModReqd: result = new CModReqdSig((typeSig as ModifierSig).Modifier, ResolveGenericArgs(typeSig.Next)); break;
				case ElementType.CModOpt: result = new CModOptSig((typeSig as ModifierSig).Modifier, ResolveGenericArgs(typeSig.Next)); break;
				case ElementType.Module: result = new ModuleSig((typeSig as ModuleSig).Index, ResolveGenericArgs(typeSig.Next)); break;
				case ElementType.Pinned: result = new PinnedSig(ResolveGenericArgs(typeSig.Next)); break;
				case ElementType.FnPtr:
					if ((typeSig as FnPtrSig).Signature is null)
						return null;

					result = new FnPtrSig((typeSig as FnPtrSig).Signature);
					break;
				case ElementType.Array:
					var arraySig = (ArraySig)typeSig;
					var sizes = new List<uint>(arraySig.Sizes);
					var lbounds = new List<int>(arraySig.LowerBounds);
					result = new ArraySig(ResolveGenericArgs(typeSig.Next), arraySig.Rank, sizes, lbounds);
					break;

				case ElementType.GenericInst:
					var gis = (GenericInstSig)typeSig;
					var genArgs = new List<TypeSig>(gis.GenericArguments.Count);
					foreach (TypeSig ga in gis.GenericArguments)
					{
						genArgs.Add(ResolveGenericArgs(ga));
					}
					result = new GenericInstSig(ResolveGenericArgs(gis.GenericType) as ClassOrValueTypeSig, genArgs);
					break;

				default:
					result = typeSig;
					break;
			}

			recursionCounter.Decrement();

			return result;
		}
	}
}
