using System.Collections.ObjectModel;
using System.Windows;


namespace BrowserChooser {
	/// <summary>
	/// Interaction logic for winMain.xaml
	/// </summary>
	public partial class winMain: Window {
		private ObservableCollection<Browser> _browsers;

		public winMain( ) {
			InitializeComponent( );
		}

		private void btnCancel_Click( object sender, RoutedEventArgs e ) {
			Application.Current.Shutdown( );			
		}

		private void lstBrowsers_Check( object sender, RoutedEventArgs e ) {
			string strBrowList = string.Empty;
			foreach( Browser br in _browsers ) {
				if( br.Checked ) {
					if( !string.IsNullOrEmpty( strBrowList ) ) {
						strBrowList += @";";
					}
					strBrowList += br.KeyName;					
				}
			}
			Properties.Settings.Default.CheckedBrowsers = strBrowList;
			Properties.Settings.Default.Save( );
		}

		private void btnOpen_Click( object sender, RoutedEventArgs e ) {
			foreach( Browser br in _browsers ) {
				if( br.Checked ) {
					string url = txtUrl.Text.Trim( );
					RememberedUrlAction rua = new RememberedUrlAction( url, br.DisplayName, false );
					if( Properties.Settings.Default.StoredActions == null ) {
						Properties.Settings.Default.StoredActions = new RememberedUrlActions( );
					}
					Properties.Settings.Default.StoredActions.remembered_actions.Add( rua );
					Properties.Settings.Default.Save( );
					br.openUrl( url );
				}
				Application.Current.Shutdown( );
			}
		}

		private void Window_Loaded( object sender, RoutedEventArgs e ) {
			_browsers = new ObservableCollection<Browser>( Browser.getSystemBrowsers( ) );
			if( !Properties.Settings.Default.hasSetDefault ) {
				if( MessageBox.Show( @"Would you like to make Browser Chooser your default URL handler?", @"Default Browser Check", MessageBoxButton.YesNo ) == MessageBoxResult.Yes ) {
					Browser.Register( );
				}
			}
			txtUrl.Text = Application.Current.Properties["Url"] as string;
			lstBrowsers.ItemsSource = _browsers;
		}

		private void btnSettings_Click( object sender, RoutedEventArgs e ) {
			frmSettings fs = new frmSettings( );
			fs.Left = Left;
			fs.Top = Top;
			try {
				this.Visibility = Visibility.Hidden;
				fs.ShowDialog( );
			} finally {
				this.Left = fs.Left;
				this.Top = fs.Top;
				this.Visibility = Visibility.Visible;
			}
			
		}
	}
}
