using System;
using System.Text;

namespace Json
{
    public class JsonSerializer
    {
        public bool Pretty { get; set; }

        public string Serialize(object obj)
        {
            if (obj == null)
            {
                return "null";
            }
            var type = obj.GetType();
            if (obj is string)
            {
                return '"' + (string)obj + '"';
            }
            if (obj is int)
            {
                return obj.ToString();
            }
            if (type.IsArray)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("[");
                object[] objects = (object[])Convert.ChangeType(obj, typeof(object[]));
                //                Object[] objects = (object[]) obj;
                foreach (var o in objects)
                {
                    builder.Append(Serialize(o));
                    builder.Append(",");
                    if (Pretty)
                    {
                        builder.Append("\n");
                    }
                }
                builder.Append("]");
                return builder.ToString();
            }
            return null;
        }
    }


    class Person
    {
        public string name;
        public int age;

        public Person(string name, int age)
        {
            this.name = name;
            this.age = age;
        }


        public static void Main(string[] args)
        {
            var person = new Person("John", 4);
            var ints = new int[50];
            var type = ints.GetType();
            var elementType = type.GetElementType();
            int x = 5;
            var obj = ints;
            object[] objects = (object[])Convert.ChangeType(obj, typeof(object[]));
        }
    }


}
