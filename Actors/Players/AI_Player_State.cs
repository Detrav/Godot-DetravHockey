using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetravHockey.Actors.Players
{
    public class AI_Player_State
    {
        public int StateIndex { get; }
        public double ProcessTimerMax { get; }
        public double CooldownTimerMax { get; }
        public double ProcessTimer { get; private set; }
        public double CooldownTimer { get; private set; }
        public bool IsProcessed => ProcessTimer > 0;
        public bool CanStart => CooldownTimer <= 0;

        public AI_Player_State(int stateIndex, double processTimer, double cooldownTimer)
        {
            StateIndex = stateIndex;
            ProcessTimerMax = processTimer;
            CooldownTimerMax = cooldownTimer;
        }

        public void Tick(double delta)
        {
            if (ProcessTimer >= 0)
                ProcessTimer -= delta;
            if (CooldownTimer >= 0)
                CooldownTimer -= delta;
        }

        public void Start()
        {
            ProcessTimer = ProcessTimerMax;
            CooldownTimer = CooldownTimerMax;
        }
    }
}
