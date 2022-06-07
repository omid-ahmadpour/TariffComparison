using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TariffComparison.Common.General
{
    public static class EnumExtension
    {
        public static string ToDisplay(this Enum value, DisplayProperty property = DisplayProperty.Name)
        {
            if (value is null)
            {
                throw new ArgumentNullException();
            }

            DisplayAttribute displayAttribute = value.GetType()
                .GetField(value.ToString())?.GetCustomAttributes<DisplayAttribute>(inherit: false)
                .FirstOrDefault();

            if (displayAttribute == null)
                return value.ToString();

            return displayAttribute.GetType().GetProperty(property.ToString())?.GetValue(displayAttribute, null)?.ToString();
        }

        public enum DisplayProperty
        {
            Description,
            Name
        }
    }
}
