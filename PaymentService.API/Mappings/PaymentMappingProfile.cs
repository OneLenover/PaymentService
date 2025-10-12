using AutoMapper;
using PaymentService.API.DTOs;
using PaymentService.API.UseCases.CreatePayment;
using PaymentService.DataAccess.Postgres.Entities;

namespace PaymentService.API.Mappings
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile() 
        {
            CreateMap<Payment, PaymentDTO>();
            CreateMap<CreatePaymentCommand, Payment>();
            CreateMap<Payment, PaymentCreatedEvent>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(_ => "PaymentCreated"));
            CreateMap<Payment, PaymentUpdatedEvent>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(_ => "PaymentStatusUpdated"));
        }
    }
}
