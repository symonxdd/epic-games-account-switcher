!include "MUI2.nsh"

!define MUI_ICON "Assets\setup.ico"
!define APP_NAME "Epic Switcher"
!define APP_SHORTCUT_NAME "Epic Switcher"
!define REGISTRY_KEY "Software\${APP_NAME}"

Name "Epic Switcher"
InstallDir "$LOCALAPPDATA\${APP_NAME}"
OutFile "EpicSwitcherSetup.exe"
Unicode True

# Request application privileges for Windows Vista
RequestExecutionLevel user

!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

# Customize the finish page text to include attribution
!define MUI_FINISHPAGE_TEXT "Thank you for installing Epic Switcher.$\r$\nSetup icon by Freepic (https://www.freepik.com)."
!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_FINISHPAGE_RUN
!define MUI_FINISHPAGE_RUN_CHECKED
!define MUI_FINISHPAGE_RUN_TEXT "Run ${APP_SHORTCUT_NAME}"
!define MUI_FINISHPAGE_RUN_FUNCTION "StartApp"
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

# Check if the application is already installed
Function CheckIfInstalled
  # Read the install directory from the registry
  ReadRegStr $0 HKCU "${REGISTRY_KEY}" "InstallDir"
  
  # If the install directory is found, display a custom large message
  StrCmp $0 "" 0 already_installed
  Goto install

already_installed:
  # Show message in a large, customized window
  MessageBox MB_ICONINFORMATION|MB_OK "The application is already installed at: $0$\r$\n$\r$\nPlease click OK to exit the installer." IDOK end_install

end_install:
  # Exit the installer after showing the message
  Quit

install:
FunctionEnd

# Call the CheckIfInstalled function at the start of installation
Function .onInit
  Call CheckIfInstalled
FunctionEnd

Section
  # Create installation directory
  SetOutPath $INSTDIR
  
  # Copy files directly into the installation directory
  File /r "bin\Release\net8.0-windows\win-x64\publish\*.*"

  # Create a shortcut to the main application
  CreateShortCut "$INSTDIR\${APP_SHORTCUT_NAME}.lnk" "$INSTDIR\AccountSwitcher.exe"

  # Copy shortcuts to Desktop and Start Menu
  CopyFiles "$INSTDIR\${APP_SHORTCUT_NAME}.lnk" "$DESKTOP"
  CopyFiles "$INSTDIR\${APP_SHORTCUT_NAME}.lnk" "$SMPROGRAMS"

  # Store installation folder in the registry for uninstaller access
  WriteRegStr HKCU "${REGISTRY_KEY}" "InstallDir" $INSTDIR
  
  # Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
SectionEnd

Section "Uninstall"
  # Remove all files in the installation directory
  Delete "$INSTDIR\*.*"
  Delete "$INSTDIR\*.dll"
  Delete "$INSTDIR\*.exe"
  Delete "$INSTDIR\*.lnk"

  # Remove shortcuts
  Delete "$DESKTOP\${APP_SHORTCUT_NAME}.lnk"
  Delete "$SMPROGRAMS\${APP_SHORTCUT_NAME}.lnk"

  # Remove installation directory
  RMDir /r "$INSTDIR"

  # Remove registry entry for the install directory
  DeleteRegKey HKCU "${REGISTRY_KEY}"
SectionEnd

Function StartApp
  ExecShell "" "$INSTDIR\AccountSwitcher.exe"
FunctionEnd
