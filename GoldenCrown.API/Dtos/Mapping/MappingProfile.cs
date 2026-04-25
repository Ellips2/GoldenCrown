using AutoMapper;
using GoldenCrown.Application.DTOs.Finance;

namespace GoldenCrown.API.Dtos.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<TransactionHistoryDto, TransactionHistoryResponse>();
        }
    }
}
