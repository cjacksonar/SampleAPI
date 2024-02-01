using AutoMapper;

namespace Profiles
{
    public class Contributor : Profile
    {
        public Contributor()
        {
            CreateMap<Data.Entities.Contributor, Data.DTOs.Contributor>();
            CreateMap<Data.DTOs.ContributorForCreation, Data.Entities.Contributor>();
            CreateMap<Data.Entities.Contributor, Data.DTOs.ContributorForCreation>();   // used this for PUT request           
        }
    }
}