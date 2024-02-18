namespace LZWCompression
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string stringToEncode = File.ReadAllText(Path.Combine(@"..\..\..\input\file.txt")).ToUpper();

			var encoder = new LZWEncoder(stringToEncode);
			encoder.SaveToFile(Path.Combine(@"..\..\..\output\resultBinary"));
			encoder.SaveDictionaryToFile(Path.Combine(@"..\..\..\output\dictionary.txt"));

            Console.WriteLine($"Result file length: {encoder.Result.Count} bits");
        }
	}
}