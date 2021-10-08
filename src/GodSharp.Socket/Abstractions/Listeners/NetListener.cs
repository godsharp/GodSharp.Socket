﻿using System;
using System.Collections.Generic;
using System.Net;

namespace GodSharp.Sockets.Abstractions
{
    public abstract class NetListener<TConnection> : INetListener<TConnection>, IDisposable where TConnection : INetConnection
    {
        public virtual TConnection Connection { get; internal set; }
        private KeepAliveOptions KeepAliveOption { get; set; }

        private bool running = false;
        public virtual bool Running
        {
            get => running;
            internal set
            {
                running = value;
            }
        }

        private byte[] buffers = null;

        public NetListener(TConnection connection)
        {
            if(connection==null) throw new ArgumentNullException(nameof(connection));
            Connection = connection;
        }

        public virtual void Start()
        {
            if (Running) return;

            try
            {
                if (KeepAliveOption != null && KeepAliveOption.KeepAlive) Connection.Instance.KeepAliveOne(KeepAliveOption.KeepAlive, KeepAliveOption.Interval, KeepAliveOption.Span);

                Running = true;
                BeginReceive();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Stop()
        {
            if (!Running) return;

            Running = false;
            var exceptions = new List<Exception>();
            try
            {
                Connection?.Instance.Close();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            
            try
            {
                Connection?.Instance.Close();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            OnStop(exceptions.Count == 0 ? null : new SocketAggregateException("Stop throw exception", exceptions.ToArray()));
        }

        protected abstract void OnStop(Exception exception);

        protected abstract void OnException(Exception exception);

        public void BeginReceive()
        {
            buffers = new byte[Connection.Instance.ReceiveBufferSize];
            
            OnBeginReceive(ref buffers);
        }

        protected abstract void OnBeginReceive(ref byte[] buffers);

        public void ReceivedCallback(IAsyncResult result)
        {
            bool error = false;
            try
            {
                if (!Running) return;

                ReceiveResult ret = OnEndReceive<ReceiveResult>(result);

                if (ret?.Length > 0)
                {
                    byte[] tmp = new byte[ret.Length];
                    Buffer.BlockCopy(buffers, 0, tmp, 0, ret.Length);

                    BeginReceive();

                    OnReceiveHandling(tmp, ret.RemoteEndPoint, Connection.LocalEndPoint);
                }
                else
                {
                    error = true;
                }
            }
            catch (Exception ex)
            {
                error = true;
                OnException(ex);
            }
            finally
            {
                if (error) Stop();
            }
        }

        internal virtual void KeepAlive(KeepAliveOptions options)
        {
            KeepAliveOption = options;
        }

        protected abstract T OnEndReceive<T>(IAsyncResult result) where T : ReceiveResult, new();

        protected abstract void OnReceiveHandling(byte[] buffers, IPEndPoint remote = null, IPEndPoint local = null);

        public abstract void Dispose();

        protected class ReceiveResult
        {
            public int Length { get; set; }
            public IPEndPoint RemoteEndPoint { get; set; }

            public ReceiveResult()
            {
            }

            public ReceiveResult(int length, IPEndPoint remoteEndPoint)
            {
                Length = length;
                RemoteEndPoint = remoteEndPoint;
            }
        }
    }
}
