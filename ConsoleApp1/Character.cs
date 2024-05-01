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
        private bool cdMove = true;
        public bool isMoving;
        public bool onGround;

        bool isRightLeg = true;
        bool isLeftLeg = false;

        int sizeX;
        int sizeY;
        int jumpForce = 1;

        public string leftHand = "/";
        public string rightHand = @"\";
        public string leftLeg = "/";
        public string rightLeg = @"\";
        string[] character;

        public Character(int x, int y) : base(x, y, "")
        {
            sizeX = 3;
            sizeY = 3;

            character = new string[]
                {   " o " ,              // " o "
         $@"{leftHand}|{rightHand}",     // "/|\"
          $@"{leftLeg} {rightLeg}" };    // "/ \"
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
        public void MoveCharacter(Field map)
        {

            if (state != MoveState.idle)
                isMoving = true;
            else
                isMoving = false;
            switch (state)
            {
                case MoveState.left: MoveLeft(map); break;
                case MoveState.right: MoveRight(map); break;
                case MoveState.jump: Jump(map); break;
            }
        }

        public async void Jump(Field map)
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
                WalkAnimation();
            }
        }

        public async void MoveRight(Field map)
        {
            if (cdMove)
            {
                cdMove = false;
                _x++;
                WalkAnimation();
                await Task.Delay(130);
                cdMove = true;
            }
            

        }
        public async void MoveLeft(Field map)
        {
            if (cdMove)
            {
                cdMove = false;
                _x--;
                WalkAnimation();
                await Task.Delay(130);
                cdMove = true;
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
                        {
                            map.map[_y + i, _x + j] = character[i][j].ToString();
                        }
                }
        }

        private async void WalkAnimation()
        {
            string inRight = ">";
            string inLeft = "<";

            if (isMoving && onGround)
                switch (state)
                {
                    case MoveState.left:
                        if (isRightLeg)
                        {
                            leftLeg = "/";
                            rightLeg = inLeft;
                            isLeftLeg = true;
                            isRightLeg = false;
                        }
                        else
                        {
                            rightLeg = "\\";
                            leftLeg = inLeft;
                            isLeftLeg = false;
                            isRightLeg = true;
                        }
                        break;
                    case MoveState.right:
                        if (isRightLeg)
                        {
                            leftLeg = "/";
                            rightLeg = inRight;
                            isLeftLeg = true;
                            isRightLeg = false;
                        }
                        else
                        {
                            rightLeg = "\\";
                            leftLeg = inRight;
                            isLeftLeg = false;
                            isRightLeg = true;
                        }
                        break;
                }
            else
            {
                leftLeg = "/";
                rightLeg = "\\";
            }
            UpdateCharacter();
        }

        void UpdateCharacter()
        {
            character = new string[]
                {   " o " ,              // " o "
         $@"{leftHand}|{rightHand}",     // "/|\"
          $@"{leftLeg} {rightLeg}" };    // "/ \"
        }

        void CheckCollision()
        {

        }
    }
}
