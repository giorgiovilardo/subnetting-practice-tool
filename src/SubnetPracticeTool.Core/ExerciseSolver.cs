using System.Collections.Immutable;
using System.Net;

namespace SubnetPracticeTool.Core;

public class ExerciseSolver
{
    private readonly IPAddress _ipAddress;
    private readonly SubnetMask _subnetMask;

    public ExerciseSolver(IPAddress ipAddress, SubnetMask subnetMask)
    {
        _ipAddress = ipAddress;
        _subnetMask = subnetMask;
    }

    public IPAddress GetNetworkAddress()
    {
        return new IPAddress(GetNetworkAddressBytes());
    }

    public IPAddress GetBroadcastAddress()
    {
        var sumNetworkOctetsAndWildcardMask = GetNetworkAddressBytes()
            .Zip(_subnetMask.GetWildcardAddressBytes())
            .Select(x => x.First + x.Second)
            .Select(Convert.ToByte)
            .ToArray();
        return new IPAddress(sumNetworkOctetsAndWildcardMask);
    }

    private byte[] GetNetworkAddressBytes()
    {
        var ipBytes = _ipAddress.GetAddressBytes();
        var subnetBytes = _subnetMask.GetAddressBytes();
        return ipBytes
            .Zip(subnetBytes)
            .Select(x => x.First & x.Second)
            .Select(Convert.ToByte)
            .ToArray();
    }

    public static ExerciseSolver FromString(string ipAddress, string subnetBits)
    {
        var ip = IPAddress.Parse(ipAddress);
        var subnet = new SubnetMask(int.Parse(subnetBits));
        return new ExerciseSolver(ip, subnet);
    }
}
