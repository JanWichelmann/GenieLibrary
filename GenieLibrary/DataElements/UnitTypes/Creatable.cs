using IORAMHelper;
using System;
using System.Collections.Generic;

namespace GenieLibrary.DataElements.UnitTypes
{
	public class Creatable : IGenieDataElement, ICloneable
	{
		#region Variablen

		/// <summary>
		/// Länge: 3;
		/// </summary>
		public List<ResourceTuple<short, short, short>> ResourceCosts;

		public short TrainTime;
		public short TrainLocationID;
		public byte ButtonID;
		public int Unknown26;
		public int Unknown27;
		public byte Unknown28;
		public byte HeroMode;
		public int GarrisonGraphic;
		public float ProjectileCount;
		public byte ProjectileCountOnFullGarrison;
		public float ProjectileSpawningAreaWidth;
		public float ProjectileSpawningAreaHeight;
		public float ProjectileSpawningAreaRandomness;
		public int AlternativeProjectileUnit;
		public int ChargingGraphic;
		public byte ChargingMode;
		public short DisplayedPierceArmor;

		#endregion Variablen

		#region Funktionen

		public Creatable ReadData(RAMBuffer buffer)
		{
			ResourceCosts = new List<ResourceTuple<short, short, short>>(3);
			for(int i = 0; i < 3; ++i)
				ResourceCosts.Add(new ResourceTuple<short, short, short>() { Type = buffer.ReadShort(), Amount = buffer.ReadShort(), Paid = buffer.ReadShort() });

			TrainTime = buffer.ReadShort();
			TrainLocationID = buffer.ReadShort();
			ButtonID = buffer.ReadByte();
			Unknown26 = buffer.ReadInteger();
			Unknown27 = buffer.ReadInteger();
			Unknown28 = buffer.ReadByte();
			HeroMode = buffer.ReadByte();
			GarrisonGraphic = buffer.ReadInteger();
			ProjectileCount = buffer.ReadFloat();
			ProjectileCountOnFullGarrison = buffer.ReadByte();
			ProjectileSpawningAreaWidth = buffer.ReadFloat();
			ProjectileSpawningAreaHeight = buffer.ReadFloat();
			ProjectileSpawningAreaRandomness = buffer.ReadFloat();
			AlternativeProjectileUnit = buffer.ReadInteger();
			ChargingGraphic = buffer.ReadInteger();
			ChargingMode = buffer.ReadByte();
			DisplayedPierceArmor = buffer.ReadShort();

			return this;
		}

		public void WriteData(RAMBuffer buffer)
		{
			AssertListLength(ResourceCosts, 3);
			ResourceCosts.ForEach(e =>
			{
				buffer.WriteShort(e.Type);
				buffer.WriteShort(e.Amount);
				buffer.WriteShort(e.Paid);
			});

			buffer.WriteShort(TrainTime);
			buffer.WriteShort(TrainLocationID);
			buffer.WriteByte(ButtonID);
			buffer.WriteInteger(Unknown26);
			buffer.WriteInteger(Unknown27);
			buffer.WriteByte(Unknown28);
			buffer.WriteByte(HeroMode);
			buffer.WriteInteger(GarrisonGraphic);
			buffer.WriteFloat(ProjectileCount);
			buffer.WriteByte(ProjectileCountOnFullGarrison);
			buffer.WriteFloat(ProjectileSpawningAreaWidth);
			buffer.WriteFloat(ProjectileSpawningAreaHeight);
			buffer.WriteFloat(ProjectileSpawningAreaRandomness);
			buffer.WriteInteger(AlternativeProjectileUnit);
			buffer.WriteInteger(ChargingGraphic);
			buffer.WriteByte(ChargingMode);
			buffer.WriteShort(DisplayedPierceArmor);
		}

		/// <summary>
		/// Gibt eine tiefe Kopie dieses Objekts zurück.
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			// Erstmal alle Wert-Typen kopieren
			Creatable clone = (Creatable)this.MemberwiseClone();

			// Referenztypen kopieren
			clone.ResourceCosts = new List<ResourceTuple<short, short, short>>(ResourceCosts);

			// Fertig
			return clone;
		}

		#endregion Funktionen
	}
}