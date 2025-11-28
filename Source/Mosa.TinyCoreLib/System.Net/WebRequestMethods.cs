namespace System.Net;

public static class WebRequestMethods
{
	public static class File
	{
		public const string DownloadFile = "GET";

		public const string UploadFile = "PUT";
	}

	public static class Ftp
	{
		public const string AppendFile = "APPE";

		public const string DeleteFile = "DELE";

		public const string DownloadFile = "RETR";

		public const string GetDateTimestamp = "MDTM";

		public const string GetFileSize = "SIZE";

		public const string ListDirectory = "NLST";

		public const string ListDirectoryDetails = "LIST";

		public const string MakeDirectory = "MKD";

		public const string PrintWorkingDirectory = "PWD";

		public const string RemoveDirectory = "RMD";

		public const string Rename = "RENAME";

		public const string UploadFile = "STOR";

		public const string UploadFileWithUniqueName = "STOU";
	}

	public static class Http
	{
		public const string Connect = "CONNECT";

		public const string Get = "GET";

		public const string Head = "HEAD";

		public const string MkCol = "MKCOL";

		public const string Post = "POST";

		public const string Put = "PUT";
	}
}
