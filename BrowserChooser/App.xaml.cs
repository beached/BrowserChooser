using System.Windows;

namespace BrowserChooser {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App: Application {		

		protected override void OnStartup( StartupEventArgs e ) {
			
			if( e.Args != null && e.Args.Length > 0 ) {
				this.Properties["Url"] = e.Args[0].Trim( );
			}
			base.OnStartup( e );
		}
	}
}
