using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SyriaTrustPlanning.Api.MiddleWares;
using SyriaTrustPlanning.Api.OptionsSetup;
using SyriaTrustPlanning.Application;
using SyriaTrustPlanning.Infrastructure;
using SyriaTrustPlanning.Persistence;
using System.Net;

try
{


    var builder = WebApplication.CreateBuilder(args);


    // Add services to the container.
    builder.Services.AddPersistenceServices(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddHttpClient();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Syria Trust Planning",
            Version = "v1"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
        }
  });
    });


    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
    builder.Services.ConfigureOptions<JwtOptionsSetup>();
    builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddLogging();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseAuthorization();
    app.UseStaticFiles();
    app.UseAuthentication();

    app.UseCors("Open");

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    var builder = WebApplication.CreateBuilder(args);

    var app = builder.Build();

    app.Use(async (Runcontext, next) =>
    {
        Runcontext.Response.Clear();
        Runcontext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        if (ex.InnerException is not null ? !string.IsNullOrEmpty(ex.InnerException.Message) : false)
            await Runcontext.Response.WriteAsync(JsonConvert.SerializeObject(new { ErrorMessage = ex.Message, ErrorInnerMessage = ex.InnerException.Message }));
        else
            await Runcontext.Response.WriteAsync(JsonConvert.SerializeObject(new { ErrorMessage = ex.Message }));

        await next.Invoke();
    });

    app.Run();
}