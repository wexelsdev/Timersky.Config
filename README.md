# Timersky.Config
### Yet another cfg on toml

## Installation - [Nuget](https://www.nuget.org/packages/Timersky.Config)
```
dotnet add package Timersky.Config --version 1.0.6
```

## Usage
```csharp
using Timersky.Config;
using Timersky.Log;
using Tomlet.Attributes;

namespace SomeNamespace;

public class Program
{
    private static readonly Config Config = ConfigManager.LoadConfig<Config>();
    private static readonly Logger Log = new(); // not necessarily
    
    public static void Main(string[] args)
    {
        Log.Info(Config.SomeProperty);
    }
}

public class Config : IConfig
{
    [TomlProperty("debug")] public bool Debug { get; set; } = false;
    [TomlProperty("some_property")] public string SomeProperty { get; set; } = "Hello World!";
}
```
