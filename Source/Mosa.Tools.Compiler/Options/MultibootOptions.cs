/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Options;
using Mosa.Tools.Compiler.Stages;
using NDesk.Options;

namespace Mosa.Tools.Compiler.Options
{
	/// <summary>
	/// </summary>
	public class MultibootOptions : BaseCompilerWithEnableOptions
	{

		public uint? VideoMode { get; protected set; }
		public uint? VideoWidth { get; protected set; }
		public uint? VideoHeight { get; protected set; }
		public uint? VideoDepth { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MapFileGeneratorOptions"/> class.
		/// </summary>
		/// <param name="optionSet">The option set.</param>
		public MultibootOptions(OptionSet optionSet)
		{
			optionSet.Add(
				"multiboot-video-mode=",
				"Specify the video mode for multiboot [{text|graphics}].",
				delegate(string v)
				{
					switch (v.ToLower())
					{
						case "text":
							this.VideoMode = 1;
							break;
						case "graphics":
							this.VideoMode = 0;
							break;
						default:
							throw new OptionException("Invalid value for multiboot video mode: " + v, "multiboot-video-mode");
					}
				}
			);

			optionSet.Add(
				"multiboot-video-width=",
				"Specify the {width} for video output, in pixels for graphics mode or in characters for text mode.",
				delegate(string v)
				{
					uint val;
					if (uint.TryParse(v, out val))
					{
						// TODO: this probably needs further validation
						this.VideoWidth = val;
					}
					else
					{
						throw new OptionException("Invalid value for multiboot video width: " + v, "multiboot-video-width");
					}
				}
			);

			optionSet.Add(
				"multiboot-video-height=",
				"Specify the {height} for video output, in pixels for graphics mode or in characters for text mode.",
				delegate(string v)
				{
					uint val;
					if (uint.TryParse(v, out val))
					{
						// TODO: this probably needs further validation
						this.VideoHeight = val;
					}
					else
					{
						throw new OptionException("Invalid value for multiboot video height: " + v, "multiboot-video-height");
					}
				}
			);

			optionSet.Add(
				"multiboot-video-depth=",
				"Specify the {depth} (number of bits per pixel) for graphics mode.",
				delegate(string v)
				{
					uint val;
					if (uint.TryParse(v, out val))
					{
						this.VideoDepth = val;
					}
					else
					{
						throw new OptionException("Invalid value for multiboot video depth: " + v, "multiboot-video-depth");
					}
				}
			);

			optionSet.Add(
				"multiboot-module=",
				"Adds a {0:module} to multiboot, to be loaded at a given {1:virtualAddress} (can be used multiple times).",
				delegate(string file, string address)
				{
					// TODO: validate and add this to a list or something
					Console.WriteLine("Adding multiboot module " + file + " at virtualAddress " + address);
				}
			);
		}

		public void ApplyTo(Multiboot0695AssemblyStage multiboot)
		{
			if (this.VideoMode.HasValue)
				multiboot.VideoMode = this.VideoMode.Value;

			if (this.VideoWidth.HasValue)
				multiboot.VideoWidth = this.VideoWidth.Value;

			if (this.VideoHeight.HasValue)
				multiboot.VideoHeight = this.VideoHeight.Value;

			if (this.VideoDepth.HasValue)
				multiboot.VideoDepth = this.VideoDepth.Value;
		}

		public override void Apply(IPipelineStage stage)
		{
			Multiboot0695AssemblyStage multiboot = stage as Multiboot0695AssemblyStage;
			if (multiboot != null)
				ApplyTo(multiboot);
		}
	}
}
