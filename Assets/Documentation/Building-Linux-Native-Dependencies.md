# Building Linux Native Dependencies
We've made an effort to include appropriate versions of our native dependencies along with Helion.  However, since there are many different Linux-based operating systems, it is not possible for us to provide prebuilt binaries for all of them.  The following steps detail how we built our dependencies on Ubuntu in the Windows Subsystem for Linux, which is a fairly minimal Linux install.  You may find that some of these dependencies are already installed on your system, in which case you may be able to delete the `.so` files we provide.

# Native Dependencies
We directly depend upon, and provide our own copies of:
1. ZMusic: https://github.com/ZDoom/ZMusic
2. SDL2: https://github.com/libsdl-org/SDL
3. FluidSynth: https://github.com/FluidSynth/fluidsynth

Note that the dependencies on ZMusic and FluidSynth imply a fairly extensive set of transitive dependencies, starting with libsndfile and libmpg123.  These are installed by default on most mainstream desktop Linux distributions.

We also depend upon, but do not provide our own copies of:
1. libGLFW -- we distribute a version provided by OpenTK, which we use for windowing, input (except for gamepads--we use SDL2 for that), and OpenGL.
2. OpenAL -- This seems to be installed by default on most desktop Linux distributions.

# Prereqs (WSL Ubuntu 22.04 and 24.04)
```
sudo apt-get install clang
sudo apt-get install cmake
sudo apt-get install pkg-config
sudo apt-get install glib-2.0
sudo apt-get install libsndfile-dev
```

# Make output dir
```
mkdir ~/Helion-libs
```

# Build ZMusic
```
cd ~
git clone https://github.com/ZDoom/ZMusic
cd ZMusic
git checkout 1.1.14
mkdir build
cd build
cmake -DCMAKE_BUILD_TYPE=Release -DDYN_MPG123=OFF -DDYN_SNDFILE=OFF ..
make -j
cp source/libzmusic.so ~/Helion-libs/
```

# Build libSDL2
```
cd ~
git clone https://github.com/libsdl-org/SDL
cd SDL
git checkout release-2.30.9
mkdir build
cd build
cmake -DCMAKE_BUILD_TYPE=Release ..
make -j
cp libSDL2-2.0.so ~/Helion-libs/libSDL2.so
```

# Build libfluidsynth
```
cd ~
git clone https://github.com/FluidSynth/fluidsynth
cd fluidsynth
git checkout v2.4.0
mkdir build
cd build
cmake -DCMAKE_BUILD_TYPE=Release ..
make -j
cp src/libfluidsynth.so.3 ~/Helion-libs/
```

# Strip symbols (optional)
```
cd ~/Helion-libs
strip *
```

These steps should produce a directory with three .so files you can copy over those distributed with Helion.