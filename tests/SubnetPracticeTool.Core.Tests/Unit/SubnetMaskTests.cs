using System.Net;

using Xunit;

namespace SubnetPracticeTool.Core.Tests.Unit;

public class SubnetMaskTests
{
    [Fact]
    public void ShouldConstructCorrectly()
    {
        var sm = new SubnetMask(24);
        Assert.Equal(new IPAddress(new byte[] { 255, 255, 255, 0 }), sm.Ip);
        Assert.Equal(24, sm.Bits);
    }

    [Fact]
    public void HasAStringRepresentationWithTheIp()
    {
        var sm = new SubnetMask(24);
        Assert.Equal("255.255.255.0", sm.ToString());
    }

    [Fact]
    public void HasAStringRepresentationWithTheCidrNotation()
    {
        var sm = new SubnetMask(24);
        Assert.Equal("24", sm.ToCidrBitsString());
    }

    [Fact]
    public void EqualityWorks()
    {
        var sm = new SubnetMask(24);
        var sm2 = new SubnetMask(24);
        Assert.Equal(sm, sm2);
    }

    [Fact]
    public void CanReturnAddressBytesLikeAnIp()
    {
        var sm = new SubnetMask(24);
        var expectedByteArray = new byte[] { 255, 255, 255, 0 };
        Assert.Equal(expectedByteArray, sm.GetAddressBytes());
    }

    [Fact]
    public void CanBeRepresentedAsAWildcard()
    {
        var sm = new SubnetMask(24);
        var expectedByteArray = new byte[] { 0, 0, 0, 255 };
        Assert.Equal(expectedByteArray, sm.GetWildcardAddressBytes());
    }
}
