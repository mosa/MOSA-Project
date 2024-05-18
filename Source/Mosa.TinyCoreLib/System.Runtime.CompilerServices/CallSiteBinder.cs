using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace System.Runtime.CompilerServices;

public abstract class CallSiteBinder
{
	public static LabelTarget UpdateLabel
	{
		get
		{
			throw null;
		}
	}

	public abstract Expression Bind(object[] args, ReadOnlyCollection<ParameterExpression> parameters, LabelTarget returnLabel);

	public virtual T? BindDelegate<T>(CallSite<T> site, object[] args) where T : class
	{
		throw null;
	}

	protected void CacheTarget<T>(T target) where T : class
	{
	}
}
