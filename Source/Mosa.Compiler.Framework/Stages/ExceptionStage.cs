﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.Exception;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Exception Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.BaseTransformStage" />
public class ExceptionStage : BaseTransformStage
{
	private readonly ExceptionManager ExceptionManager = new();

	public ExceptionStage()
		: base()
	{
		AddTranforms(ExceptionTransforms.List);
	}

	protected override void Initialize()
	{
		ExceptionManager.Initialize(Compiler);

		base.Initialize();
	}

	protected override void Setup()
	{
		ExceptionManager.Setup(Transform.MethodCompiler);

		Transform.AddManager(ExceptionManager);
	}
}
