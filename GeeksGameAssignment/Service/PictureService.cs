using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GeeksGameAssignment.Service
{
    public class PictureService : IPictureService
    {
        private string PhotosPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Resource\Photos\";

        public List<string> GetAllPictureUrls()
        {
            return Directory.GetFiles(PhotosPath, "*.jpg").ToList();
        }

        public List<string> GetPictureUrls(string partialName)
        {
            if (string.IsNullOrEmpty(partialName))
                throw new ArgumentNullException(nameof(partialName));

            return GetAllPictureUrls()
                .Select(f => new FileInfo(f))
                .Where(f => f.Name.Contains(partialName, StringComparison.InvariantCultureIgnoreCase))
                .Select(f => f.FullName)
                .ToList();
        }
    }
}
