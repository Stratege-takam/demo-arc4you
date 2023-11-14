using AutoMapper;
using EG.DemoPCBE99925.ManageCourseService.Database.Logic.Help;
using EG.DemoPCBE99925.ManageCourseService.Domain;
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
		CreateMap<ListResult<Student>, ListResultStudentDto>().ReverseMap();
  	 CreateMap<Domain.Course, CourseDto>().ReverseMap();
        CreateMap<Domain.Course, GetCourseDto>()
            .ForMember(d => d.CanDelete, opt => opt.MapFrom(src => !(src.CoursePeople != null && src.CoursePeople.Any())))
            .ForMember(d => d.CanLead, opt => opt.MapFrom(src => !(src.CoursePeople != null && src.CoursePeople.Any())))
            .ForMember(d => d.CoursePeople, opt => opt.MapFrom(src => src.CoursePeople.Select(e => new GetCoursePersonDto()
            {
                CourseId = e.CourseId,
                EndDate = e.EndDate,
                LeadId = e.LeadId,
                Id = e.Id,
                StartDate = e.StartDate,
                Lead = new TeacherDto()
                {
                    FirstName = e.Lead.FirstName,
                    LastName = e.Lead.LastName,
                    Id = e.Lead.Id,
                    HireDate = e.Lead.HireDate,
                    Salary = e.Lead.Salary
                },
                Participants = e.Participants.Select(f => new GetParticipantDto()
                {
                    EndDate = f.EndDate,
                    Id = f.Id,
                    StartDate = f.StartDate,
                    StudentId = f.StudentId,
                    Student = new StudentDto()
                    {
                        Id = f.Student.Id,
                        FirstName = f.Student.FirstName,
                        LastName = f.Student.LastName,
                        Matricule = f.Student.Matricule,
                        
                    }
                }).ToList()
            }).ToList()))
            .ForMember(d => d.Owner, opt => opt.MapFrom(src => new PersonDto()
            {
                FirstName = src.Owner.FirstName,
                LastName = src.Owner.LastName,
                Id = src.Owner.Id,
            }))
            .ForMember(d => d.OwnerId, opt => opt.MapFrom(src => src.OwnerId))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Unity, opt => opt.MapFrom(src => src.Unity))
            .ForMember(d => d.IsTeacher, opt => opt.MapFrom(src => src.Owner is Domain.Teacher))
            ;
		CreateMap<Domain.Teacher, TeacherDto>().ReverseMap();
		CreateMap<Domain.CoursePerson, CoursePersonDto>().ReverseMap();
	}
}
