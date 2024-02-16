#define USE_READABLE_BUFFERS

using System.Collections;

namespace LZWCompression
{
	public class LZW
	{
		public string WordToEncode
		{
			get;
		}

		private BitArray Result
		{
			get;
		} = new(0);

#if USE_READABLE_BUFFERS
		private List<string> _encodedWords = new();
		private List<int> _encodedCodes = new();
#endif

		private Dictionary<string, int> _wordsDictionary = new();
		private int _currentWordLength;


		public LZW(string wordToEncode)
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
					AddWordInDictionary(c.ToString());

			}
		}

		private void Encode()
		{
			string currentWord = "";
			for (int i = 0; i < WordToEncode.Length; i++)
			{
				string newWord = currentWord + WordToEncode[i];
				if (!_wordsDictionary.ContainsKey(newWord))
				{
					AddWordInDictionary(newWord);
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

#if USE_READABLE_BUFFERS
			_encodedCodes.Add(code);
			_encodedWords.Add(word);
#endif
		}

		private void AddWordInDictionary(string word)
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
			File.WriteAllBytes(filePath, bytesToWrite);
#if USE_READABLE_BUFFERS
			File.WriteAllText(filePath + "words.txt", String.Join("\n", _encodedWords));
			File.WriteAllText(filePath + "codes.txt", String.Join(" ", _encodedCodes));
#endif
		}

	}
}
