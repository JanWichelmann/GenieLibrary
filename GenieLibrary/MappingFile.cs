using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AoEBalancingTool
{
	/// <summary>
	/// Contains a mapping of dynamically assigned DAT IDs to editor IDs.
	/// </summary>
	public class MappingFile
	{
		/// <summary>
		/// The DAT hash this mapping is valid for.
		/// </summary>
		public byte[] Hash { get; set; }

		/// <summary>
		/// The mapping DAT ID => Editor ID of units.
		/// </summary>
		public Dictionary<short, short> UnitMapping { get; set; }

		/// <summary>
		/// The mapping DAT ID => Editor ID of researches.
		/// </summary>
		public Dictionary<short, short> ResearchMapping { get; set; }

		/// <summary>
		/// Reads mapping data from the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer containing the mapping data.</param>
		public MappingFile(IORAMHelper.RAMBuffer buffer)
		{
			// Read hash
			Hash = buffer.ReadByteArray(16); // 128 Bit = 16 Byte

			// Read unit IDs
			int count = buffer.ReadInteger();
			UnitMapping = new Dictionary<short, short>(count);
			for(int i = 0; i < count; ++i)
				UnitMapping.Add(buffer.ReadShort(), buffer.ReadShort());

			// Read research IDs
			count = buffer.ReadInteger();
			ResearchMapping = new Dictionary<short, short>(count);
			for(int i = 0; i < count; ++i)
				ResearchMapping.Add(buffer.ReadShort(), buffer.ReadShort());
		}

		/// <summary>
		/// Writes the mapping data into the given buffer.
		/// </summary>
		/// <param name="buffer">The buffer the mapping data shall be written to.</param>
		public void WriteData(IORAMHelper.RAMBuffer buffer)
		{
			buffer.Write(Hash);
			buffer.WriteInteger(UnitMapping.Count);
			foreach(var m in UnitMapping)
			{
				buffer.WriteShort(m.Key);
				buffer.WriteShort(m.Value);
			}
			buffer.WriteInteger(ResearchMapping.Count);
			foreach(var m in ResearchMapping)
			{
				buffer.WriteShort(m.Key);
				buffer.WriteShort(m.Value);
			}
		}

		/// <summary>
		/// Checks whether this mapping file is valid for the given genie file, by computing and comparing its hash.
		/// </summary>
		/// <param name="genieFile">The genie file to be checked.</param>
		/// <returns></returns>
		public bool CheckCompabilityToGenieFile(GenieLibrary.GenieFile genieFile)
		{
			// Calculate hash over string of all unit and research IDs/names, such that each one is contained at least once
			using(MemoryStream hashString = new MemoryStream())
			{
				// Write unit IDs and names
				HashSet<int> unitIdsRead = new HashSet<int>();
				foreach(GenieLibrary.DataElements.Civ c in genieFile.Civs)
					foreach(var u in c.Units)
						if(!unitIdsRead.Contains(u.Key))
						{
							// Append name and ID
							byte[] uName = Encoding.ASCII.GetBytes(u.Value.Name1.TrimEnd('\0'));
							hashString.Write(uName, 0, uName.Length);
							hashString.Write(BitConverter.GetBytes((short)u.Key), 0, 2);

							// Unit was added
							unitIdsRead.Add(u.Key);
						}

				// Write research IDs and names
				for(short r = 0; r < genieFile.Researches.Count; ++r)
				{
					// Append name and ID
					byte[] rName = Encoding.ASCII.GetBytes(genieFile.Researches[r].Name.TrimEnd('\0'));
					hashString.Write(rName, 0, rName.Length);
					hashString.Write(BitConverter.GetBytes(r), 0, 2);
				}

				// Calculate hash and check
				hashString.Seek(0, SeekOrigin.Begin);
				using(MD5 md5 = MD5.Create())
					return Hash.SequenceEqual(md5.ComputeHash(hashString));
			}
		}
	}
}
