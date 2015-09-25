using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace BrowserChooser {
	[Serializable]
	public class RememberedUrlAction: ISerializable {			
		public string PageUrl { get; set; }
		public List<string> SelectedBrowser { get; set; }
		public bool IsRegex { get; set; }
		public bool AutoOpen { get; set; }

		#region Constructors
		public RememberedUrlAction( ) { 
			PageUrl = string.Empty;
			SelectedBrowser = new List<string>( );
			IsRegex = false;
			AutoOpen = false;
		}

		public RememberedUrlAction( string url, List<string> browser ) {
			PageUrl = url;
			SelectedBrowser = browser; 
			IsRegex = false;
			AutoOpen = false;
		}

		public RememberedUrlAction( string url, string browser ) {
			PageUrl = url;
			SelectedBrowser = new List<string> { browser };
			IsRegex = false;
		}

		protected RememberedUrlAction( SerializationInfo info, StreamingContext ctxt ) {
			PageUrl = (string)info.GetValue( @"PageUrl", typeof( string ) );
			SelectedBrowser = (List<string>)info.GetValue( @"SelectedBrowser", typeof( bool ) );
			IsRegex = (bool)info.GetValue( @"IsRegularExpression", typeof( string ) );
			var autoOpen = info.GetValue( @"AutoOpen", typeof( string ) );
			AutoOpen = string.IsNullOrEmpty( (string)autoOpen ) ? false : (bool)autoOpen;
		}

		public RememberedUrlAction( string url, string browser, bool isRegex, bool autoOpen ) {
			PageUrl = url;
			SelectedBrowser = new List<string> { browser };
			IsRegex = isRegex;
			AutoOpen = autoOpen;
		}

		public RememberedUrlAction( string url, List<string> browser, bool isRegex, bool autoOpen ) {
			PageUrl = url;
			SelectedBrowser = browser;
			IsRegex = isRegex;
			AutoOpen = autoOpen;
		}


		#endregion

		[SecurityPermission( SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter )]
		public virtual void GetObjectData( SerializationInfo info, StreamingContext context ) {
			info.AddValue( @"PageUrl", PageUrl );
			info.AddValue( @"SelectedBrowser", SelectedBrowser );
			info.AddValue( @"IsRegularExpression", IsRegex );
			info.AddValue( @"AutoOpen", AutoOpen );
		}

		public bool IsMatch( string url ) {
			bool ret;
			if( IsRegex ) {
				var pagePattern = new Regex( PageUrl, RegexOptions.IgnoreCase | RegexOptions.Singleline );
				ret = pagePattern.IsMatch( url );				
			} else {
				ret = url.Equals( PageUrl, StringComparison.CurrentCultureIgnoreCase );
			}
			return ret;
		}
	}
	
	[Serializable]
	public class RememberedUrlActions: ISerializable {
		public ObservableCollection<RememberedUrlAction> RememberedActions;

		public RememberedUrlActions( ) {
			RememberedActions = new ObservableCollection<RememberedUrlAction>( );
		}

		protected RememberedUrlActions( SerializationInfo info, StreamingContext ctxt ) {
			RememberedActions = (ObservableCollection<RememberedUrlAction>)info.GetValue( @"RememberedUrlActions", typeof( ObservableCollection<RememberedUrlAction> ) );
		}

		public RememberedUrlActions( RememberedUrlActions rua ) {
			RememberedActions = new ObservableCollection<RememberedUrlAction>( rua.RememberedActions );
		}

		[SecurityPermission( SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter )]
		public virtual void GetObjectData( SerializationInfo info, StreamingContext context ) {
			info.AddValue( @"RememberedUrlActions", RememberedActions );
		}
	}
}
