using System;
using System.Collections.Generic;
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

        private string _board;
        private readonly Dictionary<Element, Texture2D> _textures;

        private int w, h, size;

        public SpaceRaceGame(SpaceRaceBot bot)
        {
            _textures = new Dictionary<Element, Texture2D>();
            Content = new ContentManager(Services) { RootDirectory = "Content" };

            bot.DataProvider.DataReceived += (sender, frame) => Dispatcher?.Invoke(() => DataProviderOnDataReceived(frame));
        }

        private void DataProviderOnDataReceived(DataFrame e)
        {
            _board = e.Board;

            if (size == 0)
            {
                size = (int)Math.Sqrt(_board.Length);
                w = _textures[Element.NONE].Width;
                h = _textures[Element.NONE].Width;
            }
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

            base.Initialize();
        }

        protected override void Update(GameTime time)
        {

        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Color.AliceBlue);

            if (string.IsNullOrEmpty(_board)) return;

            if (double.IsNaN(Width))
            {
                Width = w * size;
                Height = h * size;
            }

            _spriteBatch.Begin();

            for (var i = 0; i < _board.Length; i++)
            {
                var c = _board[i];
                var e = SpaceRaceSolver.GetElement(c);

                var x = i % size;
                var y = i / size;

                var rectangle = new Rectangle(x * w, y * h, w, h);

                _spriteBatch.Draw(_textures[e], rectangle, Color.White);
            }

            _spriteBatch.End();
        }
    }
}
