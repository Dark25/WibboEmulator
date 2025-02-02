namespace WibboEmulator.Games.Rooms;

using WibboEmulator.Games.Rooms.Map;

public class RoomModel
{
    public int DoorX { get; set; }
    public int DoorY { get; set; }
    public double DoorZ { get; set; }
    public int DoorOrientation { get; set; }
    public int WallHeight { get; set; }
    public string Heightmap { get; set; }

    public SquareStateType[,] SqState { get; set; }
    public short[,] SqFloorHeight { get; set; }
    public int MapSizeX { get; set; }
    public int MapSizeY { get; set; }

    public RoomModel(string id, int doorX, int doorY, double doorZ, int doorOrientation, string heightmap, int wallheight)
    {
        try
        {
            this.DoorX = doorX;
            this.DoorY = doorY;
            this.DoorZ = doorZ;
            this.DoorOrientation = doorOrientation;
            this.Heightmap = heightmap.ToLower();
            var tmpHeightmap = this.Heightmap.Split(new char[1] { Convert.ToChar(13) });
            this.MapSizeX = tmpHeightmap[0].Length;
            this.MapSizeY = tmpHeightmap.Length;
            this.SqState = new SquareStateType[this.MapSizeX, this.MapSizeY];
            this.SqFloorHeight = new short[this.MapSizeX, this.MapSizeY];
            this.WallHeight = wallheight;

            for (var y = 0; y < this.MapSizeY; y++)
            {
                var line = tmpHeightmap[y];
                line = line.Replace("\r", "");
                line = line.Replace("\n", "");
                line = line.Replace(" ", "");

                var x = 0;
                foreach (var square in line)
                {
                    if (square == 'x')
                    {
                        this.SqState[x, y] = SquareStateType.Bloked;
                    }
                    else
                    {
                        this.SqState[x, y] = SquareStateType.Open;
                        this.SqFloorHeight[x, y] = Parse(square);
                    }
                    x++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during room modeldata loading for model " + id + ": " + ex);
        }
    }

    public static short Parse(char input)
    {
        switch (input)
        {
            case 'y':
            case 'x':
            case 'z':
            case '0':
                return 0;
            case '1':
                return 1;
            case '2':
                return 2;
            case '3':
                return 3;
            case '4':
                return 4;
            case '5':
                return 5;
            case '6':
                return 6;
            case '7':
                return 7;
            case '8':
                return 8;
            case '9':
                return 9;
            case 'a':
                return 10;
            case 'b':
                return 11;
            case 'c':
                return 12;
            case 'd':
                return 13;
            case 'e':
                return 14;
            case 'f':
                return 15;
            case 'g':
                return 16;
            case 'h':
                return 17;
            case 'i':
                return 18;
            case 'j':
                return 19;
            case 'k':
                return 20;
            case 'l':
                return 21;
            case 'm':
                return 22;
            case 'n':
                return 23;
            case 'o':
                return 24;
            case 'p':
                return 25;
            case 'q':
                return 26;
            case 'r':
                return 27;
            case 's':
                return 28;
            case 't':
                return 29;
            case 'u':
                return 30;
            case 'v':
                return 31;
            case 'w':
                return 32;
            default:
                Console.WriteLine("The input was not in a correct format: input must be between (0-k) : " + input);
                return 0;
        }
    }
}
