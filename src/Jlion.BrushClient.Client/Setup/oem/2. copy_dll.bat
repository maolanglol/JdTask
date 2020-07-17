
copy "..\..\bin\Release\WP.Device.Plugins.Client.exe.config" "Out\" /Y
copy "..\..\bin\Release\Device.config" "Out\" /Y
copy "..\..\bin\Release\OpenCvSharp.CPlusPlus.dll" "Out\" /Y
copy "..\..\bin\Release\OpenCvSharp.CPlusPlus.dll.config" "Out\" /Y
copy "..\..\bin\Release\OpenCvSharp.dll" "Out\" /Y
copy "..\..\bin\Release\OpenCvSharp.dll.config" "Out\" /Y
copy "..\..\bin\Release\OpenCvSharp.Extensions.dll" "Out\" /Y
copy "..\..\bin\Release\Microsoft.Threading.Tasks.dll" "Out\" /Y

copy "..\OutUpgrade\Wp.Device.UpgradeClient.exe" "Out\" /Y
copy "..\OutUpgrade\Wp.Device.UpgradeClient.exe.config" "Out\" /Y

copy "OutStart\WP.Device.Plugins.StartClient.exe" "Out\" /Y

echo "copy resource file successful..."

pause