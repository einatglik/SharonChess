using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharonChess
{
    public class Player
    {
        public const bool BLACK = false;
        public const bool WHITE = true;
        public bool color;
        private String nickname;


        public Player(bool yourcolor, String name)
        {
            color = yourcolor;
            nickname = name;
        }

        public String getName()
        {
            return nickname;
        }

         public void SetColor(bool newcolor)
        {
            color = newcolor;
        }

        public override String ToString()
        {
            return "Name: " + nickname + " Color: " + color;
        }

    }
}
