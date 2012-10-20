; ******************* Konstanten festlegen ******************

; a) Name der Programmdatei:
#define SRC "OpenVPNManager"
#define SRCEXE SRC + ".exe"

; Wenn nicht im gleichen Pfad wie dieses Script zusätzlich:
#define SRCPATH "..\OpenVPNManager\bin\Release"

; b) Versionsnummer, wird versucht aus exe zu extrahieren:
; Wenn nicht in exe reincompiliert oder exe nicht gefunden:
#define APPVER "1.0.0.0"

; c) Programmname, wird für Startmenü usw. verwendet
#define APPNAME "OpenVPN Manager"

; d) Downloadpfad, evtl. auf lokalen Server anpassen
#define DOWNLOAD_PATH = "http://download.microsoft.com/download/7/0/3/703455ee-a747-4cc8-bd3e-98a615c3aedb/dotNetFx35setup.exe"

; ***************** Ende Konstanten festlegen ***************

; Firmenspezifische Angaben im Abschnitt [Setup] bei Bedarf bitte ändern!

; ***************** Ersetzung durchführen *******************
; Pfad + Dateiname zusammensetzen
#ifdef SRCPATH
	#define SRCEXE_0 SRCPATH + "\" + SRCEXE
	#define SRC_0 SRCPATH + "\" + SRC
#else
	#ifdef SourcePath
		#define SRCEXE_0 SourcePath + "\" + SRCEXE
		#define SRC_0 SourcePath + "\" + SRC
	#else
		#define SRCEXE_0 SRCEXE
		#define SRC_0 SRC
	#endif
#endif

; Nummer + Programmname aus exe extrahieren:
#if FileExists(SRCEXE_0) == 0
	#define APPVER_0 APPVER
	#define APPNAME_0 APPNAME
#else
	#define APPVER_0 GetFileVersion(SRCEXE_0)
	#if APPVER_0 == ""
		#define APPVER_0 APPVER
	#endif
	#define APPNAME_0 GetStringFileInfo(SRCEXE_0, PRODUCT_NAME)
	#if APPNAME_0 == ""
		#define APPNAME_0 APPNAME
	#endif
#endif
; *************** Ende Ersetzung durchführen *****************

; ******************* Beginn Setup ***************************
[Setup]
AppName={#APPNAME_0}
AppVerName={#APPNAME_0} {#APPVER_0}
DefaultDirName={pf}\jowisoftware\{#APPNAME_0}
DefaultGroupName=JoWiSoftware
UninstallDisplayIcon={app}\{#SRCEXE}
Compression=lzma
SolidCompression=false
VersionInfoVersion={#APPVER_0}
VersionInfoCompany=JoWiSoftware
VersionInfoDescription=Setup für {#APPNAME_0}
VersionInfoTextVersion={#APPVER_0}
AppCopyright=© JoWiSoftware
AllowUNCPath=false
AppPublisher=JoWiSoftware
AppPublisherURL=openvpn.jowisoftware.de
AppVersion={#APPVER_0}
AppID={{50C27607-6129-4791-8253-51CA7148BA27}
ShowLanguageDialog=auto
WizardImageFile=compiler:wizmodernimage-is.bmp
WizardSmallImageFile=compiler:wizmodernsmallimage-is.bmp
PrivilegesRequired=admin
LanguageDetectionMethod=uilanguage
UninstallDisplayName={#APPNAME_0}

[Files]
Source: {#SRCEXE_0}; DestDir: {app}
Source: {#SRCEXE_0}.config; DestDir: {app}
; Weitere Resourcen
Source: {#SRCPATH}\OpenVPN.dll; DestDir: {app}; Flags: replacesameversion
Source: {#SRCPATH}\license.txt; DestDir: {app}
Source: {#SRCPATH}\de\OpenVPNManager.resources.dll; DestDir: {app}; Flags: replacesameversion
; OpenVPN selbst
Source: openvpn-2.2.2-install.exe; DestDir: {app}\OpenVPNClient

[Icons]
Name: {group}\{#APPNAME_0}; Filename: {app}\{#SRCEXE}; WorkingDir: {app}
Name: {commondesktop}\{#APPNAME_0}; Filename: {app}\{#SRCEXE}; WorkingDir: {app}; Tasks: Desktopicon
Name: {userappdata}\Microsoft\Internet Explorer\Quick Launch\{#APPNAME_0}; Filename: {app}\{#SRCEXE}; WorkingDir: {app}; Tasks: quicklaunchicon

[Tasks]
Name: DesktopIcon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}
Name: QuickLaunchIcon; Description: {cm:CreateQuickLaunchIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked
Name: OpenVPNClient; Description: {cm:OpenVPNClient}; GroupDescription: {cm:OpenVPNClientGD}:

[Languages]
; Der erste Eintrag gilt wenn Erkennung fehlschägt, Sprachauswahl wird dann in dieser Sparache angezeigt
Name: en; MessagesFile: compiler:Default.isl
Name: de; MessagesFile: compiler:Languages\German.isl
Name: br; MessagesFile: compiler:Languages\BrazilianPortuguese.isl
Name: ct; MessagesFile: compiler:Languages\Catalan.isl
Name: cz; MessagesFile: compiler:Languages\Czech.isl
Name: dn; MessagesFile: compiler:Languages\Danish.isl
Name: nl; MessagesFile: compiler:Languages\Dutch.isl
Name: fr; MessagesFile: compiler:Languages\French.isl
Name: fn; MessagesFile: compiler:Languages\Finnish.isl
Name: hu; MessagesFile: compiler:Languages\Hungarian.isl
Name: it; MessagesFile: compiler:Languages\Italian.isl
Name: nw; MessagesFile: compiler:Languages\Norwegian.isl
Name: es; MessagesFile: compiler:Languages\Spanish.isl
Name: pl; MessagesFile: compiler:Languages\Polish.isl
Name: pg; MessagesFile: compiler:Languages\Portuguese.isl
Name: ru; MessagesFile: compiler:Languages\Russian.isl
;Name: sl; MessagesFile: compiler:Languages\Slovak.isl
Name: sv; MessagesFile: compiler:Languages\Slovenian.isl

[Run]
Filename: {app}\{#SRCEXE}; WorkingDir: {app}; Flags: skipifdoesntexist postinstall skipifsilent nowait
Filename: {app}\OpenVPNClient\openvpn-2.2.2-install.exe; WorkingDir: {app}\OpenVPNClient; Tasks: OpenVPNClient

[CustomMessages]
en.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
de.Framework35Download=Dieses Setup benötigt das .NET Framework V3.5. Bitte laden sie das .NET Framework V3.5 herunter, installieren es und starten dieses Setup erneut. Wollen sie den Download direkt starten?
br.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
ct.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
cz.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
dn.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
nl.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
fr.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
fn.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
hu.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
it.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
nw.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
es.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
pl.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
pg.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
ru.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
;sl.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?
sv.Framework35Download=This setup requires the .NET Framework V3.5. Please download and install the .NET Framework V3.5 and run this setup again. Do you want to download the framework now?

en.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
de.Framework35Setup=Dieses Setup benötigt das .NET Framework V3.5. Wollen sie das mitgelieferte Framework Setup starten?
br.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
ct.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
cz.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
dn.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
nl.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
fr.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
fn.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
hu.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
it.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
nw.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
es.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
pl.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
pg.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
ru.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
;sl.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?
sv.Framework35Setup=This setup requires the .NET Framework V3.5. Do you want to start the provided framework setup now?

en.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
de.Framework35SetupNoAdmin=Dieses Setup benötigt das .NET Framework V3.5. Bitte melden sie sich als Administrator an und starten dieses Setup erneut um das Framework zu installieren!
br.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
ct.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
cz.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
dn.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
nl.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
fr.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
fn.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
hu.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
it.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
nw.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
es.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
pl.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
pg.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
ru.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
;sl.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!
sv.Framework35SetupNoAdmin=This setup requires the .NET Framework V3.5. Please login as Administrator and run this setup again to install the Framework!

en.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
de.Framework35Failed=Kein .NET Framework V3.5 gefunden, Setup wird abgebrochen!
br.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
ct.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
cz.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
dn.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
nl.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
fr.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
fn.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
hu.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
it.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
nw.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
es.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
pl.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
pg.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
ru.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
;sl.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!
sv.Framework35Failed=No .NET Framework V3.5 detected, Setup canceled!

en.OpenVPNClient=Install OpenVPN client
de.OpenVPNClient=OpenVPN Client installieren
br.OpenVPNClient=Install OpenVPN client
ct.OpenVPNClient=Install OpenVPN client
cz.OpenVPNClient=Install OpenVPN client
dn.OpenVPNClient=Install OpenVPN client
nl.OpenVPNClient=Install OpenVPN client
fr.OpenVPNClient=Install OpenVPN client
fn.OpenVPNClient=Install OpenVPN client
hu.OpenVPNClient=Install OpenVPN client
it.OpenVPNClient=Install OpenVPN client
nw.OpenVPNClient=Install OpenVPN client
es.OpenVPNClient=Install OpenVPN client
pl.OpenVPNClient=Install OpenVPN client
pg.OpenVPNClient=Install OpenVPN client
ru.OpenVPNClient=Install OpenVPN client
;sl.OpenVPNClient=Install OpenVPN client
sv.OpenVPNClient=Install OpenVPN client

en.OpenVPNClientGD=OpenVPN client
de.OpenVPNClientGD=OpenVPN Client
br.OpenVPNClientGD=OpenVPN client
ct.OpenVPNClientGD=OpenVPN client
cz.OpenVPNClientGD=OpenVPN client
dn.OpenVPNClientGD=OpenVPN client
nl.OpenVPNClientGD=OpenVPN client
fr.OpenVPNClientGD=OpenVPN client
fn.OpenVPNClientGD=OpenVPN client
hu.OpenVPNClientGD=OpenVPN client
it.OpenVPNClientGD=OpenVPN client
nw.OpenVPNClientGD=OpenVPN client
es.OpenVPNClientGD=OpenVPN client
pl.OpenVPNClientGD=OpenVPN client
pg.OpenVPNClientGD=OpenVPN client
ru.OpenVPNClientGD=OpenVPN client
;sl.OpenVPNClientGD=OpenVPN client
sv.OpenVPNClientGD=OpenVPN client

[Code]
function InitializeSetup(): Boolean;

var
	ErrorCode: Integer;
	NetFrameWorkInstalled : Boolean;
	Result1 : Boolean;
	dotnet35RedistPath: string;
	ResultCode: Integer;
	Download: Boolean;

begin
	NetFrameWorkInstalled := RegKeyExists(HKLM,'Software\Microsoft\NET Framework Setup\NDP\v3.5');

	Download:=false

	if NetFrameWorkInstalled = true then
		Result := true
	else
		begin
			dotnet35RedistPath := ExpandConstant('{src}') + '\Framework\dotnetfx35setup.exe';

			if not FileExists(dotnet35RedistPath) then
				begin
					Result1 := MsgBox(ExpandConstant('{cm:Framework35Download}'), mbConfirmation, MB_YESNO) = idYes;
					if Result1 = false then
						Result:=false
					else
						begin
							Result:=false
							Download:=true
							ShellExec('open', '{#DOWNLOAD_PATH}','','',SW_SHOWNORMAL,ewNoWait,ErrorCode);
						end
				end
			else
				begin
					if IsAdminLoggedOn() then
						begin
							Result1 := MsgBox(ExpandConstant('{cm:Framework35Setup}'), mbConfirmation, MB_YESNO) = idYes;
							if Result1 = false then
								Result:=false
							else
								begin
									if Exec(dotnet35RedistPath, '', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
											if RegKeyExists(HKLM,'Software\Microsoft\NET Framework Setup\NDP\v3.5') then
												begin
													Result:=true
												end
											else
												Result:=false
									else
										Result:=false
								end
						end
					else
						MsgBox(ExpandConstant('{cm:Framework35SetupNoAdmin}'),mbCriticalError,MB_OK)
				end
		end;

	if not Result and not Download then
		MsgBox(ExpandConstant('{cm:Framework35Failed}'),mbCriticalError,MB_OK)
end;
