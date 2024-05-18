namespace System.Diagnostics;

public delegate ActivitySamplingResult SampleActivity<T>(ref ActivityCreationOptions<T> options);
