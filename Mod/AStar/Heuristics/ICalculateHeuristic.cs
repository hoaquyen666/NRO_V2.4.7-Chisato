namespace Assets.Scripts.Assembly_CSharp.Mod.AStar.Heuristics
{
    public interface ICalculateHeuristic
    {
        int Calculate(Position source, Position destination);
    }
}