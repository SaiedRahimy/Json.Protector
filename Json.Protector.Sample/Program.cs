using Json.Protector;
using Json.Protector.Converter;
using Json.Protector.Interfaces;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddJsonProtector();

var useNewtonSoft = true;
if (useNewtonSoft)
{
    #region newtonSoft
    builder.Services.AddSingleton<NewtonsoftJsonProtectorTypeConverter>();

    builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(
                builder.Services.BuildServiceProvider().GetRequiredService<NewtonsoftJsonProtectorTypeConverter>()
            );
        });
    #endregion
}
else
{
    builder.Services.AddSingleton<JsonConverter<JsonProtectorType>>(sp =>
    new SystemTextJsonJsonProtectorTypeConverter(sp.GetRequiredService<IEncryptionProvider>())
);
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // Register the converter with dependency injection
            options.JsonSerializerOptions.Converters.Add(
                builder.Services.BuildServiceProvider().GetRequiredService<JsonConverter<JsonProtectorType>>()
            );
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
