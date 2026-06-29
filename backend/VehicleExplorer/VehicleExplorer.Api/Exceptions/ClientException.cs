namespace VehicleExplorer.Api.Exceptions;


public sealed class ClientException(string message, Exception innerException) : Exception(message, innerException)
{
    public string? RequestUri { get; init; }
}
