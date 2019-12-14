using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using SpaceRaceG.AI;
using SpaceRaceG.DataProvider;

namespace SpaceRaceG
{
    public class SpaceRaceGame : WpfGame
    {
        SpriteBatch _spriteBatch;
        private WpfGraphicsDeviceService _graphicsDeviceManager;

        private SpriteFont _font;
        private readonly Dictionary<Element, Texture2D> _textures;

        private Board _board;
        private int _blockWidth, _blockHeight;
        private Vector2 _frameNumberPosition;

        public SpaceRaceGame(SpaceRaceBot bot)
        {
            _textures = new Dictionary<Element, Texture2D>();
            Content = new ContentManager(Services) { RootDirectory = "Content" };

            //TODO: lock!?
            bot.BoardLoaded += (sender, board) => _board = board;
        }

        protected override void Initialize()
        {
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //bomb.png
            //bullet.png
            //bullet_pack.png
            //dead_hero.png
            //explosion.png
            //gold.png
            //hero.png
            //hero_down.png
            //hero_down_left.png
            //hero_down_right.png
            //hero_left.png
            //hero_right.png
            //hero_up.png
            //hero_up_left.png
            //hero_up_right.png
            //none.png
            //other_hero.png
            //stone.png
            //wall.png

            _textures.Add(Element.NONE, Content.Load<Texture2D>("none"));
            _textures.Add(Element.EXPLOSION, Content.Load<Texture2D>("explosion"));
            _textures.Add(Element.WALL, Content.Load<Texture2D>("wall"));
            _textures.Add(Element.HERO, Content.Load<Texture2D>("hero"));

            _textures.Add(Element.HERO_down, Content.Load<Texture2D>("hero_down"));
            _textures.Add(Element.HERO_down_left, Content.Load<Texture2D>("hero_down_left"));
            _textures.Add(Element.HERO_down_right, Content.Load<Texture2D>("hero_down_right"));
            _textures.Add(Element.HERO_left, Content.Load<Texture2D>("hero_left"));
            _textures.Add(Element.HERO_right, Content.Load<Texture2D>("hero_right"));
            _textures.Add(Element.HERO_up, Content.Load<Texture2D>("hero_up"));
            _textures.Add(Element.HERO_up_left, Content.Load<Texture2D>("hero_up_left"));
            _textures.Add(Element.HERO_up_right, Content.Load<Texture2D>("hero_up_right"));

            _textures.Add(Element.OTHER_HERO, Content.Load<Texture2D>("other_hero"));
            _textures.Add(Element.DEAD_HERO, Content.Load<Texture2D>("dead_hero"));
            _textures.Add(Element.GOLD, Content.Load<Texture2D>("gold"));
            _textures.Add(Element.BOMB, Content.Load<Texture2D>("bomb"));
            _textures.Add(Element.STONE, Content.Load<Texture2D>("stone"));
            _textures.Add(Element.BULLET_PACK, Content.Load<Texture2D>("bullet_pack"));
            _textures.Add(Element.BULLET, Content.Load<Texture2D>("bullet"));

            _font = Content.Load<SpriteFont>("Font");

            _blockWidth = _textures.First().Value.Width;
            _blockHeight = _textures.First().Value.Height;

            _frameNumberPosition = new Vector2(_blockWidth + 1, _blockHeight * 0.5f);

            base.Initialize();
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Color.AliceBlue);

            if (_board == null) return;

            if (double.IsNaN(Width))
            {
                Width = _blockWidth * _board.Size.Width;
                Height = _blockHeight * _board.Size.Height;
            }

            _spriteBatch.Begin();

            foreach (var cell in _board)
            {
                _spriteBatch.Draw(_textures[cell.Element],
                    new Rectangle(cell.P.X * _blockWidth, cell.P.Y * _blockHeight, _blockWidth, _blockHeight),
                    Color.White);
            }

            _spriteBatch.DrawString(_font, _board.Frame.FrameNumber.ToString(), _frameNumberPosition, Color.Black);

            if (_board.AttentionPoints != null)
                foreach (var attentionPoint in _board.AttentionPoints)
                    _spriteBatch.DrawString(_font, "A", new Vector2(attentionPoint.X * _blockWidth, attentionPoint.Y * _blockHeight), Color.Red);

            if (_board.PathPoints != null)
                foreach (var attentionPoint in _board.PathPoints)
                    _spriteBatch.DrawString(_font, "P", new Vector2(attentionPoint.X * _blockWidth + 3, attentionPoint.Y * _blockHeight), Color.Green);

            if (!string.IsNullOrEmpty(_board.Responce))
                _spriteBatch.DrawString(_font, _board.Responce, new Vector2(2 * _blockWidth + 1, _blockHeight * 0.5f), Color.Blue);

            // if (_board.Map != null)
            //     foreach (var mapEntry in _board.Map)
            //         _spriteBatch.DrawString(_font, mapEntry.Weight.ToString(),
            //             new Vector2(mapEntry.Position.X * _blockWidth + 6, mapEntry.Position.Y * _blockHeight),
            //             Color.Yellow);

            _spriteBatch.End();
        }
    }
}
