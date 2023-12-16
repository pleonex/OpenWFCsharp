﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts => {
#pragma warning disable S1075 // URIs should not be hardcoded
    opts.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1",
        Title = "OpenWFC#",
        Description = "Open alternative to DS WFC service",
        Contact = new OpenApiContact {
            Name = "pleonex",
            Url = new Uri("https://github.com/pleonex/OpenWFCsharp"),
        },
        License = new OpenApiLicense {
            Name = "MIT license",
            Url = new Uri("https://github.com/pleonex/OpenWFCsharp/blob/main/LICENSE"),
        },
    });
#pragma warning restore S1075

    var xmlDocs = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlDocs));

    // DocFx requires operationIds. They need to be unique across full API.
    opts.CustomOperationIds(apiDesc => {
        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
