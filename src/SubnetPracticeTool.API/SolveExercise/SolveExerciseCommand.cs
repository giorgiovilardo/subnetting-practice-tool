namespace SubnetPracticeTool.API.SolveExercise;

public record SolveExerciseCommand(
    string IpAddress,
    string SubnetBits,
    string NetworkAddress,
    string BroadcastAddress
    );
