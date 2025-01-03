using Json.Protector;
using Json.Protector.Converter;
using Json.Protector.Interfaces;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var useDefaultKey = true;

if (useDefaultKey)
{
    builder.Services.AddJsonProtector();
}
else
{
    builder.Services.AddJsonProtector(options =>
    {
        options.UseDefaultKey = false;
        options.Key = "Your Key-wffJGGHG#wrwfsCsddDDFgD$#@";
        options.IV = "Your Iv-eF3RFfdgdsE";

    });
}

var useNewtonSoft = true;
if (useNewtonSoft)
{
    #region newtonSoft
    builder.Services.AddSingleton<NewtonsoftJsonProtectorTypeConverter>();
    builder.Services.AddSingleton<NewtonsoftDataProtector>();

    builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            using var serviceProvider = builder.Services.BuildServiceProvider();
            
            // Register the converter with dependency injection
            options.SerializerSettings.Converters.Add(serviceProvider.GetRequiredService<NewtonsoftJsonProtectorTypeConverter>());
            serviceProvider.GetRequiredService<NewtonsoftDataProtector>();

        });
    #endregion
}
else
{

    builder.Services.AddSingleton<JsonConverter<JsonProtectorType>>(sp => new SystemTextJsonJsonProtectorTypeConverter(sp.GetRequiredService<IEncryptionProvider>()));

    builder.Services.AddControllers().AddJsonOptions(options =>
        {
            using var serviceProvider = builder.Services.BuildServiceProvider();

            // Register the converter with dependency injection
            options.JsonSerializerOptions.Converters.Add(serviceProvider.GetRequiredService<JsonConverter<JsonProtectorType>>());

        });
}
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

app.UseAuthorization();

app.MapControllers();

app.Run();
