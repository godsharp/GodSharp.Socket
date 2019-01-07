using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GodSharp.Sockets
{
    public static class IUdpConnectionExtensions
    {
        #region Check
        private static T OnSend<T>(IUdpConnection connection, Func<T> func, T @default)
        {
            if (!OnSend(connection)) return @default;

            return func.Invoke();
        }

        private static bool OnSend(IUdpConnection connection)
        {
            if (connection == null) return false;
            if (connection.Listener == null) return false;
            if (!connection.Listener.Running) return false;

            return true;
        }
        #endregion

        #region Send

        /// <summary>
        /// Sends to.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="remoteEP">The remote ep.</param>
        /// <returns></returns>
        public static int SendTo(this IUdpConnection connection, byte[] buffer, EndPoint remoteEP) => OnSend(connection, () => connection.Instance.SendTo(buffer,remoteEP), -1);

        /// <summary>
        /// Sends to.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="str">The string.</param>
        /// <param name="remoteEP">The remote ep.</param>
        /// <returns></returns>
        public static int SendTo(this IUdpConnection connection, string str, EndPoint remoteEP) => OnSend(connection, () => connection.Instance.SendTo(str, remoteEP), -1);

        /// <summary>
        /// Sends to.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="remoteEP">The remote ep.</param>
        /// <returns></returns>
        public static int SendTo(this IUdpConnection connection, byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP) => OnSend(connection, () => connection.Instance.SendTo(buffer, offset, size, socketFlags, remoteEP), -1);

        /// <summary>
        /// Sends to asynchronous.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="e">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static bool SendToAsync(this IUdpConnection connection, SocketAsyncEventArgs e) => OnSend(connection, () => connection.Instance.SendToAsync(e), false);

        /// <summary>
        /// Begins the send to.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="size">The size.</param>
        /// <param name="socketFlags">The socket flags.</param>
        /// <param name="remoteEP">The remote ep.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public static IAsyncResult BeginSendTo(this IUdpConnection connection, byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP, AsyncCallback callback, object state) => OnSend(connection, () => connection.Instance.BeginSendTo(buffer, offset, size, socketFlags, remoteEP, callback, state), null);

        /// <summary>
        /// Begins the send to.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="remoteEP">The remote ep.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public static IAsyncResult BeginSendTo(this IUdpConnection connection, byte[] buffer, EndPoint remoteEP, AsyncCallback callback, object state) => OnSend(connection, () => connection.Instance.BeginSendTo(buffer, remoteEP, callback, state), null);

        /// <summary>
        /// Begins the send to.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="str">The string.</param>
        /// <param name="remoteEP">The remote ep.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static IAsyncResult BeginSendTo(this IUdpConnection connection, string str, EndPoint remoteEP, AsyncCallback callback, object state, Encoding encoding = null) => OnSend(connection, () => connection.Instance.BeginSendTo(str, remoteEP, callback, state, encoding), null);

        /// <summary>
        /// Ends the send.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="asyncResult">The asynchronous result.</param>
        /// <returns></returns>
        public static int EndSendTo(this IUdpConnection connection, IAsyncResult asyncResult) => OnSend(connection, () => connection.Instance.EndSendTo(asyncResult), -1);

        #endregion

        #region Receive

        /// <summary>
        /// Receives from.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="remoteEP">The remote ep.</param>
        /// <returns></returns>
        public static int ReceiveFrom(this IUdpConnection connection, byte[] buffer, ref EndPoint remoteEP)
        {
            if (!OnSend(connection)) return -1;

            return connection.Instance.ReceiveFrom(buffer, ref remoteEP);
        }

        /// <summary>
        /// Receives from asynchronous.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="e">The <see cref="SocketAsyncEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public static bool ReceiveFromAsync(this IUdpConnection connection, SocketAsyncEventArgs e) => OnSend(connection, () => connection.Instance.ReceiveFromAsync(e), false);

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
        public static IAsyncResult BeginReceiveFrom(this IUdpConnection connection, byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state)
        {
            if (!OnSend(connection)) return null;

            return connection.Instance.BeginReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);
        }

        /// <summary>
        /// Ends the receive from.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="asyncResult">The asynchronous result.</param>
        /// <param name="remoteEP">The remote ep.</param>
        /// <returns></returns>
        public static int EndReceiveFrom(this IUdpConnection connection, IAsyncResult asyncResult, ref EndPoint remoteEP)
        {
            if (!OnSend(connection)) return -1;

            return connection.Instance.EndReceiveFrom(asyncResult, ref remoteEP);
        }

        #endregion
    }
}
