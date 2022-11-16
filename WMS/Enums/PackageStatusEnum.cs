using System.Runtime.Serialization;

namespace WMS.Enums
{
    public enum PackageStatusEnum
    {
        [EnumMember(Value = "Out of warehouse")]
        OutOfWarehouse,
        Delivered,
        Sent,
        [EnumMember(Value = "Placed in warehouse")]
        PlacedInWarehouse

    }
}
