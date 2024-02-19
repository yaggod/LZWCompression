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
			float oldSize = stringToEncode.Length * sizeof(char) * 8;

			Console.WriteLine($"Result file length: {newSize} bits");

			Console.WriteLine($"k = {oldSize / newSize:N2}");
			Console.WriteLine($"S = {(newSize /oldSize):P2}");
		}
	}
}