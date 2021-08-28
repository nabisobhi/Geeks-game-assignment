using GeeksGameAssignment.Domain;
using System.Collections.Generic;

namespace GeeksGameAssignment.Service
{
    public interface INationalityService
    {
        List<Nationality> GetAllNationalities();
        List<NationalityPhoto> GetPhotos(Nationality nationality);
    }
}