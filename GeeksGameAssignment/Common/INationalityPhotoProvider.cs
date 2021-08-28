using GeeksGameAssignment.Domain;
using System.Collections.Generic;

namespace GeeksGameAssignment.Common
{
    public interface INationalityPhotoProvider : IEnumerator<NationalityPhoto>
    {
        void LoadData(IList<Nationality> nationalities);
        int Reminings { get; }
    }
}