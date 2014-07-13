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
	public interface ITypeSystemController
	{
		MosaModule CreateModule();

		MosaType CreateType(MosaType source = null);

		MosaMethod CreateMethod(MosaMethod source = null);

		MosaField CreateField(MosaField source = null);

		MosaProperty CreateProperty(MosaProperty source = null);

		MosaParameter CreateParameter(MosaParameter source = null);

		MosaModule.Mutator MutateModule(MosaModule module);

		MosaType.Mutator MutateType(MosaType type);

		MosaMethod.Mutator MutateMethod(MosaMethod method);

		MosaField.Mutator MutateField(MosaField field);

		MosaProperty.Mutator MutateProperty(MosaProperty property);

		MosaParameter.Mutator MutateParameter(MosaParameter parameter);

		void AddModule(MosaModule module);

		void AddType(MosaType type);

		void SetCorLib(MosaModule module);

		void SetEntryPoint(MosaMethod entryPoint);
	}
}