namespace System;

[AttributeUsage(AttributeTargets.Method)]
public sealed class LoaderOptimizationAttribute : Attribute
{
	public LoaderOptimization Value
	{
		get
		{
			throw null;
		}
	}

	public LoaderOptimizationAttribute(byte value)
	{
	}

	public LoaderOptimizationAttribute(LoaderOptimization value)
	{
	}
}
