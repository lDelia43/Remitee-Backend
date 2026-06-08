using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SweetMedical.Api.Errors;
using SweetMedical.Application.Doctors.Queries.GetDoctors;
using SweetMedical.Contracts.Doctors;

namespace SweetMedical.Api.Controllers;

[ApiController]
[Route("doctors")]
public class DoctorController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public DoctorController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetDoctors()
    {
        var result = await _mediator.Send(new GetDoctorsQuery());

        return result.Match(
            r => Ok(_mapper.Map<GetDoctorsResponse>(r)),
            errors => throw new DomainException(errors));
    }
}
