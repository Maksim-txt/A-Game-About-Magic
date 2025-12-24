using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;

namespace A_game_about_magic.Spells;

public class CrosshairSpell
{
    public int Damage { get; set; } = 30;

    public Vector2 Position;

    public Sprite Sprite { get; set; }


    private Circle _spellBounds;

    public Circle Bounds
    {
        get { return _spellBounds; }
    }

    public CrosshairSpell() { }


    public CrosshairSpell(Sprite sprite, Vector2 position)
    {
        Sprite = sprite;
        Position = position;
    }

    public void CreateBounds()
    {
        _spellBounds = new Circle(
            (int)(Position.X + (Sprite.Width * 0.5f)),
            (int)(Position.Y + (Sprite.Height * 0.5f)),
            (int)(Sprite.Width * 0.5f)
        );
    }

    public void Update()
    {
        Position = new Vector2(Globals.MouseInWorld.X - (int)(Sprite.Width * 0.5), Globals.MouseInWorld.Y - (int)(Sprite.Height * 0.5));
        CreateBounds();
    }

    public void LoadContent()
    {
        try
        {
            // Create the texture atlas from the XML configuration file.
            Texture2D texture = Core.Content.Load<Texture2D>("images/CrosshairSpell");
            Sprite = new Sprite(new TextureRegion(texture, 0, 0, 32, 32));
            Sprite.Scale = new Vector2(1, 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public bool CheckBounds(Enemy enemy)
    {
        if (Globals.Crosshair.Bounds.Intersects(enemy.Bounds))
            return true;

        return false;
    }
}