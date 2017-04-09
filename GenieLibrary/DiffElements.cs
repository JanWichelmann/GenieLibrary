using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AoEBalancingTool
{
	/// <summary>
	/// Represents a data element including a bool variable determining whether the element is in a "modified" state.
	/// The base value from the base genie file is used for automatic setting of the "modified" flag.
	/// This base class should only be used with primitive types, more complex types should derive and implement the PropertyChanged event for their members.
	/// </summary>
	/// <typeparam name="T">The primitive type of the data element.</typeparam>
	public class DiffElement<T> : INotifyPropertyChanged where T : IEquatable<T>
	{
		/// <summary>
		/// The object containing the diff element.
		/// </summary>
		private readonly DiffElementContainer _owningObject;

		/// <summary>
		/// Determines whether the data element has been modified.
		/// </summary>
		private bool _modified;

		/// <summary>
		/// Determines whether the data element has been modified.
		/// </summary>
		public bool Modified
		{
			get { return _modified; }
			protected set
			{
				// Update counter in owning class, if the flag has changed
				if(_modified != value)
					if(value)
						++_owningObject.ModifiedFieldsCount;
					else
						--_owningObject.ModifiedFieldsCount;

				// Update flag
				_modified = value;

				// Send update for binding controls
				OnPropertyChanged(nameof(Modified));
			}
		}

		/// <summary>
		/// The value stored in this instance.
		/// </summary>
		protected T _value;

		/// <summary>
		/// The value stored in this instance.
		/// Also called by constructor.
		/// </summary>
		public virtual T Value
		{
			get { return _value; }
			set
			{
				// Update value
				_value = value;

				// Set "modified" flag properly
				Modified = !_value.Equals(_baseValue);

				// Send update for binding controls
				OnPropertyChanged(nameof(Value));
			}
		}

		/// <summary>
		/// The base value of the data element stored in this instance.
		/// Shouldn't be changed after first initialization.
		/// </summary>
		protected T _baseValue;

		/// <summary>
		/// Initializes a new data diff element object with the given base value.
		/// </summary>
		/// <param name="owningObject">The object containing the diff element.</param>
		/// <param name="baseValue">The base value of the diff element.</param>
		public DiffElement(DiffElementContainer owningObject, T baseValue)
		{
			// Save owning object
			_owningObject = owningObject;

			// Remember base value, set as current value ("modified" flag updated automatically)
			_baseValue = baseValue;

			// Ignore warnings here, the virtual property is used to properly assign PropertyChanged events in derived classes.
			// As long as these don't have any possibly uninitialized private members, this is safe.
			Value = baseValue;
		}

		/// <summary>
		/// Returns the current value stored in the given data diff element object.
		/// </summary>
		/// <param name="diffElement">The data diff element object whose value shall be retrieved.</param>
		public static implicit operator T(DiffElement<T> diffElement)
		{
			// Return internal data element
			return diffElement._value;
		}

		/// <summary>
		/// Implementation of PropertyChanged interface.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		/// <param name="propertyName">The name of the affected property.</param>
		protected void OnPropertyChanged(string propertyName)
		{
			// Raise event
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public override string ToString()
		{
			// TODO delete, for easier debugging only
			return $"_value = {_value}"; // [{typeof(T)}]";
		}
	}

	/// <summary>
	/// Contains attack/armor class entries and sets a modified flag.
	/// </summary>
	public class AttackArmorEntryListDiffElement : DiffElement<EquatableObservableCollection<AttackArmorEntry>>
	{
		/// <summary>
		/// Initializes a new data diff element object with the given base value.
		/// </summary>
		/// <param name="owningObject">The object containing the diff element.</param>
		/// <param name="baseAttackArmorEntries">The base value of the diff element.</param>
		public AttackArmorEntryListDiffElement(DiffElementContainer owningObject, List<AttackArmorEntry> baseAttackArmorEntries)
			: base(owningObject, new EquatableObservableCollection<AttackArmorEntry>(baseAttackArmorEntries))
		{
			// Base value should be a copy of the original data element -> deep copy of list
			_baseValue = new EquatableObservableCollection<AttackArmorEntry>(baseAttackArmorEntries.Select(aae => new AttackArmorEntry(aae.ArmorClass, aae.Amount)));
		}

		private void AttackArmorEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Set "modified" flag properly
			Modified = !_value.Equals(_baseValue);

			// Send update for binding controls
			OnPropertyChanged(nameof(Value));
		}

		/// <summary>
		/// The list stored in this instance. Automatically subscribes to all list change events.
		/// </summary>
		public override EquatableObservableCollection<AttackArmorEntry> Value
		{
			get { return base.Value; }
			set
			{
				// Update value
				base.Value = value;

				// Subscribe to all list elements
				foreach(AttackArmorEntry aae in _value)
					aae.PropertyChanged += AttackArmorEntry_PropertyChanged;

				// Subscribe to change event, to update member event subscriptions properly
				_value.CollectionChanged += (sender, e) =>
				{
					// Unsubscribe from events of removed items and subscribe to events of added items
					if(e.OldItems != null)
						foreach(AttackArmorEntry aae in e.OldItems)
							aae.PropertyChanged -= AttackArmorEntry_PropertyChanged;
					if(e.NewItems != null)
						foreach(AttackArmorEntry aae in e.NewItems)
							aae.PropertyChanged += AttackArmorEntry_PropertyChanged;

					// Set "modified" flag properly
					Modified = !_value.Equals(_baseValue);

					// Send update for binding controls
					OnPropertyChanged(nameof(Value));
				};
			}
		}
	}

	/// <summary>
	/// Represents an attack/armor class entry.
	/// </summary>
	public class AttackArmorEntry : INotifyPropertyChanged, IEquatable<AttackArmorEntry>
	{
		/// <summary>
		/// The armor class.
		/// </summary>
		private ushort _armorClass;

		/// <summary>
		/// The armor value.
		/// </summary>
		private ushort _amount;

		/// <summary>
		/// The armor class.
		/// </summary>
		public ushort ArmorClass
		{
			get { return _armorClass; }
			set
			{
				// Update value and notify parent objects
				_armorClass = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ArmorClass)));
			}
		}

		/// <summary>
		/// The armor value.
		/// </summary>
		public ushort Amount
		{
			get { return _amount; }
			set
			{
				// Update value and notify parent objects
				_amount = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
			}
		}

		/// <summary>
		/// Creates a new empty entry.
		/// </summary>
		public AttackArmorEntry()
		{
			// Set default values
			ArmorClass = 0;
			Amount = 0;
		}

		/// <summary>
		/// Creates a new entry.
		/// </summary>
		/// <param name="armorClass">The armor class.</param>
		/// <param name="amount">The armor value.</param>
		public AttackArmorEntry(ushort armorClass, ushort amount)
		{
			// Save values
			ArmorClass = armorClass;
			Amount = amount;
		}

		/// <summary>
		/// Implementation of PropertyChanged interface.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Returns a unique hash code to allow efficient comparison of two instances.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() => ArmorClass << 16 | Amount;

		/// <summary>
		/// Compares this entry object with another.
		/// </summary>
		/// <param name="other">Der object to be compared with.</param>
		/// <returns></returns>
		public bool Equals(AttackArmorEntry other) => other != null && _armorClass == other._armorClass && _amount == other._amount;
	}

	/// <summary>
	/// Allows the comparison of two observable list objects.
	/// </summary>
	/// <typeparam name="T">The data type stored in the observable list objects.</typeparam>
	public class EquatableObservableCollection<T> : ObservableCollection<T>, IEquatable<EquatableObservableCollection<T>>
	{
		public EquatableObservableCollection(IEnumerable<T> collection)
			: base(collection) { }
		public bool Equals(EquatableObservableCollection<T> other) => other != null && new HashSet<T>(this).SetEquals(other);
	}

	/// <summary>
	/// Contains attack/armor class entries and sets a modified flag.
	/// </summary>
	public class ResourceCostEntryDiffElement : DiffElement<ResourceCostEntry>
	{
		/// <summary>
		/// Initializes a new data diff element object with the given base value.
		/// </summary>
		/// <param name="owningObject">The object containing the diff element.</param>
		/// <param name="baseResourceCostEntry">The base value of the diff element.</param>
		public ResourceCostEntryDiffElement(DiffElementContainer owningObject, ResourceCostEntry baseResourceCostEntry)
			: base(owningObject, baseResourceCostEntry)
		{
			// Base value should be a copy of the original data element
			_baseValue = new ResourceCostEntry(baseResourceCostEntry.ResourceType, baseResourceCostEntry.Amount, baseResourceCostEntry.Paid);
		}

		/// <summary>
		/// The resource cost entry stored in this instance. Automatically subscribes to all entry member change events.
		/// </summary>
		public override ResourceCostEntry Value
		{
			get { return base.Value; }
			set
			{
				// Update value
				base.Value = value;

				// Subscribe to member update event
				_value.PropertyChanged += (sender, e) =>
				{
					// Set "modified" flag properly
					Modified = !_value.Equals(_baseValue);

					// Send update for binding controls
					OnPropertyChanged(nameof(Value));
				};
			}
		}
	}

	/// <summary>
	/// Represents an resource cost entry.
	/// </summary>
	public class ResourceCostEntry : IEquatable<ResourceCostEntry>, INotifyPropertyChanged
	{
		/// <summary>
		/// The resource type.
		/// </summary>
		private short _resourceType;

		/// <summary>
		/// The resource amount.
		/// </summary>
		private short _amount;

		/// <summary>
		/// States whether the resource is being paid.
		/// </summary>
		private byte _paid;

		/// <summary>
		/// The resource type.
		/// </summary>
		public short ResourceType
		{
			get { return _resourceType; }
			set
			{
				// Update value and raise notification event
				_resourceType = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResourceType)));
			}
		}

		/// <summary>
		/// The resource amount.
		/// </summary>
		public short Amount
		{
			get { return _amount; }
			set
			{
				// Update value and raise notification event
				_amount = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
			}
		}

		/// <summary>
		/// States whether the resource is being paid.
		/// </summary>
		public byte Paid
		{
			get { return _paid; }
			set
			{
				// Update value and raise notification event
				_paid = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Paid)));
			}
		}

		/// <summary>
		/// Creates a new resource cost entry.
		/// </summary>
		/// <param name="resourceType">The resource type.</param>
		/// <param name="amount">The resource amount.</param>
		/// <param name="paid">States whether the resource is being paid.</param>
		public ResourceCostEntry(short resourceType, short amount, byte paid)
		{
			// Story values
			_resourceType = resourceType;
			_amount = amount;
			_paid = paid;
		}

		/// <summary>
		/// Checks for equality with another resource entry.
		/// </summary>
		/// <param name="other">The resource entry to be compared with.</param>
		/// <returns></returns>
		public bool Equals(ResourceCostEntry other) => other != null && ResourceType == other.ResourceType && Amount == other.Amount && Paid == other.Paid;

		/// <summary>
		/// Implementation of PropertyChanged interface.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
	}
}