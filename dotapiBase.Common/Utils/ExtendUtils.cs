

namespace dotapiBase.Common.Utils
{

    public static class TypeHelper
    {
        public static string? ToNameString(this Enum eff)
        {
            return Enum.GetName(eff.GetType(), eff);
        }
    }
}
