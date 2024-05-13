using System.Reflection;
using Microsoft.OpenApi.Models;
using OpenWFCsharp.Messages.Formatters;
using OpenWFCsharp.Nas;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

ILoggerFactory setupLogFactory = LoggerFactory.Create(config => config
    .SetMinimumLevel(LogLevel.Debug)
    .AddConsole());
builder.Services.AddControllers(opts => {
    opts.OutputFormatters.Insert(0, new DwcOutputFormatter());
    opts.InputFormatters.Insert(
        0,
        new DwcInputFormatter(setupLogFactory.CreateLogger<DwcInputFormatter>()));
});

builder.Services.Configure<NAuthenticationServerOptions>(
    builder.Configuration.GetSection(NAuthenticationServerOptions.OptionName));

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts => {
#pragma warning disable S1075 // URIs should not be hardcoded
    opts.SwaggerDoc("nas", new OpenApiInfo {
        Version = "v1",
        Title = "OpenWFC# - NAS",
        Description = "N Authorization Server",
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/nas/swagger.json", "OpenWFC# - NAS"));
}

app.MapControllers();

app.Run();
