using IORAMHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GenieLibrary.DataElements
{
	public class TechTreeNew
	{
		#region Konstanten

		/// <summary>
		/// Die aktuelle Version des TechTree-Datenformats.
		/// </summary>
		private const byte NEW_TECH_TREE_VERSION = 1;

		#endregion

		#region Variablen

		/// <summary>
		/// Die Stammelemente. Die Reihenfolge entspricht der Renderreihenfolge.
		/// </summary>
		public List<TechTreeElement> ParentElements;

		/// <summary>
		/// Die Design-Spezifikationen.
		/// </summary>
		public TechTreeDesign DesignData;

		#endregion Variablen

		#region Funktionen

		public TechTreeNew ReadData(RAMBuffer buffer)
		{
			// Alles lesen
			return ReadData(buffer, false);
		}

		/// <summary>
		/// Lädt die TechTree-Daten.
		/// </summary>
		/// <param name="buffer">Der Datenpuffer.</param>
		/// <param name="readTreeOnly">Gibt an, ob nur die Baumdaten (und nicht auch das Design) geladen werden sollen.</param>
		public TechTreeNew ReadData(RAMBuffer buffer, bool readTreeOnly)
		{
			// Versionsbyte lesen
			byte version = buffer.ReadByte();
			if(version == (byte)'1')
				version = 0; // Legacy, TODO in ein paar Monaten entfernen...
			if(version > NEW_TECH_TREE_VERSION)
				throw new Exception("This file was created with a newer version of this program. Please consider updating.");

			// Stammelemente lesen
			short parentCount = buffer.ReadShort();
			ParentElements = new List<TechTreeElement>();
			for(int i = 0; i < parentCount; ++i)
				ParentElements.Add(new TechTreeElement().ReadData(buffer, version));

			// Design lesen
			if(!readTreeOnly)
			{
				// Nur wenn vorhanden, um Kompatibilität zu älteren Dateien zu erhalten
				DesignData = new TechTreeDesign();
				if(buffer.Position < buffer.Length)
					DesignData.ReadData(buffer, version);
			}

			return this;
		}

		public void WriteData(RAMBuffer buffer)
		{
			// Alles schreiben
			WriteData(buffer, false);
		}

		/// <summary>
		/// Schreibt die TechTree-Daten.
		/// </summary>
		/// <param name="buffer">Der Datenpuffer.</param>
		/// <param name="writeTreeOnly">Gibt an, ob nur die Baumdaten (und nicht auch das Design) geschrieben werden sollen.</param>
		public void WriteData(RAMBuffer buffer, bool writeTreeOnly)
		{
			// Version schreiben
			buffer.WriteByte(NEW_TECH_TREE_VERSION);

			// Stammelemente schreiben
			buffer.WriteShort((short)ParentElements.Count);
			ParentElements.ForEach(p => p.WriteData(buffer));

			// Designdaten schreiben
			if(!writeTreeOnly)
				DesignData.WriteData(buffer);
		}

		#endregion Funktionen

		#region Strukturen

		public class TechTreeElement
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

			/// <summary>
			/// Der Index des Node-Hintergrunds.
			/// </summary>
			public int NodeBackgroundIndex;

			#endregion Variablen

			#region Funktionen

			public TechTreeElement ReadData(RAMBuffer buffer, byte version = NEW_TECH_TREE_VERSION)
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
					Children.Add(new TechTreeElement().ReadData(buffer, version));

				// Benötigte Elemente lesen
				short requireCount = buffer.ReadShort();
				RequiredElements = new List<Tuple<ItemType, short>>();
				for(int i = 0; i < requireCount; ++i)
					RequiredElements.Add(new Tuple<ItemType, short>((ItemType)buffer.ReadByte(), buffer.ReadShort()));

				// Node-Hintergrund lesen
				if(version >= 1)
					NodeBackgroundIndex = buffer.ReadInteger();
				else
					NodeBackgroundIndex = (int)ElementType;

				return this;
			}

			public void WriteData(RAMBuffer buffer)
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

				// Node-Hintergrund schreiben
				buffer.WriteInteger(NodeBackgroundIndex);
			}

			#endregion Funktionen

			#region Enumerationen

			/// <summary>
			/// Die möglichen Elementtypen.
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

		public class TechTreeDesign
		{
			#region Variablen

			#region SLPs

			/// <summary>
			/// Der Dateiname der Knoten-Hintergrund-SLP. Frames müssen die Abmessungen 64x64 haben.
			/// </summary>
			public string NodeSlpFileName;

			/// <summary>
			/// Die Ressourcen-ID der Knoten-Hintergrund-SLP. Frames müssen die Abmessungen 64x64 haben.
			/// </summary>
			public int NodeSlpId;

			/// <summary>
			/// Der Dateiname der Knoten-Hintergrund-SLP. Frames müssen die Abmessungen 64x64 haben.
			/// </summary>
			public string ScrollSlpFileName;

			/// <summary>
			/// Die Ressourcen-ID der Knoten-Hintergrund-SLP. Frames müssen die Abmessungen 64x64 haben.
			/// </summary>
			public int ScrollSlpId;

			/// <summary>
			/// Der Dateiname der Knoten-Hintergrund-SLP. Frames müssen die Abmessungen 64x64 haben.
			/// </summary>
			public string TileSlpFileName;

			/// <summary>
			/// Die Ressourcen-ID der Knoten-Hintergrund-SLP. Frames müssen die Abmessungen 64x64 haben.
			/// </summary>
			public int TileSlpId;

			/// <summary>
			/// Der Dateiname der Knoten-Hintergrund-SLP. Frames müssen die Abmessungen 64x64 haben.
			/// </summary>
			public string LegendAgesSlpFileName;

			/// <summary>
			/// Die Ressourcen-ID der Knoten-Hintergrund-SLP. Frames müssen die Abmessungen 64x64 haben.
			/// </summary>
			public int LegendAgesSlpId;

			/// <summary>
			/// Der Dateiname der Knoten-Hintergrund-SLP. Frames müssen die Abmessungen 64x64 haben.
			/// </summary>
			public string LegendDisableSlpFileName;

			/// <summary>
			/// Die Ressourcen-ID der Knoten-Hintergrund-SLP. Frames müssen die Abmessungen 64x64 haben.
			/// </summary>
			public int LegendDisableSlpId;

			#endregion

			#region Scroll-Einstellungen

			/// <summary>
			/// Der Abstand des Maus-Scrollbereichs vom Bildschirmrand.
			/// </summary>
			public int MouseScrollArea;

			/// <summary>
			/// Der Zeitabstand zwischen zwei Maus-Scroll-Aktionen.
			/// </summary>
			public int MouseScrollDelay;

			/// <summary>
			/// Die Länge eines Maus-Scrollschritts.
			/// </summary>
			public int MouseScrollOffset;

			/// <summary>
			/// Die Länge einen Tastatur-Scrollschritts.
			/// </summary>
			public int KeyScrollOffset;

			#endregion

			#region Button-Rechtecke

			/// <summary>
			/// Das auf den unteren rechten Bildschirmrand bezogene Rechteck des Schließen-Buttons.
			/// Datenreihenfolge: X, Y, Breite, Höhe.
			/// </summary>
			public Rectangle CloseButtonRelativeRectangle;

			/// <summary>
			/// Das auf den unteren rechten Bildschirmrand bezogene Rechteck des Nach-Links-Buttons.
			/// Datenreihenfolge: X, Y, Breite, Höhe.
			/// </summary>
			public Rectangle ScrollLeftButtonRelativeRectangle;

			/// <summary>
			/// Das auf den unteren rechten Bildschirmrand bezogene Rechteck des Nach-Rechts-Buttons.
			/// Datenreihenfolge: X, Y, Breite, Höhe.
			/// </summary>
			public Rectangle ScrollRightButtonRelativeRectangle;

			#endregion

			#region Auflösungsspezifische Einstellungen

			/// <summary>
			/// Die Positions- und Größendaten abhängig von der jeweiligen Mindest-Auflösung. 0 muss enthalten sein.
			/// Die Reihenfolge muss der Frame-Reihenfolge in den zugehörigen SLPs entsprechen.
			/// </summary>
			public Dictionary<int, ResolutionConfiguration> ResolutionData;

			#endregion

			#region Popup-Label

			/// <summary>
			/// Der Zeitabstand zwischen Maus-Hover und Anzeige des Popups.
			/// </summary>
			public int PopupLabelDelay;

			/// <summary>
			/// Die Breite des Popup-Labels.
			/// </summary>
			public int PopupLabelWidth;

			/// <summary>
			/// Der innere Abstand des Popup-Labels zum Box-Rahmen.
			/// </summary>
			public int PopupInnerPadding;

			/// <summary>
			/// Die Rahmenfarben der Popup-Box bezogen auf die 50500-Palette.
			/// </summary>
			public List<byte> PopupBoxBevelColorIndices;

			#endregion

			#region Nodes

			/// <summary>
			/// Der Index der Schriftart der Knotenbeschreibung.
			/// </summary>
			public byte NodeFontIndex;

			/// <summary>
			/// Die Knotenhintergründe.
			/// Enthält immer mindestens drei Elemente, deren Indizes zu den drei möglichen Elementtypen passen.
			/// </summary>
			public List<NodeBackground> NodeBackgrounds;

			#endregion

			#endregion

			#region Funktionen

			public void ReadData(RAMBuffer buffer, byte version = NEW_TECH_TREE_VERSION)
			{
				// SLP-Daten
				NodeSlpFileName = buffer.ReadString(buffer.ReadInteger());
				NodeSlpId = buffer.ReadInteger();
				ScrollSlpFileName = buffer.ReadString(buffer.ReadInteger());
				ScrollSlpId = buffer.ReadInteger();
				TileSlpFileName = buffer.ReadString(buffer.ReadInteger());
				TileSlpId = buffer.ReadInteger();
				LegendAgesSlpFileName = buffer.ReadString(buffer.ReadInteger());
				LegendAgesSlpId = buffer.ReadInteger();
				LegendDisableSlpFileName = buffer.ReadString(buffer.ReadInteger());
				LegendDisableSlpId = buffer.ReadInteger();

				// Scroll-Daten
				MouseScrollArea = buffer.ReadInteger();
				MouseScrollDelay = buffer.ReadInteger();
				MouseScrollOffset = buffer.ReadInteger();
				KeyScrollOffset = buffer.ReadInteger();

				// Button-Rechtecke
				CloseButtonRelativeRectangle = new Rectangle(buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger());
				ScrollLeftButtonRelativeRectangle = new Rectangle(buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger());
				ScrollRightButtonRelativeRectangle = new Rectangle(buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger());

				// Auflösungsdaten
				int count = buffer.ReadInteger();
				ResolutionData = new Dictionary<int, ResolutionConfiguration>();
				for(int i = 0; i < count; ++i)
					ResolutionData.Add(buffer.ReadInteger(), new ResolutionConfiguration().ReadData(buffer));

				// Popup-Label-Daten
				PopupLabelDelay = buffer.ReadInteger();
				PopupLabelWidth = buffer.ReadInteger();
				PopupInnerPadding = buffer.ReadInteger();
				PopupBoxBevelColorIndices = new List<byte>(6);
				for(int i = 0; i < 6; ++i)
					PopupBoxBevelColorIndices.Add(buffer.ReadByte());

				// Node-Daten
				NodeFontIndex = buffer.ReadByte();
				NodeBackgrounds = new List<NodeBackground>();
				if(version >= 1)
				{
					int nodeBackgroundCount = buffer.ReadInteger();
					for(int i = 0; i < nodeBackgroundCount; i++)
						NodeBackgrounds.Add(new NodeBackground().ReadData(buffer));
				}
				else
				{
					NodeBackgrounds.Add(new NodeBackground() { FrameIndex = 4, Name = "Research" });
					NodeBackgrounds.Add(new NodeBackground() { FrameIndex = 2, Name = "Unit" });
					NodeBackgrounds.Add(new NodeBackground() { FrameIndex = 0, Name = "Building" });
				}
			}

			public void WriteData(RAMBuffer buffer)
			{
				// SLP-Daten
				buffer.WriteInteger(NodeSlpFileName.Length);
				buffer.WriteString(NodeSlpFileName);
				buffer.WriteInteger(NodeSlpId);
				buffer.WriteInteger(ScrollSlpFileName.Length);
				buffer.WriteString(ScrollSlpFileName);
				buffer.WriteInteger(ScrollSlpId);
				buffer.WriteInteger(TileSlpFileName.Length);
				buffer.WriteString(TileSlpFileName);
				buffer.WriteInteger(TileSlpId);
				buffer.WriteInteger(LegendAgesSlpFileName.Length);
				buffer.WriteString(LegendAgesSlpFileName);
				buffer.WriteInteger(LegendAgesSlpId);
				buffer.WriteInteger(LegendDisableSlpFileName.Length);
				buffer.WriteString(LegendDisableSlpFileName);
				buffer.WriteInteger(LegendDisableSlpId);

				// Scroll-Daten
				buffer.WriteInteger(MouseScrollArea);
				buffer.WriteInteger(MouseScrollDelay);
				buffer.WriteInteger(MouseScrollOffset);
				buffer.WriteInteger(KeyScrollOffset);

				// Button-Rechtecke
				WriteRectangle(CloseButtonRelativeRectangle, buffer);
				WriteRectangle(ScrollLeftButtonRelativeRectangle, buffer);
				WriteRectangle(ScrollRightButtonRelativeRectangle, buffer);

				// Auflösungsdaten
				buffer.WriteInteger(ResolutionData.Count);
				foreach(var rd in ResolutionData)
				{
					// Daten schreiben
					buffer.WriteInteger(rd.Key);
					rd.Value.WriteData(buffer);
				};

				// Popup-Label-Daten
				buffer.WriteInteger(PopupLabelDelay);
				buffer.WriteInteger(PopupLabelWidth);
				buffer.WriteInteger(PopupInnerPadding);
				IGenieDataElement.AssertListLength(PopupBoxBevelColorIndices, 6);
				for(int i = 0; i < 6; ++i)
					buffer.WriteByte(PopupBoxBevelColorIndices[i]);

				// Node-Daten
				buffer.WriteByte(NodeFontIndex);
				IGenieDataElement.AssertTrue(NodeBackgrounds.Count >= 3);
				buffer.WriteInteger(NodeBackgrounds.Count);
				NodeBackgrounds.ForEach(n => n.WriteData(buffer));
			}

			/// <summary>
			/// Schreibt ein Rechteck in der Reihenfolge X, Y, Breite, Höhe.
			/// </summary>
			/// <param name="rect">Das zu schreibende Rechteck.</param>
			/// <param name="buffer">Der Zielpuffer.</param>
			private static void WriteRectangle(Rectangle rect, RAMBuffer buffer)
			{
				// Rechteck schreiben
				buffer.WriteInteger(rect.X);
				buffer.WriteInteger(rect.Y);
				buffer.WriteInteger(rect.Width);
				buffer.WriteInteger(rect.Height);
			}

			#endregion

			#region Strukturen

			public class ResolutionConfiguration
			{
				#region Variablen

				/// <summary>
				/// Der SLP-Index der Legenden-Spalte.
				/// </summary>
				public int LegendFrameIndex;

				/// <summary>
				/// Der SLP-Index der Zeitalter-Spalte. Frame 0 ist hierbei ohne Markierung, danach pro Zeitalter ein Frame.
				/// </summary>
				public int AgeFrameIndex;

				/// <summary>
				/// Der SLP-Index der Tile-Grafik.
				/// </summary>
				public int TileFrameIndex;

				/// <summary>
				/// Die Zeichenposition des "Deaktiviert"-Symbols in der Legende.
				/// </summary>
				public Point LegendDisableSlpDrawPosition;

				/// <summary>
				/// Das Rechteck des Kultur-Bonus und -Beschreibungs-Labels.
				/// </summary>
				public Rectangle CivBonusLabelRectangle;

				/// <summary>
				/// Das Rechteck des Kultur-Auswahlfelds.
				/// </summary>
				public Rectangle CivSelectionComboBoxRectangle;

				/// <summary>
				/// Das Rechteck des Labels über dem Kultur-Auswahlfeld.
				/// </summary>
				public Rectangle CivSelectionTitleLabelRectangle;

				/// <summary>
				/// Die Rechtecke der Legenden-Label (6 Stück).
				/// </summary>
				public List<Rectangle> LegendLabelRectangles;

				/// <summary>
				/// Die linken Zeitalter-Beschriftungs-Rechtecke. Sollte geradzahlig sein (jeweils obere und untere Zeile aufeinanderfolgend).
				/// Fehlende Werte werden interpoliert.
				/// </summary>
				public List<Rectangle> AgeLabelRectangles;

				/// <summary>
				/// Die Zeitalter-Zeichen-Offsets (immer auf den oberen Bildschirmrand bezogen). Sollte geradzahlig sein (jeweils obere und untere Zeile aufeinanderfolgend).
				/// Fehlende Werte werden interpoliert.
				/// </summary>
				public List<int> VerticalDrawOffsets;

				#endregion

				#region Funktionen

				public ResolutionConfiguration ReadData(RAMBuffer buffer)
				{
					LegendFrameIndex = buffer.ReadInteger();
					AgeFrameIndex = buffer.ReadInteger();
					TileFrameIndex = buffer.ReadInteger();
					LegendDisableSlpDrawPosition = new Point(buffer.ReadInteger(), buffer.ReadInteger());
					CivBonusLabelRectangle = new Rectangle(buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger());
					CivSelectionComboBoxRectangle = new Rectangle(buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger());
					CivSelectionTitleLabelRectangle = new Rectangle(buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger());

					LegendLabelRectangles = new List<Rectangle>(6);
					for(int i = 0; i < 6; ++i)
						LegendLabelRectangles.Add(new Rectangle(buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger()));

					int count = buffer.ReadInteger();
					AgeLabelRectangles = new List<Rectangle>(count);
					for(int i = 0; i < count; ++i)
						AgeLabelRectangles.Add(new Rectangle(buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger(), buffer.ReadInteger()));

					count = buffer.ReadInteger();
					VerticalDrawOffsets = new List<int>(count);
					for(int i = 0; i < count; ++i)
						VerticalDrawOffsets.Add(buffer.ReadInteger());

					return this;
				}

				public void WriteData(RAMBuffer buffer)
				{
					buffer.WriteInteger(LegendFrameIndex);
					buffer.WriteInteger(AgeFrameIndex);
					buffer.WriteInteger(TileFrameIndex);
					buffer.WriteInteger(LegendDisableSlpDrawPosition.X);
					buffer.WriteInteger(LegendDisableSlpDrawPosition.Y);
					WriteRectangle(CivBonusLabelRectangle, buffer);
					WriteRectangle(CivSelectionComboBoxRectangle, buffer);
					WriteRectangle(CivSelectionTitleLabelRectangle, buffer);

					IGenieDataElement.AssertListLength(LegendLabelRectangles, 6);
					LegendLabelRectangles.ForEach(r => WriteRectangle(r, buffer));

					IGenieDataElement.AssertTrue(AgeLabelRectangles.Count >= 3);
					buffer.WriteInteger(AgeLabelRectangles.Count);
					AgeLabelRectangles.ForEach(r => WriteRectangle(r, buffer));

					IGenieDataElement.AssertTrue(VerticalDrawOffsets.Count >= 3);
					buffer.WriteInteger(VerticalDrawOffsets.Count);
					VerticalDrawOffsets.ForEach(vdo => buffer.WriteInteger(vdo));
				}

				#endregion
			}

			public class NodeBackground
			{
				#region Variablen

				/// <summary>
				/// The display name.
				/// </summary>
				public string Name;

				/// <summary>
				/// The node SLP frame index.
				/// </summary>
				public int FrameIndex;

				#endregion

				#region Funktionen

				public NodeBackground ReadData(RAMBuffer buffer)
				{
					Name = buffer.ReadString(buffer.ReadInteger());
					FrameIndex = buffer.ReadInteger();

					return this;
				}

				public void WriteData(RAMBuffer buffer)
				{
					buffer.WriteInteger(Name.Length);
					buffer.WriteString(Name);
					buffer.WriteInteger(FrameIndex);
				}

				#endregion
			}

			#endregion
		}

		#endregion Strukturen
	}
}