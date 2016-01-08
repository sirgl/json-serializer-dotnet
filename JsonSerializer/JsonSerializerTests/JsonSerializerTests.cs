using System;
using FluentAssertions;
using Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace JsonSerializerTests
{
    [TestClass]
    public class JsonSerializerTests
    {
        private JsonSerializer serializer = new JsonSerializer();

        [Test]
        public void StringSerializationTest()
        {
            serializer.Serialize("Test").Should().Be("\"Test\"");
        }

        [Test]
        public void IntSerializationTest()
        {
            serializer.Serialize(5).Should().Be("5");
        }

        [Test]
        public void NullSerializationTest()
        {
            serializer.Serialize(null).Should().Be("null");
        }

        [Test]
        public void IntArraySerializerTest()
        {
            serializer.Serialize(new int[] { 3, 4, 6 }).Should().Be("[3,4,6]");
        }
    }
}
