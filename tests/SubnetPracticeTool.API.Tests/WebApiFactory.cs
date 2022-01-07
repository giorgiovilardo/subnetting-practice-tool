using System;
using System.Net.Http;

using Microsoft.AspNetCore.Mvc.Testing;

namespace SubnetPracticeTool.API.Tests;

public class WebApiFactory : IDisposable
{
    public readonly HttpClient Client;

    public WebApiFactory()
    {
        var webApp = new WebApplicationFactory<Program>();
        Client = webApp.Server.CreateClient();
    }

    public void Dispose()
    {
    }
}
