using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SweetMedical.Api.Errors;
using SweetMedical.Application.Appointments.Commands.CancelAppointment;
using SweetMedical.Application.Appointments.Commands.CreateAppointment;
using SweetMedical.Application.Appointments.Queries;
using SweetMedical.Contracts.Appointments;
using SweetMedical.Contracts.Appointments.CancelAppointment;
using SweetMedical.Contracts.Appointments.CreateAppointment;

namespace SweetMedical.Api.Controllers;

[ApiController]
[Route("appointments")]
public class AppointmentController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AppointmentController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAppointments(
        [FromQuery] Guid doctorId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAppointmentQuery(doctorId, page, pageSize));

        return result.Match(
            r => Ok(_mapper.Map<GetAppointmentsResponse>(r)),
            errors => throw new DomainException(errors));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentRequest request)
    {
        var command = new CreateAppointmentCommand(
            request.DoctorId,
            request.PatientName,
            request.ScheduledAt);

        var result = await _mediator.Send(command);

        return result.Match(
            r => CreatedAtAction(
                nameof(GetAppointments),
                new { doctorId = r.Appointment.DoctorId },
                _mapper.Map<CreateAppointmentResponse>(r.Appointment)),
            errors => throw new DomainException(errors));
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> CancelAppointment(Guid id)
    {
        var result = await _mediator.Send(new CancelAppointmentCommand(id));

        return result.Match(
            _ => Ok(new CancelAppointmentResponse(id, "Cancelled")),
            errors => throw new DomainException(errors));
    }
}
