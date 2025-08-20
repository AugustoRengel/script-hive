namespace ScriptHive.Api.Helpers;

public static class FormFileHelper
{
    public static async Task<string?> ReadFormFileAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        using var reader = new StreamReader(file.OpenReadStream());
        return await reader.ReadToEndAsync();
    }
}
