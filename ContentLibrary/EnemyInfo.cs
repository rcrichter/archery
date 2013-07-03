using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentLibrary
{
    public class EnemyInfo
    {
        public int XStart;
        public int YStart;
        public int StartTime;
        public string EnemyType;
        public float Speed;

        private EnemyInfo()
        {
        }

        public EnemyInfo(string type, int time, int x, int y, float speed)
        {
            XStart = x;
            YStart = y;
            StartTime = time;
            EnemyType = type;
            Speed = speed;
        }
    }
}
