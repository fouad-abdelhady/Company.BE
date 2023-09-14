using Company.BL.Managers.AuthManagers;
using Company.BL.Managers.StaffManagers;
using Company.DAL;
using Company.DAL.Data.Context;
using Company.PL.Filter;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var appOrigin = "Company App Origins";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {
    options.AddPolicy(name:appOrigin, policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});
//builder.Services.AddScoped<ManagerAuth>();
#region DataBase
var DbConStr = builder.Configuration.GetConnectionString("DbConStr");
Console.WriteLine(DbConStr);
builder.Services.AddDbContext<CompanyContext>(options 
    => options.UseSqlServer(DbConStr));
#endregion
#region Repos
builder.Services.AddScoped<IStaffRepo, StaffRepo>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
#endregion
#region Managers
builder.Services.AddScoped<IStaffManager, StaffManager>();
builder.Services.AddScoped<IAuthManager, AuthManager>();

#endregion
#region Authentication

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = "basicAuth";
    options.DefaultChallengeScheme = "basicAuth";
}).AddJwtBearer("basicAuth", options => {
    var secretKeyString = builder.Configuration["accesstoken"] ?? "";
    var secretKeyInBytes = Encoding.ASCII.GetBytes(secretKeyString);
    var secretKey = new SymmetricSecurityKey(secretKeyInBytes);

    options.TokenValidationParameters = new TokenValidationParameters {
        IssuerSigningKey=secretKey,
        ValidateIssuer=false,
        ValidateAudience=false
    };
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.UseAuthentication();

app.UseCors(appOrigin);

app.UseAuthorization();

app.MapControllers();

app.Run();
