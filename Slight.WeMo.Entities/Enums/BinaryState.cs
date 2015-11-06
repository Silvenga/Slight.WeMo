namespace Slight.WeMo.Entities.Enums
{
    using System;

    public enum BinaryState
    {
        Error = 0,
        Off = 1,
        On = 2
    }

    public static class BinaryStateHelp
    {
        public static string BinaryStateToString(BinaryState state)
        {
            switch (state)
            {
                case BinaryState.Off:
                    return "0";
                case BinaryState.On:
                    return "1";
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public static BinaryState StringToBinaryState(string state)
        {
            switch (state)
            {
                case "0":
                    return BinaryState.Off;
                case "1":
                    return BinaryState.On;
                default:
                    return BinaryState.Error;
            }
        }
    }
}
