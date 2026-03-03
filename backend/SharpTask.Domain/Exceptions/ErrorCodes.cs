namespace App.Entities.Exceptions;

public static class ErrorCodes
{
    // Errores relacionados con las mascostas
    public const string PetNotFound = "PET_NOT_FOUND";

    // Errores relacionados con el perfil
    public const string ProfileNotFound = "PROFILE_NOT_FOUND";

    // Errores generales
    public const string InternalError = "INTERNAL_SERVER_ERROR";
    public const string ValidationError = "VALIDATION_ERROR";
}
