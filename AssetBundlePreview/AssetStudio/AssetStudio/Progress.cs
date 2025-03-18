using System;

namespace AssetStudio;

public static class Progress
{
	public static IProgress<int> Default = new Progress<int>();

	private static int preValue;

	public static void Reset()
	{
		preValue = 0;
		Default.Report(0);
	}

	public static void Report(int current, int total)
	{
		Report((int)((float)current * 100f / (float)total));
	}

	private static void Report(int value)
	{
		if (value > preValue)
		{
			preValue = value;
			Default.Report(value);
		}
	}
}
