namespace Assets.Scripts.Snakes
{
    public class AISnake : Snake
    {
        public override void HandleMovement()
        {
            base.HandleMovement();
            //Do some crazy AI moves here to control the snake 
            
            //Set the nextDirection field based on the AI move 
            
            //Dont have to call the MoveUp/Down/Left/Right methods, can just use the inner AI will.
        }
    }
}