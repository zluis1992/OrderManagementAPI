﻿
using Infrastructure.DataSource;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Api.Tests;

class ApiApp : WebApplicationFactory<Program>
{

    readonly Guid _id = Guid.NewGuid();

    public Guid UserId => this._id;

    // We should use this service collection to access repos and seed data for tests
    public IServiceProvider GetServiceCollection()
    {
        return Services;           
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(svc =>
        {
            svc.RemoveAll(typeof(DbContextOptions<DataContext>));
            svc.AddDbContext<DataContext>(opt =>
            {
                opt.UseInMemoryDatabase("testdb");
            });

        });

        return base.CreateHost(builder);
    }

    
}

