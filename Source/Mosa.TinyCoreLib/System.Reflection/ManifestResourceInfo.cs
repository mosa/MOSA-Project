namespace System.Reflection;

public class ManifestResourceInfo
{
	public virtual string? FileName
	{
		get
		{
			throw null;
		}
	}

	public virtual Assembly? ReferencedAssembly
	{
		get
		{
			throw null;
		}
	}

	public virtual ResourceLocation ResourceLocation
	{
		get
		{
			throw null;
		}
	}

	public ManifestResourceInfo(Assembly? containingAssembly, string? containingFileName, ResourceLocation resourceLocation)
	{
	}
}
