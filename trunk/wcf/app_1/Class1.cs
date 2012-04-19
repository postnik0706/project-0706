using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace app_1
{
    public class Class1
    {
        [ServiceContract()]
        public interface ICarRentalService
        {
            [OperationContract()]
            double CalculatePrice(DateTime pickupDate, DateTime returnDate, string pickupLocation,
                string vehiclePreference);
        }

        public class CarRentalService : ICarRentalService
        {
            public double CalculatePrice(DateTime pickupDate, DateTime returnDate, string pickupLocation,
                string vehiclePreference)
            {
                Random r = new Random(DateTime.Now.Millisecond);
                return r.NextDouble() * 500;
            }
        }
    }
}
