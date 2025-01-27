using Helion.Util.Config.Components;
using Helion.Util.Configs.Impl;
using Helion.Util.Configs.Options;
using Helion.Util.Configs.Values;
using Helion.World.Entities.Players;
using static Helion.Util.Configs.Values.ConfigFilters;

namespace Helion.Util.Configs.Components;

public class ConfigPlayer: ConfigElement<ConfigPlayer>
{
    [ConfigInfo("Name of the player.")]
    [OptionMenu(OptionSectionType.Player, "Player Name", spacer: true)]
    public readonly ConfigValue<string> Name = new("Player", IfEmptyDefaultTo("Player"));

    [ConfigInfo("Gender of the player.")]
    [OptionMenu(OptionSectionType.Player, "Player Gender")]
    public readonly ConfigValue<PlayerGender> Gender = new(default, OnlyValidEnums<PlayerGender>());

    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "Group 1 1st", spacer: true)]
    public readonly ConfigValue<ConfigWeaponSlots> Group1Weapon1 = new(ConfigWeaponSlots.ShotgunOrSuperShotgun, OnlyValidEnums<ConfigWeaponSlots>());
    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "        2nd")]
    public readonly ConfigValue<ConfigWeaponSlots> Group1Weapon2 = new(ConfigWeaponSlots.ShotgunOrSuperShotgun, OnlyValidEnums<ConfigWeaponSlots>());
    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "        3rd")]
    public readonly ConfigValue<ConfigWeaponSlots> Group1Weapon3 = new(ConfigWeaponSlots.None, OnlyValidEnums<ConfigWeaponSlots>());

    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "Group 2 1st", spacer: true)]
    public readonly ConfigValue<ConfigWeaponSlots> Group2Weapon1 = new(ConfigWeaponSlots.RocketLauncher, OnlyValidEnums<ConfigWeaponSlots>());
    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "        2nd")]
    public readonly ConfigValue<ConfigWeaponSlots> Group2Weapon2 = new(ConfigWeaponSlots.Melee, OnlyValidEnums<ConfigWeaponSlots>());
    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "        3rd")]
    public readonly ConfigValue<ConfigWeaponSlots> Group2Weapon3 = new(ConfigWeaponSlots.Melee, OnlyValidEnums<ConfigWeaponSlots>());

    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "Group 3 1st", spacer: true)]
    public readonly ConfigValue<ConfigWeaponSlots> Group3Weapon1 = new(ConfigWeaponSlots.PlasmaRifle, OnlyValidEnums<ConfigWeaponSlots>());
    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "        2nd")]
    public readonly ConfigValue<ConfigWeaponSlots> Group3Weapon2 = new(ConfigWeaponSlots.BFG9000, OnlyValidEnums<ConfigWeaponSlots>());
    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "        3rd")]
    public readonly ConfigValue<ConfigWeaponSlots> Group3Weapon3 = new(ConfigWeaponSlots.None, OnlyValidEnums<ConfigWeaponSlots>());

    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "Group 4 1st", spacer: true)]
    public readonly ConfigValue<ConfigWeaponSlots> Group4Weapon1 = new(ConfigWeaponSlots.Chaingun, OnlyValidEnums<ConfigWeaponSlots>());
    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "        2nd")]
    public readonly ConfigValue<ConfigWeaponSlots> Group4Weapon2 = new(ConfigWeaponSlots.Pistol, OnlyValidEnums<ConfigWeaponSlots>());
    [ConfigInfo("")]
    [OptionMenu(OptionSectionType.Player, "        3rd")]
    public readonly ConfigValue<ConfigWeaponSlots> Group4Weapon3 = new(ConfigWeaponSlots.None, OnlyValidEnums<ConfigWeaponSlots>());
}
