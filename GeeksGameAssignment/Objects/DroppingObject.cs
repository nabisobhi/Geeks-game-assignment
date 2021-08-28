using GeeksGameAssignment.Domain;

namespace GeeksGameAssignment.Objects
{
    public class DroppingObject
    {
        public NationalityPhoto Photo { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double MoveStepsPerInterval { get; set; }
    }
}
