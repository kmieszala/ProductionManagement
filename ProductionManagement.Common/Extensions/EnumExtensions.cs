namespace ProductionManagement.Common.Extensions
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            if (enumValue != null)
            {
                var enumMember = enumValue.GetType().GetMember(enumValue.ToString()).First();
                return enumMember.GetCustomAttribute<DisplayAttribute>() != null ? enumMember.GetCustomAttribute<DisplayAttribute>().Name : enumMember.Name;
            }

            return null;
        }

        public static string GetDisplayDescription(this System.Enum enumValue)
        {
            var enumMember = enumValue.GetType().GetMember(enumValue.ToString()).First();
            return enumMember.GetCustomAttribute<DisplayAttribute>() != null ? enumMember.GetCustomAttribute<DisplayAttribute>().Description : enumMember.Name;
        }
    }
}
