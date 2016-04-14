using IORAMHelper;
using System;
using System.Collections.Generic;

namespace GenieLibrary.DataElements
{
	public class TechTreeNew : IGenieDataElement
	{
		#region Variablen

		/// <summary>
		/// Die Stammelemente. Die Reihenfolge entspricht der Renderreihenfolge.
		/// </summary>
		public List<TechTreeElement> ParentElements;

		#endregion Variablen

		#region Funktionen

		public override void ReadData(RAMBuffer buffer)
		{
			// Stammelemente lesen
			short parentCount = buffer.ReadShort();
			ParentElements = new List<TechTreeElement>();
			for(int i = 0; i < parentCount; ++i)
				ParentElements.Add(new TechTreeElement().ReadDataInline(buffer));
		}

		public override void WriteData(RAMBuffer buffer)
		{
			// Stammelemente schreiben
			buffer.WriteShort((short)ParentElements.Count);
			ParentElements.ForEach(p => p.WriteData(buffer));
		}

		#endregion Funktionen

		#region Strukturen

		public class TechTreeElement : IGenieDataElement
		{
			#region Variablen

			/// <summary>
			/// Der Typ des Elements.
			/// </summary>
			public ItemType ElementType;

			/// <summary>
			/// Die ID des zugehörigen Objekts.
			/// </summary>
			public short ElementObjectID;

			/// <summary>
			/// Das Zeitalter des Elements.
			/// </summary>
			public byte Age;

			/// <summary>
			/// Der Rendermodus des Elements.
			/// </summary>
			public ItemRenderMode RenderMode;

			/// <summary>
			/// IDs der Kulturen, die das aktuelle TechTree-Element sperren
			/// </summary>
			public List<byte> DisableCivs;

			/// <summary>
			/// Die Kindelemente des Elements. Die Reihenfolge entspricht der Renderreihenfolge.
			/// </summary>
			public List<TechTreeElement> Children;

			/// <summary>
			/// Die Elemente, die für das aktuelle Voraussetzung sind. Nur also Folge von IDs und Typen gespeichert, da nicht zwingend im Baum enthalten.
			/// </summary>
			public List<Tuple<ItemType, short>> RequiredElements;

			#endregion Variablen

			#region Funktionen

			public override void ReadData(RAMBuffer buffer)
			{
				// Eigenschaften lesen
				ElementType = (ItemType)buffer.ReadByte();
				ElementObjectID = buffer.ReadShort();
				Age = buffer.ReadByte();
				RenderMode = (ItemRenderMode)buffer.ReadByte();

				// Deaktivierende Kulturen lesen
				byte disableCivCount = buffer.ReadByte();
				DisableCivs = new List<byte>();
				for(int i = 0; i < disableCivCount; ++i)
					DisableCivs.Add(buffer.ReadByte());

				// Kindelemente lesen
				short childrenCount = buffer.ReadShort();
				Children = new List<TechTreeElement>();
				for(int i = 0; i < childrenCount; ++i)
					Children.Add(new TechTreeElement().ReadDataInline(buffer));

				// Benötigte Elemente lesen
				short requireCount = buffer.ReadShort();
				RequiredElements = new List<Tuple<ItemType, short>>();
				for(int i = 0; i < requireCount; ++i)
					RequiredElements.Add(new Tuple<ItemType, short>((ItemType)buffer.ReadByte(), buffer.ReadShort()));
			}

			public override void WriteData(RAMBuffer buffer)
			{
				// Eigenschaften schreiben
				buffer.WriteByte((byte)ElementType);
				buffer.WriteShort(ElementObjectID);
				buffer.WriteByte(Age);
				buffer.WriteByte((byte)RenderMode);

				// Deaktivierende Kulturen schreiben
				buffer.WriteByte((byte)DisableCivs.Count);
				DisableCivs.ForEach(c => buffer.WriteByte(c));

				// Kindelemente schreiben
				buffer.WriteShort((short)Children.Count);
				Children.ForEach(c => c.WriteData(buffer));

				// Benötigte Elemente schreiben
				buffer.WriteShort((short)RequiredElements.Count);
				RequiredElements.ForEach(r => { buffer.WriteByte((byte)r.Item1); buffer.WriteShort(r.Item2); });
			}

			#endregion Funktionen

			#region Enumerationen

			/// <summary>
			/// Die möglichen Elementtypen (entscheidend für die Farbe der gerenderten Kästchen, kann auch den eigentlichen Einheiten-Typen widersprechen).
			/// </summary>
			public enum ItemType : byte
			{
				Research = 0,
				Creatable = 1,
				Building = 2
			}

			/// <summary>
			/// Die möglichen Rendermodi eines TechTree-Elements.
			/// </summary>
			public enum ItemRenderMode : byte
			{
				/// <summary>
				/// Standard-Modus. Falls das Element deaktiviert ist, wird es ausgegraut.
				/// </summary>
				Standard = 0,

				/// <summary>
				/// Blendet das Element vollständig aus, falls dieses deaktiviert ist (empfehlenswert bei Nicht-Standardelementen).
				/// </summary>
				HideIfDisabled = 1
			}

			#endregion Enumerationen
		}

		#endregion Strukturen
	}
}