using AuthenticationServer.API.Model;
using AuthenticationServer.API.Service.Authenticators;
using AuthenticationServer.API.Service.PasswordHasher;
using AuthenticationServer.API.Service.RefreshTokenRepositories;
using AuthenticationServer.API.Service.TokenGenerators;
using AuthenticationServer.API.Service.TokenValidators;
using AuthenticationServer.API.Service.UserRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddSingleton<AccessTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenValidator>();
builder.Services.AddSingleton<AuthenticationConfiguration>();
builder.Services.AddSingleton<TokenGenerator>();
builder.Services.AddSingleton<Authenticator>();
builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddSingleton<IRefreshTokenRepository, InMemoryRefreshTokenRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseAuthentication();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.MapRazorPages();

app.Run();


