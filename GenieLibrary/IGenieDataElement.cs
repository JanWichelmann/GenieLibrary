using IORAMHelper;
using System;

namespace GenieLibrary
{
	/// <summary>
	/// Definiert Eigenschaften eines DAT-Datei-Datenelements.
	/// </summary>
	public class IGenieDataElement
	{
		#region Hilfsfunktionen

		/// <summary>
		/// Stellt sicher, dass die übergebene Liste die angegebene Länge hat, indem im Fehlerfall eine Exception ausgelöst wird.
		/// </summary>
		/// <param name="list">Die zu überprüfende Liste.</param>
		/// <param name="length">Die Soll-Länge der Liste.</param>
		public static void AssertListLength(System.Collections.ICollection list, int length)
		{
			// Länge abgleichen
			if(list.Count != length)
			{
				// Fehler auslösen
				throw new AssertionException("Die Listenlänge (" + list.Count + ") weicht von der Soll-Länge (" + length + ") ab.");
			}
		}

		/// <summary>
		/// Stellt sicher, dass der übergebene Wert true ist, sonst wird eine Exception ausgelöst.
		/// </summary>
		/// <param name="value">Der zu überprüfende Wahrheitswert.</param>
		public static void AssertTrue(bool value)
		{
			// Länge abgleichen
			if(!value)
			{
				// Fehler auslösen
				throw new AssertionException("Der Ausdruck wurde unerwartet zu false ausgewertet.");
			}
		}

		#endregion Hilfsfunktionen

		#region Strukturen

		/// <summary>
		/// Definiert das übliche Ressourcen-Tupel (Typ, Menge, Aktiviert) mit frei wählbaren Datentypen.
		/// </summary>
		/// <typeparam name="T">Der Datentyp des Ressourcen-Typs.</typeparam>
		/// <typeparam name="A">Der Datentyp der Ressourcen-Menge.</typeparam>
		/// <typeparam name="E">Der Datentyp des Ressourcen-Aktivierungs-Flags.</typeparam>
		public struct ResourceTuple<T, A, E>
		{
			public T Type;
			public A Amount;
			public E Mode;
		}

		#endregion Strukturen

		#region Exceptions

		/// <summary>
		/// Definiert die bei einer fehlerhaften Überprüfung ausgelöste Exception.
		/// </summary>
		public class AssertionException : Exception
		{
			#region Funktionen

			/// <summary>
			/// Löst eine neue Assert-Exception aus.
			/// </summary>
			public AssertionException()
				: base("Assertion fehlgeschlagen.")
			{ }

			/// <summary>
			/// Löst eine neue Assert-Exception mit der angegebenen Fehlermeldung aus.
			/// </summary>
			/// <param name="message">Die zugehörige Fehlermeldung.</param>
			public AssertionException(string message)
				: base(message)
			{ }

			#endregion Funktionen
		}

		#endregion Exceptions
	}
}