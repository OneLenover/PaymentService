namespace PaymentService.API.DTOs
{
    public class PaymentCreatedEvent
    {
        public string Type { get; set; } = "PaymentCreated";
        public long PaymentId { get; set; }
        public long OrderId { get; set; }
        public decimal Price {  get; set; }
        public bool Status { get; set; }
    }

    public class PaymentUpdatedEvent
    {
        public string Type { get; set; } = "PaymentStatusUpdated";
        public long PaymentId { get; set; }
        public long OrderId { get; set; }
        public bool Status { get; set;}
    }
}
