namespace FloraEdu.Domain.Authorization;

public static class AuthorizationPolicies
{
    public const string Authenticated = "Authenticated";
    public const string RegularUser = "RegularUser";
    public const string Admin = "Admin";
    public const string Specialist = "Specialist";
    public const string AdminOrSpecialist = "AdminOrSpecialist";
}
