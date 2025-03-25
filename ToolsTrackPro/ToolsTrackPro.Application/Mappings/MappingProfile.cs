using AutoMapper;
using ToolsTrackPro.Application.DTOs;
using ToolsTrackPro.Domain.Entities;

namespace ToolsTrackPro.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Tool, ToolDto>().ReverseMap();
            CreateMap<TransactionView, TransactionViewDto>().ReverseMap();
            CreateMap<ToolTransaction, ToolTransactionDto>().ReverseMap();
        }
    }
}
