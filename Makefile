#
# Makefile for supertux-editor.exe
# Copyright (C) 2008 Christoph Sommer <christoph.sommer@2008.expires.deltadevelopment.de>
#
# This program is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; either version 2 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program; if not, write to the Free Software
# Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
#

GMCS?=gmcs
GMCSFLAGS?=-debug -warn:4 -nowarn:1591 -unsafe

ifeq ($(VERBOSE),1)
Q=
else
Q=@
endif

.phony: all clean

all: supertux-editor.exe

clean:
	rm -f *.dll *.pkg.dummy supertux-editor.exe

gtkgl-sharp.dll: \
	$(wildcard gtkgl-sharp/*.cs) \
	gtk-sharp-2.0.pkg.dummy \

Lisp.dll: \
	$(wildcard Lisp/*.cs) \

Resources.dll: \
	$(wildcard Resources/*.cs) \

libeditor.dll: \
	$(wildcard libeditor/DataStructures/*.cs) $(wildcard libeditor/Libs/*.cs) $(wildcard libeditor/SceneGraph/*.cs) $(wildcard libeditor/Drawing/*.cs) $(wildcard libeditor/*.cs) \
	$(wildcard libeditor/resources/*.png) $(wildcard libeditor/resources/*.glade) \
	Resources.dll gtkgl-sharp.dll \
	gtk-sharp-2.0.pkg.dummy glade-sharp-2.0.pkg.dummy

LispReader.dll: \
	$(wildcard LispReader/*.cs) \
	Lisp.dll libeditor.dll \

supertux-editor.exe: \
	$(wildcard supertux-editor/*.cs) $(wildcard supertux-editor/Sprites/*.cs) $(wildcard supertux-editor/Tiles/*.cs) $(wildcard supertux-editor/PropertyEditors/*.cs) $(wildcard supertux-editor/LevelObjects/*.cs) $(wildcard supertux-editor/Commands/*.cs) $(wildcard supertux-editor/Tools/*.cs) \
	$(wildcard supertux-editor/resources/*.png) $(wildcard supertux-editor/resources/*.glade) \
	Lisp.dll Resources.dll LispReader.dll libeditor.dll gtkgl-sharp.dll \
	gtk-sharp-2.0.pkg.dummy glade-sharp-2.0.pkg.dummy \

%.pkg.dummy:
	$(Q)touch $@

%.exe:
	@echo MonoCSharp $@
	$(Q)$(GMCS) $(GMCSFLAGS) $(patsubst %.pkg.dummy,-pkg:%,$(filter %.pkg.dummy, $^)) -out:$@ -doc:$(patsubst %.dll,%.dll.xml,$@) -target:exe $(patsubst %,-r:%,$(filter %.dll, $^)) $(patsubst %,-resource:%,$(filter %.png %.glade, $^)) $(filter %.cs, $^)

%.dll:
	@echo MonoCSharp $@
	$(Q)$(GMCS) $(GMCSFLAGS) $(patsubst %.pkg.dummy,-pkg:%,$(filter %.pkg.dummy, $^)) -out:$@ -doc:$(patsubst %.dll,%.dll.xml,$@) -target:library $(patsubst %,-r:%,$(filter %.dll, $^)) $(patsubst %,-resource:%,$(filter %.png %.glade, $^)) $(filter %.cs, $^)
