using System;
using A_game_about_magic.Entities;
using A_game_about_magic.Spells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Entities;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Spells;

namespace A_game_about_magic.Scenes;

public class GameScene : Scene
{
    FpsMonitor fps = new FpsMonitor();
    // Defines the player entity
    private Player _player = new Player();
    private Zombie _enemy = new Zombie();


    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    // Defines the tilemap to draw.
    private Tilemap _tilemap;

    // The SpriteFont Description used to draw text
    private SpriteFont _font;

    // Defines the position to draw the score text at.
    private Vector2 _scoreTextPosition;

    // Defines the origin used when drawing the score text.
    private Vector2 _scoreTextOrigin;

    private float _globalX = 0;
    private float _globalY = 0;

    private Matrix _translation;

    public override void Initialize()
    {

        // LoadContent is called during base.Initialize().
        base.Initialize();

        // During the game scene, we want to disable exit on escape. Instead,
        // the escape key will be used to return back to the title screen
        Core.ExitOnEscape = false;

        Globals.Enemies.Add(_enemy);


        _enemy.Speed = 600;
        _player.Speed = 600;
        Pathfinder.Init(_tilemap, _player);

        Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        Globals.RoomBounds = new Rectangle(
             (int)_tilemap.TileWidth,
             (int)_tilemap.TileHeight,
             _tilemap.Columns * (int)_tilemap.TileWidth - (int)_tilemap.TileWidth * 2,
             _tilemap.Rows * (int)_tilemap.TileHeight - (int)_tilemap.TileHeight * 2
         );



        // Initial player position will be the center tile of the tile map.
        int centerRow = _tilemap.Rows / 2;
        int centerColumn = _tilemap.Columns / 2;
        _globalX = centerColumn * _tilemap.TileWidth;
        _globalY = centerRow * _tilemap.TileHeight;
        _player.Position = new Vector2(centerColumn * _tilemap.TileWidth, centerRow * _tilemap.TileHeight);
        _enemy.Position = new Vector2(32, 32);

        // Set the position of the score text to align to the left edge of the
        // room bounds, and to vertically be at the center of the first tile.
        _scoreTextPosition = new Vector2(Globals.RoomBounds.Left, _tilemap.TileHeight * 0.5f);

        // Set the origin of the text so it is left-centered.
        float scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
        _scoreTextOrigin = new Vector2(0, scoreTextYOrigin);
    }

    public override void LoadContent()
    {

        // Create the player animated sprite from the atlas.
        _player.LoadContent();
        _enemy.LoadContent();
        Globals.Crosshair.LoadContent();

        // Create the tilemap from the XML configuration file.
        _tilemap = Tilemap.FromFile(Content, "images/Map.xml");
        _tilemap.CreateTiles();

        // // Load the bounce sound effect.
        // _bounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");

        // // Load the collect sound effect.
        // _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");

        // Load the font.
        _font = Core.Content.Load<SpriteFont>("fonts/04B_30");
    }

    public override void Update(GameTime gameTime)
    {
        // Update the player animated sprite.
        _player.Sprite.Update(gameTime);

        // Check for keyboard input and handle it.
        CheckKeyboardInput();
        CalculateTranslation();
        _player.Update(gameTime);
        _tilemap.Update(Globals.MouseInWorld);
        _enemy.Update(gameTime);
        Globals.Crosshair.Update();
        Enemy.IsPlayerTurn = Globals.IsPlayersTurn;

        CheckEnemies(gameTime);

        if (Globals.Enemies.Count != 0)
            if (Globals.IsPlayersTurn && Globals.Enemies[Globals.Enemies.Count - 1].MoveDone)
                Pathfinder.Init(_tilemap, _player);
    }

    private void CheckEnemies(GameTime gameTime)
    {
        if (Globals.Enemies.Count != 0)
            for (int i = 0; i < Globals.Enemies.Count; i++)
                Globals.Enemies[i].Update(gameTime);


        if (Globals.IsPlayersTurn)
            return;

        if (!Globals.IsPlayersTurn)
            if (Globals.Enemies.Count != 0)
                for (int i = 0; i < Globals.Enemies.Count; i++)
                    Globals.Enemies[i].MoveTowards(_player.Position, _tilemap);

        Globals.IsPlayersTurn = true;
    }

    private void CheckKeyboardInput()
    {
        // Get a reference to the keyboard inof
        KeyboardInfo keyboard = Core.Input.Keyboard;
        MouseInfo mouseInfo = Core.Input.Mouse;

        // If the escape key is pressed, return to the title screen.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Back))
        {
            Core.ChangeScene(new TitleScene());
        }

        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.F11))
        {
            Core.Graphics.ToggleFullScreen();
        }

        // If the space key is held down, the movement speed increases by 1.5
        float speed = MOVEMENT_SPEED;
        if (keyboard.IsKeyDown(Keys.Space))
        {
            speed *= 1.5f;
        }

        // If the W or Up keys are down, move the player up on the screen.
        // If the W or Up keys are down, move the slime up on the screen.
        if (keyboard.IsKeyDown(Keys.W) && keyboard.IsKeyDown(Keys.LeftAlt))
        {
            _globalY -= speed;
        }

        // if the S or Down keys are down, move the slime down on the screen.
        if (keyboard.IsKeyDown(Keys.S) && keyboard.IsKeyDown(Keys.LeftAlt))
        {
            _globalY += speed;
        }

        // If the A or Left keys are down, move the slime left on the screen.
        if (keyboard.IsKeyDown(Keys.A) && keyboard.IsKeyDown(Keys.LeftAlt))
        {
            _globalX -= speed;
        }

        // If the D or Right keys are down, move the slime right on the screen.
        if (keyboard.IsKeyDown(Keys.D) && keyboard.IsKeyDown(Keys.LeftAlt))
        {
            _globalX += speed;
        }

        if (keyboard.WasKeyJustPressed(Keys.D1))
        {
            if (Player.CastingSpells.Count < 31)
                Player.CastingSpells.Add(new BaseDamageSpell());
        }
        if (keyboard.WasKeyJustPressed(Keys.D2))
        {
            Globals.Crosshair.Sprite.Scale += new Vector2(1f, 1f);
        }

        if (mouseInfo.ScrollWheelDelta > 0 && Globals.Scale < 4)
        {
            Globals.Scale += 0.1f;
        }
        if (mouseInfo.ScrollWheelDelta < 0 && Globals.Scale > 1)
        {
            Globals.Scale -= 0.1f;
        }

    }

    private void CalculateTranslation()
    {
        MouseInfo mouseInfo = Core.Input.Mouse;
        Matrix inverseTransform = Matrix.Invert(_translation);
        Globals.MouseInWorld = Vector2.Transform(new Vector2(mouseInfo.X, mouseInfo.Y), inverseTransform);
        var dx = (Globals.WindowSize.X / 2) - _globalX;
        var dy = (Globals.WindowSize.Y / 2) - _globalY;
        _translation = Matrix.CreateScale(Globals.Scale, Globals.Scale, 1) * Matrix.CreateTranslation(dx, dy, 0f);


    }

    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(Color.Black);

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _translation);

        // Draw the tilemap
        _tilemap.DrawTiles(Core.SpriteBatch);

        // Draw the player sprite.
        _player.Sprite.Draw(Core.SpriteBatch, _player.Position);

        Globals.Crosshair.Sprite.Draw(Core.SpriteBatch, Globals.Crosshair.Position);


        if (Globals.Enemies.Count != 0)
            for (int i = 0; i < Globals.Enemies.Count; i++)
                Globals.Enemies[i].Sprite.Draw(Core.SpriteBatch, _enemy.Position);


        // fps.DrawFps( Core.SpriteBatch, _font, new Vector2(10f, 10f), Color.MonoGameOrange);


        Core.SpriteBatch.DrawString(
            _font,              // spriteFont
            $"{Globals.Crosshair.CheckBounds(_enemy)}", // text
            _scoreTextPosition, // position
            Color.White,        // color
            0.0f,               // rotation
            _scoreTextOrigin,   // origin
            1.0f,               // scale
            SpriteEffects.None, // effects
            0.0f                // layerDepth
        );


        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }

}
