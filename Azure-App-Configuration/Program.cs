using Azure_App_Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("appConfiguration");
builder.Configuration.AddAzureAppConfiguration(option =>
{
    option.Connect(connectionString);
    option.ConfigureRefresh(refresh =>
    {
        refresh.Register("refreshAll", true)
            .SetCacheExpiration(TimeSpan.FromSeconds(5));
    });
});

builder.Services.Configure<WeatherConfiguration>(builder.Configuration.GetSection("weather"));
builder.Services.AddAzureAppConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAzureAppConfiguration();

app.UseAuthorization();

app.MapControllers();

app.Run();
