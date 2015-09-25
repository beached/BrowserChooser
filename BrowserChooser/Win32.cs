using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BrowserChooser {
	public static class NativeMethods {
		private const uint LoadLibraryAsDatafile = 0x00000002;

		[DllImport( "user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true )]
		private static extern int LoadStringW( IntPtr hInstance, uint uId, StringBuilder lpBuffer, int nBufferMax );

		[DllImport( "kernel32.dll" )]
		static extern IntPtr LoadLibraryEx( string lpFileName, IntPtr hFile, uint dwFlags );

		private static IntPtr LoadResourceLibrary( string lpFileName ) {
			return LoadLibraryEx( lpFileName, IntPtr.Zero, LoadLibraryAsDatafile );
		}

		[DllImport( "kernel32.dll" )]
		private static extern bool FreeLibrary( IntPtr lib );

		[DllImport( "kernel32.dll", CharSet = CharSet.Auto, EntryPoint="GetShortPathName" )]
		private static extern int _GetShortPathName( [MarshalAs( UnmanagedType.LPTStr )]string path, [MarshalAs( UnmanagedType.LPTStr )] StringBuilder shortPath, int shortPathLength );

		public static string GetShortPathName( string longPath ) {
			var shortPath = new StringBuilder( 1024 );
			_GetShortPathName( longPath, shortPath, shortPath.Capacity );
			return shortPath.ToString( );
		}

		public static string ReadStringResource( IntPtr hInstance, uint uResId ) {
			const int maxlen = 1024;
			var sb = new StringBuilder( maxlen );
			LoadStringW( hInstance, uResId, sb, maxlen );
			return sb.ToString( );
		}

		public static string GetResourceFromFile( string fileName, uint resouceId ) {			
			IntPtr hInstance;
			if( (hInstance = LoadResourceLibrary( fileName )) == IntPtr.Zero ) {
				return string.Empty;
			}
			try {
				return ReadStringResource( hInstance, resouceId );
			} finally {
				FreeLibrary( hInstance );
			}
		}

		[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Auto )]
		public struct Shfileinfo {
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 260 )]
			public string szDisplayName;
			[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 80 )]
			public string szTypeName;
		};
	}
}
