using System;
using System.Collections.Generic;
using System.Net;

using Moq;

using Xunit;

namespace SubnetPracticeTool.Core.Tests.Unit;

public class ExerciseFactoryTests
{
    [Theory]
    [MemberData(nameof(GetTestData))]
    public void ProducesCorrectDataAndAutocorrectRandomFirstOctetIfZero(byte[] generatedIpOctets, int cidrBits,
        byte[] expectedIpOctets)
    {
        var expectedIp = new IPAddress(expectedIpOctets);
        var expectedSubnet = new SubnetMask(cidrBits);
        var mockRandomGenerator = MockRandomGeneratorFrom(generatedIpOctets, cidrBits);
        var factory = new ExerciseFactory(mockRandomGenerator.Object);

        (IPAddress generatedIp, SubnetMask generatedSubnet) = factory.GetRandomExercise();

        Assert.Equal(expectedIp, generatedIp);
        Assert.Equal(expectedSubnet, generatedSubnet);
        mockRandomGenerator.Verify(random => random.NextBytes(It.IsAny<byte[]>()));
        mockRandomGenerator.Verify(random => random.Next(1, 31));
    }

    private Mock<Random> MockRandomGeneratorFrom(byte[] ipOctets, int subnetMaskBits)
    {
        var mockRandomGenerator = new Mock<Random>();
        var subnetMask = new SubnetMask(subnetMaskBits);
        mockRandomGenerator.Setup(random => random.NextBytes(It.IsAny<byte[]>()))
            .Callback<byte[]>(byteArray =>
            {
                byteArray[0] = ipOctets[0];
                byteArray[1] = ipOctets[1];
                byteArray[2] = ipOctets[2];
                byteArray[3] = ipOctets[3];
            });
        mockRandomGenerator.Setup(random => random.Next(1, 31)).Returns(subnetMaskBits);
        return mockRandomGenerator;
    }

    private static IEnumerable<object[]> GetTestData() =>
        new List<object[]>
        {
            new object[] { new byte[] { 200, 200, 200, 200 }, 24, new byte[] { 200, 200, 200, 200 } },
            new object[] { new byte[] { 100, 101, 200, 240 }, 24, new byte[] { 100, 101, 200, 240 } },
            new object[] { new byte[] { 100, 0, 0, 0 }, 8, new byte[] { 100, 0, 0, 0 } },
            new object[] { new byte[] { 200, 101, 200, 240 }, 24, new byte[] { 200, 101, 200, 240 } },
            new object[] { new byte[] { 0, 101, 200, 240 }, 16, new byte[] { 1, 101, 200, 240 } },
            new object[] { new byte[] { 0, 0, 0, 0 }, 16, new byte[] { 1, 0, 0, 0 } }
        };
}
