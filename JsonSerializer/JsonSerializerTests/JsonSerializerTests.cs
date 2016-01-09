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


        [SerializeableAttribute()]
        class SerializeablePerson
        {
            
        }

//        [Test]
//        public void IntArrayArraySerializationTest()
//        {
//            var assembly = Assembly.GetExecutingAssembly();
//            var stream = assembly.GetManifestResourceStream("JsonSerializerTests.resources.IntArrayArray.json");
//            string text;
//            using (var reader = new StreamReader(stream))
//            {
//                text = reader.ReadToEnd();
//            }
//            string s = serializer.Serialize(new int[][] {new int[] {1, 2}, new int[] {3}});
//            Console.Out.WriteLine(s);
//            s
//                .Should().Be(text);
//        }

        [Test]
        public void StringArraySerializationTest()
        {
            serializer.Serialize(new string[] {"hello", "world", "!"})
                .Should().Be("[\n    \"hello\",\n    \"world\",\n    \"!\"\n]");
        }
    }
}
