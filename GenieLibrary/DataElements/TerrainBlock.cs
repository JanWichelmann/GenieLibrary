using IORAMHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenieLibrary.DataElements
{
    public class TerrainBlock : IGenieDataElement
    {
        public const int TERRAIN_COUNT = 42;

        #region Variablen

        public int VirtualFunctionPtr;
        public int MapPointer;
        public int MapWidth;
        public int MapHeight;
        public int WorldWidth;
        public int WorldHeight;

        public List<TileSize> TileSizes;

        public short PaddingTS;

        public List<Terrain> Terrains;

        public List<TerrainBorder> TerrainBorders;

        public int MapRowOffset;
        public float MapMinX;
        public float MapMinY;
        public float MapMaxX;
        public float MapMaxY;
        public float MapMaxXplus1;
        public float MapMaxYplus1;

        public ushort TerrainsUsed2;
        public ushort BordersUsed;
        public short MaxTerrain;
        public short TileWidth;
        public short TileHeight;
        public short TileHalfHeight;
        public short TileHalfWidth;
        public short ElevHeight;
        public short CurRow;
        public short CurCol;
        public short BlockBegRow;
        public short BlockEndRow;
        public short BlockBegCol;
        public short BlockEndCol;

        public int SearchMapPtr;
        public int SearchMapRowsPtr;
        public byte AnyFrameChange;

        public byte MapVisibleFlag;
        public byte FogFlag;

        public List<byte> SomeBytes;
        public List<int> SomeInt32;

        #endregion Variablen

        #region Funktionen

        public TerrainBlock ReadData(RAMBuffer buffer)
        {
            VirtualFunctionPtr = buffer.ReadInteger();
            MapPointer = buffer.ReadInteger();
            MapWidth = buffer.ReadInteger();
            MapHeight = buffer.ReadInteger();
            WorldWidth = buffer.ReadInteger();
            WorldHeight = buffer.ReadInteger();

            TileSizes = new List<TileSize>(19);
            for(int i = 0; i < 19; ++i)
                TileSizes.Add(new TileSize().ReadData(buffer));

            PaddingTS = buffer.ReadShort();

            Terrains = new List<Terrain>(TERRAIN_COUNT);
            for(int i = 0; i < TERRAIN_COUNT; ++i)
                Terrains.Add(new Terrain().ReadData(buffer));

            TerrainBorders = new List<TerrainBorder>(16);
            for(int i = 0; i < 16; ++i)
                TerrainBorders.Add(new TerrainBorder().ReadData(buffer));

            MapRowOffset = buffer.ReadInteger();
            MapMinX = buffer.ReadFloat();
            MapMinY = buffer.ReadFloat();
            MapMaxX = buffer.ReadFloat();
            MapMaxY = buffer.ReadFloat();
            MapMaxXplus1 = buffer.ReadFloat();
            MapMaxYplus1 = buffer.ReadFloat();

            TerrainsUsed2 = buffer.ReadUShort();
            BordersUsed = buffer.ReadUShort();
            MaxTerrain = buffer.ReadShort();
            TileWidth = buffer.ReadShort();
            TileHeight = buffer.ReadShort();
            TileHalfHeight = buffer.ReadShort();
            TileHalfWidth = buffer.ReadShort();
            ElevHeight = buffer.ReadShort();
            CurRow = buffer.ReadShort();
            CurCol = buffer.ReadShort();
            BlockBegRow = buffer.ReadShort();
            BlockEndRow = buffer.ReadShort();
            BlockBegCol = buffer.ReadShort();
            BlockEndCol = buffer.ReadShort();

            SearchMapPtr = buffer.ReadInteger();
            SearchMapRowsPtr = buffer.ReadInteger();
            AnyFrameChange = buffer.ReadByte();

            MapVisibleFlag = buffer.ReadByte();
            FogFlag = buffer.ReadByte();

            SomeBytes = new List<byte>(21);
            for(int i = 0; i < 21; i++)
                SomeBytes.Add(buffer.ReadByte());

            SomeInt32 = new List<int>(157);
            for(int i = 0; i < 157; i++)
                SomeInt32.Add(buffer.ReadInteger());

            return this;
        }

        public void WriteData(RAMBuffer buffer)
        {
            buffer.WriteInteger(VirtualFunctionPtr);
            buffer.WriteInteger(MapPointer);
            buffer.WriteInteger(MapWidth);
            buffer.WriteInteger(MapHeight);
            buffer.WriteInteger(WorldWidth);
            buffer.WriteInteger(WorldHeight);

            AssertListLength(TileSizes, 19);
            TileSizes.ForEach(ts => ts.WriteData(buffer));

            buffer.WriteShort(PaddingTS);

            Terrains.ForEach(t => t.WriteData(buffer));

            AssertListLength(TerrainBorders, 16);
            TerrainBorders.ForEach(tb => tb.WriteData(buffer));

            buffer.WriteInteger(MapRowOffset);
            buffer.WriteFloat(MapMinX);
            buffer.WriteFloat(MapMinY);
            buffer.WriteFloat(MapMaxX);
            buffer.WriteFloat(MapMaxY);
            buffer.WriteFloat(MapMaxXplus1);
            buffer.WriteFloat(MapMaxYplus1);

            buffer.WriteUShort(TerrainsUsed2);
            buffer.WriteUShort(BordersUsed);
            buffer.WriteShort(MaxTerrain);
            buffer.WriteShort(TileWidth);
            buffer.WriteShort(TileHeight);
            buffer.WriteShort(TileHalfHeight);
            buffer.WriteShort(TileHalfWidth);
            buffer.WriteShort(ElevHeight);
            buffer.WriteShort(CurRow);
            buffer.WriteShort(CurCol);
            buffer.WriteShort(BlockBegRow);
            buffer.WriteShort(BlockEndRow);
            buffer.WriteShort(BlockBegCol);
            buffer.WriteShort(BlockEndCol);

            buffer.WriteInteger(SearchMapPtr);
            buffer.WriteInteger(SearchMapRowsPtr);
            buffer.WriteByte(AnyFrameChange);

            buffer.WriteByte(MapVisibleFlag);
            buffer.WriteByte(FogFlag);

            AssertListLength(SomeBytes, 21);
            SomeBytes.ForEach(b => buffer.WriteByte(b));

            AssertListLength(SomeInt32, 157);
            SomeInt32.ForEach(i => buffer.WriteInteger(i));
        }

        #endregion Funktionen

        #region Strukturen

        public class TileSize : IGenieDataElement
        {
            #region Variablen

            public short Width;
            public short Height;
            public short DeltaY;

            #endregion Variablen

            #region Funktionen

            public TileSize ReadData(RAMBuffer buffer)
            {
                Width = buffer.ReadShort();
                Height = buffer.ReadShort();
                DeltaY = buffer.ReadShort();

                return this;
            }

            public void WriteData(RAMBuffer buffer)
            {
                buffer.WriteShort(Width);
                buffer.WriteShort(Height);
                buffer.WriteShort(DeltaY);
            }

            #endregion Funktionen
        }

        public class FrameData : IGenieDataElement
        {
            #region Variablen

            public short FrameCount;
            public short AngleCount;
            public short ShapeID;

            #endregion Variablen

            #region Funktionen

            public FrameData ReadData(RAMBuffer buffer)
            {
                FrameCount = buffer.ReadShort();
                AngleCount = buffer.ReadShort();
                ShapeID = buffer.ReadShort();

                return this;
            }

            public void WriteData(RAMBuffer buffer)
            {
                buffer.WriteShort(FrameCount);
                buffer.WriteShort(AngleCount);
                buffer.WriteShort(ShapeID);
            }

            #endregion Funktionen
        }

        public class Terrain : IGenieDataElement
        {
            #region Variablen

            public byte Enabled;
            public byte Random;

            public string Name;
            public string Name2;
            public int SLP;
            public int ShapePtr;
            public int SoundID;

            public int BlendPriority;
            public int BlendType;

            public List<byte> Colors;
            public Tuple<byte, byte> CliffColors;
            public byte PassableTerrain;
            public byte ImpassableTerrain;

            public byte IsAnimated;
            public short AnimationFrames;
            public short PauseFames;
            public float Interval;
            public float PauseBetweenLoops;
            public short Frame;
            public short DrawFrame;
            public float AnimateLast;
            public byte FrameChanged;
            public byte Drawn;

            public List<FrameData> ElevationGraphics;

            public short TerrainToDraw;
            public Tuple<short, short> TerrainDimensions;

            public List<short> Borders;

            public List<short> TerrainUnitID;
            public List<short> TerrainUnitDensity;
            public List<byte> TerrainUnitCentering;
            public short NumberOfTerrainUnitsUsed;

            public short Phantom;

            #endregion Variablen

            #region Funktionen

            public Terrain ReadData(RAMBuffer buffer)
            {
                Enabled = buffer.ReadByte();
                Random = buffer.ReadByte();

                Name = buffer.ReadString(13);
                Name2 = buffer.ReadString(13);

                SLP = buffer.ReadInteger();
                ShapePtr = buffer.ReadInteger();
                SoundID = buffer.ReadInteger();

                BlendPriority = buffer.ReadInteger();
                BlendType = buffer.ReadInteger();

                Colors = new List<byte>(3);
                for(int i = 0; i < 3; ++i)
                    Colors.Add(buffer.ReadByte());

                CliffColors = new Tuple<byte, byte>(buffer.ReadByte(), buffer.ReadByte());
                PassableTerrain = buffer.ReadByte();
                ImpassableTerrain = buffer.ReadByte();

                IsAnimated = buffer.ReadByte();
                AnimationFrames = buffer.ReadShort();
                PauseFames = buffer.ReadShort();
                Interval = buffer.ReadFloat();
                PauseBetweenLoops = buffer.ReadFloat();
                Frame = buffer.ReadShort();
                DrawFrame = buffer.ReadShort();
                AnimateLast = buffer.ReadFloat();
                FrameChanged = buffer.ReadByte();
                Drawn = buffer.ReadByte();

                ElevationGraphics = new List<FrameData>(19);
                for(int i = 0; i < 19; ++i)
                    ElevationGraphics.Add(new FrameData().ReadData(buffer));

                TerrainToDraw = buffer.ReadShort();
                TerrainDimensions = new Tuple<short, short>(buffer.ReadShort(), buffer.ReadShort());

                Borders = new List<short>(TERRAIN_COUNT);
                for(int i = 0; i < TERRAIN_COUNT; ++i)
                    Borders.Add(buffer.ReadShort());

                TerrainUnitID = new List<short>(30);
                for(int i = 0; i < 30; ++i)
                    TerrainUnitID.Add(buffer.ReadShort());

                TerrainUnitDensity = new List<short>(30);
                for(int i = 0; i < 30; ++i)
                    TerrainUnitDensity.Add(buffer.ReadShort());

                TerrainUnitCentering = new List<byte>(30);
                for(int i = 0; i < 30; ++i)
                    TerrainUnitCentering.Add(buffer.ReadByte());

                NumberOfTerrainUnitsUsed = buffer.ReadShort();

                Phantom = buffer.ReadShort();

                return this;
            }

            public void WriteData(RAMBuffer buffer)
            {
                buffer.WriteByte(Enabled);
                buffer.WriteByte(Random);

                buffer.WriteString(Name, 13);
                buffer.WriteString(Name2, 13);

                buffer.WriteInteger(SLP);
                buffer.WriteInteger(ShapePtr);
                buffer.WriteInteger(SoundID);

                buffer.WriteInteger(BlendPriority);
                buffer.WriteInteger(BlendType);

                AssertListLength(Colors, 3);
                Colors.ForEach(col => buffer.WriteByte(col));

                buffer.WriteByte(CliffColors.Item1);
                buffer.WriteByte(CliffColors.Item2);

                buffer.WriteByte(PassableTerrain);
                buffer.WriteByte(ImpassableTerrain);

                buffer.WriteByte(IsAnimated);
                buffer.WriteShort(AnimationFrames);
                buffer.WriteShort(PauseFames);
                buffer.WriteFloat(Interval);
                buffer.WriteFloat(PauseBetweenLoops);
                buffer.WriteShort(Frame);
                buffer.WriteShort(DrawFrame);
                buffer.WriteFloat(AnimateLast);
                buffer.WriteByte(FrameChanged);
                buffer.WriteByte(Drawn);

                AssertListLength(ElevationGraphics, 19);
                ElevationGraphics.ForEach(eg => eg.WriteData(buffer));

                buffer.WriteShort(TerrainToDraw);
                buffer.WriteShort(TerrainDimensions.Item1);
                buffer.WriteShort(TerrainDimensions.Item2);

                Borders.ForEach(b => buffer.WriteShort(b));

                AssertListLength(TerrainUnitID, 30);
                TerrainUnitID.ForEach(tu => buffer.WriteShort(tu));

                AssertListLength(TerrainUnitDensity, 30);
                TerrainUnitDensity.ForEach(tu => buffer.WriteShort(tu));

                AssertListLength(TerrainUnitCentering, 30);
                TerrainUnitCentering.ForEach(tu => buffer.WriteByte(tu));

                buffer.WriteShort(NumberOfTerrainUnitsUsed);

                buffer.WriteShort(Phantom);
            }

            #endregion Funktionen
        }

        public class TerrainBorder : IGenieDataElement
        {
            #region Variablen

            public byte Enabled;
            public byte Random;

            public string Name;
            public string Name2;
            public int SLP;
            public int ShapePtr;
            public int SoundID;

            public List<byte> Colors;
            
            public byte IsAnimated;
            public short AnimationFrames;
            public short PauseFames;
            public float Interval;
            public float PauseBetweenLoops;
            public short Frame;
            public short DrawFrame;
            public float AnimateLast;
            public byte FrameChanged;
            public byte Drawn;

            public List<List<FrameData>> Borders;

            public short DrawTerrain;
            public short UnderlayTerrain;
            public short BorderStyle;

            #endregion Variablen

            #region Funktionen

            public TerrainBorder ReadData(RAMBuffer buffer)
            {
                Enabled = buffer.ReadByte();
                Random = buffer.ReadByte();

                Name = buffer.ReadString(13);
                Name2 = buffer.ReadString(13);

                SLP = buffer.ReadInteger();
                ShapePtr = buffer.ReadInteger();
                SoundID = buffer.ReadInteger();

                Colors = new List<byte>(3);
                for(int i = 0; i < 3; ++i)
                    Colors.Add(buffer.ReadByte());

                IsAnimated = buffer.ReadByte();
                AnimationFrames = buffer.ReadShort();
                PauseFames = buffer.ReadShort();
                Interval = buffer.ReadFloat();
                PauseBetweenLoops = buffer.ReadFloat();
                Frame = buffer.ReadShort();
                DrawFrame = buffer.ReadShort();
                AnimateLast = buffer.ReadFloat();
                FrameChanged = buffer.ReadByte();
                Drawn = buffer.ReadByte();

                Borders = new List<List<FrameData>>(19);
                for(int i = 0; i < 19; ++i)
                {
                    List<FrameData> frameData = new List<FrameData>(12);
                    for(int j = 0; j < 12; ++j)
                        frameData.Add(new FrameData().ReadData(buffer));
                    Borders.Add(frameData);
                }

                DrawTerrain = buffer.ReadShort();
                UnderlayTerrain = buffer.ReadShort();
                BorderStyle = buffer.ReadShort();

                return this;
            }

            public void WriteData(RAMBuffer buffer)
            {
                buffer.WriteByte(Enabled);
                buffer.WriteByte(Random);

                buffer.WriteString(Name, 13);
                buffer.WriteString(Name2, 13);

                buffer.WriteInteger(SLP);
                buffer.WriteInteger(ShapePtr);
                buffer.WriteInteger(SoundID);

                AssertListLength(Colors, 3);
                Colors.ForEach(col => buffer.WriteByte(col));

                buffer.WriteByte(IsAnimated);
                buffer.WriteShort(AnimationFrames);
                buffer.WriteShort(PauseFames);
                buffer.WriteFloat(Interval);
                buffer.WriteFloat(PauseBetweenLoops);
                buffer.WriteShort(Frame);
                buffer.WriteShort(DrawFrame);
                buffer.WriteFloat(AnimateLast);
                buffer.WriteByte(FrameChanged);
                buffer.WriteByte(Drawn);

                AssertListLength(Borders, 19);
                Borders.ForEach(b =>
                {
                    AssertListLength(b, 12);
                    b.ForEach(fd => fd.WriteData(buffer));
                });

                buffer.WriteShort(DrawTerrain);
                buffer.WriteShort(UnderlayTerrain);
                buffer.WriteShort(BorderStyle);
            }

            #endregion Funktionen
        }

        #endregion Strukturen
    }
}