using System.Runtime.Serialization;

namespace Generic.Repository.Enums
{
    public enum PageSort
    {
        [EnumMember(Value = nameof(ASC))]
        ASC,

        [EnumMember(Value = nameof(DESC))]
        DESC
    }
}