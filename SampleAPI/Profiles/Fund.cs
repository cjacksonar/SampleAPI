using AutoMapper;

namespace Profiles
{
    public class Fund : Profile
    {
        public Fund()
        {
            CreateMap<Data.Entities.Fund, Data.DTOs.Fund>();
            CreateMap<Data.DTOs.FundForCreation, Data.Entities.Fund>();
            CreateMap<Data.Entities.Fund, Data.DTOs.FundForCreation>();   // used this for PUT request
        }
    }
}