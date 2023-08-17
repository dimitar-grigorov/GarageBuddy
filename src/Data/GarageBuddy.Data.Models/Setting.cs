namespace GarageBuddy.Data.Models
{
    using static GarageBuddy.Common.Constants.EntityValidationConstants.Setting;

    public class Setting : BaseDeletableModel<Guid>
    {
        public Setting()
        {
        }

        public Setting(string name, string value)
        {
            Name = name;
            Value = value;
        }

        [Required]
        [MaxLength(SettingNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(SettingValueMaxLength)]
        public string Value { get; set; } = null!;
    }
}
