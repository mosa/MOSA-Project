using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Mono.UpdateProject
{
	class Options
	{
		public bool CreateMonoFile = false;
		public bool CreateMosaFile = false;
		public bool CreateOriginalFile = true;

		public bool UpdateOnChange = true;

		public bool UpdateProjectFiles = true;

		public string Destination = string.Empty;
		public string Source = string.Empty;

		public List<string> Files = new List<string>();
		public List<string> Projects = new List<string>();

		public Options(string[] args)
		{
			Process(args);
		}

		public void Process(string[] args)
		{
			foreach (string opt in args) {

				if (opt.ToLower().Equals("-mosa")) CreateMosaFile = false;
				else if (opt.ToLower().Equals("+mosa")) CreateMosaFile = true;

				if (opt.ToLower().Equals("-mono")) CreateMonoFile = false;
				else if (opt.ToLower().Equals("+mono")) CreateMonoFile = true;

				if (opt.ToLower().Equals("-original")) CreateOriginalFile = false;
				else if (opt.ToLower().Equals("+original")) CreateOriginalFile = true;

				if (opt.ToLower().Equals("-updateproject")) UpdateProjectFiles = false;
				else if (opt.ToLower().Equals("+updateproject")) UpdateProjectFiles = true;

				if (opt.ToLower().Equals("-update")) UpdateOnChange = false;
				else if (opt.ToLower().Equals("+update")) UpdateOnChange = true;

				else if (opt.ToLower().StartsWith("-source=")) Source = opt.Substring(8);
				else if (opt.ToLower().StartsWith("-destination=")) Destination = opt.Substring(13);

				else if (opt.ToLower().EndsWith(".csproj")) {
					Projects.Add(opt);
					foreach (string file in Transform.GetProjectFiles(opt))
						if (!file.EndsWith(".Internal.cs") && !file.EndsWith(".Mosa.cs"))
							Files.Add(file);
				}
				else if (opt.ToLower().EndsWith(".cs")) Files.Add(opt);

			}
		}
	}
}
