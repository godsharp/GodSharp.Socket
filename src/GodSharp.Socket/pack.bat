@echo off
nuget pack GodSharp.Socket.csproj -properties Configuration=Release
nuget push GodSharp.Socket.1.0.1.nupkg 2BFA39CA-3BED-4BF9-A857-538850B9E8FC -Source http://nuget.godsharp.com/api/v2/package