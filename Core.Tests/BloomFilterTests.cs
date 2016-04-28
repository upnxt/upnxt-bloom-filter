using System.Security.Cryptography;
using Core.Indexers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Tests
{
    [TestClass]
    public class BloomFilterTests
    {
        private string[] _planets;
        private IIndex _indexer;

        [TestInitialize]
        public void Init()
        {
            _planets = new[] {"mercury", "venus", "earth", "mars", "jupiter", "saturn", "uranus", "venus"};
            _indexer = new CryptoIndex(new HashAlgorithm[] { MD5.Create(), SHA1.Create(), SHA256.Create() });
        }

        [TestMethod]
        public void assert_earth_exists()
        {
            var filter = new BloomFilter(8, .50, _indexer);

            foreach (var planet in _planets)
                filter.Add(planet);

            Assert.IsTrue(filter.Contains("earth"));
        }

        [TestMethod]
        public void assert_pluto_exists()
        {
            var filter = new BloomFilter(8, .50, _indexer);

            foreach (var planet in _planets)
                filter.Add(planet);

            //due to high error rate pluto will end up having the same hash result as another planet in our collection 
            Assert.IsTrue(filter.Contains("pluto"));
        }

        [TestMethod]
        public void assert_pluto_actually_doesnt_exist_no_hard_feelings_you_guys()
        {
            var filter = new BloomFilter(8, .01, _indexer);

            foreach (var planet in _planets)
                filter.Add(planet);

            Assert.IsTrue(filter.Contains("earth"));
            Assert.IsTrue(filter.Contains("jupiter"));

            //due to high error rate pluto will end up having the same hash result as another planet in our collection 
            Assert.IsFalse(filter.Contains("pluto"));
        }
    }
}