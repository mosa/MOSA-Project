// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// Inline Setup Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class InlinedSetupStage : BaseCompilerStage
	{
		#region Overrides

		protected override void Setup()
		{
			var excludeList = Compiler.CompilerSettings.InlineExcludeList;
			var aggressiveList = Compiler.CompilerSettings.InlineAggressiveList;

			if ((excludeList == null || excludeList.Count == 0) && (aggressiveList == null || aggressiveList.Count == 0))
				return;

			foreach (var type in TypeSystem.AllTypes)
			{
				foreach (var method in type.Methods)
				{
					bool excluded = false;

					var name = InlinedSetupStage.RemoveReturnValue(method.FullName);

					if (excludeList != null)
					{
						foreach (var exclude in excludeList)
						{
							var match = false;

							if (name == exclude)
							{
								match = true;
							}

							//else
							//{
							//	match = Regex.Match(name, exclude).Success;
							//}

							if (match)
							{
								var methodData = Compiler.GetMethodData(method);
								methodData.DoNotInline = true;

								excluded = true;
								break;
							}
						}

						if (excluded)
							continue;
					}

					if (aggressiveList != null)
					{
						foreach (var aggressive in aggressiveList)
						{
							var match = false;

							if (name == aggressive)
							{
								match = true;
							}

							//else
							//{
							//	match = Regex.Match(name, aggressive).Success;
							//}

							if (match)
							{
								var methodData = Compiler.GetMethodData(method);
								methodData.AggressiveInlineRequested = true;

								excluded = true;
								break;
							}
						}
					}
				}
			}
		}

		protected override void Finalization()
		{
		}

		public static string RemoveReturnValue(string methodName)
		{
			int pos = methodName.LastIndexOf(":");

			if (pos < 0)
				return string.Empty;

			return methodName.Substring(0, pos);
		}

		#endregion Overrides
	}
}
