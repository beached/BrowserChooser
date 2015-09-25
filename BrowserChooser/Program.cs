using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Deployment.Application;
using System.ComponentModel;
using System.IO;

namespace BrowserChooser {
	internal sealed class Program {
/*
		private static FileInfo _fiCurrentExecutable = new FileInfo( System.Reflection.Assembly.GetExecutingAssembly( ).Location );
*/
		
		[STAThread]
		private static void Main( string[] args ) {
			//var appFolder = Path.Combine( Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ), @"DAW Software Development" ), @"BrowserChooser" );
			var strUrl = string.Empty;
			if( Environment.GetCommandLineArgs( ).Length > 1 ) {
				var arg = Environment.GetCommandLineArgs( )[1].Trim( );
				var uarg = arg.ToUpper( );
				if( uarg.Equals( @"REGISTERSYSTEM" ) ) {
					if(RunAsAdmin( arg )) {
						Browser.Register( false );
					}
					return;
				} else if( uarg.Equals( @"REGISTERUSER" ) ) {
					Browser.Register( );
					return;
				} else if( uarg.Equals( @"UNREGISTERSYSTEM" ) ) {
					if(RunAsAdmin( arg )) {
						Browser.UnRegister( false );
					}
					return;
				} else if( uarg.Equals( @"UNREGISTERUSER" ) ) {
					Browser.UnRegister( );
					return;
				}
				strUrl = arg;
			}
			if(Properties.Settings.Default.UpgradeSettings) {
				Properties.Settings.Default.Upgrade( );
				Properties.Settings.Default.UpgradeSettings = false;
			}
			var browsers = Browser.GetSystemBrowsers( );
			var rememberedUrls = Properties.Settings.Default.StoredActions;
			var hasOpened = false;
			if( rememberedUrls != null && !string.IsNullOrEmpty( strUrl ) ) {
				foreach( var br in from rua in Properties.Settings.Default.StoredActions.RememberedActions where rua.IsMatch( strUrl ) from br in browsers where !string.IsNullOrEmpty( rua.SelectedBrowser.Find( a => a.Equals( br.DisplayName, StringComparison.CurrentCultureIgnoreCase ) ) ) select br ) {
					br.OpenUrl( strUrl );
					hasOpened = true;
				}
			}
			CheckUpdate( );
			if( hasOpened ) {
				Application.Exit( );
			} else {
				Application.EnableVisualStyles( );
				Application.SetCompatibleTextRenderingDefault( false );
				var form = new FrmMain( strUrl, browsers );
				if(ApplicationDeployment.IsNetworkDeployed) {
					form.Text = string.Format( @"Browser Chooser {0}", ApplicationDeployment.CurrentDeployment.CurrentVersion );
				}
				Application.Run( form );
			}
		}

		private static bool RunAsAdmin( string args ) {
			// Run as admin if not already
			bool isAdmin = false;
			try {
				var user = System.Security.Principal.WindowsIdentity.GetCurrent( );				
				var principal = new System.Security.Principal.WindowsPrincipal( user );
				isAdmin = principal.IsInRole( System.Security.Principal.WindowsBuiltInRole.Administrator );
			} catch(Exception) {
				// Do nothing, isAdmin is already false
			}
			if(!isAdmin) {
				// Relaunch as an admin
				
				var processStartInfo = new System.Diagnostics.ProcessStartInfo( );

				processStartInfo.FileName = Application.ExecutablePath;
				processStartInfo.WorkingDirectory = Environment.CurrentDirectory;
				processStartInfo.Verb = "runas";
				processStartInfo.Arguments = args;

				processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
				processStartInfo.UseShellExecute = true;

				//System.Diagnostics.Process process = null;
				try {
					var process = System.Diagnostics.Process.Start( processStartInfo );
				} catch(Exception ex) {
					MessageBox.Show( string.Format( @"Error launching as admin: {0}", ex.Message ) );
				}
			}
			return isAdmin;
		}

		private static void CheckUpdate( ) {
			if( !ApplicationDeployment.IsNetworkDeployed ) {
				return;
			}
			var ad = ApplicationDeployment.CurrentDeployment;
			ad.CheckForUpdateCompleted += new CheckForUpdateCompletedEventHandler( ad_CheckForUpdateCompleted );
			ad.CheckForUpdateAsync( );
		}

		private static void ad_CheckForUpdateCompleted( object sender, CheckForUpdateCompletedEventArgs e ) {
			if(null != e.Error) {
				MessageBox.Show( string.Format( @"Error: Could not update new version of application.  Reason:\n {0}", e.Error.Message ) );
				return;
			} else if(e.Cancelled) {
				return;
			}

			var ad = ApplicationDeployment.CurrentDeployment;
			ad.UpdateCompleted += new AsyncCompletedEventHandler( ad_UpdateCompleted );
			ad.UpdateAsync( );
		}

		private static void ad_UpdateCompleted( object sender, AsyncCompletedEventArgs e ) {
			if(null != e.Error) {
				MessageBox.Show( string.Format( @"Error: Could not update new version of application.  Reason:\n {0}", e.Error.Message ) );
				return;
			} else if(e.Cancelled) {
				return;
			}
			return;		// Let the update happen next time
		}
	}
}

public static class RecursiveDirectory {
	/// <summary>
	/// Recursively create directory
	/// </summary>
	/// <param name="dirInfo">Folder path to create.</param>
	public static void CreateDirectory( this DirectoryInfo dirInfo ) {
		if(dirInfo.Parent != null) {
			CreateDirectory( dirInfo.Parent );
		}

		if(!dirInfo.Exists) {
			dirInfo.Create( );
		}
	}
}