namespace ProyectoEdificios.Services.Projects
{
    public interface IChangeControlWorkbookSource
    {
        Task<Stream?> OpenReadAsync(CancellationToken cancellationToken = default);
    }
}
