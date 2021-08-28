using GeeksGameAssignment.Objects;

namespace GeeksGameAssignment.Common
{
    public class ObjectFader : IObjectFader
    {
        private readonly int _stepsForFading;
        private readonly double _fadeFactor;
        private int _fadeSteps = 0;
        private double _xStep = 0;
        private double _yStep = 0;

        private DroppingObject _droppingObject;
        public ObjectFader(int stepsForFading, double fadeFactor)
        {
            _stepsForFading = stepsForFading;
            _fadeFactor = fadeFactor;
        }

        public void ReInitialize(DroppingObject droppingObject, double targetX, double targetY)
        {
            _droppingObject = droppingObject;
            _fadeSteps = 0;
            _xStep = (targetX - _droppingObject.X) / _stepsForFading;
            _yStep = (targetY - _droppingObject.Y) / _stepsForFading;
        }

        public void DoNextStep()
        {
            _droppingObject.X += _xStep;
            _droppingObject.Y += _yStep;
            _droppingObject.Width *= _fadeFactor;
            _droppingObject.Height *= _fadeFactor;

            _fadeSteps++;
        }

        public bool NextSteps()
        {
            return _fadeSteps <= _stepsForFading;
        }
    }
}
