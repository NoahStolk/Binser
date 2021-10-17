using Binser;
using Binser.Data;

byte[] serialized = ScoreSerializer.Serialize(new Score(128.2, 12, 3, 0x0A));
Console.WriteLine(string.Join('-', serialized));
