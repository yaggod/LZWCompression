namespace LZWCompression
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string stringToEncode = File.ReadAllText(@"..\..\..\input\file.txt").ToUpper();

			var encoder = new LZWEncoder(stringToEncode);
			encoder.SaveToFile(@"..\..\..\output\resultBinary");
			encoder.SaveDictionaryToFile(@"..\..\..\output\dictionary.txt");

            Console.WriteLine($"Result file length: {encoder.Result.Count} bits");
        }
	}
}