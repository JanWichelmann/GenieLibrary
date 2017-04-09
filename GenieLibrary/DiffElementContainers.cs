using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AoEBalancingTool
{
	/// <summary>
	/// Interface for classes containing DiffElement objects.
	/// This interface provides a property to count modified elements.
	/// </summary>
	public abstract class DiffElementContainer : INotifyPropertyChanged
	{
		/// <summary>
		/// States whether there are modified public fields.
		/// </summary>
		private bool _hasModifiedFields;

		/// <summary>
		/// The count of modified public fields.
		/// </summary>
		public abstract int ModifiedFieldsCount { get; set; }

		/// <summary>
		/// States whether there are modified public fields.
		/// </summary>
		public bool HasModifiedFields
		{
			get { return _hasModifiedFields; }
			protected set
			{
				_hasModifiedFields = value;
				OnPropertyChanged(nameof(HasModifiedFields));
			}
		}

		/// <summary>
		/// Implementation of PropertyChanged interface.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the property change event.
		/// </summary>
		/// <param name="name">The name of the changed property.</param>
		protected void OnPropertyChanged(string name)
		{
			// Raise event
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#region Static I/O helper functions

		#region Read

		/// <summary>
		/// Reads a byte member from the given buffer.
		/// The modification flag is read also, the internal modification counter is incremented.
		/// </summary>
		/// <param name="buffer">The buffer where the member shall be read from.</param>
		/// <param name="member">The member where the read data shall be stored.</param>
		protected static void ReadMember(IORAMHelper.RAMBuffer buffer, DiffElement<byte> member)
		{
			// Member must be defined
			if(member == null)
				return;

			// Read member if modified
			if(buffer.ReadByte() == 1)
				member.Value = buffer.ReadByte();
		}

		/// <summary>
		/// Reads a short member from the given buffer.
		/// The modification flag is read also.
		/// </summary>
		/// <param name="buffer">The buffer where the member shall be read from.</param>
		/// <param name="member">The member where the read data shall be stored.</param>
		protected static void ReadMember(IORAMHelper.RAMBuffer buffer, DiffElement<short> member)
		{
			// Member must be defined
			if(member == null)
				return;

			// Read member if modified
			if(buffer.ReadByte() == 1)
				member.Value = buffer.ReadShort();
		}

		/// <summary>
		/// Reads a floating point member from the given buffer.
		/// The modification flag is read also.
		/// </summary>
		/// <param name="buffer">The buffer where the member shall be read from.</param>
		/// <param name="member">The member where the read data shall be stored.</param>
		protected static void ReadMember(IORAMHelper.RAMBuffer buffer, DiffElement<float> member)
		{
			// Member must be defined
			if(member == null)
				return;

			// Read member if modified
			if(buffer.ReadByte() == 1)
				member.Value = buffer.ReadFloat();
		}

		/// <summary>
		/// Reads a attack/armor entry list member from the given buffer.
		/// The modification flag is read also.
		/// </summary>
		/// <param name="buffer">The buffer where the member shall be read from.</param>
		/// <param name="member">The member where the read data shall be stored.</param>
		protected static void ReadMember(IORAMHelper.RAMBuffer buffer, AttackArmorEntryListDiffElement member)
		{
			// Member must be defined
			if(member == null)
				return;

			// Read member if modified
			if(buffer.ReadByte() == 1)
			{
				// Read entries
				int count = buffer.ReadInteger();
				List<AttackArmorEntry> entries = new List<AttackArmorEntry>(count);
				for(int i = 0; i < count; ++i)
					entries.Add(new AttackArmorEntry
					(
						 buffer.ReadUShort(),
						 buffer.ReadUShort()
					));
				member.Value = new EquatableObservableCollection<AttackArmorEntry>(entries);
			}
		}

		/// <summary>
		/// Reads a resource cost member from the given buffer.
		/// The modification flag is read also.
		/// </summary>
		/// <param name="buffer">The buffer where the member shall be read from.</param>
		/// <param name="member">The member where the read data shall be stored.</param>
		protected static void ReadMember(IORAMHelper.RAMBuffer buffer, DiffElement<ResourceCostEntry> member)
		{
			// Member must be defined
			if(member == null)
				return;

			// Read member if modified
			if(buffer.ReadByte() == 1)
				member.Value = new ResourceCostEntry(buffer.ReadShort(), buffer.ReadShort(), buffer.ReadByte());
		}

		#endregion

		#region Save

		/// <summary>
		/// Saves the given byte member into the given buffer.
		/// The modification flag is written also.
		/// </summary>
		/// <param name="buffer">The buffer for the member to be written to.</param>
		/// <param name="member">The member to be written.</param>
		protected static void SaveMember(IORAMHelper.RAMBuffer buffer, DiffElement<byte> member)
		{
			// Member must be defined
			if(member == null)
				return;

			// Write modification flag
			buffer.WriteByte((byte)(member.Modified ? 1 : 0));

			// Write member
			if(member.Modified)
				buffer.WriteByte(member);
		}

		/// <summary>
		/// Saves the given short member into the given buffer.
		/// The modification flag is written also.
		/// </summary>
		/// <param name="buffer">The buffer for the member to written to.</param>
		/// <param name="member">The member to be written.</param>
		protected static void SaveMember(IORAMHelper.RAMBuffer buffer, DiffElement<short> member)
		{
			// Member must be defined
			if(member == null)
				return;

			// Write modification flag
			buffer.WriteByte((byte)(member.Modified ? 1 : 0));

			// Write member
			if(member.Modified)
				buffer.WriteShort(member);
		}

		/// <summary>
		/// Saves the given floating point member into the given buffer.
		/// The modification flag is written also.
		/// </summary>
		/// <param name="buffer">The buffer for the member to written to.</param>
		/// <param name="member">The member to be written.</param>
		protected static void SaveMember(IORAMHelper.RAMBuffer buffer, DiffElement<float> member)
		{
			// Member must be defined
			if(member == null)
				return;

			// Write modification flag
			buffer.WriteByte((byte)(member.Modified ? 1 : 0));

			// Write member
			if(member.Modified)
				buffer.WriteFloat(member);
		}

		/// <summary>
		/// Saves the given attack/armor entry list member into the given buffer.
		/// The modification flag is written also.
		/// </summary>
		/// <param name="buffer">The buffer for the member to written to.</param>
		/// <param name="member">The member to be written.</param>
		protected static void SaveMember(IORAMHelper.RAMBuffer buffer, AttackArmorEntryListDiffElement member)
		{
			// Member must be defined
			if(member == null)
				return;

			// Write modification flag
			buffer.WriteByte((byte)(member.Modified ? 1 : 0));

			// Write member
			if(member.Modified)
			{
				// Write count
				buffer.WriteInteger(member.Value.Count);

				// Write members
				foreach(AttackArmorEntry aae in member.Value)
				{
					// Write member fields
					buffer.WriteUShort(aae.ArmorClass);
					buffer.WriteUShort(aae.Amount);
				}
			}
		}

		/// <summary>
		/// Saves the given resource cost member into the given buffer.
		/// The modification flag is written also.
		/// </summary>
		/// <param name="buffer">The buffer for the member to written to.</param>
		/// <param name="member">The member to be written.</param>
		protected static void SaveMember(IORAMHelper.RAMBuffer buffer, ResourceCostEntryDiffElement member)
		{
			// Member must be defined
			if(member == null)
				return;

			// Write modification flag
			buffer.WriteByte((byte)(member.Modified ? 1 : 0));

			// Write member
			if(member.Modified)
			{
				// Write member fields
				buffer.WriteShort(member.Value.ResourceType);
				buffer.WriteShort(member.Value.Amount);
				buffer.WriteByte(member.Value.Paid);
			}
		}

		#endregion

		#endregion
	}

	/// <summary>
	/// Defines one unit data element.
	/// The internal modification counter should be kept consistent!
	/// </summary>
	public class UnitEntry : DiffElementContainer
	{
		#region Fields

		/// <summary>
		/// The count of modified public fields.
		/// </summary>
		private int _modifiedFieldsCount = 0;

		#endregion

		#region Public Fields

		// Main stats
		public DiffElement<short> HitPoints { get; set; }
		public DiffElement<float> Speed { get; set; }
		public DiffElement<float> RotationSpeed { get; set; }
		public DiffElement<float> LineOfSight { get; set; }
		public DiffElement<float> SearchRadius { get; set; }

		// Attack values
		public DiffElement<float> MinRange { get; set; }
		public DiffElement<float> MaxRange { get; set; }
		public DiffElement<float> DisplayedRange { get; set; }
		public DiffElement<float> ReloadTime { get; set; }
		public DiffElement<float> DisplayedReloadTime { get; set; }
		public DiffElement<float> BlastRadius { get; set; }
		public AttackArmorEntryListDiffElement Attacks { get; set; }
		public DiffElement<short> DisplayedAttack { get; set; }

		// Projectile data
		public DiffElement<float> ProjectileCount { get; set; }
		public DiffElement<byte> ProjectileCountOnFullGarrison { get; set; }
		public DiffElement<short> ProjectileFrameDelay { get; set; }
		public DiffElement<short> ProjectileAccuracyPercent { get; set; }
		public DiffElement<float> ProjectileDispersion { get; set; }
		public DiffElement<float> ProjectileGraphicDisplacementX { get; set; }
		public DiffElement<float> ProjectileGraphicDisplacementY { get; set; }
		public DiffElement<float> ProjectileGraphicDisplacementZ { get; set; }
		public DiffElement<float> ProjectileSpawningAreaWidth { get; set; }
		public DiffElement<float> ProjectileSpawningAreaHeight { get; set; }
		public DiffElement<float> ProjectileSpawningAreaRandomness { get; set; }

		// Armor values
		public AttackArmorEntryListDiffElement Armors { get; set; }
		public DiffElement<short> DisplayedMeleeArmor { get; set; }
		public DiffElement<short> DisplayedPierceArmor { get; set; }

		// Garrison values
		public DiffElement<byte> GarrisonCapacity { get; set; }
		public DiffElement<float> GarrisonHealRateFactor { get; set; }

		// Creation values
		public DiffElement<short> TrainTime { get; set; }
		public ResourceCostEntryDiffElement Cost1 { get; set; }
		public ResourceCostEntryDiffElement Cost2 { get; set; }
		public ResourceCostEntryDiffElement Cost3 { get; set; }

		#endregion

		#region Properties

		/// <summary>
		/// The count of modified public fields.
		/// Should be kept consistent with the actual "modified" flags!
		/// </summary>
		public override int ModifiedFieldsCount
		{
			get { return _modifiedFieldsCount; }
			set
			{
				_modifiedFieldsCount = value;
				HasModifiedFields = (_modifiedFieldsCount > 0);
				OnPropertyChanged(nameof(ModifiedFieldsCount));
			}
		}

		/// <summary>
		/// The displayed name of the unit entry.
		/// </summary>
		public string DisplayName { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the whole unit entry from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the unit entry data.</param>
		public void Read(IORAMHelper.RAMBuffer buffer)
		{
			// Reset counter
			ModifiedFieldsCount = 0;

			// Read members
			ReadMember(buffer, HitPoints);
			ReadMember(buffer, Speed);
			ReadMember(buffer, RotationSpeed);
			ReadMember(buffer, LineOfSight);
			ReadMember(buffer, SearchRadius);

			ReadMember(buffer, MinRange);
			ReadMember(buffer, MaxRange);
			ReadMember(buffer, DisplayedRange);
			ReadMember(buffer, ReloadTime);
			ReadMember(buffer, DisplayedReloadTime);
			ReadMember(buffer, BlastRadius);
			ReadMember(buffer, Attacks);
			ReadMember(buffer, DisplayedAttack);

			ReadMember(buffer, ProjectileCount);
			ReadMember(buffer, ProjectileCountOnFullGarrison);
			ReadMember(buffer, ProjectileFrameDelay);
			ReadMember(buffer, ProjectileAccuracyPercent);
			ReadMember(buffer, ProjectileDispersion);
			ReadMember(buffer, ProjectileGraphicDisplacementX);
			ReadMember(buffer, ProjectileGraphicDisplacementY);
			ReadMember(buffer, ProjectileGraphicDisplacementZ);
			ReadMember(buffer, ProjectileSpawningAreaWidth);
			ReadMember(buffer, ProjectileSpawningAreaHeight);
			ReadMember(buffer, ProjectileSpawningAreaRandomness);

			ReadMember(buffer, Armors);
			ReadMember(buffer, DisplayedMeleeArmor);
			ReadMember(buffer, DisplayedPierceArmor);

			ReadMember(buffer, GarrisonCapacity);
			ReadMember(buffer, GarrisonHealRateFactor);

			ReadMember(buffer, TrainTime);
			ReadMember(buffer, Cost1);
			ReadMember(buffer, Cost2);
			ReadMember(buffer, Cost3);
		}

		/// <summary>
		/// Saves the whole unit entry into the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer for the members to written to.</param>
		public void Save(IORAMHelper.RAMBuffer buffer)
		{
			// Write members
			SaveMember(buffer, HitPoints);
			SaveMember(buffer, Speed);
			SaveMember(buffer, RotationSpeed);
			SaveMember(buffer, LineOfSight);
			SaveMember(buffer, SearchRadius);

			SaveMember(buffer, MinRange);
			SaveMember(buffer, MaxRange);
			SaveMember(buffer, DisplayedRange);
			SaveMember(buffer, ReloadTime);
			SaveMember(buffer, DisplayedReloadTime);
			SaveMember(buffer, BlastRadius);
			SaveMember(buffer, Attacks);
			SaveMember(buffer, DisplayedAttack);

			SaveMember(buffer, ProjectileCount);
			SaveMember(buffer, ProjectileCountOnFullGarrison);
			SaveMember(buffer, ProjectileFrameDelay);
			SaveMember(buffer, ProjectileAccuracyPercent);
			SaveMember(buffer, ProjectileDispersion);
			SaveMember(buffer, ProjectileGraphicDisplacementX);
			SaveMember(buffer, ProjectileGraphicDisplacementY);
			SaveMember(buffer, ProjectileGraphicDisplacementZ);
			SaveMember(buffer, ProjectileSpawningAreaWidth);
			SaveMember(buffer, ProjectileSpawningAreaHeight);
			SaveMember(buffer, ProjectileSpawningAreaRandomness);

			SaveMember(buffer, Armors);
			SaveMember(buffer, DisplayedMeleeArmor);
			SaveMember(buffer, DisplayedPierceArmor);

			SaveMember(buffer, GarrisonCapacity);
			SaveMember(buffer, GarrisonHealRateFactor);

			SaveMember(buffer, TrainTime);
			SaveMember(buffer, Cost1);
			SaveMember(buffer, Cost2);
			SaveMember(buffer, Cost3);
		}

		#endregion
	}

	/// <summary>
	/// Defines one research data element.
	/// The internal modification counter should be kept consistent!
	/// </summary>
	public class ResearchEntry : DiffElementContainer
	{
		#region Fields

		/// <summary>
		/// The count of modified public fields.
		/// </summary>
		private int _modifiedFieldsCount = 0;

		#endregion

		#region Public Fields

		// Stats
		public DiffElement<short> ResearchTime { get; set; }
		public ResourceCostEntryDiffElement Cost1 { get; set; }
		public ResourceCostEntryDiffElement Cost2 { get; set; }
		public ResourceCostEntryDiffElement Cost3 { get; set; }

		#endregion

		#region Properties

		/// <summary>
		/// The count of modified public fields.
		/// Should be kept consistent with the actual "modified" flags!
		/// </summary>
		public override int ModifiedFieldsCount
		{
			get { return _modifiedFieldsCount; }
			set
			{
				_modifiedFieldsCount = value;
				HasModifiedFields = (_modifiedFieldsCount > 0);
				OnPropertyChanged(nameof(ModifiedFieldsCount));
			}
		}

		/// <summary>
		/// The displayed name of the research entry.
		/// </summary>
		public string DisplayName { get; set; }

		#endregion

		#region Functions

		/// <summary>
		/// Reads the whole research entry from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the research entry data.</param>
		public void Read(IORAMHelper.RAMBuffer buffer)
		{
			// Reset counter
			ModifiedFieldsCount = 0;

			// Read members
			ReadMember(buffer, ResearchTime);
			ReadMember(buffer, Cost1);
			ReadMember(buffer, Cost2);
			ReadMember(buffer, Cost3);
		}

		/// <summary>
		/// Saves the whole research entry into the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer for the members to written to.</param>
		public void Save(IORAMHelper.RAMBuffer buffer)
		{
			// Write members
			SaveMember(buffer, ResearchTime);
			SaveMember(buffer, Cost1);
			SaveMember(buffer, Cost2);
			SaveMember(buffer, Cost3);
		}

		#endregion
	}
}
