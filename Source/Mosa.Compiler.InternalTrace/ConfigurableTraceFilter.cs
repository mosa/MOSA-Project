/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.InternalTrace
{
	public enum MatchType { Exact, Contains, StartsWith, Any, Except, NotContains, NotStartsWith, Exclude };

	public class ConfigurableTraceFilter : ITraceFilter
	{
		public bool IsLogging = false;
		public bool ExcludeInternalMethods = true;

		public string Type = string.Empty;
		public string Method = string.Empty;
		public string Stage = string.Empty;

		public MatchType TypeMatch = MatchType.Contains;
		public MatchType MethodMatch = MatchType.Contains;
		public MatchType StageMatch = MatchType.Any;

		bool ITraceFilter.IsLogging { get { return IsLogging; } }

		bool ITraceFilter.IsMatch(RuntimeMethod method, string stage)
		{
			return IsMatch(method.DeclaringType.Name, method.Name, stage);
		}

		protected bool IsMatch(string type, string method, string stage)
		{
			if (!IsLogging)
				return false;

			if (ExcludeInternalMethods && method.Contains("<$>"))
				return false;

			if (!Compare(TypeMatch,Type,type))
				return false;

			if (!Compare(MethodMatch, Method, method))
				return false;
	
			if (!Compare(StageMatch, Stage, stage))
				return false;
			    
			return true;
		}

		protected bool Compare(MatchType matchType, string matchString, string name)
		{
			switch (matchType)
			{
				case MatchType.Any: return true;
				case MatchType.Contains: return name.Contains(matchString);
				case MatchType.StartsWith: return name.Contains(matchString);
				case MatchType.Exact: return String.Compare(name, matchString) == 0;
				case MatchType.Except: return String.Compare(name, matchString) != 0;
				case MatchType.NotContains: return !name.Contains(matchString);
				case MatchType.NotStartsWith: return !name.Contains(matchString);

				case MatchType.Exclude: return !matchString.Contains(name);

				default: return false;
			}
		}
	}
}
