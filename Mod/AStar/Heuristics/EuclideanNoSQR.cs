using System;

namespace Assets.Scripts.Assembly_CSharp.Mod.AStar.Heuristics
{
    public class EuclideanNoSQR : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var heuristicEstimate = 2;
            var h = (int)(heuristicEstimate * (System.Math.Pow((source.Row - destination.Row), 2) + System.Math.Pow((source.Column - destination.Column), 2)));
            return h;
        }
    }
}