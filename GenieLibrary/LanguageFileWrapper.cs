using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace GenieLibrary
{
	/// <summary>
	/// Verwaltet eine Liste von Language-DLL-Dateien, aus denen eingebettete String-Ressourcen ausgelesen werden können.
	/// </summary>
	public class LanguageFileWrapper
	{
		#region Konstanten

		/// <summary>
		/// Die maximale Länge eines abgerufenen Strings.
		/// </summary>
		public const int MAX_STRING_LENGTH = 1024;

		#endregion Konstanten

		#region Variablen

		/// <summary>
		/// Die Zeiger auf die geladenen DLL-Dateien.
		/// </summary>
		private List<IntPtr> _filePointers;

		#endregion Variablen

		#region Funktionen

		/// <summary>
		/// Lädt die angegebenen Dateien. Diese sollten absteigend nach Priorität sortiert sein, da Strings in derselben Reihenfolge gesucht werden, wie die Dateinamen übergeben wurden.
		/// </summary>
		/// <param name="files">Die zu ladenden Dateien.</param>
		public LanguageFileWrapper(params string[] files)
		{
			// Dateien laden im "LOAD_LIBRARY_AS_DATAFILE"-Modus laden
			_filePointers = new List<IntPtr>();
			foreach(string currF in files)
				_filePointers.Add(LoadLibraryEx(currF, IntPtr.Zero, 2));

			// Wenn eine Datei nicht geladen werden konnte, Fehlermeldung anzeigen
			if(_filePointers.Exists(fp => fp == IntPtr.Zero))
				MessageBox.Show("DLL-Error #" + Marshal.GetLastWin32Error());
		}

		/// <summary>
		/// Destruktor. Gibt die geladenen DLLs wieder frei.
		/// </summary>
		~LanguageFileWrapper()
		{
			// Handles freigeben
			_filePointers.ForEach(p => FreeLibrary(p));
		}

		/// <summary>
		/// Ruft den String mit der gegebenen ID ab. Wenn dieser nicht gefunden wird, wird ein leerer String zurückgegeben.
		/// </summary>
		/// <param name="stringID">Die ID des abzurufenden Strings.</param>
		/// <returns></returns>
		public string GetString(uint stringID)
		{
			// Die Liste durchlaufen, bis ein nicht-leerer String zurückgegeben wurde oder alle DLLs abgefragt worden sind
			StringBuilder result = new StringBuilder(MAX_STRING_LENGTH);
			foreach(IntPtr p in _filePointers)
				if(LoadString(p, stringID, result, MAX_STRING_LENGTH) != 0)
					break;

			// Ergebnis zurückgeben
			return result.ToString();
		}

		#endregion Funktionen

		#region Importierte Funktionen

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr LoadLibraryEx(string path, IntPtr zero, int flags);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool FreeLibrary(IntPtr handle);

		[DllImport("user32.dll")]
		public static extern int LoadString(IntPtr instance, uint stringID, StringBuilder buffer, int bufferSize);

		#endregion Importierte Funktionen
	}
}