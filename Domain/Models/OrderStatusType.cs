namespace Domain.Models;

public enum OrderStatusType
{
    Created = 1,
    Pending,
    Success,
    Fail,
    Reject,
    Error
}