using Ambev.DeveloperEvaluation.Application.Sales.CommonDTO;
using Ambev.DeveloperEvaluation.Application.Services.Interface;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Functional.Services;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Ambev.DeveloperEvaluation.Functional.Utilities;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public Mock<ICustomerInfoService> CustomerInfoMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            ConfigureCustomerInfoMock();
            services.RemoveAll<ICustomerInfoService>();
            services.AddSingleton(CustomerInfoMock.Object);

            services.RemoveAll<IAuthenticationService>();
            services.RemoveAll<IAuthenticationHandlerProvider>();
            services.RemoveAll<IAuthenticationSchemeProvider>();
            services.RemoveAll<IConfigureOptions<AuthenticationOptions>>();
            services.RemoveAll<IConfigureOptions<JwtBearerOptions>>();
            services.RemoveAll<IUnitOfWork>();

            services.AddSingleton<IUnitOfWork, MockUnitOfWork>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.TestScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.TestScheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.TestScheme, _ => { });

            RemoveService<DbContextOptions<DefaultContext>>(services);
            services.AddDbContext<DefaultContext>(options =>
            {
                options.UseInMemoryDatabase("TestDB");
                options.EnableDetailedErrors();
                options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                options.EnableSensitiveDataLogging();
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            db.Database.EnsureCreated();
        });

        builder.UseEnvironment("Development");
    }

    private void ConfigureCustomerInfoMock()
    {
        CustomerInfoMock
            .Setup(x => x.GetCustomerInfoAsync(Guid.Parse(TestAuthHandler.TestUserId)))
            .ReturnsAsync(new CustomerInfoDto(
                CustomerId: Guid.Parse(TestAuthHandler.TestUserId),
                UserName: TestAuthHandler.TestUsername,
                Email: "test@ambev.com.br",
                Phone: "15999999999",
                Category: "0"));
    }

    private void RemoveService<T>(IServiceCollection services)
    {
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(T));
        if (descriptor != null) services.Remove(descriptor);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddLogging();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
        });

        return base.CreateHost(builder);
    }

    public IAuthenticationService GetAuthService()
    {
        return Services.GetRequiredService<IAuthenticationService>();
    }

    public IServiceScope CreateScope()
    {
        return Services.CreateScope();
    }
}
