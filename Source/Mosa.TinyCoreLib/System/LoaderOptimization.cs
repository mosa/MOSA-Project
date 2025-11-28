namespace System;

public enum LoaderOptimization
{
	NotSpecified = 0,
	SingleDomain = 1,
	MultiDomain = 2,
	[Obsolete("LoaderOptimization.DomainMask has been deprecated and is not supported.")]
	DomainMask = 3,
	MultiDomainHost = 3,
	[Obsolete("LoaderOptimization.DisallowBindings has been deprecated and is not supported.")]
	DisallowBindings = 4
}
