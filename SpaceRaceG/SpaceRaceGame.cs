using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using SpaceRaceG.DataProvider;

namespace SpaceRaceG
{
    public class SpaceRaceGame : WpfGame
    {
        SpriteBatch _spriteBatch;
        private WpfGraphicsDeviceService _graphicsDeviceManager;

        private SpriteFont _font;
        private readonly Dictionary<Element, Texture2D> _textures;

        private DataFrame _frame;
        private int _blockWidth, _blockHeight, _boardSize;
        private Vector2 _frameNumberPosition;

        public SpaceRaceGame(SpaceRaceBot bot)
        {
            _textures = new Dictionary<Element, Texture2D>();
            Content = new ContentManager(Services) { RootDirectory = "Content" };

            bot.DataProvider.DataReceived += (sender, frame) => Dispatcher?.Invoke(() => DataProviderOnDataReceived(frame));
        }

        private void DataProviderOnDataReceived(DataFrame e)
        {
            _frame = e;

            if (_boardSize != 0) return;

            _boardSize = (int)Math.Sqrt(e.Board.Length);
            _blockWidth = _textures.First().Value.Width;
            _blockHeight = _textures.First().Value.Height;

            _frameNumberPosition = new Vector2(_blockWidth + 1, _blockHeight * 0.5f);
        }

        protected override void Initialize()
        {
            _graphicsDeviceManager = new WpfGraphicsDeviceService(this);
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _textures.Add(Element.NONE, Content.Load<Texture2D>("none"));
            _textures.Add(Element.EXPLOSION, Content.Load<Texture2D>("explosion"));
            _textures.Add(Element.WALL, Content.Load<Texture2D>("wall"));
            _textures.Add(Element.HERO, Content.Load<Texture2D>("hero"));
            _textures.Add(Element.OTHER_HERO, Content.Load<Texture2D>("other_hero"));
            _textures.Add(Element.DEAD_HERO, Content.Load<Texture2D>("dead_hero"));
            _textures.Add(Element.GOLD, Content.Load<Texture2D>("gold"));
            _textures.Add(Element.BOMB, Content.Load<Texture2D>("bomb"));
            _textures.Add(Element.STONE, Content.Load<Texture2D>("stone"));
            _textures.Add(Element.BULLET_PACK, Content.Load<Texture2D>("bullet_pack"));
            _textures.Add(Element.BULLET, Content.Load<Texture2D>("bullet"));

            _font = Content.Load<SpriteFont>("Font");

            base.Initialize();
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Color.AliceBlue);

            if (string.IsNullOrEmpty(_frame.Board)) return;

            if (double.IsNaN(Width))
            {
                Width = _blockWidth * _boardSize;
                Height = _blockHeight * _boardSize;
            }

            _spriteBatch.Begin();

            for (var i = 0; i < _frame.Board.Length; i++)
            {
                var c = _frame.Board[i];
                var e = SpaceRaceSolver.GetElement(c);

                var x = i % _boardSize;
                var y = i / _boardSize;

                var rectangle = new Rectangle(x * _blockWidth, y * _blockHeight, _blockWidth, _blockHeight);

                _spriteBatch.Draw(_textures[e], rectangle, Color.White);
            }

            _spriteBatch.DrawString(_font, _frame.FrameNumber.ToString(), _frameNumberPosition, Color.Black);

            _spriteBatch.End();
        }
    }
}
