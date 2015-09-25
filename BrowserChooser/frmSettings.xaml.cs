using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BrowserChooser {
	/// <summary>
	/// Interaction logic for frmSettings.xaml
	/// </summary>
	public partial class frmSettings: Window {
		private RememberedUrlActions _browserActions;

		public frmSettings( ) {
			InitializeComponent( );
		}

		private void btnMakeDefault_Click( object sender, RoutedEventArgs e ) {
			Browser.Register( );
		}

		private void btnClose_Click( object sender, RoutedEventArgs e ) {
			Close( );
		}

		private void btnSave_Click( object sender, RoutedEventArgs e ) {
			// Save Settings
			Close( );
		}

		private void Window_Loaded( object sender, RoutedEventArgs e ) {
			if( Properties.Settings.Default.StoredActions != null && Properties.Settings.Default.StoredActions.remembered_actions.Count > 0 ) {
				_browserActions = Properties.Settings.Default.StoredActions;
				lstRemeberedBrowsers.ItemsSource = _browserActions.remembered_actions;
			}
		}
	}
}
