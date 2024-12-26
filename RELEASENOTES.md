# 0.9.6.0 (Pre-release)

## Features:
  - Enable trimming (reduces file size of release artifacts)
  - Allow interpolation to finish when game is paused and on level exit for smooth transitions
  - Fade sprites into max distance
  - Autosave timer that automatically writes a quicksave every x seconds
  - Frustum culling for the automap
  - Controller gyroscope support (tested with PS DualShock 4, should work with other SDL-supported controllers)
  - Emulate boom behavior that let player move out of one-sided lines
  - Controller rumble feedback
  - Add option for berserk intensity
  - Add option to disable crosshair shrinking on target
  - Add use command to allow for separate bindings for shotgun/super shotgun [bind x "use shotgun"] [bind y "use supershotgun"]
  - Add custom weapon groups similar to the ones found in Woof!, with the added bonus of being assignable to any button.
  - Added order independent transparency to fix issues with multiple transparent sprites causing transparent textures not to render
  - Support texture filtering with sprites
  - Refraction effect for spectres
  - Added nearest to bilinear and nearest to trilinear texture filter options

## Bug fixes:
  - Fix per ammo values for box ammo and backpack amount from dehacked patch
  - Fix crash from bad colormaps (Fixes Necromantic Thirst)
  - Fix sight and shoot traverse functions to correctly handle resizes (Fixes Junkfood MAP98)
  - Fix A_WeaponSound not functioning when called from the flash state (Fixes Junkfood pistol firing sound)
  - Fix block player lines blocking friendly monsters (Fixes TWOGERS MAP31)
  - Fix generalized crushers to not slow down with fast and turbo speeds to match boom behavior
  - Fix two-sided lines drawing in the automap when back and front sector ceiling and floor values match
  - Fix allocation issue with automap rendering that could cause slowdown
  - Fix friendly enemies in closets not setting target to player even when not in sight
  - Fix monsters teleporting from monster closets playing sight sound
  - Fix translation flags using incorrect colormap indices (fixes kdikdizd)
  - Fix colormaps for transfer heights not setting on two-sided lines
  - Fix cases where lines were being marked for automap when they weren't visible
  - Fix sprite x offset rendering (fixes small red torch and burning barrel twitching)
  - Fix issue with bump use breaking on maps with voodoo dolls
  - Fix issue with emulating vanilla behavior that didn't clear velocity when stuck in walls causing issues with slide moving code
  - Fix custom map name not taking priority and entertext for changing clusters with mapinfo
  - Fix minimum x/y velocity to match vanilla behavior
  - Added sprites and sounds for mbf helper dog
  - Fix label clear in UMAPINFO
  - Fallback to status bar background texture (borderflat) defined in gameinfo
  - Fix issue with two-sided middle walls and texture filtering causing edge pixels to render incorrectly
  - Fix spawn ceiling being incorrectly applied after initial map start
  - Fix ordering to set flags first when something dies before setting the death state to match original behavior
  - Fix index check on setting death state to match original behavior. Fixes 0x0.wad spawn fall objects not being removed.
  - Fix one-sided walls and flats with null texture rendering (fixes 0x0.wad) 