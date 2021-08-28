using GeeksGameAssignment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeeksGameAssignment.Service
{
    public class NationalityService : INationalityService
    {
        protected readonly IPictureService _pictureService;
        protected readonly IList<Nationality> _nationalities;

        public NationalityService(IPictureService pictureService)
        {
            _pictureService = pictureService;
            _nationalities = new List<Nationality>
            {
                new Nationality{Id = 1, Name = "Japanese"},
                new Nationality{Id = 2, Name = "Chinese"},
                new Nationality{Id = 3, Name = "Korean"},
                new Nationality{Id = 4, Name = "Thai"},
            };
        }

        public List<Nationality> GetAllNationalities()
        {
            return _nationalities.ToList();
        }

        public List<NationalityPhoto> GetPhotos(Nationality nationality)
        {
            if (nationality is null)
                throw new ArgumentNullException(nameof(nationality));

            return _pictureService.GetPictureUrls(nationality.Name)
                .Select(p => new NationalityPhoto
                {
                    NationalityId = nationality.Id,
                    PictureUrl = p
                })
                .ToList();
        }
    }
}