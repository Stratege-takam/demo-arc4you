using AutoMapper;
using EG.DemoPCBE99925.ManageCourseService.Facade.Dtos;

namespace EG.DemoPCBE99925.ManageCourseService.Facade;

/// <summary>
/// Class used to defined the Dto mapping with Domain object (with Facade concerns).
/// </summary>
public class MappingProfile : Profile
{
	/// <summary>
	/// Create the mapping.
	/// </summary>
	public MappingProfile()
	{
		CreateMap<Domain.Participant, ParticipantDto>().ReverseMap();
		CreateMap<Domain.Student, StudentDto>().ReverseMap();
		CreateMap<Domain.Course, CourseDto>().ReverseMap();
		CreateMap<Domain.Teacher, TeacherDto>().ReverseMap();
	}
}
