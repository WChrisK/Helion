namespace Helion.Util.SerializationContexts
{
    using Helion.Maps.Shared;
    using Helion.Models;
    using System.Text.Json.Serialization;

    [JsonSourceGenerationOptions(
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault,
        PropertyNameCaseInsensitive = true,
        IncludeFields = true)]
    [JsonSerializable(typeof(DemoModel), TypeInfoPropertyName = "DemoModel")]
    [JsonSerializable(typeof(SkillLevel), TypeInfoPropertyName = "SkillLevel")]
    public partial class DemoModelSerializationContext : JsonSerializerContext
    {
    }
}
