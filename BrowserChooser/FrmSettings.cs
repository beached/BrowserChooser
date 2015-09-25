using System;
using System.Windows.Forms;

namespace BrowserChooser {
	public partial class FrmSettings: Form {
		private RememberedUrlActions _browserActions;

		public FrmSettings( ) {
			InitializeComponent( );
		}

		private void btnClose_Click( object sender, EventArgs e ) {
			Close( );
		}

		private void btnSave_Click( object sender, EventArgs e ) {
			Close( );
		}

		private void btnMakeDefault_Click( object sender, EventArgs e ) {
			if(null != Properties.Settings.Default.StoredActions && 0 < Properties.Settings.Default.StoredActions.RememberedActions.Count) {
				_browserActions = Properties.Settings.Default.StoredActions;
				//lstRemeberedBrowsers.ItemsSource = _browserActions.remembered_actions;
			}
			Browser.Register( );
		}
	}
}
