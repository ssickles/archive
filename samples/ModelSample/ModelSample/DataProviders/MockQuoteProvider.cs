using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ModelSample
{
    /// <summary>
    /// Mock IStockQuoteProvider that alwyas returns 100.
    /// </summary>
    public class MockQuoteProvider : IStockQuoteProvider
    {
        public bool TryGetQuote(string symbol, out double quote)
        {
            Thread.Sleep(1000);
            Random rnd = new Random();
            quote = rnd.Next(100);
            return true;
        }
    }
}
