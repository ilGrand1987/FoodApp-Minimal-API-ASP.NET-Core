using FoodApp.DbContexts;
using FoodApp.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FoodApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using FoodApp.Extensions;
using System.Net;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// register the DbContext on the container, getting the
// connection string from appSettings   
builder.Services.AddDbContext<DishesDbContext>(o => o.UseSqlite(
    builder.Configuration["ConnectionStrings:Connection"]));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdminFromItaly", policy =>
        policy
              .RequireRole("Admin")
              .RequireClaim("country", "Italy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("TokenAuth",
        new()
        {
            Name = "Authorization",
            Description = "Token-based authentication and authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            In = ParameterLocation.Header
        });
    options.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "TokenAuth" }
            }, new List<string>() }
    });
});

//to make this policy work, we have to generate a token in cmd passing the command:
//dotnet user-jwts create --audience menu-api --claim country=Italy --role Admin

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    //app.UseExceptionHandler(configureApplicationBuilder =>
    //{
    //    configureApplicationBuilder.Run(
    //        async context =>
    //        {
    //            context.Response.StatusCode = (int)
    //            HttpStatusCode.InternalServerError;
    //            context.Response.ContentType = "text/html";
    //            await context.Response.WriteAsync(
    //                "An unexpected problem happened.");
    //        });
    //});
}

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.RegisterDishesEndpoints();
app.RegisterIngredientsEndpoints();

// recreate & migrate the database on each run, for demo purposes
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<DishesDbContext>();
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

app.Run();

