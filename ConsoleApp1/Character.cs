using Game.GameObjects;
using Game.Interfaces;
using Game.Map;

namespace Game.Characters
{
    public class Character : GameObject, IFallable
    {
        public enum MoveState { left, right, jump, idle }
        public MoveState state = MoveState.idle;
        public bool useGravity = true;
        private bool isMoving = true;
        public bool onGround;
        int sizeX;
        int sizeY;
        int jumpForce = 1;
        string[] character = {  " o " ,
                               @"/|\",
                               @"/ \" };

        public Character(int x, int y) : base(x, y, "")
        {
            sizeX = character[0].Length;
            sizeY = character.Length;
        }

        public (int x, int y) GetPos() => (_x, _y);

        public override bool CheckGround(Field map)
        {
            for (int i = 0; i < sizeX; i++)
                if (map.map[_y + sizeY, _x + i] != " ")
                {
                    onGround = true;
                    return true;
                }
            onGround = false;
            return false;
        }
        public void MoveCharacter()
        {
            switch (state)
            {
                case MoveState.left: MoveLeft(); break;
                case MoveState.right: MoveRight(); break;
                case MoveState.jump: Jump(); break;
            }
        }

        public async void Jump()
        {
            bool nextJump = true;
            if (onGround)
            {
                onGround = false;
                state = MoveState.idle;
                useGravity = false;
                for (int i = 0; i < jumpForce; i++)
                {
                    if (_y-- > 0)
                        if (nextJump)
                        {
                            nextJump = false;
                            _y--;
                            await Task.Delay(200);
                            nextJump = true;
                        }
                }
                useGravity = true;

            }

        }

        public async void MoveRight()
        {
            if (isMoving)
            {
                isMoving = false;
                _x++;
                state = MoveState.idle;
                await Task.Delay(100);
                isMoving = true;
            }
            

        }
        public async void MoveLeft()
        {
            if (isMoving)
            {
                isMoving = false;
                _x--;
                state = MoveState.idle;
                await Task.Delay(100);
                isMoving = true;
            }
        }

        public override void Fall()
        {
                _y++;
        }

        public override void Draw(Field map)
        {
            for (int i = 0; i < sizeY; i++)
                for (int j = 0; j < sizeX; j++)
                {
                    if (character[i][j] != ' ')
                        if(_x+j > 0 && _x+j < map.sizeX - 1)
                            map.map[_y + i, _x + j] = character[i][j].ToString();
                }
        }
    }
}
