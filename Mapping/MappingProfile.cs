using ApexaContractorAPI.Entities;
using ApexaContractorAPI.Models;
using AutoMapper;

namespace ApexaContractorAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contractor, ContractorModel>().ReverseMap();
            CreateMap<Contracts, ContractsModel>().ReverseMap();
        }
    }
}
