/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using Mosa.Compiler.Verifier;

namespace Mosa.Compiler.Verifier.TableStage
{
	public class MetadataTables : BaseVerificationStage
	{

		protected override void Run()
		{
			List<BaseVerificationStage> stages = new List<BaseVerificationStage>() { 
				new Assembly()
			};

			foreach (BaseVerificationStage stage in stages)
			{
				stage.Run(verify);

				if (verify.HasErrors)
					return;
			}

		}

	}
}

