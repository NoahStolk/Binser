namespace Binser.Extensions;

public static class BinaryWriterExtensions
{
	public static void Write(this BinaryWriter bw, DateTime dateTime)
	{
		bw.Write(dateTime.Ticks);
	}
}
