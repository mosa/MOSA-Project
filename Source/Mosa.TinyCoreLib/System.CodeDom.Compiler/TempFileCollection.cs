using System.Collections;

namespace System.CodeDom.Compiler;

public class TempFileCollection : ICollection, IEnumerable, IDisposable
{
	public string BasePath
	{
		get
		{
			throw null;
		}
	}

	public int Count
	{
		get
		{
			throw null;
		}
	}

	public bool KeepFiles
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	int ICollection.Count
	{
		get
		{
			throw null;
		}
	}

	bool ICollection.IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	object ICollection.SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public string TempDir
	{
		get
		{
			throw null;
		}
	}

	public TempFileCollection()
	{
	}

	public TempFileCollection(string tempDir)
	{
	}

	public TempFileCollection(string tempDir, bool keepFiles)
	{
	}

	public string AddExtension(string fileExtension)
	{
		throw null;
	}

	public string AddExtension(string fileExtension, bool keepFile)
	{
		throw null;
	}

	public void AddFile(string fileName, bool keepFile)
	{
	}

	public void CopyTo(string[] fileNames, int start)
	{
	}

	public void Delete()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~TempFileCollection()
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int start)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}

	void IDisposable.Dispose()
	{
	}
}
