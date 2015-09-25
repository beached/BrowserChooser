using System;
using System.Runtime.InteropServices;

namespace BrowserChooser.COM {
	public enum ASSOCIATIONLEVEL {
		AL_MACHINE,
		AL_EFFECTIVE,
		AL_USER,
	};

	public enum ASSOCIATIONTYPE {
		AT_FILEEXTENSION,
		AT_URLPROTOCOL,
		AT_STARTMENUCLIENT,
		AT_MIMETYPE,
	};

	[Guid( "4e530b0a-e611-4c77-a3ac-9031d022281b" ), InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
	public interface IApplicationAssociationRegistration {
		void QueryCurrentDefault( [In, MarshalAs( UnmanagedType.LPWStr )] string pszQuery,
		 [In, MarshalAs( UnmanagedType.I4 )] ASSOCIATIONTYPE atQueryType,
		 [In, MarshalAs( UnmanagedType.I4 )] ASSOCIATIONLEVEL alQueryLevel,
		 [Out, MarshalAs( UnmanagedType.LPWStr )] out string ppszAssociation );

		void QueryAppIsDefault(
			[In, MarshalAs( UnmanagedType.LPWStr )] string pszQuery,
			[In] ASSOCIATIONTYPE atQueryType,
			[In] ASSOCIATIONLEVEL alQueryLevel,
			[In, MarshalAs( UnmanagedType.LPWStr )] string pszAppRegistryName,
			[Out] out bool pfDefault );

		void QueryAppIsDefaultAll(
			[In] ASSOCIATIONLEVEL alQueryLevel,
			[In, MarshalAs( UnmanagedType.LPWStr )] string pszAppRegistryName,
			[Out] out bool pfDefault );

		void SetAppAsDefault(
			[In, MarshalAs( UnmanagedType.LPWStr )] string pszAppRegistryName,
			[In, MarshalAs( UnmanagedType.LPWStr )] string pszSet,
			[In] ASSOCIATIONTYPE atSetType );

		void SetAppAsDefaultAll(
			[In, MarshalAs( UnmanagedType.LPWStr )] string pszAppRegistryName );

		void ClearUserAssociations( );
	}

	[ComImport, Guid( "591209c7-767b-42b2-9fba-44ee4615f2c7" )]//
	class ApplicationAssociationRegistration {
	}
}
