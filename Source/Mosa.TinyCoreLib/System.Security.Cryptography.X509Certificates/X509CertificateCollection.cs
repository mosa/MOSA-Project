using System.Collections;

namespace System.Security.Cryptography.X509Certificates;

public class X509CertificateCollection : CollectionBase
{
	public class X509CertificateEnumerator : IEnumerator
	{
		public X509Certificate Current
		{
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				throw null;
			}
		}

		public X509CertificateEnumerator(X509CertificateCollection mappings)
		{
		}

		public bool MoveNext()
		{
			throw null;
		}

		public void Reset()
		{
		}

		bool IEnumerator.MoveNext()
		{
			throw null;
		}

		void IEnumerator.Reset()
		{
		}
	}

	public X509Certificate this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public X509CertificateCollection()
	{
	}

	public X509CertificateCollection(X509CertificateCollection value)
	{
	}

	public X509CertificateCollection(X509Certificate[] value)
	{
	}

	public int Add(X509Certificate value)
	{
		throw null;
	}

	public void AddRange(X509CertificateCollection value)
	{
	}

	public void AddRange(X509Certificate[] value)
	{
	}

	public bool Contains(X509Certificate value)
	{
		throw null;
	}

	public void CopyTo(X509Certificate[] array, int index)
	{
	}

	public new X509CertificateEnumerator GetEnumerator()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public int IndexOf(X509Certificate value)
	{
		throw null;
	}

	public void Insert(int index, X509Certificate value)
	{
	}

	protected override void OnValidate(object value)
	{
	}

	public void Remove(X509Certificate value)
	{
	}
}
