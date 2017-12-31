using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GodSharp.Sockets.Internal
{
    internal class ListenerCollection : IDictionary<Guid, Listener>
    {
        private Dictionary<Guid, Listener> listeners;

        public Listener this[Guid key] { get => listeners[key]; set => listeners[key] = value; }

        public ICollection<Guid> Keys => listeners.Keys;

        public ICollection<Listener> Values => listeners.Values;

        public int Count => listeners.Count;

        public bool IsReadOnly => false;

        public void Add(Guid key, Listener value)
        {
            listeners.Add(key, value);
        }

        public void Add(KeyValuePair<Guid, Listener> item)
        {
            listeners.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            listeners.Clear();
        }

        public bool Contains(KeyValuePair<Guid, Listener> item)
        {
            return listeners.Contains(item);
        }

        public bool ContainsKey(Guid key)
        {
            return listeners.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<Guid, Listener>[] array, int arrayIndex)
        {
            //listeners.CopyTo
        }

        public IEnumerator<KeyValuePair<Guid, Listener>> GetEnumerator()
        {
            return listeners.GetEnumerator();
        }

        public bool Remove(Guid key)
        {
            return listeners.Remove(key);
        }

        public bool Remove(KeyValuePair<Guid, Listener> item)
        {
            return listeners.Remove(item.Key);
        }

        public bool TryGetValue(Guid key, out Listener value)
        {
            return listeners.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return listeners.GetEnumerator();
        }
    }
}
