using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;


using A_game_about_magic.Scenes;

namespace A_game_about_magic;

public class Game1 : Core
{

    private static int width = 800;
    private static int height = 600;

    public Game1() : base("A game about magic", 800, 600, false)
    {
        Globals.WindowSize = new Point(width, height);
    }


    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        ChangeScene(new GameScene());
    }
}
