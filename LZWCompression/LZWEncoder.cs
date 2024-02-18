#define CREATE_READABLE_FILES

using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace LZWCompression
{
	public class LZWEncoder
	{
		public string WordToEncode
		{
			get;
		}

		public BitArray Result
		{
			get;
		} = new(0);

		public ReadOnlyDictionary<string, int> StartingDictionary
		{
			get;
			private set;
		}

#if CREATE_READABLE_FILES
		private List<string> _encodedWords = new();
		private List<int> _encodedCodes = new();
#endif

		private Dictionary<string, int> _wordsDictionary = new();
		private int _currentWordLength;


		public LZWEncoder(string wordToEncode)
		{
			WordToEncode = wordToEncode;
			InitializeDictionary();
			Encode();
		}


		private void InitializeDictionary()
		{
			foreach (char c in WordToEncode)
			{
				if (!_wordsDictionary.ContainsKey(c.ToString()))
					AddWordToDictionary(c.ToString());
			}
			StartingDictionary = new( new Dictionary<string, int>(_wordsDictionary));
		}

		private void Encode()
		{
			string currentWord = "";
			for (int i = 0; i < WordToEncode.Length; i++)
			{
				string newWord = currentWord + WordToEncode[i];
				if (!_wordsDictionary.ContainsKey(newWord))
				{
					AddWordToDictionary(newWord);
					AddWordToResult(currentWord);
					currentWord = WordToEncode[i].ToString();
				}
				else
				{
					currentWord = newWord;
				}
			}

			AddWordToResult(currentWord);
		}

		private void AddWordToResult(string word)
		{
			int code = _wordsDictionary[word];
			BitArray bits = new BitArray(new int[] { code });
			Result.Length += _currentWordLength;
			for (int offset = 1; offset <= _currentWordLength; offset++)
			{
				Result[Result.Length - offset] = bits[offset - 1];
			}

#if CREATE_READABLE_FILES
			_encodedCodes.Add(code);
			_encodedWords.Add(word);
#endif
		}

		private void AddWordToDictionary(string word)
		{
			_wordsDictionary.Add(word, _wordsDictionary.Count);
			if (IsPowerOfTwo(_wordsDictionary.Count))
				_currentWordLength++;
		}

		private static bool IsPowerOfTwo(int x)
		{
			return (x & (x - 1)) == 0;
		}

		public void SaveToFile(string filePath)
		{
			int bytesCount = (int) MathF.Ceiling(Result.Count / 8f);
			byte[] bytesToWrite = new byte[bytesCount];
			Result.CopyTo(bytesToWrite, 0);

			File.WriteAllBytes(filePath, bytesToWrite);
#if CREATE_READABLE_FILES
			File.WriteAllText(filePath + "_words.txt", String.Join("\n", _encodedWords));
			File.WriteAllText(filePath + "_codes.txt", String.Join(" ", _encodedCodes));
#endif
		}

		internal void SaveDictionaryToFile(string filePath)
		{
			File.WriteAllLines(filePath, StartingDictionary.Select(pair => $"{pair.Key}:\t {pair.Value}"));
		}
	}
}
