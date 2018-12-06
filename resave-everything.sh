#!/bin/bash

# Resave all levels and worldmaps in both the main game and the addons

set -e

make

if [ $# -eq 0 ]; then
    ./supertux-editor.exe --resave ../addons-src/*/*/*/*/*.{stl,stwm} ../addons-src/*/*/*/*.{stl,stwm}
    ./supertux-editor.exe --resave ../supertux/data/levels/*/*.{stl,stwm}
else
    ./supertux-editor.exe --resave "$@"
fi

# EOF #
