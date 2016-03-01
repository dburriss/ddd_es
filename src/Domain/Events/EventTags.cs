using System;
using System.Collections.Generic;

namespace Domain.Events
{
    //TODO: make instance and pass in as constructor
    public static class EventTags
    {
        static IDictionary<Type, string> tags;
        static EventTags()
        {
            tags = new Dictionary<Type, string>();
            //{
            //    { typeof(Committee), "COMMITTEE" }
            //};
        }

        public static IEnumerable<string> Tags
        {
            get
            {
                return tags.Values;
            }
        }

        public static IEnumerable<Type> Types
        {
            get
            {
                return tags.Keys;
            }
        }

        public static void Set(Type key, string tag)
        {
            tags[key] = tag;
        }

        public static string Get(Type key)
        {
            return tags[key];
        }

        public static string GetOrCreateIfNotExists(Type key)
        {
            if (!tags.ContainsKey(key))
            {
                tags.Add(key, key.AssemblyQualifiedName);
            }
            return tags[key];
        }

        public static bool Remove(Type key)
        {
            return tags.Remove(key);
        }
    }
}
