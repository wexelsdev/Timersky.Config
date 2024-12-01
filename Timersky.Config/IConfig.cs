using Tomlet.Attributes;

namespace Timersky.Config;

/// <summary>
/// Represents a configuration structure with a debug property, 
/// allowing it to be mapped from a TOML file.
/// </summary>
public interface IConfig
{
    /// <summary>
    /// Gets or sets a value indicating whether debugging is enabled.
    /// </summary>
    [TomlProperty("debug")]
    public bool Debug { get; set; }
}
