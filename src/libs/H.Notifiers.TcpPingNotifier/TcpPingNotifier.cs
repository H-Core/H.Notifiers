using System;
using System.Net.Sockets;

namespace H.Notifiers
{
    /// <summary>
    /// 
    /// </summary>
    public class TcpPingNotifier : TimerNotifier
    {
        #region Properties

        private string Ip { get; set; } = string.Empty;
        private int Port { get; set; }
        private bool OnlyIfNetworkActive { get; set; } = true;

        #endregion

        #region Contructors

        /// <summary>
        /// 
        /// </summary>
        public TcpPingNotifier()
        {
            AddSetting(nameof(Ip), o => Ip = o, _ => true, Ip);
            AddSetting(nameof(Port), o => Port = o, _ => true, Port);
            AddSetting(nameof(OnlyIfNetworkActive), o => OnlyIfNetworkActive = o, _ => true, OnlyIfNetworkActive);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool OnResult()
        {
            //var isAvailableNetworkActive = GetVariable("$internet$", true);
            //if (OnlyIfNetworkActive && !isAvailableNetworkActive)
            //{
            //    return false;
            //}

            try
            {
                using (new TcpClient(Ip, Port))
                {
                }

                return false;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return true;
            }
        }

        #endregion

    }
}
