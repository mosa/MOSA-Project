namespace System.Reflection.Emit;

public abstract class EventBuilder
{
	public void AddOtherMethod(MethodBuilder mdBuilder)
	{
	}

	protected abstract void AddOtherMethodCore(MethodBuilder mdBuilder);

	public void SetAddOnMethod(MethodBuilder mdBuilder)
	{
	}

	protected abstract void SetAddOnMethodCore(MethodBuilder mdBuilder);

	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
	}

	protected abstract void SetCustomAttributeCore(ConstructorInfo con, ReadOnlySpan<byte> binaryAttribute);

	public void SetRaiseMethod(MethodBuilder mdBuilder)
	{
	}

	protected abstract void SetRaiseMethodCore(MethodBuilder mdBuilder);

	public void SetRemoveOnMethod(MethodBuilder mdBuilder)
	{
	}

	protected abstract void SetRemoveOnMethodCore(MethodBuilder mdBuilder);
}
