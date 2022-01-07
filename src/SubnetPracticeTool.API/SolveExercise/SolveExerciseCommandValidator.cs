using FluentValidation;

namespace SubnetPracticeTool.API.SolveExercise;

public class SolveExerciseCommandValidator : AbstractValidator<SolveExerciseCommand>
{
    public SolveExerciseCommandValidator()
    {
        RuleFor(command => command.IpAddress)
            .NotEmpty().WithMessage("Ip address is required")
            .Must(BeAnIpAddress).WithMessage("Please provide a valid IP address");
        RuleFor(command => command.SubnetBits)
            .NotEmpty().WithMessage("Subnet bits are required")
            .Must(BeInCidrRange).WithMessage("Please provide some valid subnet bits");
        RuleFor(command => command.NetworkAddress)
            .NotEmpty().WithMessage("Network address is required")
            .Must(BeAnIpAddress).WithMessage("Please provide a valid IP address");
        RuleFor(command => command.BroadcastAddress)
            .NotEmpty().WithMessage("Broadcast address is required")
            .Must(BeAnIpAddress).WithMessage("Please provide a valid IP address");
    }

    private bool BeAnIpAddress(string ipAddress) =>
        ipAddress.Split('.').Select(isAValidIpNumber).All(x => x);

    private bool BeInCidrRange(string subnetBits) =>
        int.TryParse(subnetBits, out int i) && i is >= 1 and <= 32;

    private bool isAValidIpNumber(string ipOctet) =>
        int.TryParse(ipOctet, out int i) && i is >= 0 and <= 255;
}
