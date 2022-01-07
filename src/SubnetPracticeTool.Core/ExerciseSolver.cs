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

    public bool IsValidSolution(string networkAddress, string broadcastAddress) =>
        GetNetworkAddress().ToString() == networkAddress && GetBroadcastAddress().ToString() == broadcastAddress;

    public IEnumerable<Dictionary<string, string>> GetSolutionErrors(string networkAddress, string broadcastAddress)
    {
        var result = new List<Dictionary<string, string>>();
        if (networkAddress != GetNetworkAddress().ToString())
        {
            result.Add(new Dictionary<string, string> { { "field", "networkAddress" }, { "message", $"{networkAddress} is not the network address for {_ipAddress}/{_subnetMask.Bits}" } });
        }
        if (broadcastAddress != GetBroadcastAddress().ToString())
        {
            result.Add(new Dictionary<string, string> { { "field", "broadcastAddress" }, { "message", $"{broadcastAddress} is not the broadcast address for {_ipAddress}/{_subnetMask.Bits}" } });
        }

        return result;
    }
}
