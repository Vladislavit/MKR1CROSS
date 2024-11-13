using System;
using System.IO;
using Helpers;

namespace CrossPlatformModule1
{
    public abstract class Program
    {
        private static void Main()
        {
            try
            {
                string basePath = FileSearch.FindProjectDirectory(AppContext.BaseDirectory) ??
                                  throw new FileNotFoundException("Could not find project directory");

                string inputFilePath = Path.Combine(basePath, "INPUT.txt");
                string outputFilePath = Path.Combine(basePath, "OUTPUT.txt");
                int[] ints = ParseInput(inputFilePath);
                Console.WriteLine($"Parsed numbers from file [\"{inputFilePath}\"]: N {ints[0]}, K {ints[1]}");

                int res = BinaryNumbersCount(ints[0], ints[1]);
                File.WriteAllText(outputFilePath, res.ToString());
                Console.WriteLine($"Result is written to output file [\"{outputFilePath}\"]: {res}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error message: " + e.Message);
            }
        }

        public static int[] ParseInput(string inputFilePath)
        {
            if (File.ReadAllLines(inputFilePath).Length == 0)
            {
                throw new FileNotFoundException("Could not find input file", inputFilePath);
            }

            string[] numbers = File.ReadAllLines(inputFilePath)[0].Trim().Split(' ');
            if (numbers.Length != 2)
                throw new Exception($"Invalid number of inputs (2 != {numbers.Length})): {inputFilePath}");

            var ints = numbers.Select(x =>
            {
                if (int.TryParse(x, out int number))
                    return number;
                throw new Exception($"Invalid number: {x}");
            }).ToArray();

            if (!IsInRange(ints[0], 1))
                throw new Exception($"Invalid value for N: {ints[0]}. It should be between 1 and 10^9.");

            if (!IsInRange(ints[1], 0))
                throw new Exception($"Invalid value for K: {ints[1]}. It should be between 0 and 10^9.");

            return ints;
        }

        public static bool IsInRange(int value, int min) => value >= min && value <= 1000000000;


        public static int BinaryNumbersCount(int n, int k)
        {
            int count = 0;
            for (int i = 1; i <= n; i++)
            {
                string binary = DecimalToBinary(i);
                if (ZerosCount(binary, k))
                    count++;
            }

            return count;
        }

        public static bool ZerosCount(string binary, int k)
        {
            int zeros = 0;
            foreach (var bit in binary)
                if (bit == '0')
                    zeros++;
            return zeros == k;
        }

        public static string DecimalToBinary(int number) => DecimalToBinaryHelper(number, "");

        private static string DecimalToBinaryHelper(int number, string binary)
        {
            binary = (number % 2) + binary;
            return number < 2 ? binary : DecimalToBinaryHelper(number / 2, binary);
        }
    }
}