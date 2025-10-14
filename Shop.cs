using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_RPG_11
{
    internal class Shop
    {
        private GameManager gameManager;

        public Shop(GameManager manager)
        {
            gameManager = manager;
        }

        public void ShowShopDisplay()
        {
            Console.Clear();
        }
    }
}
