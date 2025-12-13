using System;

namespace Assets.Scripts.Assembly_CSharp.Mod.AStar.Heuristics
{
    public class Euclidean : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var heuristicEstimate = 2;
            var h = (int)(heuristicEstimate * System.Math.Sqrt(System.Math.Pow((source.Row - destination.Row), 2) + System.Math.Pow((source.Column - destination.Column), 2)));
            return h;
        }
    }
}