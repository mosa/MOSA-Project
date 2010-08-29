
namespace Mosa.Runtime.Metadata.Signatures
{
	using System;

	public interface ISignatureContext
	{
		SigType GetGenericMethodArgument(int index);

		SigType GetGenericTypeArgument(int index);
	}
}
