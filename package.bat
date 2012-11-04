@echo off
echo Cleaning up
rmdir /S /Q dist
rmdir /S /Q OpenVPNManager\bin
rmdir /S /Q OpenVPN\bin
mkdir dist

echo Building project
msbuild openvpnmanager.sln /p:Configuration=Release

echo Packaging bin.zip
"c:\Program Files\7-Zip\7z" a -tzip dist\bin.zip .\OpenVPNManager\bin\Release\*

echo Building setup
"c:\Program Files (x86)\NSIS\makensis.exe" Setup\setup.nsi
move Setup\setup.exe dist\

@echo on
