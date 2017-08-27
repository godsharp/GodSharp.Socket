using System;
using System.Collections;
using System.Collections.Generic;

namespace GodSharp.Sockets
{
    public class SocketClientCollection: IEnumerable<Sender>,IList<Sender>
    {
        private Dictionary<Guid, Listener> listeners = null;
        private Dictionary<string, Guid> clientMap = null;
        private List<Sender> clients;

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public SocketClientCollection()
        {
            listeners = new Dictionary<Guid, Listener>();
            clientMap = new Dictionary<string, Guid>();
            clients = new List<Sender>();
        }
        
        /// <summary>
        /// Gets the <see cref="Sender"/> with the specified unique identifier.
        /// </summary>
        /// <value>
        /// The <see cref="Sender"/>.
        /// </value>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public Sender this[Guid guid]
        {
            get { return this.listeners[guid].Sender; }
        }

        /// <summary>
        /// Gets the <see cref="Sender"/> with the specified host.
        /// </summary>
        /// <value>
        /// The <see cref="Sender"/>.
        /// </value>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public Sender this[string host,int port]
        {
            get
            {
                string k = $"{host.Trim()}:{port}";

                if (!clientMap.ContainsKey(k))
                {
                    return null;
                }

                Guid guid = clientMap[k];
                
                if (!listeners.ContainsKey(guid))
                {
                    return null;
                }

                return this.listeners[guid].Sender;
            }
        }

        /// <summary>
        /// Gets the <see cref="Sender"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="Sender"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public Sender this[int index]
        {
            get
            {
                if (index < 0 || index > clients.Count - 1)
                {
                    return null;
                }

                return clients[index];
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Sender> GetEnumerator()
        {
            return clients.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return clients.GetEnumerator();
        }

        public int IndexOf(Sender item)
        {
            return clients.IndexOf(item);
        }

        public void Insert(int index, Sender item)
        {
            clients.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            clients.RemoveAt(index);
        }

        public void Add(Sender item)
        {
            clients.Add(item);
        }

        public void Clear()
        {
            clients.Clear();
        }

        public bool Contains(Sender item)
        {
            return clients.Contains(item);
        }

        public void CopyTo(Sender[] array, int arrayIndex)
        {
            clients.CopyTo(array, arrayIndex);
        }

        public bool Remove(Sender item)
        {
            return clients.Remove(item);
        }
        
        Sender IList<Sender>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
