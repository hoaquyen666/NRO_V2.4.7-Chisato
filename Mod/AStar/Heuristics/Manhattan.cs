using System;

namespace Assets.Scripts.Assembly_CSharp.Mod.AStar.Heuristics
{
    public class Manhattan : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var heuristicEstimate = 2;
            var h = heuristicEstimate * (System.Math.Abs(source.Row - destination.Row) + System.Math.Abs(source.Column - destination.Column));
            return h;
        }
    }
}