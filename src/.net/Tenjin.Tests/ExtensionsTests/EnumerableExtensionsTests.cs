using System.Collections.Generic;
using NUnit.Framework;
using Tenjin.Extensions;

namespace Tenjin.Tests.ExtensionsTests
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void IsNotEmpty_WhenCollectionIsNull_ReturnsFalse()
        {
            IEnumerable<object>? collection = null;

            Assert.IsFalse(collection.IsNotEmpty());
        }

        [Test]
        public void IsNotEmpty_WhenIEnumerableIsEmpty_ReturnsFalse()
        {
            Assert.IsFalse(new List<object>().IsNotEmpty());
        }

        [Test]
        public void IsNotEmpty_WhenIEnumerableIsNotEmpty_ReturnsTrue()
        {
            var array = new[] { 1, 2, 3 };

            Assert.IsTrue(array.IsNotEmpty());
        }

        [Test]
        public void IsEmpty_WhenCollectionIsNull_ReturnsTrue()
        {
            IEnumerable<object>? collection = null;

            Assert.IsTrue(collection.IsEmpty());
        }

        [Test]
        public void IsEmpty_WhenIEnumerableIsEmpty_ReturnsTrue()
        {
            Assert.IsTrue(new List<object>().IsEmpty());
        }

        [Test]
        public void IsEmpty_WhenIEnumerableIsNotEmpty_ReturnsFalse()
        {
            var array = new[] { 1, 2, 3 };

            Assert.IsFalse(array.IsEmpty());
        }
    }
}
