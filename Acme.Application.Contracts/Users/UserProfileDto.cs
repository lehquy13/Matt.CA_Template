using Matt.SharedKernel.Application.Contracts.Primitives;

namespace Acme.Application.Contracts.Users;

public class UserProfileDto : AuditedEntityDto<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int BirthYear { get; set; } = 1960;
    public string DetailAddress { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = "Learner"; // This currently is not mapped

    public string Avatar { get; set; } =
        "https://res.cloudinary.com/dhehywasc/image/upload/v1686121404/default_avatar2_ws3vc5.png";
}