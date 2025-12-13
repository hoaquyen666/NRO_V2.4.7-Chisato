using System;

namespace Assets.Scripts.Assembly_CSharp.Mod.AStar.Heuristics
{
    public class Custom1 : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var heuristicEstimate = 2;
            var dxy = new Position(System.Math.Abs(destination.Row - source.Row), System.Math.Abs(destination.Column - source.Column));
            var Orthogonal = System.Math.Abs(dxy.Row - dxy.Column);
            var Diagonal = System.Math.Abs(((dxy.Row + dxy.Column) - Orthogonal) / 2);
            var h = heuristicEstimate * (Diagonal + Orthogonal + dxy.Row + dxy.Column);
            return h;
        }
    }
}