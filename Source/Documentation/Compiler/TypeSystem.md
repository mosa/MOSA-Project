MOSA Type System
================

MOSA Type System is the type system used in MOSA (obviously). It is responsible for converting raw metadata models read from dnlib (the metadata reading library used in MOSA) to custom models used by compiler.

Structure
---------
The root of type system is `TypeSystem`. From there, the built-in primitive types are exposed via `BuiltIn` property, and modules can be access through `Modules` property.

The most important class in the type system is `MosaUnit`, it represents a member of the type system. From `MosaUnit`, `MosaModule`, `MosaType`, `MosaMethod`, `MosaField` are derived.

The type system is populated through `ITypeSystemController` interface, which is passed to `IMetadata` when the type system loads metadata. The creation and mutation of `MosaUnit` instances are controlled by it. That is, instances of `MosaUnit` are immutable after the population of type system.


Workflow
------------
1. Caller uses an instance of `IModuleLoader` to load the modules
2. Caller invokes `IModuleLoader.GetMetadata()` to obtain an instance of `IMetadata```
3. Caller invokes `TypeSystem.Load(IMetadata)` to initialize the type system with the specified metadata.

By default, `MosaModuleLoader` will uses `CLRMetadata` as the metadata loader.

1. `CLRMetadata` uses `MetadataLoader.Load(ModuleDef)` to load ModuleDef, TypeDef, MethodDef, and FieldDef into respective `MosaUnit` derived instances. At this stage, only basic properties are loaded (e.g. Names, Namespaces, DeclaringType, Attributes), NO references are resolved.
2. `CLRMetadata` then invoke `MetadataResolver.Resolve()` to resolve metadata references. At this stage, type, method, and field references are resolved. 
3. For each type references, `MetadataResolver` uses `MetadataLoader.GetType(TypeSig)` to obtain/create the instance of `MosaType` for TypeDef/Ref/Spec. At this stage, generic methods/types are also resolved into individual instantiations.