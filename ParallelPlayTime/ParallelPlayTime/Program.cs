using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelPlayTime
{
    class Program
    {
        static Object lockObj = new Object();

        static void Main(string[] args)
        {
            var mySymbols = new Symbols();
            ConcurrentDictionary<string, int> original = new ConcurrentDictionary<string, int>();
            ConcurrentDictionary<string, int> copy = new ConcurrentDictionary<string, int>();

            foreach (var symbol in mySymbols.All.OrderBy(s => s))
            {
                original.TryAdd(symbol, new Random().Next(0, 100));
                copy.TryAdd(symbol, 0);
            }

            int i = 0;

            Parallel.ForEach(original, orig =>
            {
                Thread.Sleep(orig.Value);

                lock (lockObj)
                {
                    copy.TryUpdate(orig.Key, orig.Value, 0);
                    Console.WriteLine("{0} :: {1}", orig.Key, i);
                    Interlocked.Increment(ref i);
                }
            });

            int j = 0;
        }
    }
}
