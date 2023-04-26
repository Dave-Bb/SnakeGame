namespace Assets.Scripts.Snakes
{
    public interface ISnakeController
    {
        void HandleMovement();
        bool MoveUp();
        bool MoveDown();
        bool MoveLeft();
        bool MoveRight();
    }
}