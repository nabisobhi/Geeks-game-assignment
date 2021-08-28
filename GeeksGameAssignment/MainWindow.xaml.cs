using GeeksGameAssignment;
using GeeksGameAssignment.Common;
using GeeksGameAssignment.Service;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApplication1
{

    public partial class Window1 : Window, INationalityGameRenderer
    {
        protected readonly NationalityGame _nationalityGame;

        private Image _droppingImage;

        private bool isDraggingImage;
        private Point mousePosition;

        private const int StepsForFading = 50;
        private const double FadeFactor = 95 / 100.0;

        public Window1()
        {
            InitializeComponent();
            var objectFader = new ObjectFader(StepsForFading, FadeFactor);
            var pictureService = new PictureService();
            var nationalityService = new NationalityService(pictureService);
            var nationalityPhotoProvider = new NationalityPhotoProvider(nationalityService);
            _nationalityGame = new NationalityGame(this, nationalityService, nationalityPhotoProvider, objectFader,
                this.Width, this.Height - 50);
            InitializeDroppingImage();
            Start();
        }

        public void Start()
        {
            _nationalityGame.StartGame();
            RenderBoxes();
        }

        private void InitializeDroppingImage()
        {
            _droppingImage = new Image
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            paintCanvas.Children.Add(_droppingImage);
            Canvas.SetZIndex(_droppingImage, 10);
        }

        private void RenderBoxes()
        {
            foreach (var box in _nationalityGame.Boxes)
            {
                var label = new Label
                {
                    Content = box.Nationality.Name,
                    Height = box.Size.Height,
                    Width = box.Size.Width,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Background = Brushes.Orange
                };
                paintCanvas.Children.Add(label);
                Canvas.SetLeft(label, box.Location.X);
                Canvas.SetTop(label, box.Location.Y);
            }
        }

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as Image;

            if (image != null && paintCanvas.CaptureMouse())
            {
                mousePosition = e.GetPosition(paintCanvas);
                isDraggingImage = true;
            }
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDraggingImage)
            {
                paintCanvas.ReleaseMouseCapture();
                isDraggingImage = false;
            }
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingImage)
            {
                var position = e.GetPosition(paintCanvas);
                var offset = position - mousePosition;
                mousePosition = position;


                Canvas.SetLeft(_droppingImage, Canvas.GetLeft(_droppingImage) + offset.X);
                Canvas.SetTop(_droppingImage, Canvas.GetTop(_droppingImage) + offset.Y);

                if(_nationalityGame.CheckDroppingAcceptable(Canvas.GetLeft(_droppingImage), Canvas.GetTop(_droppingImage),
                    _droppingImage.Width, _droppingImage.Height))
                {
                    paintCanvas.ReleaseMouseCapture();
                    isDraggingImage = false;
                }
            }
        }

        public void DroppingPhotoChanged(string url)
        {
            _droppingImage.Source = new BitmapImage(new Uri(url));
        }

        public void DroppingPhotoMoved(double x, double y, double width, double height)
        {
            if (!isDraggingImage)
            {
                _droppingImage.Height = height;
                _droppingImage.Width = width;
                Canvas.SetLeft(_droppingImage, x);
                Canvas.SetTop(_droppingImage, y);
            }
        }

        public void ScoreChanged(int score)
        {
            scoreLabel.Content = $"Score: {score}";
        }

        public void GameOver()
        {
            MessageBox.Show("Your score is " + _nationalityGame.Score.ToString(), "Game Over", MessageBoxButton.OK, MessageBoxImage.Hand);
            playAgain.Visibility = Visibility.Visible;
        }

        public void RemaingsChanged(int remainings)
        {
            remainingLabel.Content = $"Remaining: {remainings}";
        }

        private void playAgain_Click(object sender, RoutedEventArgs e)
        {
            playAgain.Visibility = Visibility.Hidden;
            Start();
        }
    }
}

