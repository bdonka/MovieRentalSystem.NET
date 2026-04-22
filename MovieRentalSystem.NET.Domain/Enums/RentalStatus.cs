namespace MovieRentalSystem.NET.Domain.Enums;

public enum RentalStatus
{
    Preparing, // when rental is ordered online, worker should prepare the copy for the customer
    ReadyToCollect, // worker has prepared the copy and is ready to be collected
    Active, // customer has collected the copy
    Completed // customer has returned the copy to the store
}
