namespace LZWCompression
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Dictionary<string, int> wordsDictionary = new();
			string stringToEncode = File.ReadAllText(@"..\..\..\file.txt").ToUpper();

			var encoder = new LZWEncoder(stringToEncode);
			encoder.SaveToFile(@"..\..\..\resultBinary");

            Console.WriteLine($"Result file length: {encoder.Result.Count} bits");
        }
	}
}