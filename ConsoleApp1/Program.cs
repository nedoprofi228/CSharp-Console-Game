using Game.Map;
using Game.GameObjects;
using Game.Characters;


namespace Game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();   
            game.StartGame();
        }

        public class Game
        {
            Random rand = new Random();
            public Field map = new();

            Character character = new Character(5, 5);
            ConsoleKeyInfo? userInput;

            
            public async void StartGame()
            {
                Thread thread = new Thread(CheckUserinput);
                thread.Start();
                Console.CursorVisible = false;
                for (int i = 0; i < 20; i++)
                {
                    map.AddGameObject(new Stone(rand.Next(10), rand.Next(0,5), "o"));
                }
                map.AddGameObject(new BigStone(rand.Next(10), rand.Next(0, 5), "#", 4, 3, 1));
                map.AddGameObject(character);

                while (true)
                {
                    map.ShowMap();
                    map.CreateMap();
                    map.DrawGameObjects();
                    map.Gravity();
                    character.MoveCharacter();
                }
            }

            public void CheckUserinput()
            {

                while (true)
                {
                    userInput = Console.ReadKey(true);
                    if (userInput != null)
                        switch (userInput?.Key)
                        {
                            case ConsoleKey.Spacebar: character.state = Character.MoveState.jump; break;
                            case ConsoleKey.A: character.state = Character.MoveState.left; break;
                            case ConsoleKey.D: character.state = Character.MoveState.right; break;
                            default: character.state = Character.MoveState.idle; break;
                        }
                }
            }
        }

        
    }
}
