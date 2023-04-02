// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CLR;

public sealed class ClrPlugSystem : PlugSystem
{
	#region Data Members

	protected readonly TypeSystem TypeSystem;

	protected readonly MosaType PlugMethodAttribute;

	protected readonly List<MosaMethod> PlugMethodList = new List<MosaMethod>();
	protected readonly Dictionary<MosaMethod, MosaMethod> PlugMethods = new Dictionary<MosaMethod, MosaMethod>();
	protected readonly HashSet<MosaMethod> NoReplacement = new HashSet<MosaMethod>();

	private readonly object _lock = new object();

	#endregion Data Members

	private const string PlugMethodAttributeName = "Mosa.Runtime.Plug.PlugAttribute";

	public ClrPlugSystem(TypeSystem typeSystem)
	{
		TypeSystem = typeSystem;

		// Collect all plug methods
		foreach (var type in TypeSystem.AllTypes)
		{
			foreach (var method in type.Methods)
			{
				if (!method.IsStatic)
					continue;

				var methodAttribute = method.FindCustomAttribute(PlugMethodAttributeName);

				if (methodAttribute != null)
				{
					PlugMethodList.Add(method);
				}
			}
		}
	}

	public override MosaMethod GetReplacement(MosaMethod targetMethod)
	{
		lock (_lock)
		{
			if (NoReplacement.Contains(targetMethod))
				return null;

			if (PlugMethods.ContainsKey(targetMethod))
			{
				return PlugMethods[targetMethod];
			}
		}

		// check if plugged
		var newMethod = CheckForPlug(targetMethod);

		if (newMethod != null)
		{
			CreatePlug(targetMethod, newMethod);
		}
		else
		{
			lock (_lock)
			{
				NoReplacement.Add(targetMethod);
			}
		}

		return newMethod;
	}

	public override void CreatePlug(MosaMethod targetMethod, MosaMethod newMethod)
	{
		lock (_lock)
		{
			if (!PlugMethods.ContainsKey(targetMethod))
			{
				PlugMethods.Add(targetMethod, newMethod);
			}
		}
	}

	private MosaMethod CheckForPlug(MosaMethod method)
	{
		var completeMethodName = method.DeclaringType.FullName + "::" + method.Name;

		foreach (var plugMethod in PlugMethodList)
		{
			var methodAttribute = plugMethod.FindCustomAttribute(PlugMethodAttributeName);
			var plugMethodTarget = (string)methodAttribute.Arguments[0].Value;

			if (completeMethodName == plugMethodTarget)
			{
				if (method.IsStatic)
				{
					if (Matches(method, plugMethod))
					{
						return plugMethod;
					}
				}
				else if (MatchesWithStaticThis(method, plugMethod))
				{
					return plugMethod;
				}
			}
		}

		return null;
	}

	private static bool Matches(MosaMethod targetMethod, MosaMethod plugMethod)
	{
		if (targetMethod.Signature.Parameters.Count != plugMethod.Signature.Parameters.Count)
			return false;

		for (var i = 0; i < targetMethod.Signature.Parameters.Count; i++)
		{
			if (!targetMethod.Signature.Parameters[i].Equals(plugMethod.Signature.Parameters[i]))
			{
				return false;
			}
		}

		return true;
	}

	private static bool MatchesWithStaticThis(MosaMethod targetMethod, MosaMethod plugMethod)
	{
		if (targetMethod.Signature.Parameters.Count != plugMethod.Signature.Parameters.Count - 1)
			return false;

		for (var i = 0; i < targetMethod.Signature.Parameters.Count; i++)
		{
			if (!targetMethod.Signature.Parameters[i].Equals(plugMethod.Signature.Parameters[i + 1]))
			{
				return false;
			}
		}

		return true;
	}
}
