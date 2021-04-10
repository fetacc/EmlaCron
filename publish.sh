#!/usr/bin/env bash

version=$1

mkdir -p ./dist

dotnet publish -c Release -r win10-x64
dotnet publish -c Release -r linux-x64
dotnet publish -c Release -r osx-x64

cp -R ./src/bin/Release/net5.0/win10-x64/publish/ ./dist/EmlaCron-windows-$version
cp -R ./src/bin/Release/net5.0/linux-x64/publish/ ./dist/EmlaCron-linux-$version
cp -R ./src/bin/Release/net5.0/osx-x64/publish/ ./dist/EmlaCron-osx-$version

zip -r ./dist/EmlaCron-windows-$version.zip ./dist/EmlaCron-windows-$version
tar -czvf ./dist/EmlaCron-linux-$version.tar.gz ./dist/EmlaCron-linux-$version
tar -czvf ./dist/EmlaCron-osx-$version.tar.gz ./dist/EmlaCron-osx-$version

rm -rf ./dist/EmlaCron-windows-$version
rm -rf ./dist/EmlaCron-linux-$version
rm -rf ./dist/EmlaCron-osx-$version
