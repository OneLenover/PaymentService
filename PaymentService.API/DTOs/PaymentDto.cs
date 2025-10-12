namespace PaymentService.API.DTOs
{
    // Объекты для передачи данных
    public record PaymentDTO(long Id, long OrderId, decimal Price, bool Status, DateTime DateCreate);
    
    public record CreatePaymentDto(long OrderId, decimal Price);

}
