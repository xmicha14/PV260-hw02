using BenchmarkDotNet.Running;
using System;

namespace XMLRepair
{
    public class Program
    {
        static void Main(string[] args)
        {
            var results = BenchmarkRunner.Run<XMLCorrection>();
            Console.ReadLine();
        }
    }
}
