using AutoMapper;
using InvoiceManagerApi.Enums;
using InvoiceManagerApi.Models;
using InvoiceManagerApiFinal.DTOs;
using InvoiceManagerApiFinal.Models;

namespace InvoiceManagerApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Customer

        CreateMap<Customer, CustomerResponseDto>()
            .ForMember(dest => dest.InvoiceCount,
                opt => opt.MapFrom(src => src.Invoices.Count()))
            .ForMember(dest => dest.InvoicesSum,
                opt => opt.MapFrom(src =>
                    src.Invoices.Sum(i => i.Rows.Sum(ir => ir.Sum))));

        CreateMap<CustomerCreateRequest, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<CustomerUpdateRequest, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        // Invoice

        CreateMap<Invoice, InvoiceResponseDto>()
            .ForMember(dest => dest.Status,
            opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.RowsCount,
            opt => opt.MapFrom(src => src.Rows.Count()))
            .ForMember(dest => dest.CustomerName,
            opt => opt.MapFrom(src => src.Customer.Name))
            .ForMember(dest => dest.CustomerEmail,
            opt => opt.MapFrom(src => src.Customer.Email));

        CreateMap<InvoiceCreateRequest, Invoice>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore());

        CreateMap<InvoiceRowUpdateRequest, Invoice>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Customer, opt => opt.Ignore());

        // Invoice Row

        CreateMap<InvoiceRow, InvoiceRowResponseDto>()
            .ForMember(dest => dest.InvoiceStatus,
            opt => opt.MapFrom(src => src.Invoice.Status.ToString()));

        CreateMap<InvoiceRowCreateRequest, InvoiceRow>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Invoice, opt => opt.Ignore())
            .ForMember(dest => dest.Sum, opt => opt.MapFrom(src => src.Rate * src.Quantity));

        CreateMap<InvoiceRowUpdateRequest, InvoiceRow>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Invoice, opt => opt.Ignore())
            .ForMember(dest => dest.Sum, opt => opt.MapFrom(src => src.Rate * src.Quantity));

        // Auth

        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<User, AuthResponseDto>();

        // User
        CreateMap<User, UserResponseDto>();
    }
}
