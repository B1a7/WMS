namespace WMS.Models.Dtos.Layout
{
    public struct Coordinates
    {
        public int X { get; }
        public int Y { get; }
        public int Z { get; }


        public Coordinates(int x, int y, int z)
        { 
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() => $"{X}.{Y}.{Z}";
    }

}
