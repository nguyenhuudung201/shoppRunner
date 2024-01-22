using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopRunner.DatabaseContexts;
using System.Text;
using Microsoft.Extensions.FileProviders;
using ShopRunner.Entities;
using ShopRunner.Repositories;
using ShopRunner.Services.TokenService;
using ShopRunner.Services.OrderService;
using ShopRunner.Services.AccountService;
using ShopRunner.Services.UserSetvice;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        );
});

builder.Services
    .AddControllers()
    .AddJsonOptions(configure =>
    {
        configure.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        
    });
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<ShopContext>(opts =>
    opts.UseSqlServer(builder.Configuration["ConnectionString:UserDB"]));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IOrderService, OrderServicecs>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
builder.Services.AddScoped<IGenericRepository<Size>, GenericRepository<Size>>();
builder.Services.AddScoped<IGenericRepository<Color>, GenericRepository<Color>>();
builder.Services.AddScoped<IGenericRepository<Order>, GenericRepository<Order>>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
/*app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});*/
app.UseCors("MyPolicy");
app.UseStaticFiles();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
