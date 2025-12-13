using System;

namespace Assets.Scripts.Assembly_CSharp.Mod.AStar.Heuristics
{
    public class DiagonalShortcut : ICalculateHeuristic
    {
        public int Calculate(Position source, Position destination)
        {
            var hDiagonal = System.Math.Min(System.Math.Abs(source.Row - destination.Row),
                System.Math.Abs(source.Column - destination.Column));
            var hStraight = (System.Math.Abs(source.Row - destination.Row) + System.Math.Abs(source.Column - destination.Column));
            var heuristicEstimate = 2;
            var h = (heuristicEstimate * 2) * hDiagonal + heuristicEstimate * (hStraight - 2 * hDiagonal);
            return h;
        }
    }
}