// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem
{
	internal interface ITypeSystemController
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
