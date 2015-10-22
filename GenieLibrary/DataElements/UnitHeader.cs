using IORAMHelper;
using System;
using System.Collections.Generic;

namespace GenieLibrary.DataElements
{
	public class UnitHeader : IGenieDataElement
	{
		#region Variablen

		public byte Exists;
		public List<UnitCommand> Commands;

		#endregion Variablen

		#region Funktionen

		public override void ReadData(RAMBuffer buffer)
		{
			Exists = buffer.ReadByte();

			if(Exists != 0)
			{
				int commandCount = buffer.ReadUShort();
				Commands = new List<UnitCommand>(commandCount);
				for(int i = 0; i < commandCount; ++i)
					Commands.Add(new UnitCommand().ReadDataInline(buffer));
			}
		}

		public override void WriteData(RAMBuffer buffer)
		{
			buffer.WriteByte(Exists);

			if(Exists != 0)
			{
				buffer.WriteUShort((ushort)Commands.Count);
				Commands.ForEach(e => e.WriteData(buffer));
			}
		}

		#endregion Funktionen

		#region Strukturen

		public class UnitCommand : IGenieDataElement,ICloneable
		{
			#region Variablen

			public short One;
			public short ID;
			public byte Unknown1;
			public short Type;
			public short ClassID;
			public short UnitID;
			public short TerrainID;
			public short ResourceIn;
			public short ResourceProductivityMultiplier;
			public short ResourceOut;
			public short Resource;
			public float WorkRateMultiplier;
			public float ExecutionRadius;
			public float ExtraRange;
			public byte Unknown4;
			public float Unknown5;
			public byte SelectionEnabler;
			public byte Unknown7;
			public short PlunderSource;
			public short Unknown9;
			public byte SelectionMode;
			public byte RightClickMode;
			public byte Unknown12;

			/// <summary>
			/// Länge: 6.
			/// </summary>
			public List<short> Graphics;

			#endregion Variablen

			#region Funktionen

			public override void ReadData(RAMBuffer buffer)
			{
				One = buffer.ReadShort();
				ID = buffer.ReadShort();
				Unknown1 = buffer.ReadByte();
				Type = buffer.ReadShort();
				ClassID = buffer.ReadShort();
				UnitID = buffer.ReadShort();
				TerrainID = buffer.ReadShort();
				ResourceIn = buffer.ReadShort();
				ResourceProductivityMultiplier = buffer.ReadShort();
				ResourceOut = buffer.ReadShort();
				Resource = buffer.ReadShort();
				WorkRateMultiplier = buffer.ReadFloat();
				ExecutionRadius = buffer.ReadFloat();
				ExtraRange = buffer.ReadFloat();
				Unknown4 = buffer.ReadByte();
				Unknown5 = buffer.ReadFloat();
				SelectionEnabler = buffer.ReadByte();
				Unknown7 = buffer.ReadByte();
				PlunderSource = buffer.ReadShort();
				Unknown9 = buffer.ReadShort();
				SelectionMode = buffer.ReadByte();
				RightClickMode = buffer.ReadByte();
				Unknown12 = buffer.ReadByte();

				Graphics = new List<short>(6);
				for(int i = 0; i < 6; ++i)
					Graphics.Add(buffer.ReadShort());
			}

			public override void WriteData(RAMBuffer buffer)
			{
				buffer.WriteShort(One);
				buffer.WriteShort(ID);
				buffer.WriteByte(Unknown1);
				buffer.WriteShort(Type);
				buffer.WriteShort(ClassID);
				buffer.WriteShort(UnitID);
				buffer.WriteShort(TerrainID);
				buffer.WriteShort(ResourceIn);
				buffer.WriteShort(ResourceProductivityMultiplier);
				buffer.WriteShort(ResourceOut);
				buffer.WriteShort(Resource);
				buffer.WriteFloat(WorkRateMultiplier);
				buffer.WriteFloat(ExecutionRadius);
				buffer.WriteFloat(ExtraRange);
				buffer.WriteByte(Unknown4);
				buffer.WriteFloat(Unknown5);
				buffer.WriteByte(SelectionEnabler);
				buffer.WriteByte(Unknown7);
				buffer.WriteShort(PlunderSource);
				buffer.WriteShort(Unknown9);
				buffer.WriteByte(SelectionMode);
				buffer.WriteByte(RightClickMode);
				buffer.WriteByte(Unknown12);

				AssertListLength(Graphics, 6);
				Graphics.ForEach(e => buffer.WriteShort(e));
			}
			
			/// <summary>
			 /// Gibt eine tiefe Kopie dieses Objekts zurück.
			 /// </summary>
			 /// <returns></returns>
			public object Clone()
			{
				// Erstmal alle Wert-Typen kopieren
				UnitCommand clone = (UnitCommand)this.MemberwiseClone();

				// Referenztypen kopieren
				clone.Graphics = new List<short>(Graphics);

				// Fertig
				return clone;
			}

			#endregion Funktionen
		}

		#endregion Strukturen
	}
}