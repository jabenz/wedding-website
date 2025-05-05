using System.ComponentModel.DataAnnotations;

namespace api.Models;

public sealed record RsvpRequest(
    // [Required] string InviteCode, 
    [Required] string Name,
    [Required][EmailAddress] string Email,
    [Required] int Extras
);