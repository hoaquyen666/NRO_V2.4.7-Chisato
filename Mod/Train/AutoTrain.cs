using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Analytics;

namespace Assets.Scripts.Assembly_CSharp.Mod.Train
{
    internal class AutoTrain
    {
        private static AutoTrain instance = new AutoTrain();
        public bool isAutoFind = false;

        private List<Point> currentPath = null;
        private int currentPathIndex = 0;
        private Mob lastTarget = null;

        private const int RECALC_PATH_DISTANCE = 100;
        private const int ATTACK_RANGE = 30;
        private const int PATH_UPDATE_INTERVAL = 300;
        private long lastPathUpdateTime = 0;

        public static AutoTrain gI()
        {
            return instance;
        }

        public void GoFindAttackMob()
        {
            while (isAutoFind)
            {
                try
                {
                    if (Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5 ||
                Char.myCharz().isCharge || Char.myCharz().isFlyAndCharge || Char.myCharz().isUseChargeSkill())
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    if (!IsMobValid(Char.myCharz().mobFocus))
                    {
                        Char.myCharz().mobFocus = GetClosestMob();
                        ResetPath();
                    }

                    Mob target = Char.myCharz().mobFocus;

                    if (target != null)
                    {
                        int dist = GetDistance(Char.myCharz().cx, Char.myCharz().cy, target.x, target.y);

                        if (dist > ATTACK_RANGE)
                        {
                            MoveToTarget(target);
                        }
                        else
                        {
                            StopMoving();

                            if (IsMobValid(target))
                            {
                                AttackTarget(target);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
                catch (Exception ex)
                {
                    Char.myCharz().addInfo("Error in GoFindAttackMob: " + ex.Message);
                    Thread.Sleep(500);
                }
            }

            StopMoving();
            ResetPath();
        }

        private void MoveToTarget(Mob target)
        {

            bool needRecalculate = ShouldRecalculatePath(target);

            if (needRecalculate)
            {
                /*currentPath = AStar.getPathtoMob(
                    Char.myCharz().cx,
                    Char.myCharz().cy,
                    target.x,
                    target.y
                );*/

                currentPathIndex = 0;
                lastTarget = target;
                lastPathUpdateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (currentPath == null || currentPath.Count == 0)
                {
                    MoveDirectly(target);
                    return;
                }
            }

            if (currentPath != null && currentPathIndex < currentPath.Count)
            {
                FollowPath(target);
            }
        }

        private bool ShouldRecalculatePath(Mob target)
        {
            if (currentPath == null || currentPath.Count == 0)
                return true;

            if (lastTarget != target)
                return true;

            if (currentPath.Count > 0)
            {
                Point lastGoal = currentPath[currentPath.Count - 1];
                int targetMoved = GetDistance(lastGoal.x, lastGoal.y, target.x, target.y);
                if (targetMoved > RECALC_PATH_DISTANCE)
                    return true;
            }

            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (now - lastPathUpdateTime > PATH_UPDATE_INTERVAL)
                return true;

            if (currentPathIndex >= currentPath.Count)
                return true;

            return false;
        }

        private void FollowPath(Mob target)
        {
            if (currentPath == null || currentPathIndex >= currentPath.Count)
                return;

            Point nextPoint = currentPath[currentPathIndex];
            Char player = Char.myCharz();

            int distToNext = GetDistance(player.cx, player.cy, nextPoint.x, nextPoint.y);

            if (distToNext < 24)
            {
                currentPathIndex++;

                if (currentPathIndex >= currentPath.Count)
                {
                    int distToTarget1 = GetDistance(player.cx, player.cy, target.x, target.y);
                    if (distToTarget1 > ATTACK_RANGE)
                    {
                        ResetPath();
                    }
                    return;
                }

                nextPoint = currentPath[currentPathIndex];
            }

            if (player.currentMovePoint == null ||
                player.currentMovePoint.xEnd != nextPoint.x ||
                player.currentMovePoint.yEnd != nextPoint.y)
            {
                Char.myCharz().currentMovePoint = new MovePoint(target.x, target.y);
            }

            int distToTarget2 = GetDistance(player.cx, player.cy, target.x, target.y);
            if (distToTarget2 <= ATTACK_RANGE)
            {
                StopMoving();
            }
        }

        private void MoveDirectly(Mob target)
        {
            Char.myCharz().currentMovePoint = new MovePoint(target.x, target.y);
        }

        private void StopMoving()
        {
            Char player = Char.myCharz();
            player.currentMovePoint = null;
            player.cvx = 0;
            player.cvy = 0;
        }

        private void ResetPath()
        {
            currentPath = null;
            currentPathIndex = 0;
            lastTarget = null;
        }

        private void AttackTarget(Mob target)
        {
            if (!IsMobValid(target))
                return;

            MyVector vMob = new MyVector();
            vMob.addElement(target);
            Service.gI().sendPlayerAttack(vMob, new MyVector(), 1);

            int cooldownTime = 400;
            int checkInterval = 50;
            int elapsed = 0;

            while (elapsed < cooldownTime && isAutoFind)
            {
                if (!IsMobValid(target))
                    break;

                Thread.Sleep(checkInterval);
                elapsed += checkInterval;
            }
        }

        private bool IsMobValid(Mob mob)
        {
            if (mob == null) return false;
            if (mob.isDie) return false;
            if (mob.hp <= 0) return false;
            if (mob.status == 0 || mob.status == 1) return false;
            if (mob.isMobMe) return false;
            return true;
        }

        private Mob GetClosestMob()
        {
            Mob closest = null;
            int minDis = 9999;

            for (int i = 0; i < GameScr.vMob.size(); i++)
            {
                Mob m = (Mob)GameScr.vMob.elementAt(i);
                if (IsMobValid(m))
                {
                    int d = GetDistance(Char.myCharz().cx, Char.myCharz().cy, m.x, m.y);
                    if (d < minDis)
                    {
                        minDis = d;
                        closest = m;
                    }
                }
            }
            return closest;
        }

        private int GetDistance(int x1, int y1, int x2, int y2)
        {
            return Res.abs(x1 - x2) + Res.abs(y1 - y2);
        }
    }
}
