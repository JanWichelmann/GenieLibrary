using IORAMHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenieLibrary.DataElements
{
    public class RandomMaps : IGenieDataElement
    {
        #region Variablen

        public int RandomMapsPtr;
        public List<MapInfo1> Maps1;
        public List<MapInfo2> Maps2;

        #endregion Variablen

        #region Funktionen

        public RandomMaps ReadData(RAMBuffer buffer)
        {
            int mapCount = buffer.ReadInteger();

            RandomMapsPtr = buffer.ReadInteger();

            Maps1 = new List<MapInfo1>(mapCount);
            for(int i = 0; i < mapCount; ++i)
                Maps1.Add(new MapInfo1().ReadData(buffer));
            Maps2 = new List<MapInfo2>(mapCount);
            for(int i = 0; i < mapCount; ++i)
                Maps2.Add(new MapInfo2().ReadData(buffer));

            return this;
        }

        public void WriteData(RAMBuffer buffer)
        {
            buffer.WriteInteger(Maps1.Count);

            buffer.WriteInteger(RandomMapsPtr);

            Maps1.ForEach(m => m.WriteData(buffer));
            Maps2.ForEach(m => m.WriteData(buffer));
        }

        #endregion Funktionen

        #region Strukturen

        public class MapInfo1 : IGenieDataElement
        {
            #region Variablen

            public int MapID;
            public int BorderSouthWest;
            public int BorderNorthWest;
            public int BorderNorthEast;
            public int BorderSouthEast;
            public int BorderUsage;
            public int WaterShape;
            public int BaseTerrain;
            public int LandCoverage;
            public int UnusedID;

            public int MapLandsCount;
            public int MapLandsPtr;

            public int MapTerrainsCount;
            public int MapTerrainsPtr;

            public int MapUnitsCount;
            public int MapUnitsPtr;

            public int MapElevationsCount;
            public int MapElevationsPtr;

            #endregion Variablen

            #region Funktionen

            public MapInfo1 ReadData(RAMBuffer buffer)
            {
                MapID = buffer.ReadInteger();
                BorderSouthWest = buffer.ReadInteger();
                BorderNorthWest = buffer.ReadInteger();
                BorderNorthEast = buffer.ReadInteger();
                BorderSouthEast = buffer.ReadInteger();
                BorderUsage = buffer.ReadInteger();
                WaterShape = buffer.ReadInteger();
                BaseTerrain = buffer.ReadInteger();
                LandCoverage = buffer.ReadInteger();
                UnusedID = buffer.ReadInteger();

                MapLandsCount = buffer.ReadInteger();
                MapLandsPtr = buffer.ReadInteger();
                MapTerrainsCount = buffer.ReadInteger();
                MapTerrainsPtr = buffer.ReadInteger();
                MapUnitsCount = buffer.ReadInteger();
                MapUnitsPtr = buffer.ReadInteger();
                MapElevationsCount = buffer.ReadInteger();
                MapElevationsPtr = buffer.ReadInteger();

                return this;
            }

            public void WriteData(RAMBuffer buffer)
            {
                buffer.WriteInteger(MapID);
                buffer.WriteInteger(BorderSouthWest);
                buffer.WriteInteger(BorderNorthWest);
                buffer.WriteInteger(BorderNorthEast);
                buffer.WriteInteger(BorderSouthEast);
                buffer.WriteInteger(BorderUsage);
                buffer.WriteInteger(WaterShape);
                buffer.WriteInteger(BaseTerrain);
                buffer.WriteInteger(LandCoverage);
                buffer.WriteInteger(UnusedID);

                buffer.WriteInteger(MapLandsCount);
                buffer.WriteInteger(MapLandsPtr);
                buffer.WriteInteger(MapTerrainsCount);
                buffer.WriteInteger(MapTerrainsPtr);
                buffer.WriteInteger(MapUnitsCount);
                buffer.WriteInteger(MapUnitsPtr);
                buffer.WriteInteger(MapElevationsCount);
                buffer.WriteInteger(MapElevationsPtr);
            }

            #endregion Funktionen
        }

        public class MapInfo2 : IGenieDataElement
        {
            #region Variablen

            public int BorderSouthWest;
            public int BorderNorthWest;
            public int BorderNorthEast;
            public int BorderSouthEast;
            public int BorderUsage;
            public int WaterShape;
            public int BaseTerrain;
            public int LandCoverage;
            public int UnusedID;

            public int MapLandsCount;
            public int MapLandsPtr;
            public List<MapLand> MapLands;

            public int MapTerrainsCount;
            public int MapTerrainsPtr;
            public List<MapTerrain> MapTerrains;

            public int MapUnitsCount;
            public int MapUnitsPtr;
            public List<MapUnit> MapUnits;

            public int MapElevationsCount;
            public int MapElevationsPtr;
            public List<MapElevation> MapElevations;

            #endregion Variablen

            #region Funktionen

            public MapInfo2 ReadData(RAMBuffer buffer)
            {
                BorderSouthWest = buffer.ReadInteger();
                BorderNorthWest = buffer.ReadInteger();
                BorderNorthEast = buffer.ReadInteger();
                BorderSouthEast = buffer.ReadInteger();
                BorderUsage = buffer.ReadInteger();
                WaterShape = buffer.ReadInteger();
                BaseTerrain = buffer.ReadInteger();
                LandCoverage = buffer.ReadInteger();
                UnusedID = buffer.ReadInteger();

                MapLandsCount = buffer.ReadInteger();
                MapLandsPtr = buffer.ReadInteger();
                MapLands = new List<MapLand>(MapLandsCount);
                for(int i = 0; i < MapLandsCount; ++i)
                    MapLands.Add(new MapLand().ReadData(buffer));

                MapTerrainsCount = buffer.ReadInteger();
                MapTerrainsPtr = buffer.ReadInteger();
                MapTerrains = new List<MapTerrain>(MapTerrainsCount);
                for(int i = 0; i < MapTerrainsCount; ++i)
                    MapTerrains.Add(new MapTerrain().ReadData(buffer));

                MapUnitsCount = buffer.ReadInteger();
                MapUnitsPtr = buffer.ReadInteger();
                MapUnits = new List<MapUnit>(MapUnitsCount);
                for(int i = 0; i < MapUnitsCount; ++i)
                    MapUnits.Add(new MapUnit().ReadData(buffer));

                MapElevationsCount = buffer.ReadInteger();
                MapElevationsPtr = buffer.ReadInteger();
                MapElevations = new List<MapElevation>(MapElevationsCount);
                for(int i = 0; i < MapElevationsCount; ++i)
                    MapElevations.Add(new MapElevation().ReadData(buffer));

                return this;
            }

            public void WriteData(RAMBuffer buffer)
            {
                buffer.WriteInteger(BorderSouthWest);
                buffer.WriteInteger(BorderNorthWest);
                buffer.WriteInteger(BorderNorthEast);
                buffer.WriteInteger(BorderSouthEast);
                buffer.WriteInteger(BorderUsage);
                buffer.WriteInteger(WaterShape);
                buffer.WriteInteger(BaseTerrain);
                buffer.WriteInteger(LandCoverage);
                buffer.WriteInteger(UnusedID);

                buffer.WriteInteger(MapLandsCount);
                buffer.WriteInteger(MapLandsPtr);
                MapLands.ForEach(m => m.WriteData(buffer));

                buffer.WriteInteger(MapTerrainsCount);
                buffer.WriteInteger(MapTerrainsPtr);
                MapTerrains.ForEach(m => m.WriteData(buffer));

                buffer.WriteInteger(MapUnitsCount);
                buffer.WriteInteger(MapUnitsPtr);
                MapUnits.ForEach(m => m.WriteData(buffer));

                buffer.WriteInteger(MapElevationsCount);
                buffer.WriteInteger(MapElevationsPtr);
                MapElevations.ForEach(m => m.WriteData(buffer));
            }

            #endregion Funktionen
        }

        public class MapElevation : IGenieDataElement
        {
            #region Variablen

            public int Proportion = 0;
            public int Terrain = -1;
            public int ClumpCount = 0;
            public int BaseTerrain = -1;
            public int BaseElevation = 0;
            public int TileSpacing = 0;

            #endregion Variablen

            #region Funktionen

            public MapElevation ReadData(RAMBuffer buffer)
            {
                Proportion = buffer.ReadInteger();
                Terrain = buffer.ReadInteger();
                ClumpCount = buffer.ReadInteger();
                BaseTerrain = buffer.ReadInteger();
                BaseElevation = buffer.ReadInteger();
                TileSpacing = buffer.ReadInteger();

                return this;
            }

            public void WriteData(RAMBuffer buffer)
            {
                buffer.WriteInteger(Proportion);
                buffer.WriteInteger(Terrain);
                buffer.WriteInteger(ClumpCount);
                buffer.WriteInteger(BaseTerrain);
                buffer.WriteInteger(BaseElevation);
                buffer.WriteInteger(TileSpacing);
            }

            #endregion Funktionen
        }

        public class MapUnit : IGenieDataElement
        {
            #region Variablen

            public int Unit = -1;
            public int HostTerrain = -1;
            public byte GroupPlacing = 0;
            public byte ScaleFlag = 0;
            public short Padding1 = 0;
            public int ObjectsPerGroup = 1;
            public int Fluctuation = 0;
            public int GroupsPerPlayer = 1;
            public int GroupArea = 1;
            public int PlayerID = 0;
            public int SetPlaceForAllPlayers = 1;
            public int MinDistanceToPlayers = 2;
            public int MaxDistanceToPlayers = 6;

            #endregion Variablen

            #region Funktionen

            public MapUnit ReadData(RAMBuffer buffer)
            {
                Unit = buffer.ReadInteger();
                HostTerrain = buffer.ReadInteger();
                GroupPlacing = buffer.ReadByte();
                ScaleFlag = buffer.ReadByte();
                Padding1 = buffer.ReadShort();
                ObjectsPerGroup = buffer.ReadInteger();
                Fluctuation = buffer.ReadInteger();
                GroupsPerPlayer = buffer.ReadInteger();
                GroupArea = buffer.ReadInteger();
                PlayerID = buffer.ReadInteger();
                SetPlaceForAllPlayers = buffer.ReadInteger();
                MinDistanceToPlayers = buffer.ReadInteger();
                MaxDistanceToPlayers = buffer.ReadInteger();

                return this;
            }

            public void WriteData(RAMBuffer buffer)
            {
                buffer.WriteInteger(Unit);
                buffer.WriteInteger(HostTerrain);
                buffer.WriteByte(GroupPlacing);
                buffer.WriteByte(ScaleFlag);
                buffer.WriteShort(Padding1);
                buffer.WriteInteger(ObjectsPerGroup);
                buffer.WriteInteger(Fluctuation);
                buffer.WriteInteger(GroupsPerPlayer);
                buffer.WriteInteger(GroupArea);
                buffer.WriteInteger(PlayerID);
                buffer.WriteInteger(SetPlaceForAllPlayers);
                buffer.WriteInteger(MinDistanceToPlayers);
                buffer.WriteInteger(MaxDistanceToPlayers);
            }

            #endregion Funktionen
        }

        public class MapTerrain : IGenieDataElement
        {
            #region Variablen

            public int Proportion = 0;
            public int Terrain = -1;
            public int ClumpCount = 0;
            public int EdgeSpacing = 0;
            public int PlacementTerrain = -1;
            public int Clumpiness = 0;

            #endregion Variablen

            #region Funktionen

            public MapTerrain ReadData(RAMBuffer buffer)
            {
                Proportion = buffer.ReadInteger();
                Terrain = buffer.ReadInteger();
                ClumpCount = buffer.ReadInteger();
                EdgeSpacing = buffer.ReadInteger();
                PlacementTerrain = buffer.ReadInteger();
                Clumpiness = buffer.ReadInteger();

                return this;
            }

            public void WriteData(RAMBuffer buffer)
            {
                buffer.WriteInteger(Proportion);
                buffer.WriteInteger(Terrain);
                buffer.WriteInteger(ClumpCount);
                buffer.WriteInteger(EdgeSpacing);
                buffer.WriteInteger(PlacementTerrain);
                buffer.WriteInteger(Clumpiness);
            }

            #endregion Funktionen
        }

        public class MapLand : IGenieDataElement
        {
            #region Variablen

            public int LandID = 1;
            public int Terrain = -1;
            public int LandSpacing = 2;
            public int BaseSize = 7;
            public byte Zone = 0;
            public byte PlacementType = 1;
            public short Padding1 = 0;
            public int BaseX = 0;
            public int BaseY = 0;
            public byte LandProportion = 100;
            public byte ByPlayerFlag = 1;
            public short Padding2 = 0;
            public int StartAreaRadius = 10;
            public int TerrainEdgeFade = 25;
            public int Clumpiness = 8;

            #endregion Variablen

            #region Funktionen

            public MapLand ReadData(RAMBuffer buffer)
            {
                LandID = buffer.ReadInteger();
                Terrain = buffer.ReadInteger();
                LandSpacing = buffer.ReadInteger();
                BaseSize = buffer.ReadInteger();
                Zone = buffer.ReadByte();
                PlacementType = buffer.ReadByte();
                Padding1 = buffer.ReadShort();
                BaseX = buffer.ReadInteger();
                BaseY = buffer.ReadInteger();
                LandProportion = buffer.ReadByte();
                ByPlayerFlag = buffer.ReadByte();
                Padding2 = buffer.ReadShort();
                StartAreaRadius = buffer.ReadInteger();
                TerrainEdgeFade = buffer.ReadInteger();
                Clumpiness = buffer.ReadInteger();

                return this;
            }

            public void WriteData(RAMBuffer buffer)
            {
                buffer.WriteInteger(LandID);
                buffer.WriteInteger(Terrain);
                buffer.WriteInteger(LandSpacing);
                buffer.WriteInteger(BaseSize);
                buffer.WriteByte(Zone);
                buffer.WriteByte(PlacementType);
                buffer.WriteShort(Padding1);
                buffer.WriteInteger(BaseX);
                buffer.WriteInteger(BaseY);
                buffer.WriteByte(LandProportion);
                buffer.WriteByte(ByPlayerFlag);
                buffer.WriteShort(Padding2);
                buffer.WriteInteger(StartAreaRadius);
                buffer.WriteInteger(TerrainEdgeFade);
                buffer.WriteInteger(Clumpiness);
            }

            #endregion Funktionen
        }


        #endregion Strukturen
    }
}
