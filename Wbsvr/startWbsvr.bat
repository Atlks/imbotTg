 title "wbsvrrr"
chcp 65001


:: 定义源文件路径和目标目录路径
set "sourceFile=%~dp0\wbsvr.dll"
set "targetDir=%~dp0\bin\debug\net8.0"

:: 检查目标目录是否存在，如果不存在则创建它


:: 复制文件到目标目录
copy /Y "%sourceFile%" "%targetDir%"

:: 检查复制操作是否成功
if %ERRORLEVEL% == 0 (
    echo File copied successfully.
) else (
    echo Failed to copy file.
)


:: 定义源文件路径和目标目录路径
set "sourceFile=%~dp0\wbsvr.exe"
set "targetDir=%~dp0\bin\debug\net8.0"

:: 检查目标目录是否存在，如果不存在则创建它


:: 复制文件到目标目录
copy /Y "%sourceFile%" "%targetDir%"

:: 检查复制操作是否成功
if %ERRORLEVEL% == 0 (
    echo File copied successfully.
) else (
    echo Failed to copy file.
)


cd /d "%~dp0\bin\Debug\net8.0"
wbsvr.exe
pause