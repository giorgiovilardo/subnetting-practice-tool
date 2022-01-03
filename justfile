test:
  dotnet test --nologo -v q

format:
  dotnet format -v n

run:
  dotnet run -v q --project src/SubnetPracticeTool.API

hotrun:
  dotnet watch run --project src/SubnetPracticeTool.API -- -v q --project src/SubnetPracticeTool.API
