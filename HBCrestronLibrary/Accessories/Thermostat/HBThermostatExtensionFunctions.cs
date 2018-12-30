namespace HBCrestronLibrary.Accessories.Thermostat
{
    static class HBThermostatExtension
    {
        public static double FtoC(double f)
        {
            return (5.0 / 9.0) * (f - 32);
        }

        public static double CtoF(double c)
        {
            return ((9.0 / 5.0) * c) + 32;
        }
    }
}