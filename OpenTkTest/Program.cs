using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.IO;

namespace OpenTkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (NeoKabutoTutorial2 game = new NeoKabutoTutorial2())
                game.Run(30, 30);
        }
    }    
}
