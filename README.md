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

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   bashCopy codedotnet add package Json.Protector   `

Configuration
-------------

### Using Newtonsoft.Json

To configure Json.Protector with **Newtonsoft.Json**, add the following to your Program.cs file:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   csharpCopy codebuilder.Services.AddSingleton();  builder.Services.AddControllers()      .AddNewtonsoftJson(options =>      {          options.SerializerSettings.Converters.Add(              builder.Services.BuildServiceProvider().GetRequiredService()          );      });   `

### Using System.Text.Json

To configure Json.Protector with **System.Text.Json**, use the following code:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   csharpCopy codebuilder.Services.AddSingleton>(sp =>      new SystemTextJsonJsonProtectorTypeConverter(sp.GetRequiredService())  );  builder.Services.AddControllers()      .AddJsonOptions(options =>      {          options.JsonSerializerOptions.Converters.Add(              builder.Services.BuildServiceProvider().GetRequiredService>()          );      });   `

Usage
-----

To encrypt and protect your sensitive data, use the JsonProtectorType.This type is equivalent to a string and supports text data.

Here’s an example:

Plain textANTLR4BashCC#CSSCoffeeScriptCMakeDartDjangoDockerEJSErlangGitGoGraphQLGroovyHTMLJavaJavaScriptJSONJSXKotlinLaTeXLessLuaMakefileMarkdownMATLABMarkupObjective-CPerlPHPPowerShell.propertiesProtocol BuffersPythonRRubySass (Sass)Sass (Scss)SchemeSQLShellSwiftSVGTSXTypeScriptWebAssemblyYAMLXML`   csharpCopy codepublic class UserProfile  {      public string Name { get; set; }      public JsonProtectorType SensitiveInfo { get; set; }  }  // Example Data  var profile = new UserProfile  {      Name = "John Doe",      SensitiveInfo = "This is encrypted data"  };   `

When serialized, SensitiveInfo will be automatically encrypted. Upon deserialization, it will be decrypted back to its original value.

Contributing
------------

Contributions are welcome! Feel free to submit issues or pull requests to improve this library.
