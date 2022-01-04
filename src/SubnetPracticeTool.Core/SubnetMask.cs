using System.Net;

namespace SubnetPracticeTool.Core;

public record SubnetMask
{
    public SubnetMask(int cidrBits)
    {
        Bits = cidrBits;
        Ip = new IPAddress(GetByteArrayFromCidrBits(cidrBits));
    }

    private byte[] GetByteArrayFromCidrBits(int cidrBits)
    {
        var subnet = new string('1', cidrBits) + new string('0', 32 - cidrBits);
        var bytes = Enumerable.Range(0, 4)
            .Select(i => subnet.Substring(8 * i, 8))
            .Select(s => Convert.ToByte(s, 2))
            .ToArray();

        return bytes;
    }

    public int Bits { get; }
    public IPAddress Ip { get; }

    public override string ToString()
    {
        return $"{Ip}";
    }

    public string ToCidrBitsString()
    {
        return $"{Bits}";
    }

    public byte[] GetAddressBytes()
    {
        return Ip.GetAddressBytes();
    }

    /// <summary>
    /// Remember! The wildcard mask is the inverse of the subnet mask.
    /// </summary>
    /// <returns>The byte array of the wildcard mask.</returns>
    public byte[] GetWildcardAddressBytes()
    {
        return Ip.GetAddressBytes().Select(octet => octet ^ 255).Select(Convert.ToByte).ToArray();
    }
};
