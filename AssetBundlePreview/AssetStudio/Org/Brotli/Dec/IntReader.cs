namespace Org.Brotli.Dec;

internal sealed class IntReader
{
	private byte[] byteBuffer;

	private int[] intBuffer;

	internal static void Init(IntReader ir, byte[] byteBuffer, int[] intBuffer)
	{
		ir.byteBuffer = byteBuffer;
		ir.intBuffer = intBuffer;
	}

	internal static void Convert(IntReader ir, int intLen)
	{
		for (int i = 0; i < intLen; i++)
		{
			ir.intBuffer[i] = (ir.byteBuffer[i * 4] & 0xFF) | ((ir.byteBuffer[i * 4 + 1] & 0xFF) << 8) | ((ir.byteBuffer[i * 4 + 2] & 0xFF) << 16) | ((ir.byteBuffer[i * 4 + 3] & 0xFF) << 24);
		}
	}
}
