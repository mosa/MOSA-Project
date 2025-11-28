using System.Linq.Expressions;
using System.Reflection;

namespace System.ComponentModel.Composition.Registration;

public class PartBuilder
{
	internal PartBuilder()
	{
	}

	public PartBuilder AddMetadata(string name, Func<Type, object> itemFunc)
	{
		throw null;
	}

	public PartBuilder AddMetadata(string name, object value)
	{
		throw null;
	}

	public PartBuilder Export()
	{
		throw null;
	}

	public PartBuilder Export(Action<ExportBuilder> exportConfiguration)
	{
		throw null;
	}

	public PartBuilder ExportInterfaces()
	{
		throw null;
	}

	public PartBuilder ExportInterfaces(Predicate<Type> interfaceFilter)
	{
		throw null;
	}

	public PartBuilder ExportInterfaces(Predicate<Type> interfaceFilter, Action<Type, ExportBuilder> exportConfiguration)
	{
		throw null;
	}

	public PartBuilder ExportProperties(Predicate<PropertyInfo> propertyFilter)
	{
		throw null;
	}

	public PartBuilder ExportProperties(Predicate<PropertyInfo> propertyFilter, Action<PropertyInfo, ExportBuilder> exportConfiguration)
	{
		throw null;
	}

	public PartBuilder ExportProperties<T>(Predicate<PropertyInfo> propertyFilter)
	{
		throw null;
	}

	public PartBuilder ExportProperties<T>(Predicate<PropertyInfo> propertyFilter, Action<PropertyInfo, ExportBuilder> exportConfiguration)
	{
		throw null;
	}

	public PartBuilder Export<T>()
	{
		throw null;
	}

	public PartBuilder Export<T>(Action<ExportBuilder> exportConfiguration)
	{
		throw null;
	}

	public PartBuilder ImportProperties(Predicate<PropertyInfo> propertyFilter)
	{
		throw null;
	}

	public PartBuilder ImportProperties(Predicate<PropertyInfo> propertyFilter, Action<PropertyInfo, ImportBuilder> importConfiguration)
	{
		throw null;
	}

	public PartBuilder ImportProperties<T>(Predicate<PropertyInfo> propertyFilter)
	{
		throw null;
	}

	public PartBuilder ImportProperties<T>(Predicate<PropertyInfo> propertyFilter, Action<PropertyInfo, ImportBuilder> importConfiguration)
	{
		throw null;
	}

	public PartBuilder SelectConstructor(Func<ConstructorInfo[], ConstructorInfo> constructorFilter)
	{
		throw null;
	}

	public PartBuilder SelectConstructor(Func<ConstructorInfo[], ConstructorInfo> constructorFilter, Action<ParameterInfo, ImportBuilder> importConfiguration)
	{
		throw null;
	}

	public PartBuilder SetCreationPolicy(CreationPolicy creationPolicy)
	{
		throw null;
	}
}
public class PartBuilder<T> : PartBuilder
{
	internal PartBuilder()
	{
	}

	public PartBuilder<T> ExportProperty(Expression<Func<T, object>> propertyFilter)
	{
		throw null;
	}

	public PartBuilder<T> ExportProperty(Expression<Func<T, object>> propertyFilter, Action<ExportBuilder> exportConfiguration)
	{
		throw null;
	}

	public PartBuilder<T> ExportProperty<TContract>(Expression<Func<T, object>> propertyFilter)
	{
		throw null;
	}

	public PartBuilder<T> ExportProperty<TContract>(Expression<Func<T, object>> propertyFilter, Action<ExportBuilder> exportConfiguration)
	{
		throw null;
	}

	public PartBuilder<T> ImportProperty(Expression<Func<T, object>> propertyFilter)
	{
		throw null;
	}

	public PartBuilder<T> ImportProperty(Expression<Func<T, object>> propertyFilter, Action<ImportBuilder> importConfiguration)
	{
		throw null;
	}

	public PartBuilder<T> ImportProperty<TContract>(Expression<Func<T, object>> propertyFilter)
	{
		throw null;
	}

	public PartBuilder<T> ImportProperty<TContract>(Expression<Func<T, object>> propertyFilter, Action<ImportBuilder> importConfiguration)
	{
		throw null;
	}

	public PartBuilder<T> SelectConstructor(Expression<Func<ParameterImportBuilder, T>> constructorFilter)
	{
		throw null;
	}
}
