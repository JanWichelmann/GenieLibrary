using IORAMHelper;
using System;
using System.Collections.Generic;

namespace GenieLibrary.DataElements.UnitTypes
{
	public class Type50 : IGenieDataElement, ICloneable
	{
		#region Variablen

		public short DefaultArmor;

		/// <summary>
		/// Format: Klasse =&gt; Wert.
		/// </summary>
		public Dictionary<ushort, ushort> Attacks;

		/// <summary>
		/// Format: Klasse =&gt; Wert.
		/// </summary>
		public Dictionary<ushort, ushort> Armors;

		public short TerrainRestrictionForDamageMultiplying;
		public float MaxRange;
		public float BlastRadius;
		public float ReloadTime;
		public short ProjectileUnitID;
		public short ProjectileAccuracyPercent;
		public byte TowerMode;
		public short ProjectileFrameDelay;

		/// <summary>
		/// Länge: 3.
		/// </summary>
		public List<float> ProjectileGraphicDisplacement;

		public byte BlastLevel;
		public float MinRange;
		public float ProjectileDispersion;
		public short AttackGraphic;
		public short DisplayedMeleeArmor;
		public short DisplayedAttack;
		public float DisplayedRange;
		public float DisplayedReloadTime;

		#endregion Variablen

		#region Funktionen

		public Type50 ReadData(RAMBuffer buffer)
		{
			DefaultArmor = buffer.ReadShort();

			ushort attackCount = buffer.ReadUShort();
			Attacks = new Dictionary<ushort, ushort>(attackCount);
			for(int i = 0; i < attackCount; ++i)
				Attacks[buffer.ReadUShort()] = buffer.ReadUShort();

			ushort armourCount = buffer.ReadUShort();
			Armors = new Dictionary<ushort, ushort>(armourCount);
			for(int i = 0; i < armourCount; ++i)
				Armors[buffer.ReadUShort()] = buffer.ReadUShort();

			TerrainRestrictionForDamageMultiplying = buffer.ReadShort();
			MaxRange = buffer.ReadFloat();
			BlastRadius = buffer.ReadFloat();
			ReloadTime = buffer.ReadFloat();
			ProjectileUnitID = buffer.ReadShort();
			ProjectileAccuracyPercent = buffer.ReadShort();
			TowerMode = buffer.ReadByte();
			ProjectileFrameDelay = buffer.ReadShort();

			ProjectileGraphicDisplacement = new List<float>(3);
			for(int i = 0; i < 3; ++i)
				ProjectileGraphicDisplacement.Add(buffer.ReadFloat());

			BlastLevel = buffer.ReadByte();
			MinRange = buffer.ReadFloat();
			ProjectileDispersion = buffer.ReadFloat();
			AttackGraphic = buffer.ReadShort();
			DisplayedMeleeArmor = buffer.ReadShort();
			DisplayedAttack = buffer.ReadShort();
			DisplayedRange = buffer.ReadFloat();
			DisplayedReloadTime = buffer.ReadFloat();

			return this;
		}

		public void WriteData(RAMBuffer buffer)
		{
			buffer.WriteShort(DefaultArmor);

			buffer.WriteUShort((ushort)Attacks.Count);
			foreach(KeyValuePair<ushort, ushort> currA in Attacks)
			{
				buffer.WriteUShort(currA.Key);
				buffer.WriteUShort(currA.Value);
			}

			buffer.WriteUShort((ushort)Armors.Count);
			foreach(KeyValuePair<ushort, ushort> currA in Armors)
			{
				buffer.WriteUShort(currA.Key);
				buffer.WriteUShort(currA.Value);
			}

			buffer.WriteShort(TerrainRestrictionForDamageMultiplying);
			buffer.WriteFloat(MaxRange);
			buffer.WriteFloat(BlastRadius);
			buffer.WriteFloat(ReloadTime);
			buffer.WriteShort(ProjectileUnitID);
			buffer.WriteShort(ProjectileAccuracyPercent);
			buffer.WriteByte(TowerMode);
			buffer.WriteShort(ProjectileFrameDelay);

			AssertListLength(ProjectileGraphicDisplacement, 3);
			ProjectileGraphicDisplacement.ForEach(e => buffer.WriteFloat(e));

			buffer.WriteByte(BlastLevel);
			buffer.WriteFloat(MinRange);
			buffer.WriteFloat(ProjectileDispersion);
			buffer.WriteShort(AttackGraphic);
			buffer.WriteShort(DisplayedMeleeArmor);
			buffer.WriteShort(DisplayedAttack);
			buffer.WriteFloat(DisplayedRange);
			buffer.WriteFloat(DisplayedReloadTime);
		}

		/// <summary>
		/// Gibt eine tiefe Kopie dieses Objekts zurück.
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			// Erstmal alle Wert-Typen kopieren
			Type50 clone = (Type50)this.MemberwiseClone();

			// Referenztypen kopieren
			clone.Attacks = new Dictionary<ushort, ushort>(Attacks);
			clone.Armors = new Dictionary<ushort, ushort>(Armors);
			clone.ProjectileGraphicDisplacement = new List<float>(ProjectileGraphicDisplacement);

			// Fertig
			return clone;
		}

		#endregion Funktionen
	}
}