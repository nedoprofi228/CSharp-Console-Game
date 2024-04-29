
using Game.Map;
using Game.Interfaces;

namespace Game.GameObjects
{
    #region parentForGameObjects

    public class GameObject
    {
        string _simvol = "#";
        public int _x = 10;
        public int _y = 10;

        public GameObject(int x, int y, string simvol)
        {
            _y = y;
            _x = x;
            _simvol = simvol;
        }

        public virtual void Draw(Field map)
        {
            if (_x < map.sizeX && _x > 0 && _y < map.sizeY && _y > 0)
                map.map[_y, _x] = _simvol;
        }

        public virtual void Fall()
        {

        }

        public virtual bool CheckGround(Field map)
        {
            if (_y > map.sizeY)
                return true;
            return false;
        }

        public virtual void SetPos(int x, int y)
        {
            _x = x;
            _y = y;
        }

    }

    public class BigGameObject : GameObject
    {
        int sizeObjX;
        int sizeObjY;
        int width;
        bool canFall = true;
        string _simvol;
        public BigGameObject(int x, int y, string simvol, int sizeObjX, int sizeObjY, int width) : base(x, y, simvol)
        {
            this._simvol = simvol;
            this.width = width;
            this.sizeObjY = sizeObjY;
            this.sizeObjX = sizeObjX;
        }

        public override void Fall()
        {
            if (canFall)
                _y++;
        }

        public override bool CheckGround(Field map)
        {
            if (_y + sizeObjY < map.sizeY - 1)
            {
                for (int i = 0; i < sizeObjX; i++)
                {
                    if (map.map[_y + sizeObjY, _x + i] != " ")
                    {
                        canFall = false;
                        return true;
                    }
                }
            }
            canFall = true;
            return false;
        }

        public override void Draw(Field map)
        {
            for (int i = 0; i < sizeObjY; i++)
            {
                for (int j = 0; j < sizeObjX; j++)
                {
                    if (i > 0 + (width - 1) && i < sizeObjY - width && j > 0 + (width - 1) && j < sizeObjX - width)
                        continue;
                    else
                        if (_y + i > 0 && _y + i < map.sizeY && _x + j > 0 && _x + j < map.sizeX)
                        map.map[_y + i, _x + j] = _simvol;
                }
            }
        }
    }

    #endregion

    #region gameObjects
    public class BigStone : BigGameObject, IFallable
    {
        public BigStone(int x, int y, string simvol, int sizeX, int sizeY, int width) : base(x, y, simvol, sizeX, sizeY, width) { }

    }

    public class Stone : GameObject, IFallable
    {
        bool canFall = true;
        public Stone(int x, int y, string simvol) : base(x, y, simvol) { }

        public override void Fall()
        {
            if (canFall)
                _y++;
        }

        public override bool CheckGround(Field map)
        {
            if (_y == map.sizeY - 1 || map.map[_y + 1, _x] != " ")
            {
                canFall = false;
                return true;
            }
            canFall = true;
            return false;
        }
    }
    #endregion
}

