using System.Collections.Generic;

namespace DLib.Pathfinding
{
    public class Vertex
    {
        public Math.Vector Pos { get; private set; }
        public List<Vertex> Neighbours { get; private set; }
        public double distance = double.MaxValue;
        public List<Vertex> route = new List<Vertex>();

        public Vertex(int x, int y)
        {
            Neighbours = new List<Vertex>();
            Pos = new Math.Vector(x, y);
        }

        public void AddConnection(params Vertex[] neighbours)
        {
            foreach (Vertex neighbour in neighbours)
                if (!Neighbours.Contains(neighbour))
                {
                    Neighbours.Add(neighbour);
                    neighbour.AddConnection(this);
                }
        }

        public double GetDistance(Vertex vertex) => Pos.Distance(vertex.Pos);
    }
}
