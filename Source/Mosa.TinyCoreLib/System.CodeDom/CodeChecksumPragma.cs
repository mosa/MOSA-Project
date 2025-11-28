namespace System.CodeDom;

public class CodeChecksumPragma : CodeDirective
{
	public Guid ChecksumAlgorithmId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] ChecksumData
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string FileName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CodeChecksumPragma()
	{
	}

	public CodeChecksumPragma(string fileName, Guid checksumAlgorithmId, byte[] checksumData)
	{
	}
}
