Json.Protector
==============

**Json.Protector** is a .NET library designed to protect sensitive data during JSON serialization and deserialization. This package supports both Newtonsoft.Json and [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview).

Features
--------

*   Protects sensitive data by encrypting it during serialization.
    
*   Automatically decrypts data during deserialization.
    
*   Provides seamless integration with Newtonsoft.Json and System.Text.Json.
    

Installation
------------

You can install this package via NuGet:

```bash
dotnet add package Json.Protector   
```
Configuration
-------------
### Using Use Default Key
```csharp
builder.Services.AddJsonProtector();
```
### Using Set Encryption Key
```csharp
builder.Services.AddJsonProtector(options =>
{
    options.UseDefaultKey = false;
    options.Key = "Your Key-wffJGGHG#wrwfsCsddDDFgD$#@";
    options.IV = "Your Iv-eF3RFfdgdsE";

});
```

### Using Newtonsoft.Json

To configure Json.Protector with **Newtonsoft.Json**, add the following to your Program.cs file:

```csharp

 builder.Services.AddSingleton<NewtonsoftJsonProtectorTypeConverter>();
 builder.Services.AddSingleton<NewtonsoftDataProtector>();

 builder.Services.AddControllers().AddNewtonsoftJson(options =>
     {
         using var serviceProvider = builder.Services.BuildServiceProvider();
         
         // Register the converter with dependency injection
         options.SerializerSettings.Converters.Add(serviceProvider.GetRequiredService<NewtonsoftJsonProtectorTypeConverter>());
         serviceProvider.GetRequiredService<NewtonsoftDataProtector>();

     });


```
### Using System.Text.Json

To configure Json.Protector with **System.Text.Json**, use the following code:

```csharp
 builder.Services.AddSingleton<JsonConverter<JsonProtectorType>>(sp => new SystemTextJsonJsonProtectorTypeConverter(sp.GetRequiredService<IEncryptionProvider>()));

 builder.Services.AddControllers().AddJsonOptions(options =>
     {
         using var serviceProvider = builder.Services.BuildServiceProvider();

         // Register the converter with dependency injection
         options.JsonSerializerOptions.Converters.Add(serviceProvider.GetRequiredService<JsonConverter<JsonProtectorType>>());

     });


```
Usage
-----

To encrypt and protect your sensitive data, use the JsonProtectorType.This type is equivalent to a string and supports text data.

Here’s an example:

```csharp
public class UserProfile
{
    public string Name { get; set; }
    public JsonProtectorType SensitiveInfo { get; set; }
}

// Example Data
var profile = new UserProfile
{
    Name = "saied rahimi",
    SensitiveInfo = "This is encrypted data"
};


```

or

```csharp
public class UserProfile
{
    public string Name { get; set; }

     [JsonConverter(typeof(NewtonsoftDataProtector))]
    public string SensitiveInfo { get; set; }
}

// Example Data
var profile = new UserProfile
{
    Name = "saied rahimi",
    SensitiveInfo = "This is encrypted data"
};


```


Result
-----
```json
//encrypted result
{
  "name": "saied rahimi",
  "sensitiveInfo": "itNydZU4Y33CCPe/Bp45wOZwuI8CEofwCnO6m5VNhFs="
}

//decrypted result
{
  "name": "saied rahimi",
  "message": "This is encrypted data"
}


```

When serialized, SensitiveInfo will be automatically encrypted. Upon deserialization, it will be decrypted back to its original value.

Contributing
------------

Contributions are welcome! Feel free to submit issues or pull requests to improve this library.
