using System;

namespace ApexaContractorAPI.Classes
{
    public static class HealthStatus
    {
        public static string GetRandomHealthStatusByProbability()
        {
            var result = "";
            var num = new Random().Next(1, 10);

            if (num <= 2)
            {
                //20% probability
                result = "Red";
            }
            else if (num <= 4)
            {
                //20% probability
                result = "Yellow";
            }
            else
            {
                //60% probability
                result = "Green";
            }
            return result;
        }
    }
}
