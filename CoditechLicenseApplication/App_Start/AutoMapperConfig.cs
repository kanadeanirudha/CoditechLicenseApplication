using AutoMapper;

using Coditech.DataAccessLayer.DataEntity;
using Coditech.Model;
using Coditech.Utilities.Filters;
using Coditech.ViewModel;

namespace Coditech
{
    public static class AutoMapperConfig
    {
        public static void Execute()
        {
            Mapper.CreateMap<FilterTuple, FilterDataTuple>();
            Mapper.CreateMap<UserModel, UserLoginViewModel>().ReverseMap();
            Mapper.CreateMap<UserModel, UserMaster>().ReverseMap();

            Mapper.CreateMap<ApplicationLicenseDetailsModel, ApplicationLicenseDetailsViewModel>().ReverseMap();
            Mapper.CreateMap<ApplicationLicenseDetailsModel, ApplicationLicenseDetail>().ReverseMap();
        }
    }
}

