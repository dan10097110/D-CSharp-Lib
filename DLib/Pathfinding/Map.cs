namespace DLib.Pathfinding
{
    public class Map
    {
        public Vertex[] Vertices { get; private set; }

        public Map(params Vertex[] vertices) => Vertices = vertices;
    }
}
