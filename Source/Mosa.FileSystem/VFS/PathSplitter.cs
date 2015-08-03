// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.VFS
{
	internal class PathSplitter
	{
		protected string path;
		protected int[] seperators;
		protected int length;

		public PathSplitter(string path)
		{
			this.path = path;
			MarkSeperators();
		}

		protected void MarkSeperators()
		{
			if (path.Length == 0)
			{
				length = 0;
				seperators = new int[0];
				return;
			}

			int count = 0;
			for (int i = 0; i < path.Length; i++)
				if ((path[i] == System.IO.Path.DirectorySeparatorChar) || (path[i] == System.IO.Path.AltDirectorySeparatorChar))
					count++;

			seperators = new int[count];

			if (count == 0)
			{
				length = 1;
				return;
			}

			count = 0;
			for (int i = 0; i < path.Length; i++)
				if ((path[i] == System.IO.Path.DirectorySeparatorChar) || (path[i] == System.IO.Path.AltDirectorySeparatorChar))
					seperators[count++] = i;

			if (seperators[count - 1] == path.Length - 1)
				length = count;
			else
				length = count + 1;
		}

		public string GetPath(int index)
		{
			if ((index > length) || (length == 0))
				return string.Empty;

			//if ((length == 0) && (seperators.Length == 0))
			//    return path;

			int start = (index == 0) ? 0 : seperators[index - 1] + 1;
			int end = (index == seperators.Length) ? path.Length : seperators[index];

			if (start >= end)
				return string.Empty;

			return path.Substring(start, end - start);
		}

		public string this[int index]
		{
			get
			{
				if (index >= Length)
					return null;

				return GetPath(index);
			}
		}

		public int Length
		{
			get
			{
				return length;
			}
		}

		public string Last
		{
			get
			{
				return GetPath(Length - 1);
			}
		}

		public int FindFirst(string path)
		{
			for (int i = 0; i < length; i++)
			{
				if (GetPath(i) == path)
					return i;
			}

			return -1;
		}
	}
}