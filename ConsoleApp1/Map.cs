using Game.GameObjects;
using Game.Interfaces;
using System.Linq;
using Game.Characters;

namespace Game.Map
{
    public class Field
    {
        List<GameObject> gameObjects = new();

        public string[,] map;
        public int sizeX = 30;
        public int sizeY = 15;

        bool isFall = false;
        public Field(int sizeX, int sizeY)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            CreateMap();
        }
        public Field()
        {
            CreateMap();
        }

        public void CreateMap()
        {
            map = new string[sizeY, sizeX];
            for (int i = 0; i < sizeY; i++)
            {
                
                    for (int j = 0; j < sizeX; j++)
                    {
                    if (i == 0 || i == sizeY - 1)
                        map[i, j] = "0";
                    else
                        map[i, j] = " ";
                    }
            }
                
                
        }

        async public Task Gravity()
        {
            if (!isFall)
            {
                _SortGameObjects();
                isFall = true;
                for (int i = gameObjects.Count-1; i >= 0; i--)
                {
                    if (gameObjects[i] is IFallable)
                        if (gameObjects[i].CheckGround(this))
                        {
                            //gameObjects[i].SetPos(rand.Next(sizeX), 0);
                            //_RemoveGameObject(i);
                        }
                        else
                        {
                            gameObjects[i].Fall();
                        }
                }
                await Task.Delay(300);
                isFall = false;
            }
        }

        public void DrawGameObjects()
        {
            foreach (var obj in gameObjects)
                obj.Draw(this);
        }

        async public void ShowMap()
        {
            
            for (int i = 0; i < sizeY; i++)
            {
                for(int j=0; j < sizeX; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }

        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        private void _RemoveGameObject(int index)
        {
            gameObjects.RemoveAt(index);
        }

        private void _SortGameObjects()
        {
            gameObjects = gameObjects.OrderBy(p => p._y).ToList();
        }

        
    }
}
