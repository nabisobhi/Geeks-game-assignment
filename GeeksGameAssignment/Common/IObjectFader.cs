using GeeksGameAssignment.Objects;

namespace GeeksGameAssignment.Common
{
    public interface IObjectFader
    {
        void DoNextStep();
        bool NextSteps();
        void ReInitialize(DroppingObject droppingObject, double targetX, double targetY);
    }
}