namespace LZWCompression
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string stringToEncode = File.ReadAllText(Path.Combine(@"../../../input/file.txt")).ToUpper();

			var encoder = new LZWEncoder(stringToEncode);
			encoder.SaveToFile(Path.Combine(@"../../../output/resultBinary"));
			encoder.SaveDictionaryToFile(Path.Combine(@"../../../output/dictionary.txt"));

			float newSize = encoder.Result.Count;
			float oldSize = stringToEncode.Length * 8;
			float oldSizeSimplest = stringToEncode.Length * MathF.Ceiling(MathF.Log2(encoder.StartingDictionary.Count));

			Console.WriteLine($"Result file length: {newSize} bits\n");

			Console.WriteLine($"k = {oldSize / newSize:N2} compared with UTF-8");
			Console.WriteLine($"k = {oldSizeSimplest / newSize:N2} compared with simplest encoding\n");
			Console.WriteLine($"S = {(newSize / oldSize):P2} compared with UTF-8");
			Console.WriteLine($"S = {(newSize / oldSizeSimplest):P2} compared with simplest encoding\n");
		}
	}
}