using Microsoft.OpenApi.Models;
using OpenWFCsharp.Dls;
using OpenWFCsharp.Dls.Storage;
using OpenWFCsharp.Messages.Formatters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opts => {
    opts.InputFormatters.Insert(0, new DwcInputFormatter());
});

builder.Services.AddSingleton<IContentStorage, FileSystemContentStorage>();

builder.Services.Configure<DownloadServerOptions>(
    builder.Configuration.GetSection(DownloadServerOptions.OptionName));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts => {
#pragma warning disable S1075 // URIs should not be hardcoded
    opts.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1",
        Title = "OpenWFC# - DLS",
        Description = "Download server",
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

app.MapControllers();

app.Run();
