// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.CompilerStages;

/// <summary>
/// UnitTest Stage - Do no inline the unit test methods
/// </remarks>
/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
public sealed class UnitTestStage : BaseCompilerStage
{
	private const string MosaUnitTestAttributeName = "Mosa.UnitTests.MosaUnitTestAttribute";

	#region Overrides

	protected override void Setup()
	{
		foreach (var type in TypeSystem.AllTypes)
		{
			foreach (var method in type.Methods)
			{
				var methodAttribute = method.FindCustomAttribute(MosaUnitTestAttributeName);

				if (methodAttribute != null)
				{
					var methodData = Compiler.GetMethodData(method);

					methodData.DoNotInline = true;
				}
			}
		}
	}

	#endregion Overrides
}
