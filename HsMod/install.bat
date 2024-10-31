if exist "%PROGRAMFILES(x86)%\Hearthstone\BepInEx\core\BepInEx.dll" (
    copy /y Release\HsMod.dll "D:\code\HSCentric\default_runtime\BepinEXPlugin\HsMod.dll"
) else (
    echo BepInEx is not exist!
)