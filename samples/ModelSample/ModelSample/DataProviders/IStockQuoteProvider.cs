using System;
using System.Collections.Generic;
using System.Text;

namespace ModelSample
{
    /// <summary>
    /// Interface for a provider of stock quotes.
    /// </summary>
    public interface IStockQuoteProvider
    {
        /// <summary>
        /// Get a quote. This function may block on slow operations like hitting the network.
        /// </summary>
        /// <param name="symbol">The stock symbol.</param>
        /// <param name="quote">The quote.</param>
        /// <returns>Whether we were able to get a quote.</returns>
        bool TryGetQuote(string symbol, out double quote);
    }
}
