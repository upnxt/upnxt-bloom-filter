using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Core.Indexers
{
    /// <summary>
    ///     being lazy using built in .net hashing algorithms. In the real world we would use non-cryptographic hashing algorithms
    /// </summary>
    public class CryptoIndex : IIndex
    {
        private readonly HashAlgorithm[] _hash;

        public CryptoIndex(HashAlgorithm[] hash)
        {
            _hash = hash;
        }

        public int Index(string value, int m)
        {
            //pass the value through each hashing algorithm
            var hashValue = _hash.Aggregate(Encoding.Unicode.GetBytes(value), (current, hashAlgorithm) => hashAlgorithm.ComputeHash(current));

            //convert to simple index value
            var index = Math.Abs(BitConverter.ToInt32(hashValue, 0) % m);
            return index;
        }
    }
}