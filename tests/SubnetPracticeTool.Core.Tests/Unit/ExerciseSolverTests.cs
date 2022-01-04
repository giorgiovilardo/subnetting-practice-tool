using System.Net;

using Xunit;

namespace SubnetPracticeTool.Core.Tests.Unit;

public class ExerciseSolverTests
{
    [Theory]
    [InlineData(new byte[] { 192, 168, 0, 100 }, 24, new byte[] { 192, 168, 0, 0 })]
    [InlineData(new byte[] { 10, 0, 0, 129 }, 25, new byte[] { 10, 0, 0, 128 })]
    public void ShouldReturnTheCorrectNetworkAddress(byte[] ipAddress, int subnetMask, byte[] expectedNetworkAddress)
    {
        var ip = new IPAddress(ipAddress);
        var sm = new SubnetMask(subnetMask);
        var expectedNetworkIp = new IPAddress(expectedNetworkAddress);
        var exerciseSolver = new ExerciseSolver(ip, sm);
        Assert.Equal(expectedNetworkIp, exerciseSolver.GetNetworkAddress());
    }

    [Theory]
    [InlineData("192.168.0.1", "24")]
    [InlineData("192.168.0.1", "22")]
    [InlineData("1.1.1.1", "10")]
    public void ConstructableFromAIpAndSubnetBitsStringPair(string ipAddress, string subnetBits)
    {
        var solver = ExerciseSolver.FromString(ipAddress, subnetBits);
        Assert.NotNull(solver);
    }
    
    [Theory]
    [InlineData(new byte[] { 192, 168, 0, 100 }, 24, new byte[] { 192, 168, 0, 255 })]
    [InlineData(new byte[] { 10, 0, 0, 129 }, 25, new byte[] { 10, 0, 0, 255 })]
    [InlineData(new byte[] { 10, 0, 0, 129 }, 23, new byte[] { 10, 0, 1, 255 })]
    public void ShouldReturnTheCorrectBroadcastAddress(byte[] ipAddress, int subnetMask, byte[] expectedBroadcastAddress)
    {
        var ip = new IPAddress(ipAddress);
        var sm = new SubnetMask(subnetMask);
        var expectedBroadcastIp = new IPAddress(expectedBroadcastAddress);
        var exerciseSolver = new ExerciseSolver(ip, sm);
        Assert.Equal(expectedBroadcastIp, exerciseSolver.GetBroadcastAddress());
    }
}
