using AutoMapper;

namespace Profiles
{
    public class RegisteredUser : Profile
    {
        public RegisteredUser()
        {
            CreateMap<Data.Entities.RegisteredUser, Data.DTOs.RegisteredUser>();
            CreateMap<Data.DTOs.RegisteredUserForCreation, Data.Entities.RegisteredUser>();
            CreateMap<Data.Entities.RegisteredUser, Data.DTOs.RegisteredUserForCreation>(); // used this for PUT request
            CreateMap<Data.DTOs.RegisteredUserForUpdate, Data.Entities.RegisteredUser>();   // user this for PATCH request   
        }
    }
}