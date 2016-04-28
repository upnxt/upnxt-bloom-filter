using System;
using System.Collections;
using Core.Indexers;

namespace Core
{
    public class BloomFilter
    {
        private readonly IIndex _indexer;
        private readonly int _m;
        private readonly BitArray _table;

        /// <summary>
        /// </summary>
        /// <param name="n">number of elements we would like in our table</param>
        /// <param name="errorRate">percentage of false positives we are willing to deal with</param>
        /// <param name="indexer"></param>
        public BloomFilter(int n, double errorRate, IIndex indexer)
        {
            //how big our table will be. wikipedia has calculations so we don't have to guess
            _m = TableSize(n, errorRate);

            //size should be based on the % of acceptable false positives
            _table = new BitArray(_m);
            _indexer = indexer;
        }

        /// <summary>
        /// Add a new hash to the index
        /// </summary>
        public void Add(string value)
        {
            var index = _indexer.Index(value, _m);
            _table[index] = true;
        }

        /// <summary>
        /// Find an index that matches the hash of value
        /// </summary>
        public bool Contains(string value)
        {
            var index = _indexer.Index(value, _m);
            return _table[index];
        }

        /// <summary>
        /// Calculate the table size based on n and acceptable false positives
        /// </summary>
        private static int TableSize(int n, double errorRate)
        {
            var m = Math.Ceiling(Math.Abs(n * Math.Log(errorRate)) / Math.Pow(Math.Log(2), 2));
            return (int)m;
        }
    }
}