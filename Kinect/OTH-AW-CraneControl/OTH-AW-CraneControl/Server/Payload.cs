using System.Diagnostics;

namespace Cranecontrol.Server
{
    public class Payload
    {
        private float fLeftVelocity;
        private float fRightVelocity;

        private bool LastPowerState = true;
        private int counterP;

        public Payload(float acceleration)
        {
            this.acceleration = acceleration;
        }

        public float acceleration { get; set; }
        public bool ReglerState { get; private set; }
        public bool PowerState { get; private set; }
        public int LeftVelocity { get; private set; }
        public int RightVelocity { get; private set; }

        public bool RightState { get; private set; }
        public bool LefttState { get; private set; }
        public bool UpState { get; private set; }
        public bool DownState { get; private set; }

        public byte[] ServerData => ReturnServerData();

        private byte[] ReturnServerData()
        {
            var data = new byte[5];

            if (counterP > 0)
            {
                data[0] = 1;
                counterP--;
            }
            else
            {
                data[0] = 0;
            }
            
            if (ReglerState)
                data[4] = 1;
            else
                data[4] = 2;

            if (UpState) data[3] = 1;

            if (DownState) data[3] = 2;

            if (LefttState) data[1] = (byte)LeftVelocity;

            if (RightState) data[2] = (byte)RightVelocity;

            return data;
        }

        public int Refresh(bool left, bool right, bool up, bool down, bool regler, bool power)
        {
            LastPowerState = PowerState;
            PowerState = power;
            if ((PowerState != LastPowerState)&&counterP == 0) counterP = 10;
            ReglerState = regler;
            DownState = down;
            LefttState = left;
            RightState = right;
            UpState = up;
            acceleration = acceleration;

            if (LefttState)
            {
                fRightVelocity = 0;
                fLeftVelocity += acceleration;
                LeftVelocity = (int)fLeftVelocity;
                if (LeftVelocity >= 100) LeftVelocity = 100;
                return LeftVelocity;
            }
            else if (RightState)
            {
                fLeftVelocity = 0;
                fRightVelocity += acceleration;
                RightVelocity = (int)fRightVelocity;
                if (RightVelocity >= 100) RightVelocity = 100;
                return RightVelocity;
            }
            fRightVelocity = 0;
            fLeftVelocity = 0;
            return 0;
        }
    }
}