internal class Program
{
	private static readonly Dictionary<char, int> _handBuffer = new();
	private static int _jValue = 11;

	private static void Main(string[] args)
	{
		Part1();
		_jValue = 1;
		Part2();
	}

	private static void Part1()
	{
		int sum = 0;

		string[] lines = File.ReadAllLines("Input1.txt");
		SortedList<Hand, Hand> sortedHands = new();
		foreach (string line in lines)
		{
			string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			Hand hand = GetPart1Hand(parts[0], int.Parse(parts[1]));
			sortedHands.Add(hand, hand);
		}

		for (int i = 0; i < sortedHands.Count; i++)
		{
			sum += (i + 1) * sortedHands.GetValueAtIndex(i).Bid;
		}

		Console.WriteLine($"Part 1: {sum}");
	}

	private static void Part2()
	{
		int sum = 0;

		string[] lines = File.ReadAllLines("Input1.txt");
		SortedList<Hand, Hand> sortedHands = new();
		foreach (string line in lines)
		{
			string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			Hand hand = GetPart2Hand(parts[0], int.Parse(parts[1]));
			sortedHands.Add(hand, hand);
		}

		for (int i = 0; i < sortedHands.Count; i++)
		{
			sum += (i + 1) * sortedHands.GetValueAtIndex(i).Bid;
		}

		Console.WriteLine($"Part 2: {sum}");
	}

	private static int GetCardValue(char c)
	{
		if (c >= '2' && c <= '9')
		{
			return c - '0';
		}

		return c switch
		{
			'T' => 10,
			'J' => _jValue,
			'Q' => 12,
			'K' => 13,
			'A' => 14
		};
	}

	private static Hand GetPart1Hand(string hand, int bid)
	{
		_handBuffer.Clear();
		HandType handType = HandType.High;
		foreach (char c in hand)
		{
			_handBuffer.TryAdd(c, 0);
			_handBuffer[c]++;

			if (_handBuffer[c] == 2)
			{
				if (handType == HandType.High)
				{
					handType = HandType.OnePair;
				}
				else if (handType == HandType.OnePair)
				{
					handType = HandType.TwoPair;
				}
				else if (handType == HandType.Three)
				{
					handType = HandType.Full;
				}
			}
			else if (_handBuffer[c] == 3)
			{
				if (handType == HandType.OnePair)
				{
					handType = HandType.Three;
				}
				else if (handType == HandType.TwoPair)
				{
					handType = HandType.Full;
				}
			}
			else if (_handBuffer[c] == 4)
			{
				handType = HandType.Four;
			}
			else if (_handBuffer[c] == 5)
			{
				handType = HandType.Five;
			}
		}

		return new Hand(handType, bid, hand);
	}

	private static Hand GetPart2Hand(string hand, int bid)
	{
		_handBuffer.Clear();
		HandType handType = HandType.High;
		int jCount = 0;
		foreach (char c in hand)
		{
			_handBuffer.TryAdd(c, 0);

			if (c == 'J')
			{
				jCount++;
				continue;
			}

			_handBuffer[c]++;

			if (_handBuffer[c] == 2)
			{
				if (handType == HandType.High)
				{
					handType = HandType.OnePair;
				}
				else if (handType == HandType.OnePair)
				{
					handType = HandType.TwoPair;
				}
				else if (handType == HandType.Three)
				{
					handType = HandType.Full;
				}
			}
			else if (_handBuffer[c] == 3)
			{
				if (handType == HandType.OnePair)
				{
					handType = HandType.Three;
				}
				else if (handType == HandType.TwoPair)
				{
					handType = HandType.Full;
				}
			}
			else if (_handBuffer[c] == 4)
			{
				handType = HandType.Four;
			}
			else if (_handBuffer[c] == 5)
			{
				handType = HandType.Five;
			}
		}

		if (handType == HandType.High && jCount > 0)
		{
			handType = jCount switch
			{
				1 => HandType.OnePair,
				2 => HandType.Three,
				3 => HandType.Four,
				4 or 5 => HandType.Five
			};
		}
		else if (handType == HandType.OnePair && jCount > 0)
		{
			handType = jCount switch
			{
				1 => HandType.Three,
				2 => HandType.Four,
				3 => HandType.Five
			};
		}
		else if (handType == HandType.TwoPair && jCount == 1)
		{
			handType = HandType.Full;
		}
		else if (handType == HandType.Three && jCount > 0)
		{
			handType = jCount switch
			{
				1 => HandType.Four,
				2 => HandType.Five
			};
		}
		else if (handType == HandType.Four && jCount == 1)
		{
			handType = HandType.Five;
		}

		return new Hand(handType, bid, hand);
	}

	private record Hand(HandType HandType, int Bid, string HandString) : IComparable<Hand>
	{
		public int CompareTo(Hand? other)
		{
			if (other is null)
			{
				return 1;
			}

			if (HandType != other.HandType)
			{
				return HandType - other.HandType;
			}

			for (int i = 0; i < HandString.Length; i++)
			{
				if (HandString[i] != other.HandString[i])
				{
					return GetCardValue(HandString[i]) - GetCardValue(other.HandString[i]);
				}
			}

			return 0;
		}
	}

	private enum HandType
	{
		High,
		OnePair,
		TwoPair,
		Three,
		Full,
		Four,
		Five
	}
}