using FloraEdu.Application.Authentication.Implementations;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Application.Services.Implementations;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.Authorization;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Validators;
using FloraEdu.Persistence;
using FloraEdu.Web.Middlewares;
using FloraEdu.Web.Options;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

var connectionString = config.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddCors();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

builder.Services
    .AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthorizationPolicies.Authenticated, policy => policy.RequireAuthenticatedUser())
    .AddPolicy(AuthorizationPolicies.Admin, policy => policy.RequireRole(Roles.Admin))
    .AddPolicy(AuthorizationPolicies.Specialist, policy => policy.RequireRole(Roles.Specialist))
    .AddPolicy(AuthorizationPolicies.AdminOrSpecialist, policy => policy.RequireRole(Roles.Admin, Roles.Specialist))
    .AddPolicy(AuthorizationPolicies.RegularUser, policy => policy.RequireRole(Roles.RegularUser));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

// Add services to the container.
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPlantService, PlantService>();

// Register validators
builder.Services.AddScoped<IValidator<PlantCreateOrUpdateDto>, PlantDtoValidator>();

builder.Services.AddTransient<IUserService, UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UsePathBase(new PathString("/api"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseGlobalExceptionHandler();

app.MapControllers();

app.Run();
