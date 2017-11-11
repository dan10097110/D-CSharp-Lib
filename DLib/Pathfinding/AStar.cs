using System.Collections.Generic;

namespace DLib.Pathfinding
{
    public static class AStar
    {
        public static Vertex[] Solve(Vertex start, Vertex finish)
        {
            List<Vertex> outest = new List<Vertex>() { start };
            start.distance = 0;
            while (outest[0] != finish)
            {
                foreach (Vertex neighbor in outest[0].Neighbours)
                {
                    double newDistance = outest[0].distance + outest[0].GetDistance(neighbor);
                    if (newDistance < neighbor.distance)
                    {
                        neighbor.distance = newDistance;
                        neighbor.route = new List<Vertex>(outest[0].route);
                        neighbor.route.Add(outest[0]);
                        outest.Add(neighbor);
                    }
                }
                outest.Remove(outest[0]);
                outest.Sort(delegate (Vertex a, Vertex b) { return a.GetDistance(finish).CompareTo(b.GetDistance(finish)); });
            }
            return finish.route.ToArray();
        }
    }
}
