using System.Collections.Generic;
using A_game_about_magic.Spells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Entities;

namespace A_game_about_magic;

public class Globals
{
    public static CrosshairSpell Crosshair = new CrosshairSpell();
    public static List<Enemy> Enemies { get; set; } = new List<Enemy>();
    public static Vector2 MouseInWorld;
    public static bool IsPlayersTurn { get; set; } = true;
    public static Rectangle RoomBounds;
    public static float Scale { get; set; } = 1;
    public static Point WindowSize { get; set; }
}