namespace SubnetPracticeTool.API.SolveExercise;

public record SolveExerciseCommandResponse(bool IsValid, IEnumerable<WrongSolutionItem>? Details = null);
