namespace System.ComponentModel.Design;

public enum ViewTechnology
{
	[Obsolete("ViewTechnology.Passthrough has been deprecated. Use ViewTechnology.Default instead.")]
	Passthrough,
	[Obsolete("ViewTechnology.WindowsForms has been deprecated. Use ViewTechnology.Default instead.")]
	WindowsForms,
	Default
}
