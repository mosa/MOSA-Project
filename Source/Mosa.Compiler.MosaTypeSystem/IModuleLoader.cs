/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

namespace Mosa.Compiler.MosaTypeSystem
{
	public interface IModuleLoader
	{
		void LoadModuleFromFile(string file);
		IMetadata CreateMetadata();
	}
}
