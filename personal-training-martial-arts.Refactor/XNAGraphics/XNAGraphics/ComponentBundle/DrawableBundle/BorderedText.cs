using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNAGraphics.ComponentBundle.DrawableBundle
{
    public class BorderedText : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        public string text;
        public float border;
        public Color border_color;

        public BorderedText(Object font, string text, float border, Color border_color)
            : this(font, text, 0, 0, Color.White, border, border_color) { }

        public BorderedText(Object font, string text, int x, int y, float border, Color border_color)
            : this(font, text, x, y, Color.White, border, border_color) { }

        public BorderedText(Object font, string text, int x, int y, Color color, float border, Color border_color)
            : base(x, y, color)
        {
            this.sprite = font;
            this.text = text;

            this.border = border;
            this.border_color = border_color;
        }

        protected override void onLoad(Game game)
        { }

        protected override void onUpdate(GameTime gameTime)
        { }

        protected override void onDraw(SpriteBatch spriteBatch)
        {
            SpriteFont sf = (SpriteFont)this.sprite;

            // Pintamos los bordes, por cada eje, según el borde pintamos una réplica de texto en el color del borde
            for (int i = (-1 * (int)this.border); i <= (int)this.border; i++)
            {
                for (int j = (-1 * (int)this.border); j <= (int)this.border; j++)
                {
                    // Simplificamos cuentas pintando solo los pares (menos calidad del efecto, pero no se nota)
                    if ((i % 2 == 0) && (j % 2 == 0))
                        spriteBatch.DrawString(sf, this.text, this.getPosition() + new Vector2(i, j), this.border_color, this.rotation, sf.MeasureString(this.text) / 2, this.scale, SpriteEffects.None, 0f);
                }
            }

            // Pintamos el texto normal
            // IMPORTANT: Para calcular el centro de nuestra cadena de texto usamos "sf.MeasureString(this.text)/2" y así ponerlo de origen para rotaciones, escalas, etc.
            spriteBatch.DrawString(sf, this.text, this.getPosition(), this.color, this.rotation, sf.MeasureString(this.text) / 2, this.scale, SpriteEffects.None, 0f);
        }
    }
}