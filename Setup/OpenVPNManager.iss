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
#define DOWNLOAD_PATH = "http://download.microsoft.com/download/9/5/A/95A9616B-7A37-4AF6-BC36-D6EA96C8DAAE/dotNetFx40_Full_x86_x64.exe"

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
en.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
de.Framework4Download=Dieses Setup benötigt das .NET Framework V4.0. Bitte laden sie das .NET Framework V4.0 herunter, installieren es und starten dieses Setup erneut. Wollen sie den Download direkt starten?
br.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
ct.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
cz.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
dn.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
nl.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
fr.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
fn.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
hu.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
it.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
nw.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
es.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
pl.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
pg.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
ru.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
;sl.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?
sv.Framework4Download=This setup requires the .NET Framework V4.0. Please download and install the .NET Framework V4.0 and run this setup again. Do you want to download the framework now?

en.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
de.Framework4Setup=Dieses Setup benötigt das .NET Framework V4.0. Wollen sie das mitgelieferte Framework Setup starten?
br.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
ct.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
cz.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
dn.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
nl.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
fr.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
fn.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
hu.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
it.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
nw.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
es.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
pl.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
pg.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
ru.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
;sl.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?
sv.Framework4Setup=This setup requires the .NET Framework V4.0. Do you want to start the provided framework setup now?

en.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
de.Framework4SetupNoAdmin=Dieses Setup benötigt das .NET Framework V4.0. Bitte melden sie sich als Administrator an und starten dieses Setup erneut um das Framework zu installieren!
br.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
ct.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
cz.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
dn.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
nl.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
fr.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
fn.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
hu.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
it.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
nw.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
es.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
pl.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
pg.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
ru.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
;sl.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!
sv.Framework4SetupNoAdmin=This setup requires the .NET Framework V4.0. Please login as Administrator and run this setup again to install the Framework!

en.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
de.Framework4Failed=Kein .NET Framework V4.0 gefunden, Setup wird abgebrochen!
br.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
ct.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
cz.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
dn.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
nl.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
fr.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
fn.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
hu.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
it.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
nw.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
es.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
pl.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
pg.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
ru.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
;sl.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!
sv.Framework4Failed=No .NET Framework V4.0 detected, Setup canceled!

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
	dotnet40RedistPath: string;
	ResultCode: Integer;
	Download: Boolean;

begin
	NetFrameWorkInstalled := RegKeyExists(HKLM,'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\full');

	Download:=false

	if NetFrameWorkInstalled = true then
		Result := true
	else
		begin
			dotnet40RedistPath := ExpandConstant('{src}') + '\Framework\dotNetFx40_Full_x86_x64.exe';

			if not FileExists(dotnet40RedistPath) then
				begin
					Result1 := MsgBox(ExpandConstant('{cm:Framework4Download}'), mbConfirmation, MB_YESNO) = idYes;
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
							Result1 := MsgBox(ExpandConstant('{cm:Framework4Setup}'), mbConfirmation, MB_YESNO) = idYes;
							if Result1 = false then
								Result:=false
							else
								begin
									if Exec(dotnet40RedistPath, '', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
											if RegKeyExists(HKLM,'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\full') then
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
						MsgBox(ExpandConstant('{cm:Framework4SetupNoAdmin}'),mbCriticalError,MB_OK)
				end
		end;

	if not Result and not Download then
		MsgBox(ExpandConstant('{cm:Framework4Failed}'),mbCriticalError,MB_OK)
end;
