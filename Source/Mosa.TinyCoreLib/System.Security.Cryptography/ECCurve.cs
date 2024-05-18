namespace System.Security.Cryptography;

public struct ECCurve
{
	public enum ECCurveType
	{
		Implicit,
		PrimeShortWeierstrass,
		PrimeTwistedEdwards,
		PrimeMontgomery,
		Characteristic2,
		Named
	}

	public static class NamedCurves
	{
		public static ECCurve brainpoolP160r1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP160t1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP192r1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP192t1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP224r1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP224t1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP256r1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP256t1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP320r1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP320t1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP384r1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP384t1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP512r1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve brainpoolP512t1
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve nistP256
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve nistP384
		{
			get
			{
				throw null;
			}
		}

		public static ECCurve nistP521
		{
			get
			{
				throw null;
			}
		}
	}

	private object _dummy;

	private int _dummyPrimitive;

	public byte[]? A;

	public byte[]? B;

	public byte[]? Cofactor;

	public ECCurveType CurveType;

	public ECPoint G;

	public HashAlgorithmName? Hash;

	public byte[]? Order;

	public byte[]? Polynomial;

	public byte[]? Prime;

	public byte[]? Seed;

	public bool IsCharacteristic2
	{
		get
		{
			throw null;
		}
	}

	public bool IsExplicit
	{
		get
		{
			throw null;
		}
	}

	public bool IsNamed
	{
		get
		{
			throw null;
		}
	}

	public bool IsPrime
	{
		get
		{
			throw null;
		}
	}

	public Oid Oid
	{
		get
		{
			throw null;
		}
	}

	public static ECCurve CreateFromFriendlyName(string oidFriendlyName)
	{
		throw null;
	}

	public static ECCurve CreateFromOid(Oid curveOid)
	{
		throw null;
	}

	public static ECCurve CreateFromValue(string oidValue)
	{
		throw null;
	}

	public void Validate()
	{
	}
}
