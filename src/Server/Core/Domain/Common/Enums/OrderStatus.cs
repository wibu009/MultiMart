namespace MultiMart.Domain.Common.Enums;

public enum OrderStatus
{
    PendingPayment, // The order has been placed but not yet paid.
    PaymentConfirmed, // The payment has been received.
    Processing, // The order is being processed/prepared.
    Shipped, // The order has been shipped.
    Delivered, // The order has been delivered to the customer.
    Cancelled, // The order has been cancelled.
    Returned // The order has been returned by the customer.
}