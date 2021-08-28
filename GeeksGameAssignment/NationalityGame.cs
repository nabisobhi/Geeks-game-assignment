using GeeksGameAssignment.Common;
using GeeksGameAssignment.Domain;
using GeeksGameAssignment.Objects;
using GeeksGameAssignment.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace GeeksGameAssignment
{
    public class NationalityGame
    {
        private const long ClockIntervalInMilliseconds = 10;
        private const long TotalTimeOfDroppingObjectInMilliseconds = 3000;
        private const double AcceptableMargin = 20;
        private Size DefaultBoxSize = new(100, 50);
        private Size DefaultDroppingPictureSize = new(100, 100);
        private const int CorectScore = 20;
        private const int WrongScore = -5;

        private readonly DispatcherTimer _timer;
        protected readonly INationalityGameRenderer _nationalityGameRenderer;
        private double _boardWidth;
        private double _boardHeight;

        private bool _fadeMode = false;

        private readonly IObjectFader _objectFader; 
        private INationalityService _nationalityService;
        private INationalityPhotoProvider _nationalityPhotoProvider;

        public IList<Box> Boxes { get; private set; }
        public int Score { get; private set; }
        private DroppingObject _droppingObject;

        private readonly Random _random = new();

        public NationalityGame(INationalityGameRenderer nationalityGameRenderer, INationalityService nationalityService,
            INationalityPhotoProvider nationalityPhotoProvider, IObjectFader objectFader, double width, double height)
        {
            _nationalityGameRenderer = nationalityGameRenderer;
            _boardWidth = width;
            _boardHeight = height;

            _nationalityService = nationalityService;
            _nationalityPhotoProvider = nationalityPhotoProvider;

            InitializeDroppingImage();

            _objectFader = objectFader;

            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(timer_Tick);
        }

        public void StartGame()
        {
            var nationalities = _nationalityService.GetAllNationalities()
                .OrderBy(n => _random.NextDouble())
                .Take(4)
                .ToList();

            if (nationalities.Count < 4)
                throw new ArgumentOutOfRangeException(nameof(nationalities));

            _nationalityPhotoProvider.LoadData(nationalities);

            InitializeBoxes(nationalities);

            Score = 0;
            _nationalityGameRenderer?.ScoreChanged(Score);

            _timer.Interval = new TimeSpan(ClockIntervalInMilliseconds * TimeSpan.TicksPerMillisecond);
            _timer.Start();

            DropNextImage();
        }

        private void InitializeDroppingImage()
        {
            _droppingObject = new DroppingObject
            {                
                MoveStepsPerInterval = _boardHeight * ClockIntervalInMilliseconds * 1.0 / TotalTimeOfDroppingObjectInMilliseconds,
            };
        }

        private void DropNextImage()
        {
            if (!_nationalityPhotoProvider.MoveNext())
            {
                _nationalityGameRenderer?.GameOver();
                _timer.Stop();
                return;
            }
            _nationalityGameRenderer?.RemaingsChanged(_nationalityPhotoProvider.Reminings);
            _droppingObject.Photo = _nationalityPhotoProvider.Current;
            _droppingObject.Y = 0;
            _droppingObject.X = (_boardWidth - DefaultDroppingPictureSize.Width) / 2;
            _droppingObject.Width = DefaultDroppingPictureSize.Width;
            _droppingObject.Height = DefaultDroppingPictureSize.Height;

            _nationalityGameRenderer?.DroppingPhotoMoved(_droppingObject.X, _droppingObject.Y,
                _droppingObject.Width, _droppingObject.Height);
            _nationalityGameRenderer?.DroppingPhotoChanged(_droppingObject.Photo.PictureUrl);
        }

        private void InitializeBoxes(List<Nationality> nationalities)
        {
            if (nationalities.Count() < 4)
                throw new ArgumentOutOfRangeException(nameof(nationalities));

            Boxes = new List<Box>
            {
                new Box
                {
                    Size = DefaultBoxSize,
                    Location = new Point(0, 0),
                    Nationality = nationalities[0],
                },
                new Box
                {
                    Size = DefaultBoxSize,
                    Location = new Point(0, _boardHeight - DefaultBoxSize.Height),
                    Nationality = nationalities[1],
                },
                new Box
                {
                    Size = DefaultBoxSize,
                    Location = new Point(_boardWidth - DefaultBoxSize.Width, 0),
                    Nationality = nationalities[2],
                },
                new Box
                {
                    Size = DefaultBoxSize,
                    Location = new Point(_boardWidth - DefaultBoxSize.Width, _boardHeight - DefaultBoxSize.Height),
                    Nationality = nationalities[3],
                }
            };
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_fadeMode)
            {
                _objectFader.DoNextStep();

               _nationalityGameRenderer?.DroppingPhotoMoved(_droppingObject.X, _droppingObject.Y, _droppingObject.Width,
                    _droppingObject.Height);

                if (!_objectFader.NextSteps())
                {
                    _fadeMode = false;
                    DropNextImage();
                }
            }
            else
            {
                _droppingObject.Y += _droppingObject.MoveStepsPerInterval;
                _nationalityGameRenderer?.DroppingPhotoMoved(_droppingObject.X, _droppingObject.Y, _droppingObject.Width,
                    _droppingObject.Height);

                if (_droppingObject.Y > _boardHeight)
                {
                    DropNextImage();
                }
            }
        }

        public bool CheckDroppingAcceptable(double x, double y, double width, double height)
        {
            if (_fadeMode == true)
                return _fadeMode;

            foreach (var box in Boxes)
            {
                if (IsAnyPointInBox(x, y, width, height, box))
                {
                    _fadeMode = true;

                    Score += _droppingObject.Photo.NationalityId == box.Nationality.Id ? CorectScore : WrongScore;
                    _nationalityGameRenderer?.ScoreChanged(Score);

                    _droppingObject.X = x;
                    _droppingObject.Y = y;

                    _objectFader.ReInitialize(_droppingObject, box.Location.X + box.Size.Width / 2.0,
                        box.Location.Y + box.Size.Height / 2.0);

                    break;
                }
            }
            return _fadeMode;
        }

        private static bool IsAnyPointInBox(double x, double y, double width, double height, Box box)
        {
            return x + width + AcceptableMargin > box.Location.X && x < box.Location.X + box.Size.Width + AcceptableMargin
                && y + height + AcceptableMargin > box.Location.Y && y < box.Location.Y + box.Size.Height + AcceptableMargin;
        }

    }
}
