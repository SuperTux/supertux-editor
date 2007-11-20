#!/bin/sh

# This script installs the gtksourceview language specification for squirrel
# (and the .nut mimetype)
specdir="$HOME/.gnome2/gtksourceview-1.0/language-specs"
mkdir -pv "$specdir"
cp -p squirrel.lang "$specdir"

test -z "$XDG_DATA_HOME" && XDG_DATA_HOME="$HOME/.local/share"
MIMEDIR="$XDG_DATA_HOME/mime"
mkdir -pv "$MIMEDIR"
cp -p squirrel.xml "$MIMEDIR"
