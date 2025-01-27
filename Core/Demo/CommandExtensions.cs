using Helion.World.Entities.Players;

namespace Helion.Demo;

public static class CommandExtensions
{
    public static DemoTickCommands ToDemoTickCommand(this TickCommands cmd)
    {
        return cmd switch
        {
            TickCommands.Attack => DemoTickCommands.Attack,
            TickCommands.Jump => DemoTickCommands.Jump,
            TickCommands.Crouch => DemoTickCommands.Crouch,
            TickCommands.Use => DemoTickCommands.Use,
            TickCommands.Strafe => DemoTickCommands.Strafe,
            TickCommands.NextWeapon => DemoTickCommands.NextWeapon,
            TickCommands.PreviousWeapon => DemoTickCommands.PreviousWeapon,
            TickCommands.WeaponSlot1 => DemoTickCommands.WeaponSlot1,
            TickCommands.WeaponSlot2 => DemoTickCommands.WeaponSlot2,
            TickCommands.WeaponSlot3 => DemoTickCommands.WeaponSlot3,
            TickCommands.WeaponSlot4 => DemoTickCommands.WeaponSlot4,
            TickCommands.WeaponSlot5 => DemoTickCommands.WeaponSlot5,
            TickCommands.WeaponSlot6 => DemoTickCommands.WeaponSlot6,
            TickCommands.WeaponSlot7 => DemoTickCommands.WeaponSlot7,
            TickCommands.WeaponGroup1 => DemoTickCommands.WeaponGroup1,
            TickCommands.WeaponGroup2 => DemoTickCommands.WeaponGroup2,
            TickCommands.WeaponGroup3 => DemoTickCommands.WeaponGroup3,
            TickCommands.WeaponGroup4 => DemoTickCommands.WeaponGroup4,
            TickCommands.CenterView => DemoTickCommands.CenterView,
            _ => DemoTickCommands.None,
        };
    }

    public static TickCommands ToTickCommand(this DemoTickCommands cmd)
    {
        return cmd switch
        {
            DemoTickCommands.Attack => TickCommands.Attack,
            DemoTickCommands.Jump => TickCommands.Jump,
            DemoTickCommands.Crouch => TickCommands.Crouch,
            DemoTickCommands.Use => TickCommands.Use,
            DemoTickCommands.Strafe => TickCommands.Strafe,
            DemoTickCommands.NextWeapon => TickCommands.NextWeapon,
            DemoTickCommands.PreviousWeapon => TickCommands.PreviousWeapon,
            DemoTickCommands.WeaponSlot1 => TickCommands.WeaponSlot1,
            DemoTickCommands.WeaponSlot2 => TickCommands.WeaponSlot2,
            DemoTickCommands.WeaponSlot3 => TickCommands.WeaponSlot3,
            DemoTickCommands.WeaponSlot4 => TickCommands.WeaponSlot4,
            DemoTickCommands.WeaponSlot5 => TickCommands.WeaponSlot5,
            DemoTickCommands.WeaponSlot6 => TickCommands.WeaponSlot6,
            DemoTickCommands.WeaponSlot7 => TickCommands.WeaponSlot7,
            DemoTickCommands.WeaponGroup1 => TickCommands.WeaponGroup1,
            DemoTickCommands.WeaponGroup2 => TickCommands.WeaponGroup2,
            DemoTickCommands.WeaponGroup3 => TickCommands.WeaponGroup3,
            DemoTickCommands.WeaponGroup4 => TickCommands.WeaponGroup4,
            DemoTickCommands.CenterView => TickCommands.CenterView,
            _ => TickCommands.None,
        };
    }
}
