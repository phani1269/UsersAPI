using Redis.OM;
using UsersAPI;
using UsersAPI.Helper;
using UsersAPI.Helper.Interfaces;
using UsersAPI.Models;
using UsersAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        builder.WithOrigins("http://localhost", "http://host.docker.internal")
               .AllowAnyMethod()
               .AllowAnyHeader()
                );
});
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient<ITokenExchangeService, TokenExchangeService>();
// adding the object of HttpResponseMessage to the service
builder.Services.AddScoped<HttpResponseMessage>();
// adding the object of AuthenticationResponseModel to the service
builder.Services.AddScoped<AuthenticationData>();
// class to manage cookie operations
builder.Services.AddScoped<ICookieManager, CookieManager>();
builder.Services.AddHostedService<IndexCreationService>();
builder.Services.AddSingleton(new RedisConnectionProvider(builder.Configuration["REDIS_CONNECTION_STRING"]));
builder.Services.AddHttpClient<ITokenExchangeService, TokenExchangeService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
