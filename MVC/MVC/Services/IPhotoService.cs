using CloudinaryDotNet.Actions;

namespace MVC.Services 
{
    public interface IPhotoService 
    {
        Task<ImageUploadResult> AddPhotsAsync(IFormFile file);
        Task<DeletionResult> DeletePhotAsync(string publicId);
    }
}
