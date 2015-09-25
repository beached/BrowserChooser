using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace BrowserChooser {
	public partial class FrmMain: Form {
		private readonly List<Browser> _availableBrowsers;
		private readonly string _urlToOpen;

		public FrmMain( string url, IEnumerable<Browser> browsers ) {
			_urlToOpen = url;
			_availableBrowsers = new List<Browser>( browsers );			
			InitializeComponent( );					
		}

		private void frmMain_Load( object sender, EventArgs e ) {
			foreach( var currentBrowser in _availableBrowsers ) {
				lstBrowsers.Items.Add( currentBrowser.DisplayName, currentBrowser.Checked );
			}
			
			lstBrowsers.ItemCheck += delegate( object o, ItemCheckEventArgs args ) {
				if( args.NewValue == args.CurrentValue ) {
					return;
				} 
				var isChecked = args.NewValue == CheckState.Checked;
				foreach( var currentBrowser in _availableBrowsers.Where( currentBrowser => currentBrowser.DisplayName.Equals( (string)lstBrowsers.Items[args.Index] ) ) ) {
					currentBrowser.Checked = isChecked;
					break;
				}
			};
			txtUrl.Text = _urlToOpen;			
		}

		private void btnCancel_Click( object sender, EventArgs e ) {
			Application.Exit( );
		}

		private void btnOpen_Click( object sender, EventArgs e ) {
			foreach( var currentBrowser in _availableBrowsers ) {
				if( !currentBrowser.Checked ) {
					continue;
				}
				var url = txtUrl.Text.Trim( );
				if( chkRemember.Checked ) {
					var rua = new RememberedUrlAction { PageUrl = url, SelectedBrowser = new List<string>{ currentBrowser.DisplayName } };
					var ruas = Properties.Settings.Default.StoredActions ?? new RememberedUrlActions( );
					ruas.RememberedActions.Add( rua );
					Properties.Settings.Default.StoredActions = ruas;
					Properties.Settings.Default.Save( );
				}
				currentBrowser.OpenUrl( url );
			}
			SaveCheckedBrowsers( );
			Application.Exit( );
		}

		private void SaveCheckedBrowsers( ) {
			var strBrowserList = string.Empty;
			foreach( var currentBrowser in _availableBrowsers.Where( browser => browser.Checked ) ) {
				if( !string.IsNullOrEmpty( strBrowserList ) ) {
					strBrowserList += @";";
				}
				strBrowserList += currentBrowser.KeyName;
			}
			Properties.Settings.Default.CheckedBrowsers = strBrowserList;
			Properties.Settings.Default.Save( );
		}

		private void btnSettings_Click( object sender, EventArgs e ) {
			var fs = new FrmSettings {Left = Left, Top = Top, Width = Width, Height = Height};
			try {
				Visible = false;
				fs.ShowDialog( );
			} finally {
				Left = fs.Left;
				Top = fs.Top;
				Visible = true;
			}
		}
	}
}
