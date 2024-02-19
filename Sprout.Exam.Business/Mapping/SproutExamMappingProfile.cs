using AutoMapper;
using Sprout.Exam.Business.Models;
using Sprout.Exam.Common;
using Sprout.Exam.DataAccess.Entities;

namespace Sprout.Exam.Business.Mapping
{
    public class SproutExamMappingProfile : Profile
    {
        public SproutExamMappingProfile()
        {
            CreateMap<EmployeeEntity, EmployeeDto>()
                .ForMember(dest => dest.Birthdate, opt => opt.MapFrom(src => src.Birthdate.ToString(Constants.BIRTHDATE_FORMAT)))
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => (int)src.EmployeeTypeId));

            CreateMap<EmployeeRequest, EmployeeEntity>()
                .ForMember(dest => dest.EmployeeTypeId, opt => opt.MapFrom(src => src.TypeId));
        }
    }
}
