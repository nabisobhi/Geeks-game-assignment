namespace GeeksGameAssignment
{
    public interface INationalityGameRenderer
    {
        void DroppingPhotoChanged(string url);
        void DroppingPhotoMoved(double x, double y, double width, double height);
        void RemaingsChanged(int remainings);
        void ScoreChanged(int score);
        void GameOver();
    }
}