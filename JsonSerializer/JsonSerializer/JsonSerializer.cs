using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Json
{
    public class JsonSerializer
    {
        private const string tab = "    ";

        public string Serialize(object obj)
        {
            return SerializeByDepth(obj, 0);
        }

        private string GetTabs(int depth)
        {
            return new string(' ', 4 * depth);
        }

        private string SerializeString(object obj, int depth)
        {
            return GetTabs(depth) + "\"" + obj + "\"";
        }

        private string SerializePrimitive(object obj, int depth)
        {
            return GetTabs(depth) + obj;
        }

        private string SerializeNull(int depth)
        {
            return GetTabs(depth) + "null";
        }

        private string SerializeEnumerable(object obj, int depth)
        {
            var type = obj.GetType();
            Type elementType = type.GetElementType();
            if (elementType.IsPrimitive || elementType == typeof (string))
            {
                StringBuilder stringBuilder1 = new StringBuilder();
                IEnumerable<object> enumerable1 = ((IList)obj).Cast<object>();

                stringBuilder1
                    .Append("[")
                    .Append(string.Join(", ", enumerable1.Select(o => SerializeByDepth(o, 0))))
                    .Append("]");
                return stringBuilder1.ToString();
            }
            

            if (type.HasElementType)
            {
                if (!elementType.IsPrimitive && elementType != typeof(string))
                {
                    var customAttributesData = type.GetCustomAttributesData();
                    if (customAttributesData.Any(data => data.AttributeType == typeof(NonSerializedAttribute)))
                    {
                        return null;
                    }
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            IEnumerable<object> enumerable = ((IList)obj).Cast<object>();
            
            stringBuilder
                .Append("[\n" + GetTabs(depth + 1))
                .Append(string.Join(",\n" + GetTabs(depth + 1), enumerable.Select(o => SerializeByDepth(o, depth + 1))))
                .Append("\n" + GetTabs(depth) + "]");
            return stringBuilder.ToString();
        }

        private string SerializeObject(object obj, int depth)
        {
            var type = obj.GetType();
            if (!IsSerializeableType(obj) && !(obj is string))
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();


            var fieldInfos = type.GetFields();
            stringBuilder.Append("{\n");
            foreach (var fieldInfo in fieldInfos)
            {
                var attributes = fieldInfo.GetCustomAttributes();

                if (attributes.OfType<NonSerializedAttribute>().Any())
                {
                    continue;
                }
                var value = fieldInfo.GetValue(obj);
                stringBuilder.Append(SerializeString(fieldInfo.Name, depth + 1));
                stringBuilder.Append(" : ");
                if (IsSimpleType(value))
                {
                    stringBuilder.Append(SerializeByDepth(value, 0) + "\n");
                    continue;
                }
                stringBuilder.Append(SerializeByDepth(value, depth + 1) + "\n");
            }
            stringBuilder.Append(GetTabs(depth) + "}");
            return stringBuilder.ToString();
        }

        private bool IsSerializeableType(Object obj)
        {
            var type = obj.GetType();
            if (IsSimpleType(obj)) return true;
            var attributes = type.GetCustomAttributes(true);
            return attributes
                .OfType<SerializableAttribute>()
                .Any();
        }

        private static bool IsSimpleType(object obj)
        {
            var type = obj.GetType();
            if (type.IsPrimitive || obj is string)
            {
                return true;
            }
            return false;
        }

        private string SerializeByDepth(object obj, int depth)
        {
            if (obj == null)
            {
                return SerializeNull(depth);
            }
            if (obj is string)
            {
                return SerializeString(obj, depth);
            }
            if (obj.GetType().IsPrimitive)
            {
                return SerializePrimitive(obj, depth);
            }
            if (obj is IEnumerable)
            {
                return SerializeEnumerable(obj, depth);
            }

            if (IsSerializeableType(obj))
            {
                return SerializeObject(obj, depth);
            }
            return "";
        }
    }
}
