﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OpenVPNManager.lang {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("OpenVPNManager.lang.strings", typeof(strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not communicate with OpenVPN Manager - perhaps remote control is not allowed?.
        /// </summary>
        internal static string ARGS_Error {
            get {
                return ResourceManager.GetString("ARGS_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -install-autostart   -   Install autostart and quit
        ///-remove-autostart   -   Remove autostart and quit
        ///-connect name   -   Connect to VPN &quot;name&quot;
        ///-disconnect name   -   Disconnect from VPN &quot;name&quot;
        ///-quit   -   Shut down running OpenVPN Manager.
        /// </summary>
        internal static string ARGS_Help {
            get {
                return ResourceManager.GetString("ARGS_Help", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid value &quot;{1}&quot; for argument {0}!
        ///Valid values are:
        ///{2}.
        /// </summary>
        internal static string ARGS_Invalid_Parameter {
            get {
                return ResourceManager.GetString("ARGS_Invalid_Parameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing value for argument &quot;{0}&quot;.
        /// </summary>
        internal static string ARGS_Missing_Parameter {
            get {
                return ResourceManager.GetString("ARGS_Missing_Parameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown argument &quot;{0}&quot;.
        /// </summary>
        internal static string ARGS_Unknown_Parameter {
            get {
                return ResourceManager.GetString("ARGS_Unknown_Parameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not load Config file. Ignoring!.
        /// </summary>
        internal static string BOX_Config_Error {
            get {
                return ResourceManager.GetString("BOX_Config_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to connect!
        ///
        ///Details:.
        /// </summary>
        internal static string BOX_Error_Connect {
            get {
                return ResourceManager.GetString("BOX_Error_Connect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while loading configuration:.
        /// </summary>
        internal static string BOX_Error_Information {
            get {
                return ResourceManager.GetString("BOX_Error_Information", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find smart card. Please insert one and press retry..
        /// </summary>
        internal static string BOX_NoKey {
            get {
                return ResourceManager.GetString("BOX_NoKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to reconnect to this VPN?.
        /// </summary>
        internal static string BOX_Reconnect {
            get {
                return ResourceManager.GetString("BOX_Reconnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to These settings are saved in the registry. You can change them with administrator rights.
        ///They are located in HKEY_LOCAL_MACHINE\SOFTWARE\OpenVPN..
        /// </summary>
        internal static string BOX_Service_How_Change {
            get {
                return ResourceManager.GetString("BOX_Service_How_Change", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The OpenVPN service is not installed.
        /// </summary>
        internal static string BOX_Service_Not_Installed {
            get {
                return ResourceManager.GetString("BOX_Service_Not_Installed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OpenVPN Manager and the OpenVPN service are using the same directory.
        ///OpenVPN Manager can&apos;t figure out how the connections should be controlled, so the service control features are deactivated.
        ///Change one of the directories or change the service file extension..
        /// </summary>
        internal static string BOX_Service_Same_Path {
            get {
                return ResourceManager.GetString("BOX_Service_Same_Path", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In order to edit settings all connection must be closed. Close all connections?.
        /// </summary>
        internal static string BOX_Settings_Close {
            get {
                return ResourceManager.GetString("BOX_Settings_Close", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The update information could not be downloaded.
        /// </summary>
        internal static string BOX_UpdateError {
            get {
                return ResourceManager.GetString("BOX_UpdateError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The update information hat an invalid format.
        /// </summary>
        internal static string BOX_UpdateFormat {
            get {
                return ResourceManager.GetString("BOX_UpdateFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A new Version (%s) was found. Do you want to see details in your browser?.
        /// </summary>
        internal static string BOX_UpdateInformation {
            get {
                return ResourceManager.GetString("BOX_UpdateInformation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The update information did not contain this program!.
        /// </summary>
        internal static string BOX_UpdateMissing {
            get {
                return ResourceManager.GetString("BOX_UpdateMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No new version was found..
        /// </summary>
        internal static string BOX_UpdateNone {
            get {
                return ResourceManager.GetString("BOX_UpdateNone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while starting OpenVPN. Do you want to see its log file?.
        /// </summary>
        internal static string BOX_VPN_Error {
            get {
                return ResourceManager.GetString("BOX_VPN_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error while starting OpenVPN..
        /// </summary>
        internal static string BOX_VPNS_Error {
            get {
                return ResourceManager.GetString("BOX_VPNS_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to disabled.
        /// </summary>
        internal static string DIALOG_Disabled {
            get {
                return ResourceManager.GetString("DIALOG_Disabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to enabled.
        /// </summary>
        internal static string DIALOG_Enabled {
            get {
                return ResourceManager.GetString("DIALOG_Enabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All files.
        /// </summary>
        internal static string DIALOG_Filter_Allfiles {
            get {
                return ResourceManager.GetString("DIALOG_Filter_Allfiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Applications.
        /// </summary>
        internal static string DIALOG_Filter_Application {
            get {
                return ResourceManager.GetString("DIALOG_Filter_Application", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Copy IP.
        /// </summary>
        internal static string DIALOG_IP_Copy {
            get {
                return ResourceManager.GetString("DIALOG_IP_Copy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to service.
        /// </summary>
        internal static string DIALOG_Service {
            get {
                return ResourceManager.GetString("DIALOG_Service", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select OpenVPN configuration directory.
        /// </summary>
        internal static string DIALOG_Title_Folder {
            get {
                return ResourceManager.GetString("DIALOG_Title_Folder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Search OpenVPN.
        /// </summary>
        internal static string DIALOG_Title_Open_OpenVPN {
            get {
                return ResourceManager.GetString("DIALOG_Title_Open_OpenVPN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connect (Alt+O).
        /// </summary>
        internal static string QUICKINFO_Connect {
            get {
                return ResourceManager.GetString("QUICKINFO_Connect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disconnect (Alt+O).
        /// </summary>
        internal static string QUICKINFO_Disconnect {
            get {
                return ResourceManager.GetString("QUICKINFO_Disconnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An Update is available!.
        /// </summary>
        internal static string QUICKINFO_Update {
            get {
                return ResourceManager.GetString("QUICKINFO_Update", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to See the &quot;about&quot; Dialog for more information..
        /// </summary>
        internal static string QUICKINFO_Update_More {
            get {
                return ResourceManager.GetString("QUICKINFO_Update_More", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connected.
        /// </summary>
        internal static string STATE_Connected {
            get {
                return ResourceManager.GetString("STATE_Connected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing....
        /// </summary>
        internal static string STATE_Initializing {
            get {
                return ResourceManager.GetString("STATE_Initializing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stopped.
        /// </summary>
        internal static string STATE_Stopped {
            get {
                return ResourceManager.GetString("STATE_Stopped", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stopping....
        /// </summary>
        internal static string STATE_Stopping {
            get {
                return ResourceManager.GetString("STATE_Stopping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string STETE_Error {
            get {
                return ResourceManager.GetString("STETE_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Connect.
        /// </summary>
        internal static string TRAY_Connect {
            get {
                return ResourceManager.GetString("TRAY_Connect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Disconnect.
        /// </summary>
        internal static string TRAY_Disconnect {
            get {
                return ResourceManager.GetString("TRAY_Disconnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Edit.
        /// </summary>
        internal static string TRAY_Edit {
            get {
                return ResourceManager.GetString("TRAY_Edit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Show &amp;error information.
        /// </summary>
        internal static string TRAY_Error_Information {
            get {
                return ResourceManager.GetString("TRAY_Error_Information", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;Show.
        /// </summary>
        internal static string TRAY_Show {
            get {
                return ResourceManager.GetString("TRAY_Show", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Adding routes....
        /// </summary>
        internal static string VPNSTATE_ADD_ROUTES {
            get {
                return ResourceManager.GetString("VPNSTATE_ADD_ROUTES", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Assigning IP....
        /// </summary>
        internal static string VPNSTATE_ASSIGN_IP {
            get {
                return ResourceManager.GetString("VPNSTATE_ASSIGN_IP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authenticating....
        /// </summary>
        internal static string VPNSTATE_AUTH {
            get {
                return ResourceManager.GetString("VPNSTATE_AUTH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connected.
        /// </summary>
        internal static string VPNSTATE_CONNECTED {
            get {
                return ResourceManager.GetString("VPNSTATE_CONNECTED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connecting....
        /// </summary>
        internal static string VPNSTATE_CONNECTING {
            get {
                return ResourceManager.GetString("VPNSTATE_CONNECTING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exiting....
        /// </summary>
        internal static string VPNSTATE_EXITING {
            get {
                return ResourceManager.GetString("VPNSTATE_EXITING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Downloading config....
        /// </summary>
        internal static string VPNSTATE_GET_CONFIG {
            get {
                return ResourceManager.GetString("VPNSTATE_GET_CONFIG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reconnecting....
        /// </summary>
        internal static string VPNSTATE_RECONNECTING {
            get {
                return ResourceManager.GetString("VPNSTATE_RECONNECTING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Waiting for Server....
        /// </summary>
        internal static string VPNSTATE_WAIT {
            get {
                return ResourceManager.GetString("VPNSTATE_WAIT", resourceCulture);
            }
        }
    }
}
