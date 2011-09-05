using System.IO;

namespace Mosa.Tools.Mono.UpdateProject
{
	static public class CompareFile
	{
		static public bool Compare(string file1, string file2)
		{
			if (file1.Equals(file2))
				return true;

			if (!File.Exists(file1) || !File.Exists(file2))
				return false;

			FileStream first = new FileStream(file1, FileMode.Open);
			FileStream second = new FileStream(file2, FileMode.Open);

			if (first.Length != second.Length)
			{
				first.Close();
				second.Close();
				return false;
			}

			int byte1;
			int byte2;

			do
			{
				byte1 = first.ReadByte();
				byte2 = second.ReadByte();
			}
			while ((byte1 == byte2) && (byte1 != -1));

			first.Close();
			second.Close();

			return (byte1 == byte2);

		}

	}
}
