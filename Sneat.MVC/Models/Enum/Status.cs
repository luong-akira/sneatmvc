
namespace Sneat.MVC.Models.Enum
{
    public enum Status
    {
        ACTIVE = 1,
        IN_ACTIVE = 0
    }

    public enum WorkPackageStatus
    {
        New = 1,
        InProgress = 2,
        InTesting = 3,
        Closed = 4,
        InSpecification = 5,
        Specified = 6,
        Developed = 7,
        Tested = 8,
        OnHold = 9,
        Rejected = 10,
        TestFailed = 11
    }
}