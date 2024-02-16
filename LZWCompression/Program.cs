namespace LZWCompression
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Dictionary<string, int> wordsDictionary = new();
			string stringToEncode = File.ReadAllText(@"..\..\..\file.txt").ToUpper(); ;
			new LZW(stringToEncode).SaveToFile(@"..\..\..\resultBinary.txt");
        }
	}
}