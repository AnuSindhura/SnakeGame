using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    class Input
    {
        //load list of available keyboard uttons
        private static Hashtable keyTable = new Hashtable();

        public static bool KeyPress(Keys key)
        {
            if(keyTable[key]== null)
            {
                //if hashtable is empty then return false
                return false;
            }
            // if hashtable is not empty then return true
            return (bool)keyTable[key];
        }

        public static void changeState(Keys key , bool state)
        {
            keyTable[key] = state;
        }
    }
}
