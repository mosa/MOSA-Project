// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transformation
{
	public static class TransformationDirectory
	{
		public static List<BaseTransformation> Transformations = new List<BaseTransformation>();

		public static void Add(BaseTransformation transformation)
		{
			lock (Transformations)
			{
				Transformations.Add(transformation);
			}
		}

		public enum MatchType { Contains, StartsWith, EndsWith };

		public static List<BaseTransformation> GetTransformations(string match, MatchType type)
		{
			var matches = new List<BaseTransformation>();

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
}
