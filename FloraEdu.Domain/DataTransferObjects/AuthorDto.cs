namespace FloraEdu.Domain.DataTransferObjects;

public class AuthorDto : BaseDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AuthorBiography { get; set; }
}
