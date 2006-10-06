@echo off
rem Set variables
rem Base dir:
set BASE=..\..
set DIST=%BASE%\dist-tmp
set SOURCE=%BASE%\bin\Release
set INNOSOURCE=%BASE%\tools\innosetup

rem Remove any old dist dir
rmdir /S /Q %DIST%

mkdir %DIST%

echo Copying GPL
copy %BASE%\COPYING %DIST%\COPYING.txt

echo Copying binaries
copy %SOURCE%\supertux-editor.exe %DIST%
copy %SOURCE%\*.dll %DIST%
copy %SOURCE%\*.pdb %DIST%

rem Remove Dock.* as it isn't used any longer
del /Q %DIST%\Dock.*

echo Copying Inno Setup files
copy %INNOSOURCE%\supertux-editor.iss %DIST%
copy %INNOSOURCE%\supertux-editor.ico %DIST%


echo Creating data dir
mkdir %DIST%\data
mkdir %DIST%\data\brushes
copy %BASE%\data\brushes\*.csv %DIST%\data\brushes

pause
