LangString DESC_SEC_MAIN ${LANG_ENGLISH} "Install OpenVPN Manager."
LangString DESC_SEC_OPENVPN ${LANG_ENGLISH} "Also install OpenVPN 2.2."
LangString DESC_SEC_DOTNET ${LANG_ENGLISH} "Download and install the (required) .NET 2.0 runtimes if they are not already installed."
LangString DESC_SEC_SERVICE ${LANG_ENGLISH} "Install and automatically start OpenVPN Manager Service."

LangString DESC_SEC_MAIN ${LANG_GERMAN} "Installation von OpenVPN Manager."
LangString DESC_SEC_OPENVPN ${LANG_GERMAN} "Installation von OpenVPN 2.2."
LangString DESC_SEC_DOTNET ${LANG_GERMAN} "Herunterladen und Installieren der ben�tigten .NET 2.0 Runtimes, wenn sie nicht bereits installiert wind."
LangString DESC_SEC_SERVICE ${LANG_GERMAN} "OpenVPN Manager Service installieren und automatisch starten."

LangString DESC_SEC_MAIN ${LANG_FRENCH} "Installer OpenVPN Manager."
LangString DESC_SEC_OPENVPN ${LANG_FRENCH} "Installer OpenVPN 2.2."
LangString DESC_SEC_DOTNET ${LANG_FRENCH} "T�l�charger et installer les runtimes .NET 2.0 s'ils ne sont pas d�j� install�s."
LangString DESC_SEC_SERVICE ${LANG_FRENCH} "Installer et d�marrer automatiquement le service OpenVPN Manager."

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC_MAIN} $(DESC_SEC_MAIN)
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC_OPENVPN} $(DESC_SEC_OPENVPN)
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC_DOTNET} $(DESC_SEC_DOTNET)
  !insertmacro MUI_DESCRIPTION_TEXT ${SEC_SERVICE} $(DESC_SEC_SERVICE)
!insertmacro MUI_FUNCTION_DESCRIPTION_END
