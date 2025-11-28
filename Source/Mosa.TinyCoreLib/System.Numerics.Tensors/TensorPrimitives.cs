namespace System.Numerics.Tensors;

public static class TensorPrimitives
{
	public static void ConvertToHalf(ReadOnlySpan<float> source, Span<Half> destination)
	{
		throw null;
	}

	public static void ConvertToSingle(ReadOnlySpan<Half> source, Span<float> destination)
	{
		throw null;
	}

	public static void Abs(ReadOnlySpan<float> x, Span<float> destination)
	{
	}

	public static void Add(ReadOnlySpan<float> x, ReadOnlySpan<float> y, Span<float> destination)
	{
	}

	public static void Add(ReadOnlySpan<float> x, float y, Span<float> destination)
	{
	}

	public static void AddMultiply(ReadOnlySpan<float> x, ReadOnlySpan<float> y, ReadOnlySpan<float> multiplier, Span<float> destination)
	{
	}

	public static void AddMultiply(ReadOnlySpan<float> x, ReadOnlySpan<float> y, float multiplier, Span<float> destination)
	{
	}

	public static void AddMultiply(ReadOnlySpan<float> x, float y, ReadOnlySpan<float> multiplier, Span<float> destination)
	{
	}

	public static void Cosh(ReadOnlySpan<float> x, Span<float> destination)
	{
	}

	public static float CosineSimilarity(ReadOnlySpan<float> x, ReadOnlySpan<float> y)
	{
		throw null;
	}

	public static float Distance(ReadOnlySpan<float> x, ReadOnlySpan<float> y)
	{
		throw null;
	}

	public static void Divide(ReadOnlySpan<float> x, ReadOnlySpan<float> y, Span<float> destination)
	{
	}

	public static void Divide(ReadOnlySpan<float> x, float y, Span<float> destination)
	{
	}

	public static float Dot(ReadOnlySpan<float> x, ReadOnlySpan<float> y)
	{
		throw null;
	}

	public static void Exp(ReadOnlySpan<float> x, Span<float> destination)
	{
	}

	public static int IndexOfMax(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static int IndexOfMaxMagnitude(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static int IndexOfMin(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static int IndexOfMinMagnitude(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static float Norm(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static void Log(ReadOnlySpan<float> x, Span<float> destination)
	{
	}

	public static void Log2(ReadOnlySpan<float> x, Span<float> destination)
	{
	}

	public static float Max(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static void Max(ReadOnlySpan<float> x, ReadOnlySpan<float> y, Span<float> destination)
	{
		throw null;
	}

	public static float MaxMagnitude(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static void MaxMagnitude(ReadOnlySpan<float> x, ReadOnlySpan<float> y, Span<float> destination)
	{
		throw null;
	}

	public static float Min(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static void Min(ReadOnlySpan<float> x, ReadOnlySpan<float> y, Span<float> destination)
	{
		throw null;
	}

	public static float MinMagnitude(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static void MinMagnitude(ReadOnlySpan<float> x, ReadOnlySpan<float> y, Span<float> destination)
	{
		throw null;
	}

	public static void Multiply(ReadOnlySpan<float> x, ReadOnlySpan<float> y, Span<float> destination)
	{
	}

	public static void Multiply(ReadOnlySpan<float> x, float y, Span<float> destination)
	{
	}

	public static void MultiplyAdd(ReadOnlySpan<float> x, ReadOnlySpan<float> y, ReadOnlySpan<float> addend, Span<float> destination)
	{
	}

	public static void MultiplyAdd(ReadOnlySpan<float> x, ReadOnlySpan<float> y, float addend, Span<float> destination)
	{
	}

	public static void MultiplyAdd(ReadOnlySpan<float> x, float y, ReadOnlySpan<float> addend, Span<float> destination)
	{
	}

	public static void Negate(ReadOnlySpan<float> x, Span<float> destination)
	{
	}

	public static float Product(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static float ProductOfDifferences(ReadOnlySpan<float> x, ReadOnlySpan<float> y)
	{
		throw null;
	}

	public static float ProductOfSums(ReadOnlySpan<float> x, ReadOnlySpan<float> y)
	{
		throw null;
	}

	public static void Sigmoid(ReadOnlySpan<float> x, Span<float> destination)
	{
	}

	public static void Sinh(ReadOnlySpan<float> x, Span<float> destination)
	{
	}

	public static void SoftMax(ReadOnlySpan<float> x, Span<float> destination)
	{
	}

	public static void Subtract(ReadOnlySpan<float> x, ReadOnlySpan<float> y, Span<float> destination)
	{
	}

	public static void Subtract(ReadOnlySpan<float> x, float y, Span<float> destination)
	{
	}

	public static float Sum(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static float SumOfMagnitudes(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static float SumOfSquares(ReadOnlySpan<float> x)
	{
		throw null;
	}

	public static void Tanh(ReadOnlySpan<float> x, Span<float> destination)
	{
	}
}
