using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Aes : Sse2
{
	public new abstract class X64 : Sse2.X64
	{
		public new static bool IsSupported
		{
			get
			{
				throw null;
			}
		}

		internal X64()
		{
		}
	}

	public new static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	internal Aes()
	{
	}

	public static Vector128<byte> Decrypt(Vector128<byte> value, Vector128<byte> roundKey)
	{
		throw null;
	}

	public static Vector128<byte> DecryptLast(Vector128<byte> value, Vector128<byte> roundKey)
	{
		throw null;
	}

	public static Vector128<byte> Encrypt(Vector128<byte> value, Vector128<byte> roundKey)
	{
		throw null;
	}

	public static Vector128<byte> EncryptLast(Vector128<byte> value, Vector128<byte> roundKey)
	{
		throw null;
	}

	public static Vector128<byte> InverseMixColumns(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector128<byte> KeygenAssist(Vector128<byte> value, [ConstantExpected] byte control)
	{
		throw null;
	}
}
