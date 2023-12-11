namespace Tenjin.Models.Random;

/// <summary>
/// The collection of parameters to be used for random generation.
/// </summary>
public record RandomGenerationParameters
{
    /// <summary>
    /// The seed to be used.
    /// </summary>
    /// <remarks>
    /// When this value is not set, it will use a random seed.
    /// </remarks>
    public int? Seed { get; init; }

    /// <summary>
    /// The minimum double value to be generated, when generating random double values.
    /// </summary>
    public double? MinimumDouble { get; init; }

    /// <summary>
    /// The maximum double value to be generated, when generating random double values.
    /// </summary>
    public double? MaximumDouble { get; init; }

    /// <summary>
    /// The minimum int-32 value to be generated, when generating random integer values.
    /// </summary>
    public int? MinimumInt32 { get; init; }

    /// <summary>
    /// The maximum int-32 value to be generated, when generating random integer values.
    /// </summary>
    public int? MaximumInt32 { get; init; }

    /// <summary>
    /// The minimum length to be used for the random generation of strings.
    /// </summary>
    public uint? MinimumLength { get; init; }

    /// <summary>
    /// The maximum length to be used for the random generation of strings.
    /// </summary>
    public uint? MaximumLength { get; init; }

    /// <summary>
    /// The fixed length to be used for the random generation of strings.
    /// </summary>
    public uint? Length { get; init; }

    /// <summary>
    /// The allowed characters to be used for the random generation of strings.
    /// </summary>
    public string AllowedRandomCharacters { get; init; } = string.Empty;

    /// <summary>
    /// The Random instance that should be used.
    /// </summary>
    public System.Random? Random { get; set; }
}