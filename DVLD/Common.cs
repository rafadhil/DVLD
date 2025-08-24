using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Common
{
    public enum enTestType
    {
        Vision =1,
        Written = 2,
        Street = 3
    }

    public enum enLicenseIssueReason
    {
        FirstTime = 1,
        Renew = 2,
        ReplacementForDamagedLicense = 3,
        ReplacementForLostLicense = 4
    }

    public enum enApplicationType
    {
        NewLocalDrivingLicense = 1,
        RenewDrivingLicense = 2,
        ReplacementForLostLicense = 3,
        ReplacementForDamagedLicense = 4,
        ReleaseDetainedLicense = 5,
        NewInternationalLicense = 6,
        RetakeTest = 7
    }

    public enum enLicenseClass
    {
        SmallMotorcycle = 1,
        HeavyMotorcycle = 2,
        OrdinaryDrivingLicense = 3,
        Commercial = 4,
        Agricultural = 5,
        SmallAndMediumBus = 6,
        TruckAndHeavyVehicle = 7
    }

}
