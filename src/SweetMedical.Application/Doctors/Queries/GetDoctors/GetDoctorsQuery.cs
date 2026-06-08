using ErrorOr;
using MediatR;
using SweetMedical.Application.Doctors.Common.GetDoctors;

namespace SweetMedical.Application.Doctors.Queries.GetDoctors;

public record GetDoctorsQuery() : IRequest<ErrorOr<GetDoctorResult>>;