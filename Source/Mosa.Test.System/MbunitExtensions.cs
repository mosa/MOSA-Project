using System;
using Gallio.Common.Reflection;
using Gallio.Framework.Data;
using Gallio.Framework.Pattern;
using MbUnit.Framework;
using Mosa.Test.System.Numbers;

namespace Mosa.Test.System
{
	#region Series

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class BAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.B, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class CAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.C, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I1Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I1, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I2Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I2, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I4Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I4, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U1Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U1, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U2Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U2, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U4Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U4, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8, GetMetadata(), false));
		}
	}

	#endregion

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I4SmallAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I4Small, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4NotNaNAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4NotNaN, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8NotNaNAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8NotNaN, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4NumberAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4Number, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8NumberAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8Number, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4NumberNotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4NumberNotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8NumberNotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8NumberNotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4FitsI4Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4FitsI4, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8FitsI4Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8FitsI4, GetMetadata(), false));
		}
	}

	#region Series Above Zero

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class CAboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.CAboveZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I1AboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I1AboveZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I2AboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I2AboveZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I4AboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I4AboveZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I8AboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I8AboveZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U1AboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U1AboveZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U2AboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U2AboveZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U4AboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U4AboveZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U8AboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U8AboveZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4AboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4AboveZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8AboveZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8AboveZero, GetMetadata(), false));
		}
	}

	#endregion

	#region Series Below Zero

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class CBelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.CBelowZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I1BelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I1BelowZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I2BelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I2BelowZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I4BelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I4BelowZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I8BelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I8BelowZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U1BelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U1BelowZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U2BelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U2BelowZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U4BelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U4BelowZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U8BelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U8BelowZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4BelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4BelowZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8BelowZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8BelowZero, GetMetadata(), false));
		}
	}

	#endregion

	#region Series Not Zero

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class CNotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.CNotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I1NotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I1NotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I2NotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I2NotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I4NotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I4NotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I8NotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I8NotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U1NotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U1NotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U2NotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U2NotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U4NotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U4NotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U8NotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U8NotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4NotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4NotZero, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8NotZeroAttribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8NotZero, GetMetadata(), false));
		}
	}

	#endregion

	#region Series Up To 8

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class CUpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.CUpTo8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I1UpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I1UpTo8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I2UpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I2UpTo8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I4UpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I4UpTo8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I8UpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I8UpTo8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U1UpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U1UpTo8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U2UpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U2UpTo8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U4UpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U4UpTo8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U8UpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U8UpTo8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4UpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4UpTo8, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8UpTo8Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8UpTo8, GetMetadata(), false));
		}
	}

	#endregion

	#region Series Up To 16

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class CUpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.CUpTo16, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I1UpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I1UpTo16, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I2UpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I2UpTo16, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I4UpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I4UpTo16, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I16UpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I1UpTo16, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U1UpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U1UpTo16, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U2UpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U2UpTo16, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U4UpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U4UpTo16, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U8UpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U8UpTo16, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4UpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4UpTo16, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8UpTo16Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8UpTo16, GetMetadata(), false));
		}
	}

	#endregion

	#region Series Up To 32

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class CUpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.CUpTo32, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I1UpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I1UpTo32, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I2UpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I2UpTo32, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I4UpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I4UpTo32, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class I8UpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.I8UpTo32, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U1UpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U1UpTo32, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U2UpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U2UpTo32, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U4UpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U4UpTo32, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class U8UpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.U8UpTo32, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R4UpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R4UpTo32, GetMetadata(), false));
		}
	}

	[AttributeUsage(PatternAttributeTargets.DataContext, AllowMultiple = false, Inherited = true)]
	public class R8UpTo32Attribute : DataAttribute
	{
		protected override void PopulateDataSource(IPatternScope scope, DataSource dataSource, ICodeElementInfo codeElement)
		{
			dataSource.AddDataSet(new ValueSequenceDataSet(Series.R8UpTo32, GetMetadata(), false));
		}
	}

	#endregion

}
