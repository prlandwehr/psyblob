using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soopah.Xna.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PgameNS
{
    public static class PCgamepad
    {
        public struct MyGamePadState
        {
            public MyGamePadButtons Buttons;
            public MyGamePadDPad DPad;
            public MyGamePadThumbSticks Thumbsticks;
        }

        public struct MyGamePadButtons
        {
            public ButtonState Triangle;
            public ButtonState Circle;
            public ButtonState Cross;
            public ButtonState Square;
            public ButtonState R2;
            public ButtonState L2;
            public ButtonState R1;
            public ButtonState L1;
            public ButtonState Select;
            public ButtonState Start;
            public ButtonState R3;
            public ButtonState L3;
        }

        public struct MyGamePadDPad
        {
            public ButtonState Left;
            public ButtonState Right;
            public ButtonState Up;
            public ButtonState Down;
        }

        public struct MyGamePadThumbSticks
        {
            public Vector2 Left;
            public Vector2 Right;
        }


        public static MyGamePadState GetState(PlayerIndex player)
        {
            int playerIndex = System.Convert.ToInt16(player);

            MyGamePadState myGamePadState = new MyGamePadState();

            myGamePadState.Buttons.Triangle = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[0];
            myGamePadState.Buttons.Circle = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[1];
            myGamePadState.Buttons.Cross = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[2];
            myGamePadState.Buttons.Square = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[3];
            myGamePadState.Buttons.R2 = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[4];
            myGamePadState.Buttons.L2 = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[5];
            myGamePadState.Buttons.R1 = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[6];
            myGamePadState.Buttons.L1 = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[7];
            myGamePadState.Buttons.Select = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[8];
            myGamePadState.Buttons.Start = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[9];
            myGamePadState.Buttons.R3 = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[10];
            myGamePadState.Buttons.L3 = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].Buttons.List[11];

            myGamePadState.DPad.Down = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].DPad.Down;
            myGamePadState.DPad.Up = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].DPad.Up;
            myGamePadState.DPad.Left = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].DPad.Left;
            myGamePadState.DPad.Right = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].DPad.Right;

            myGamePadState.Thumbsticks.Left = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].ThumbSticks.Left;
            myGamePadState.Thumbsticks.Right = Soopah.Xna.Input.DirectInputGamepad.Gamepads[playerIndex].ThumbSticks.Right;

            return myGamePadState;
        }
    }

}
