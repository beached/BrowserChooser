using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BrowserChooser.COM;
using Microsoft.Win32;
using System.Windows;

namespace BrowserChooser {
	public sealed class Browser {
		public string DisplayName { get; set; }
		public string ExecPath { get; set; }
		public string CommandLine { get; set; }
		public bool Checked { get; set; }
		public string KeyName { get; set; }

		private const string StrRegLoc = @"Software\Clients\StartMenuInternet";
		private static readonly FileInfo FiCurrentExecutable = new FileInfo( System.Reflection.Assembly.GetExecutingAssembly( ).Location );


		public void OpenUrl( string strUrl ) {
			using( var proc = new Process( ) ) {
				proc.EnableRaisingEvents = false;
				proc.StartInfo.FileName = ExecPath;
				proc.StartInfo.Arguments = string.Format( CommandLine, strUrl );
				proc.Start( );
			}
		}

		private static RegistryKey OpenSubKeyOrCreate( RegistryKey parentKey, string path, bool deleteFirst = false ) {
			if( deleteFirst ) {
				try {
					parentKey.DeleteSubKeyTree( path );
				} catch( ArgumentException ) {
					// Key Doesn't exist
				}
			}
			var reg = parentKey.OpenSubKey( path, true ) ?? parentKey.CreateSubKey( path );
			return reg;
		}

		private static void AddHandler( string protocol, string description, string application, RegistryKey regScope ) {
			try {
				using(var regProto = OpenSubKeyOrCreate( regScope, protocol, true )) {
					if( null == regProto ) {
						return;
					}
					regProto.SetValue( string.Empty, description );
					regProto.SetValue( "URL Protocol", string.Empty );
					RegistryKey r;
					using(var regShOp = OpenSubKeyOrCreate( regProto, @"shell\open", true )) {							
						using(r = OpenSubKeyOrCreate( regShOp, @"ddeexec", true )) {
							r.SetValue( string.Empty, string.Empty );
							using(var regApp = r.CreateSubKey( @"Application" )) {
								Debug.Assert( null != regApp, "regApp != null" );
								regApp.SetValue( string.Empty, string.Empty );
							}
							using(RegistryKey regTopic = r.CreateSubKey( @"Topic" )) {
								Debug.Assert( null != regTopic, "regTopic != null" );
								regTopic.SetValue( string.Empty, string.Empty );
							}
						}
						using(r = OpenSubKeyOrCreate( regShOp, @"command" )) {
							r.SetValue( string.Empty, string.Format( "\"{0}\"  \"%1\"", application ) );
						}
					}
					using(r = OpenSubKeyOrCreate( regProto, @"DefaultIcon" )) {
						r.SetValue( string.Empty, FiCurrentExecutable.Name );
					}
				}
			} catch(Exception e) {
				MessageBox.Show( e.Message );
			}
		}

		private static void RegisterProtocolHandler( string protocol, string description, string application, RegistryKey regScope ) {
			try {								
				const string strSoftClass = @"SOFTWARE\Classes\";
				AddHandler( protocol, description, application, regScope.OpenSubKey( strSoftClass, true ) );

				// If 64-bit OS, also register in the 32-bit RegScope area.
				const string strSoft64Classes = @"SOFTWARE\Classes\Wow6432Node";
				//const String strSoft64Classes = @"SOFTWARE\Wow6432Node\Classes";
				AddHandler( protocol, description, application, regScope.OpenSubKey( strSoft64Classes, true ) );

				using( var regProto = Registry.CurrentUser.OpenSubKey( @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\" + protocol + @"\UserChoice", true ) ) {
					if( null != regProto ) { // This is vista
						regProto.SetValue( @"Progid", FiCurrentExecutable.Name );
					}
				}
			} catch( Exception e ) {
				MessageBox.Show( e.Message );
			}
		}


		public static bool IsDefault( bool perUser = true ) {
			var rkRoot = perUser ? Registry.CurrentUser : Registry.LocalMachine;
			var appregpath = string.Format( @"Software\Classes\{0}", FiCurrentExecutable.Name );
			var app = FiCurrentExecutable.FullName;
			using(var regCmd = OpenSubKeyOrCreate( rkRoot, string.Format( @"{0}\shell\open\command", appregpath ) )) {
				var currentValue = regCmd.GetValue( string.Empty ) as string;
				if(null != currentValue) {
					return currentValue.Equals( string.Format( "\"{0}\" \"%1\"", app ) );
				}
			}
			return false;
		}

		public static void Register( bool perUser = true ) {
			var rkRoot = perUser ? Registry.CurrentUser : Registry.LocalMachine;
			var success = true;
			var app = FiCurrentExecutable.FullName;
			try {
				var appregpath = string.Format( @"Software\Classes\{0}", FiCurrentExecutable.Name );
				var appcompatregpath = string.Format( @"{0}\Capabilities", appregpath ); 

				using(var regCmd = OpenSubKeyOrCreate( rkRoot, string.Format( @"{0}\shell\open\command", appregpath ) )) {
					regCmd.SetValue( string.Empty, string.Format( "\"{0}\" \"%1\"", app ) );
					regCmd.SetValue( @"FriendlyAppName", @"Browser Chooser" );
				}

				// ReSharper disable once SuspiciousTypeConversion.Global
				var registration = (IApplicationAssociationRegistration)new ApplicationAssociationRegistration( );
				using(var rkCapabilities = OpenSubKeyOrCreate( rkRoot, appcompatregpath )) {
					rkCapabilities.SetValue( @"ApplicationDescription", @"Allows you to choose which web browser to open when opening links", RegistryValueKind.String );
					rkCapabilities.SetValue( @"ApplicationName", @"Browser Chooser", RegistryValueKind.String );
					using(var rkUrlAssociations = OpenSubKeyOrCreate( rkCapabilities, @"UrlAssociations" )) {						
						foreach(var p in Properties.Settings.Default.URLHandlers.Items) {
							rkUrlAssociations.SetValue( p.Proto, FiCurrentExecutable.Name, RegistryValueKind.String );							
						}
					}
				}
				using(var rkRegisteredApps = OpenSubKeyOrCreate( rkRoot, @"Software\RegisteredApplications" )) {
					rkRegisteredApps.SetValue( FiCurrentExecutable.Name, appcompatregpath, RegistryValueKind.String );
				}
				foreach(var p in Properties.Settings.Default.URLHandlers.Items) {
					RegisterProtocolHandler( p.Proto, p.Desc, app, rkRoot );
					registration.SetAppAsDefault( FiCurrentExecutable.Name, p.Proto, ASSOCIATIONTYPE.AT_URLPROTOCOL );
				}
				registration.SetAppAsDefaultAll( FiCurrentExecutable.Name );

			} catch(Exception ex) {
				MessageBox.Show( string.Format( @"Error registering as default URL handler: {0}", ex.Message ) );
				success = false;
			}
			Properties.Settings.Default.hasSetDefault = success;
			Properties.Settings.Default.Save( );
		}

		public static void UnRegister( bool perUser = true ) {
			var rkRoot = perUser ? Registry.CurrentUser : Registry.LocalMachine;
			try {

				using(var regClass = rkRoot.OpenSubKey( @"Software\Classes", true )) {
					if(null != regClass) {						
						regClass.DeleteSubKeyTree( FiCurrentExecutable.Name );
					}
				}
	
				using(var rkRegisteredApps = OpenSubKeyOrCreate( rkRoot, @"Software\RegisteredApplications" )) {
					if(null != rkRegisteredApps) {
						rkRegisteredApps.DeleteValue( FiCurrentExecutable.Name, false );
					}
				}
			} catch(Exception ex) {
				MessageBox.Show( string.Format( @"Error unregistering as default URL handler: {0}", ex.Message ) );
			}
		}

		public static IList<Browser> GetSystemBrowsers( ) {
			var ret = new List<Browser>( );
			var straCheckedBrowsers = Properties.Settings.Default.CheckedBrowsers.Split( new[] { ';' } );
			using( var regSysReg = Registry.LocalMachine.OpenSubKey( StrRegLoc ) ) {
				Debug.Assert( null != regSysReg, "regSysReg != null" );
				foreach( var strSubKey in regSysReg.GetSubKeyNames( ).Where( strSubKey => string.Compare( strSubKey, FiCurrentExecutable.Name, StringComparison.OrdinalIgnoreCase ) != 0 ) ) {
					using( var regCurBrowser = regSysReg.OpenSubKey( strSubKey ) ) {
						Debug.Assert( null != regCurBrowser, "regCurBrowser != null" );
						var strLocalizedString = regCurBrowser.GetValue( @"LocalizedString", string.Empty ) as string;
						if( string.IsNullOrEmpty( strLocalizedString ) ) {
							strLocalizedString = regCurBrowser.GetValue( string.Empty, string.Empty ) as string;
						}
						if( null != strLocalizedString && strLocalizedString.StartsWith( @"@" ) ) {
							// This is a resource location
							var param = strLocalizedString.Substring( 1 ).Split( new[] { ',' } );
							if( param[1][0] == '-' ) {
								param[1] = param[1].Substring( 1 );
							}
							strLocalizedString = NativeMethods.GetResourceFromFile( param[0], UInt32.Parse( param[1] ) );
							if( string.IsNullOrEmpty( strLocalizedString ) ) {
								strLocalizedString = regCurBrowser.GetValue( string.Empty, string.Empty ) as string;
							}
						}
						if( string.IsNullOrEmpty( strLocalizedString ) ) {
							continue;
						}
						var br = new Browser {KeyName = strSubKey, DisplayName = strLocalizedString};
						using( var regCmd = regCurBrowser.OpenSubKey( @"Shell\Open\Command" ) ) {
							Debug.Assert( null != regCmd, "regCmd != null" );
							br.ExecPath = regCmd.GetValue( string.Empty, string.Empty ) as string;
						}

						br.CommandLine = @"{0}";
						// Check settings and see if we have chosen this one to be clicked in the past
						if( null != Properties.Settings.Default.CheckedBrowsers ) {
							br.Checked = Array.Exists( straCheckedBrowsers, s => string.CompareOrdinal( strSubKey, s ) == 0 );
						}
						ret.Add( br );
					}
				}
			}
			return ret;
		}
	}
}
