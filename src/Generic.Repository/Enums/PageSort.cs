using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Generic.Repository.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PageSort
    {
        [EnumMember(Value = nameof(ASC))]
        ASC,

        [EnumMember(Value = nameof(DESC))]
        DESC
    }
}