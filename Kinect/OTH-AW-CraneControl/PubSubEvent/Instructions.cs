namespace SharedRessources
{
    public class Instructions
    {
        public bool axisState;
        public bool controllerState;
        public bool downState;
        public bool leftState;
        public bool rightState;
        public bool upState;

        public void resetDirections()
        {
            leftState = false;
            rightState = false;
            upState = false;
            downState = false;
        }
    }
}