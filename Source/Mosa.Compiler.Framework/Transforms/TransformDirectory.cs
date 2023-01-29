// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transforms;

public static class TransformDirectory
{
	public static List<BaseTransform> Transformations = new List<BaseTransform>();

	public static void Add(BaseTransform transform)
	{
		lock (Transformations)
		{
			Transformations.Add(transform);
		}
	}

	public enum MatchType { Contains, StartsWith, EndsWith };

	public static List<BaseTransform> GetTransforms(string match, MatchType type)
	{
		var matches = new List<BaseTransform>();

		lock (Transformations)
		{
			foreach (var transform in Transformations)
			{
				if ((type == MatchType.StartsWith && transform.Name.StartsWith(match))
				    || (type == MatchType.Contains && transform.Name.Contains(match))
				    || (type == MatchType.EndsWith && transform.Name.EndsWith(match)))
				{
					matches.Add(transform);
				}
			}
		}

		return matches;
	}
}
