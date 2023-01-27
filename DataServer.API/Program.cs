
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
    {
        AuthenticationConfiguration AuthenticationConfiguration = new AuthenticationConfiguration();

        o.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationConfiguration.AccessTokenSecret)),
            ValidIssuer=AuthenticationConfiguration.Issuer,
            ValidAudience=AuthenticationConfiguration.Audience,
            ValidateIssuerSigningKey= true,
            ValidateAudience = true,
            ValidateIssuer = true
        };
    }); 
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

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public class AuthenticationConfiguration
{
    public string AccessTokenSecret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
