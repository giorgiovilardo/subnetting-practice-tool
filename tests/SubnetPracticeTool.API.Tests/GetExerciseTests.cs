using System.Net.Http;
using System.Net.Http.Json;

using SubnetPracticeTool.API.GetExercise;

using Xunit;

namespace SubnetPracticeTool.API.Tests;

public class GetExerciseTests : IClassFixture<WebApiFactory>
{
    private readonly HttpClient _client;

    public GetExerciseTests(WebApiFactory factory) => _client = factory.Client;

    [Fact]
    public async void ShouldReturnAnIpAddress()
    {
        var apiResponse = await _client.GetFromJsonAsync<GetExerciseQuery>("/exercise");
        Assert.Matches(@"\d{1,3}", apiResponse!.IpAddress);
    }

    [Fact]
    public async void ShouldReturnACidrSubnetMask()
    {
        var apiResponse = await _client.GetFromJsonAsync<GetExerciseQuery>("/exercise");
        Assert.Matches(@"\d{1,2}", apiResponse!.SubnetMask);
    }
}
