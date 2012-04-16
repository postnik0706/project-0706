using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace My.Service.Contracts
{
    [ServiceContract()]
    public interface ICarRentalService
	{
        [OperationContract()]
        double CalculatePrice(DateTime pickupDate, DateTime returnDate, string pickupLocation, string carType);
	}
}