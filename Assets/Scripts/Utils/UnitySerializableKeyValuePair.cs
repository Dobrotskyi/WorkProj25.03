using System;
using UnityEngine;

namespace Code.Utils
{
    [Serializable]
    public struct UnitySerializableKeyValue<K, V>
    {
        public UnitySerializableKeyValue(K key, V value)
        {
            _key = key;
            _value = value;
        }

        [SerializeField] private K _key;
        [SerializeField] private V _value;

        public K Key => _key;
        public V Value => _value;
    }
}
