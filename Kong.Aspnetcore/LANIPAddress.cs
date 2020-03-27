using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 提供本机局域网ip获取
    /// </summary>
    static class LANIPAddress
    {
        /// <summary>
        /// 返回与目标域名或ip在同一网段的本机ip
        /// </summary>
        /// <param name="targetHost">目标域名或ip</param>
        /// <returns></returns>
        public static IPAddress GetMatchLANIPAddress(string targetHost)
        {
            var targetIpAddressArray = Dns.GetHostAddresses(targetHost);
            var targets = targetIpAddressArray.Select(item => GetMatchLANIPAddress(item));
            return targets.FirstOrDefault(item => item != null);
        }

        /// <summary>
        /// 返回与目标ip在同一网段的本机ip
        /// </summary>
        /// <param name="targetIPAddress">目标ipaddress</param>
        /// <returns></returns>
        public static IPAddress GetMatchLANIPAddress(IPAddress targetIPAddress)
        {
            static IEnumerable<(IPAddress ip, IPAddress mask, IPAddress gateway)> GetIpConfigs(NetworkInterface network)
            {
                var gateways = network.GetIPProperties()
                    .GatewayAddresses
                    .Select(item => item.Address)
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
                .Where(item => targetIPAddress.IsInNetwork(item.gateway, item.mask))
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
