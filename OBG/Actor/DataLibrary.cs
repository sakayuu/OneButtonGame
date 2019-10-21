using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBG.Actor
{
    static class DataLibrary
    {
        public static CharacterManager characterManagerData;

        public static void SetData(CharacterManager charasData)
        {
            characterManagerData = charasData;
        }
    }
}
