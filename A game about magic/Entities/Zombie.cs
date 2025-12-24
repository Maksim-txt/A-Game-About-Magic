using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

namespace A_game_about_magic.Entities;

public class Zombie : Enemy
{
    private Dictionary<string, AnimatedSprite> Sprites;

    public Zombie() : base()
    {
        Sprites = new Dictionary<string, AnimatedSprite>();
    }

    public Zombie(AnimatedSprite sprite, Vector2 position, float speed) : base(sprite, position, speed)
    {
        Sprites = new Dictionary<string, AnimatedSprite>();
    }

    public void LoadContent()
    {
        try
        {
            // Create the texture atlas from the XML configuration file.
            TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");
            Sprites.Add("enemy-animation", atlas.CreateAnimatedSprite("enemy-animation"));
            Sprite = Sprites["enemy-animation"];
            Sprite.Scale = new Vector2(1, 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        CreateBounds();
        Checks();
    }

    private void Checks()
    {
        MouseInfo mouse = Core.Input.Mouse;

        if (Globals.Crosshair.Bounds.Intersects(Bounds))
            Sprite.Color = Color.Black;
        else
            Sprite.Color = Color.White;
            
        if (Globals.Enemies.Contains(this) && mouse.WasButtonJustPressed(MouseButton.Left) && Globals.Crosshair.Bounds.Intersects(Bounds))
        {
            Spells = Player.CastingSpells;
            Player.CastingSpells = new List<MonoGameLibrary.Spells.BaseDamageSpell>();
        }

        if (HitPoints <= 0)
            Globals.Enemies.Remove(this);
    }
}
