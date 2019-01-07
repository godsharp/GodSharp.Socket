using System;
using System.Net.Sockets;
using System.Text;

namespace GodSharp.Sockets
{
    public static class ITcpConnectionExtensions
    {
        #region Check
        private static T OnSend<T>(ITcpConnection connection, Func<T> func, T @default)
        {
            if (!OnSend(connection)) return @default;

            return func.Invoke();
        }

        private static bool OnSend(ITcpConnection connection)
        {
            if (connection == null) return false;
            if (connection.Listener == null) return false;
            if (!connection.Listener.Running) return false;

            return true;
        }
        #endregion

        #region Send

        /// <summary>
        /// Sends the specified buffer.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static int Send(this ITcpConnection connection, byte[] buffer) => OnSend(connection, () => connection.Instance.Send(buffer), -1);

        /// <summary>
        /// Sends the specified string.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="str">The string.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static int Send(this ITcpConnection connection, string str, Encoding encoding = null) => OnSend(connection, () => connection.Instance.Send(str, encoding), -1);

        /// <summary>
        /// Sends the specified buffer.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <returns></returns>
        public static int Send(this ITcpConnection connection, byte[] buffer, int offset, int size, SocketFlags socketFlags = SocketFlags.None) => OnSend(connection, () => connection.Instance.Send(buffer, offset, size, socketFlags), -1);

        /// <summary>
        /// Sends the asynchronous.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="e">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static bool SendAsync(this ITcpConnection connection, SocketAsyncEventArgs e) => OnSend(connection, () => connection.Instance.SendAsync(e), false);

        /// <summary>
        /// Begins the send.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public static IAsyncResult BeginSend(this ITcpConnection connection, byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
        {
            errorCode = SocketError.Success;

            if (!OnSend(connection)) return null;

            return connection.Instance.BeginSend(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        /// <summary>
        /// Begins the send.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="str">The string.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static IAsyncResult BeginSend(this ITcpConnection connection, string str, AsyncCallback callback, object state, Encoding encoding = null) => OnSend(connection, () => connection.Instance.BeginSend(str, callback, state, encoding), null);

        /// <summary>
        /// Ends the send.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="asyncResult">The asynchronous result.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns></returns>
        public static int EndSend(this ITcpConnection connection, IAsyncResult asyncResult, out SocketError errorCode)
        {
            errorCode = SocketError.Success;

            if (!OnSend(connection)) return -1;

            return connection.Instance.EndSend(asyncResult, out errorCode);
        }

        /// <summary>
        /// Ends the send.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="asyncResult">The asynchronous result.</param>
        /// <returns></returns>
        public static int EndSend(this ITcpConnection connection, IAsyncResult asyncResult) => OnSend(connection, () => connection.Instance.EndSend(asyncResult), -1);

        #endregion

        #region Receive

        /// <summary>
        /// Receives the specified buffer.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static int Receive(this ITcpConnection connection, byte[] buffer) => OnSend(connection, () => connection.Instance.Receive(buffer), -1);

        /// <summary>
        /// Receives the asynchronous.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="e">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static bool ReceiveAsync(this ITcpConnection connection, SocketAsyncEventArgs e) => OnSend(connection, () => connection.Instance.ReceiveAsync(e), false);

        /// <summary>
        /// Begins the receive.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public static IAsyncResult BeginReceive(this ITcpConnection connection, byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state)
        {
            errorCode = SocketError.Success;

            if (!OnSend(connection)) return null;

            return connection.Instance.BeginReceive(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        /// <summary>
        /// Ends the receive.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="asyncResult">The asynchronous result.</param>
        /// <param name="errorCode">The error code.</param>
        /// <returns></returns>
        public static int EndReceive(this ITcpConnection connection, IAsyncResult asyncResult, out SocketError errorCode)
        {
            errorCode = SocketError.Success;

            if (!OnSend(connection)) return -1;

            return connection.Instance.EndReceive(asyncResult, out errorCode);
        }

        /// <summary>
        /// Ends the receive.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="asyncResult">The asynchronous result.</param>
        /// <returns></returns>
        public static int EndReceive(this ITcpConnection connection, IAsyncResult asyncResult) => OnSend(connection, () => connection.Instance.EndReceive(asyncResult), -1); 

        #endregion
    }
}
