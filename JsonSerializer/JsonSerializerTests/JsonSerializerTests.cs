using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Json;
using NUnit.Framework;
using TestContext = NUnit.Framework.TestContext;

namespace JsonSerializerTests
{
    [TestFixture]
    public class JsonSerializerTests
    {
        private readonly JsonSerializer serializer = new JsonSerializer();

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
        public void IntArraySerializationTest()
        {
            serializer.Serialize(new int[] {3, 4, 6})
                .Should().Be("[\n    3,\n    4,\n    6\n]");
        }


        [Serializable]
        class SerializeablePerson
        {
            public string x;
            [NonSerialized]
            public int y;
            public int[] m;

            public SerializeablePerson(string x, int y, int[] m)
            {
                this.x = x;
                this.y = y;
                this.m = m;
            }
        }

        //Just don't want to write...
        [Test]
        public void ClassSerialization()
        {
            var serializeablePerson = new SerializeablePerson("asdf", 3, new int[] {3, 4, 5});
            Console.Out.WriteLine(serializer.Serialize(serializeablePerson));
        }


        [Test]
        public void ClassSerialization2()
        {
            var serializeablePersons = new SerializeablePerson[] {new SerializeablePerson("asd", 2, new int[] {123, 12}), new SerializeablePerson("asdasd", 23, new int[] {12,  21})  };
            Console.Out.WriteLine(serializer.Serialize( serializeablePersons ));
        }

        [Test]
        public void StringArraySerializationTest()
        {
            serializer.Serialize(new string[] {"hello", "world", "!"})
                .Should().Be("[\n    \"hello\",\n    \"world\",\n    \"!\"\n]");
        }
    }
}
