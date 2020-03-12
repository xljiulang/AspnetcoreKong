using System;
using System.Buffers.Binary;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Kong.Aspnetcore
{
    /// <summary>
    /// 提供本机ip获取
    /// </summary>
    static class LocalIPAddress
    {
        /// <summary>
        /// 返回本地与目标ipaddress同一网段的ip
        /// </summary>
        /// <param name="hostnameOrAddress"></param>
        /// <returns></returns>
        public static IPAddress GetIPAddress(string hostnameOrAddress)
        {
            var kongIps = Dns.GetHostAddresses(hostnameOrAddress);
            var targets = kongIps.Select(item => GetIPAddress(item));
            return targets.FirstOrDefault(item => item != null);
        }

        /// <summary>
        /// 返回本地与目标ipaddress同一网段的ip
        /// </summary>
        /// <param name="ipaddress">目标ipaddress</param>
        /// <returns></returns>
        public static IPAddress GetIPAddress(IPAddress ipaddress)
        {
            var nets = NetworkInterface.GetAllNetworkInterfaces();
            var gateways = nets.SelectMany(net => net.GetIPProperties().GatewayAddresses.Select(item => item.Address));
            var ips = nets.SelectMany(net => net.GetIPProperties().UnicastAddresses.Where(item => item.Address.AddressFamily == AddressFamily.InterNetwork));

            var q = from g in gateways
                    join i in ips
                    on g.GetAddressBytes().First() equals i.Address.GetAddressBytes().First()
                    orderby BinaryPrimitives.ReadInt32BigEndian(i.IPv4Mask.GetAddressBytes())
                    select new { ip = i.Address, gateway = g, mask = i.IPv4Mask };

            var network = q.FirstOrDefault(item => ipaddress.IsInNetwork(item.gateway, item.mask));
            return network?.ip;
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
