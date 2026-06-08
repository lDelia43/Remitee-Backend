using ErrorOr;

namespace SweetMedical.Api.Errors;

public class DomainException : Exception
{
    public List<Error> Errors { get; }

    public DomainException(List<Error> errors) : base(errors[0].Description)
    {
        Errors = errors;
    }
}
