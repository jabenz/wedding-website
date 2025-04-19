using api.Entities;
using api.Models;

namespace api.Extensions;

public static class RsvpEntityExtenstions
{
    public static Registration ToRegistration(this RsvpEntity form)
        => new()
        {
            Name = form.Name,
            Email = form.Email,
            Extras = form.Extras,
            CreatedAt = form.Timestamp?.DateTime ?? DateTime.UtcNow,
        };
}