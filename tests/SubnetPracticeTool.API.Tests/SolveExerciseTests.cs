using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using SubnetPracticeTool.API.SolveExercise;

using Xunit;

namespace SubnetPracticeTool.API.Tests;

public class SolveExerciseTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;

    public SolveExerciseTests(WebApiFactory factory) => _client = factory.Client;

    [Fact]
    public async void ResponseWithAValidSolution()
    {
        var (apiResponse, responseContent) = await PostA(ValidSolution());
        
        Assert.True(apiResponse.IsSuccessStatusCode);
        Assert.True(responseContent is { IsValid: true });
    }

    [Fact]
    public async void ResponseWithAnInvalidNetworkAddressSolution()
    {
        var (apiResponse, responseContent) = await PostA(InvalidNetworkAddressSolution());
        
        Assert.False(apiResponse.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
        Assert.Single(responseContent.Details!);
        Assert.Equal("networkAddress", responseContent.Details!.First().WrongSolutionField);
        Assert.Equal("192.168.0.44 is not the network address for 192.168.0.1/24", responseContent.Details!.First().Message);
        Assert.True(responseContent is { IsValid: false });
    }
    
    [Fact]
    public async void ResponseWithAnInvalidBroadcastAddressSolution()
    {
        var (apiResponse, responseContent) = await PostA(InvalidBroadcastAddressSolution());
        
        Assert.False(apiResponse.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
        Assert.Single(responseContent.Details!);
        Assert.Equal("broadcastAddress", responseContent.Details!.First().WrongSolutionField);
        Assert.Equal("192.168.0.44 is not the broadcast address for 192.168.0.1/24", responseContent.Details!.First().Message);
        Assert.True(responseContent is { IsValid: false });
    }
    
    [Fact]
    public async void ResponseWithAnInvalidNetworkAndBroadcastAddressSolution()
    {
        var (apiResponse, responseContent) = await PostA(TotallyInvalidSolution());
        var firstError = responseContent.Details!.ElementAt(0);
        var secondError = responseContent.Details!.ElementAt(1);
        
        Assert.False(apiResponse.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
        Assert.Equal(2, responseContent.Details!.Count());
        Assert.Equal("networkAddress", firstError.WrongSolutionField);
        Assert.Equal("192.168.0.55 is not the network address for 192.168.0.1/24", firstError.Message);
        Assert.Equal("broadcastAddress", secondError.WrongSolutionField);
        Assert.Equal("192.168.0.44 is not the broadcast address for 192.168.0.1/24", secondError.Message);
        Assert.True(responseContent is { IsValid: false });
    }

    private async Task<(HttpResponseMessage apiResponse, SolveExerciseCommandResponse responseContent)>
        PostA(
            SolveExerciseCommand solution)
    {
        var apiResponse = await _client.PostAsJsonAsync("/solution", solution);
        var responseContent = (await apiResponse.Content.ReadFromJsonAsync<SolveExerciseCommandResponse>())!;
        return (apiResponse, responseContent);
    }

    private SolveExerciseCommand ValidSolution() =>
        new("192.168.0.1", "24", "192.168.0.0", "192.168.0.255");

    private SolveExerciseCommand InvalidNetworkAddressSolution() =>
        ValidSolution() with { NetworkAddress = "192.168.0.44" };

    private SolveExerciseCommand InvalidBroadcastAddressSolution() =>
        ValidSolution() with { BroadcastAddress = "192.168.0.44" };

    private SolveExerciseCommand TotallyInvalidSolution() =>
        ValidSolution() with { BroadcastAddress = "192.168.0.44", NetworkAddress = "192.168.0.55" };
}
