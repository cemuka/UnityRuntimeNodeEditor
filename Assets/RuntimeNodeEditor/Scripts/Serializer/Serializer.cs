using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RuntimeNodeEditor
{
    public class Serializer
    {
        private List<SerializedValue> _collection;

        public Serializer()
        {
            _collection = new List<SerializedValue>();
        }

        public Serializer Add(string key, string value)
        {
            _collection.Add(new SerializedValue()
            {
                key = key,
                value = value
            });
            return this;
        }

        public string Get(string key)
        {
            var result = string.Empty;

            var query = _collection.FirstOrDefault(s => s.key == key);

            if (query != null)
            {
                result = query.value;
            }

            return result;
        }

        public SerializedValue[] Serialize()
        {
            return _collection.ToArray();
        }

        public void Deserialize(SerializedValue[] data)
        {
            _collection = new List<SerializedValue>(data);
        }
    }
}