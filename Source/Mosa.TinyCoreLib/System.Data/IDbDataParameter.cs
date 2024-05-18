namespace System.Data;

public interface IDbDataParameter : IDataParameter
{
	byte Precision { get; set; }

	byte Scale { get; set; }

	int Size { get; set; }
}
