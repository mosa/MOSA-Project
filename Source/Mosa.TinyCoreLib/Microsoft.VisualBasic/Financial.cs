using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic;

[StandardModule]
public sealed class Financial
{
	internal Financial()
	{
	}

	public static double DDB(double Cost, double Salvage, double Life, double Period, double Factor = 2.0)
	{
		throw null;
	}

	public static double FV(double Rate, double NPer, double Pmt, double PV = 0.0, DueDate Due = DueDate.EndOfPeriod)
	{
		throw null;
	}

	public static double IPmt(double Rate, double Per, double NPer, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
	{
		throw null;
	}

	public static double IRR(ref double[] ValueArray, double Guess = 0.1)
	{
		throw null;
	}

	public static double MIRR(ref double[] ValueArray, double FinanceRate, double ReinvestRate)
	{
		throw null;
	}

	public static double NPer(double Rate, double Pmt, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
	{
		throw null;
	}

	public static double NPV(double Rate, ref double[] ValueArray)
	{
		throw null;
	}

	public static double Pmt(double Rate, double NPer, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
	{
		throw null;
	}

	public static double PPmt(double Rate, double Per, double NPer, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
	{
		throw null;
	}

	public static double PV(double Rate, double NPer, double Pmt, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod)
	{
		throw null;
	}

	public static double Rate(double NPer, double Pmt, double PV, double FV = 0.0, DueDate Due = DueDate.EndOfPeriod, double Guess = 0.1)
	{
		throw null;
	}

	public static double SLN(double Cost, double Salvage, double Life)
	{
		throw null;
	}

	public static double SYD(double Cost, double Salvage, double Life, double Period)
	{
		throw null;
	}
}
