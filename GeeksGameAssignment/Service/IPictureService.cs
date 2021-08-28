using System.Collections.Generic;

namespace GeeksGameAssignment.Service
{
    public interface IPictureService
    {
        List<string> GetAllPictureUrls();
        List<string> GetPictureUrls(string partialName);
    }
}