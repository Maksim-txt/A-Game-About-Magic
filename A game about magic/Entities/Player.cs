using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Spells;

namespace A_game_about_magic.Entities;

public class Player : Entity
{
    private Dictionary<string, AnimatedSprite> Sprites;

    // X Y coordinate where player need to go
    private static Vector2 _destination;
    public bool Moving { get; private set; } = false;

    /// <summary>
    /// Array with length 30 of the spells that are cast on this enemy
    /// </summary>
    public BaseDamageSpell[] Spells { get; set; }

    /// <summary>
    /// Array with length 30 of the player spells
    /// </summary>
    public BaseDamageSpell[] PlayerSpells { get; set; }

    /// <summary>
    /// List with max length 30 of the spells that will be cast on the enemy
    /// </summary>
    public static List<BaseDamageSpell> CastingSpells { get; set; } = new List<BaseDamageSpell>();

    public Player() : base()
    {
        Sprites = new Dictionary<string, AnimatedSprite>();
        Spells = new BaseDamageSpell[30];
    }

    public Player(AnimatedSprite sprite, Vector2 position, float speed) : base(sprite, position, speed)
    {
        Sprites = new Dictionary<string, AnimatedSprite>();
        Spells = new BaseDamageSpell[30];
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        CreateBounds();
        CheckKeyboardInput();
    }

    public void LoadContent()
    {
        try
        {
            // Create the texture atlas from the XML configuration file.
            TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");
            Sprites.Add("player-animation", atlas.CreateAnimatedSprite("player-animation"));
            Sprites.Add("player-walk-animation", atlas.CreateAnimatedSprite("player-walk-animation"));
            Sprite = Sprites["player-animation"];
            Sprite.Scale = new Vector2(1, 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }


    private void CheckKeyboardInput()
    {
        // Get a reference to the keyboard inof
        KeyboardInfo keyboard = Core.Input.Keyboard;
        MouseInfo mouse = Core.Input.Mouse;

        if (Moving == false)
            Sprite = Sprites["player-animation"];

        // If the W or Up keys are down, move the slime up on the screen.
        if (keyboard.IsKeyDown(Keys.Enter) && this.MoveDone)
        {
            Globals.IsPlayersTurn = false;
        }


        // Use distance based checks to determine if the player is within the
        // bounds of the game screen, and if it is outside that screen edge,
        // move it back inside.
        if (Bounds.Left < Globals.RoomBounds.Left)
        {
            Position = new Vector2(Globals.RoomBounds.Left, Position.Y);
        }
        else if (Bounds.Right > Globals.RoomBounds.Right)
        {
            Position = new Vector2(Globals.RoomBounds.Right - Sprite.Width, Position.Y);
        }

        if (Bounds.Top < Globals.RoomBounds.Top)
        {
            Position = new Vector2(Position.X, Globals.RoomBounds.Top);
        }
        else if (Bounds.Bottom > Globals.RoomBounds.Bottom)
        {
            Position = new Vector2(Position.X, Globals.RoomBounds.Bottom - Sprite.Height);
        }
    }

}
