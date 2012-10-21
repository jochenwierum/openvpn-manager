LangString DESC_SEC_MAIN ${LANG_ENGLISH} "Install OpenVPN Manager."
LangString DESC_SEC_OPENVPN ${LANG_ENGLISH} "Also install OpenVPN 2.2."
LangString DESC_SEC_DOTNET ${LANG_ENGLISH} "Download and install the (required) .NET 2.0 runtimes if they are not already installed."

LangString DESC_SEC_MAIN ${LANG_GERMAN} "Intallation von OpenVPN Manager."
LangString DESC_SEC_OPENVPN ${LANG_GERMAN} "Installation von OpenVPN 2.2."
LangString DESC_SEC_DOTNET ${LANG_GERMAN} "Herunterladen und Installieren der benötigten .NET 2.0 Runtimes, wenn sie nicht bereits installiert wind."


!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC_MAIN} $(DESC_SEC_MAIN)
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC_OPENVPN} $(DESC_SEC_OPENVPN)
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC_DOTNET} $(DESC_SEC_DOTNET)
!insertmacro MUI_FUNCTION_DESCRIPTION_END