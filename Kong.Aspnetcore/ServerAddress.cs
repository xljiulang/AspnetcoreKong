using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 提供本机局域网ip获取
    /// </summary>
    static class ServerAddress
    {
        /// <summary>
        /// 返回能与指定远程地址通讯的本机服务地址
        /// </summary>
        /// <param name="remoteUri">远程uri</param>
        /// <returns></returns>
        public static async Task<IPAddress> GetServerAddressAsync(Uri remoteUri)
        {
            var address = Dns
                .GetHostAddresses(remoteUri.Host)
                .Select(item => GetLocalIPAddress(item))
                .FirstOrDefault(item => item != null);

            if (address != null)
            {
                return address;
            }

            EndPoint endPoint;
            if (remoteUri.HostNameType == UriHostNameType.Dns)
            {
                endPoint = new DnsEndPoint(remoteUri.Host, remoteUri.Port);
            }
            else
            {
                endPoint = new IPEndPoint(IPAddress.Parse(remoteUri.Host), remoteUri.Port);
            }

            using var cancel = new CancellationTokenSource(TimeSpan.FromSeconds(1d));
            address = await GetLocalIPAddressAsync(endPoint, cancel.Token);
            if (address != null)
            {
                return address;
            }

            throw new ServerAddressNotFoundException($"无法找到与{remoteUri.Host}可通讯的服务ip");
        }

        /// <summary>
        /// 返回能与指定远程地址通讯的本机地址
        /// </summary>
        /// <param name="remote"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static async Task<IPAddress> GetLocalIPAddressAsync(EndPoint remote, CancellationToken token)
        {
            try
            {
                using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var connectTask = socket.ConnectAsync(remote);
                var delayTask = Task.Delay(Timeout.Infinite, token);
                await await Task.WhenAny(connectTask, delayTask);
                var local = (IPEndPoint)socket.LocalEndPoint;
                return local.Address;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 返回能与指定远程地址通讯的本机地址
        /// </summary>
        /// <param name="remoteAddress">远程地址</param>
        /// <returns></returns>
        private static IPAddress GetLocalIPAddress(IPAddress remoteAddress)
        {
            static IEnumerable<(IPAddress ip, IPAddress mask, IPAddress gateway)> GetIpConfigs(NetworkInterface network)
            {
                var gateways = network.GetIPProperties()
                    .GatewayAddresses
                    .Select(item => item.Address)
                    .Distinct()
                    .Where(item => item.AddressFamily == AddressFamily.InterNetwork);

                var ipMasks = network.GetIPProperties()
                    .UnicastAddresses
                    .Where(item => item.Address.AddressFamily == AddressFamily.InterNetwork);

                return from gateway in gateways
                       join ipMask in ipMasks
                       on true equals true
                       where ipMask.Address.IsInNetwork(gateway, ipMask.IPv4Mask)
                       orderby BinaryPrimitives.ReadInt32BigEndian(ipMask.Address.GetAddressBytes())
                       select (ipMask.Address, ipMask.IPv4Mask, gateway);
            }

            var match = NetworkInterface
                .GetAllNetworkInterfaces()
                .OrderBy(item => item.OperationalStatus)
                .SelectMany(item => GetIpConfigs(item))
                .Where(item => remoteAddress.IsInNetwork(item.gateway, item.mask))
                .FirstOrDefault();

            return match.ip;
        }


        /// <summary>
        /// 获取是否在指定网段内
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="gateway"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        private static bool IsInNetwork(this IPAddress ipAddress, IPAddress gateway, IPAddress mask)
        {
            var masks = mask.GetAddressBytes().AsSpan();
            var gateways = gateway.GetAddressBytes().AsSpan();
            var ipBytes = ipAddress.GetAddressBytes().AsSpan();

            for (var i = 0; i < masks.Length; i++)
            {
                if ((gateways[i] & masks[i]) != (ipBytes[i] & masks[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
