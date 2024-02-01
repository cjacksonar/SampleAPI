using AutoMapper;

namespace Profiles
{
    public class Contribution : Profile
    {
        public Contribution()
        {
            CreateMap<Data.Entities.Contribution, Data.DTOs.Contribution>();
            CreateMap<Data.DTOs.ContributionForCreation, Data.Entities.Contribution>();
            CreateMap<Data.Entities.Contribution, Data.DTOs.ContributionForCreation>();   // used this for PUT request  
        }
    }
}