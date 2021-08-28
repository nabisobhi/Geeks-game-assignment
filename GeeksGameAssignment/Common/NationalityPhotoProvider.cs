using GeeksGameAssignment.Domain;
using GeeksGameAssignment.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GeeksGameAssignment.Common
{
    public class NationalityPhotoProvider : INationalityPhotoProvider
    {
        private INationalityService _nationalityService;

        private List<NationalityPhoto> _photos;
        private int _currentPhoto = 0;

        private readonly Random _random = new();

        public NationalityPhotoProvider(INationalityService nationalityService)
        {
            _nationalityService = nationalityService;
        }

        public void LoadData(IList<Nationality> nationalities)
        {
            _photos = nationalities
                .Select(_nationalityService.GetPhotos)
                .SelectMany(ps => ps)
                .OrderBy(p => _random.NextDouble())
                .ToList();

            Reset();
        }

        public NationalityPhoto Current => _photos[_currentPhoto];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _photos.Clear();
        }

        public bool MoveNext()
        {
            _currentPhoto++;
            return _currentPhoto < _photos.Count;
        }

        public int Reminings => _photos.Count - _currentPhoto - 1;

        public void Reset()
        {
            _currentPhoto = 0;
        }
    }
}
