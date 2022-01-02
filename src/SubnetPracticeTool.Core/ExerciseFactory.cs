using System.Net;

namespace SubnetPracticeTool.Core;

public class ExerciseFactory
{
    private readonly Random _rng;

    public ExerciseFactory(Random rng)
    {
        _rng = rng;
    }

    public (IPAddress, SubnetMask) GetRandomExercise() =>
        (GenerateRandomIp(), GenerateRandomSubnetMask());

    private IPAddress GenerateRandomIp()
    {
        var octets = new byte[4];
        _rng.NextBytes(octets);
        if (octets[0] == 0) octets[0] = 0b1;
        return new IPAddress(octets);
    }

    private SubnetMask GenerateRandomSubnetMask()
    {
        return new SubnetMask(_rng.Next(1, 31));
    }
}
