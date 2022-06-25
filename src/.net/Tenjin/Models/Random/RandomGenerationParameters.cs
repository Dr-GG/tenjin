namespace Tenjin.Models.Random
{
    public record RandomGenerationParameters
    {
        public int? Seed { get; init; }
        public double? MinimumDouble { get; init; }
        public double? MaximumDouble { get; init; }
        public uint? MinimumLength { get; init; }
        public uint? MaximumLength { get; init; }
        public uint? Length { get; init; }
        public string AllowedRandomCharacters { get; init; } = string.Empty;
    }
}
