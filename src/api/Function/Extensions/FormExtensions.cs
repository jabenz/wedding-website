using api.Helper;
using api.Models;
using Microsoft.AspNetCore.Http;

namespace api.Extensions;

public static class FormExtensions
{
    public static RsvpRequest GetRsvpRequest(this IFormCollection form)
    {
        if (!form.TryGetValue(FormKeys.Name, out var nameValue))
            throw new ArgumentNullException(FormKeys.Name);

        if (!form.TryGetValue(FormKeys.Email, out var emailValue))
            throw new ArgumentNullException(FormKeys.Email);

        if (!form.TryGetValue(FormKeys.Extras, out var extrasValue))
            throw new ArgumentNullException(FormKeys.Extras);

        return new RsvpRequest(nameValue!, emailValue!, int.Parse(extrasValue!));
    }

}