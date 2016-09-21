using PaysonIntegrationCO2.Models.Enums;

namespace PaysonIntegrationCO2.Models
{
    public class Gui
    {
        public ColorScheme ColorScheme { get; set; }

        public string Locale { get; set; }

        public bool RequestPhone { get; set; }

        public bool PhoneOptional { get; set; }
    }

}