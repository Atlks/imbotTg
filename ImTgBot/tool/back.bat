@echo off
title "bekdb"
setlocal


 

:loop
    

    :: 执行 aaa.exe
    start "" "C:\path\to\aaa.exe"




 

:: 设置源文件夹和目标根文件夹
set "sourceFolder=D:\prj\bin\Debug\net8.0\mercht商家数据"
set "backupRoot=d:\bek"

:: 获取当前日期
for /f "tokens=2 delims==" %%I in ('"wmic os get localdatetime /value"') do set dt=%%I
set "year=%dt:~0,4%"
set "month=%dt:~4,2%"
set "day=%dt:~6,2%"

:: 创建备份子文件夹
set "backupFolder=%backupRoot%\%year%-%month%-%day%"
if not exist "%backupFolder%" mkdir "%backupFolder%"

:: 执行备份
xcopy "%sourceFolder%" "%backupFolder%" /s /e /i /y

echo Backup completed successfully.


:: 等待 23 小时（82800 秒）
    timeout /t 82800
    :: 返回到循环开始
    goto loop

endlocal