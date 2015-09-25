using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace BrowserChooser {
	[Serializable]
	public class Protocol: ISerializable {

		public string Proto { get; set; }
		public string Desc { get; set; }

		public Protocol( ) { }

		public Protocol( string proto, string desc ) {
			Proto = proto;
			Desc = desc;
		}

		protected Protocol( SerializationInfo info, StreamingContext ctxt ) {
			Proto = (string)info.GetValue( @"Protocol", typeof( string ) );
			Desc = (string)info.GetValue( @"Desc", typeof( string ) );
		}
		
		[SecurityPermission( SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter )]
		public virtual void GetObjectData( SerializationInfo info, StreamingContext context ) {
			info.AddValue( @"Protocol", Proto);
			info.AddValue( @"Desc", Desc );
		}
	}

	[Serializable]
	public class Protocols: ISerializable {
		private List<Protocol> _protocols = new List<Protocol>( );
		public List<Protocol> Items { get { return _protocols; } set { _protocols = value; } }

		public Protocols( ) { }

		public Protocols( IEnumerable<Protocol> protos ) {
			_protocols = new List<Protocol>( protos );
		}

		public Protocols( Protocols p ) {
			_protocols = new List<Protocol>( p._protocols );
		}

		protected Protocols( SerializationInfo info, StreamingContext ctxt ) {
			Items = (List<Protocol>)info.GetValue( @"Protocols", typeof( List<Protocol> ) );
		}

		[SecurityPermission( SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter )]
		public virtual void GetObjectData( SerializationInfo info, StreamingContext context ) {			
			info.AddValue( @"Desc", Items );
		}
	}
}
