namespace DLib
{
    public struct Coord
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public Coord(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
