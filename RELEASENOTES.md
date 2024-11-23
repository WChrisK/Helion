# 0.9.6.0 (Pre-release)

## Features:
  - Enable trimming (reduces file size of release artifacts)
  - Allow interpolation to finish when game is paused and on level exit for smooth transitions
  - Fade sprites into max distance
  - Autosave timer that automatically writes a quicksave every x seconds
  - Frustum culling for sprites

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
  - Fix issues with automap line marker that was marking lines as seen that were incorrect
  - Fix colormaps for transfer heights not setting on two-sided lines