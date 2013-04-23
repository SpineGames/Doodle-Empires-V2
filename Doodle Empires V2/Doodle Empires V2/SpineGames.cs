using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Spine_Library
{
    /// <summary>
    /// Acess to some basic drawing functions
    /// </summary>
    public abstract class DrawFunctions
    {
        /// <summary>
        /// draws a line between two vectors
        /// </summary>
        /// <param name="drawTex">A blank texture to draw from</param>
        /// <param name="batch">The SpriteBatch to draw to</param>
        /// <param name="width">The width of the line</param>
        /// <param name="color">The color to draw in</param>
        /// <param name="point1">The point to draw from</param>
        /// <param name="point2">The point to draw to</param>
        public static void drawLine(Texture2D drawTex, SpriteBatch batch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(drawTex, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        /// <summary>
        /// draws a line from a vector with a length and direction
        /// </summary>
        /// <param name="drawTex">A blank texture to draw from</param>
        /// <param name="batch">The SpriteBatch to draw to</param>
        /// <param name="width">The width of the line</param>
        /// <param name="color">The color to draw in</param>
        /// <param name="point1">The orgin point of the line</param>
        /// <param name="length">The length of the line from the orgin</param>
        /// <param name="angle">The angle in RADIANS from to orgin to draw line</param>
        public static void drawLine(Texture2D drawTex, SpriteBatch batch, float width, Color color, Vector2 point1, int length, float angle)
        {
            batch.Draw(drawTex, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        /// <summary>
        /// draws an arrow with the base at vector1 and the tip at vector2
        /// </summary>
        /// <param name="drawTex">A blank texture to draw from</param>
        /// <param name="batch">The SpriteBatch to draw to</param>
        /// <param name="lineWidth">The width of the line to draw</param>
        /// <param name="color">The color of the arrow</param>
        /// <param name="point1">The orgin point of the arrow</param>
        /// <param name="point2">The point of the arrow</param>
        public static void drawArrow(Texture2D drawTex, SpriteBatch batch, float lineWidth, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            drawLine(drawTex, batch, lineWidth, color, point1, point2);
            batch.Draw(drawTex, point2, null, color, angle - (float)(Math.PI / 1.15), Vector2.Zero, new Vector2(length / 10, lineWidth), SpriteEffects.None, 0);
            batch.Draw(drawTex, point2, null, color, angle + (float)(Math.PI / 1.15), Vector2.Zero, new Vector2(length / 10, lineWidth), SpriteEffects.None, 0);
        }

        /// <summary>
        /// draws the rectangle from an XNA rectangle
        /// </summary>
        /// <param name="drawTex">A blank texture to draw from</param>
        /// <param name="batch">the SpriteBatch to draw to</param>
        /// <param name="lineWidth">The width of the lines to draw</param>
        /// <param name="color">The color to draw in</param>
        /// <param name="rect">The rectangle to draw</param>
        public static void drawRectangle(Texture2D drawTex, SpriteBatch batch, float lineWidth, Color color, Rectangle rect)
        {
            Vector2 point1 = new Vector2(rect.X, rect.Y);
            Vector2 point2 = new Vector2(rect.X + rect.Width, rect.Y);
            Vector2 point3 = new Vector2(rect.X, rect.Y + rect.Height);
            Vector2 point4 = new Vector2(rect.X + rect.Width, rect.Y + rect.Height);

            drawLine(drawTex, batch, lineWidth, color, point1, point2);
            drawLine(drawTex, batch, lineWidth, color, point1, point3);
            drawLine(drawTex, batch, lineWidth, color, point2, point4);
            drawLine(drawTex, batch, lineWidth, color, new Vector2(point3.X - lineWidth, point3.Y), point4);
        }

        /// <summary>
        /// Draws a rectangle between two vectors
        /// </summary>
        /// <param name="drawTex">A blank texture to draw width</param>
        /// <param name="batch">The SpriteBatch to draw from</param>
        /// <param name="lineWidth">The width of the lines to draw</param>
        /// <param name="color">The color or the rectangle</param>
        /// <param name="vector1">The upper-left corner of the rectangle</param>
        /// <param name="vector2">The lower-right corner of the rectangle</param>
        public static void drawRectangle(Texture2D drawTex, SpriteBatch batch, float lineWidth, Color color, Vector2 vector1, Vector2 vector2)
        {
            Vector2 point1 = vector1;
            Vector2 point2 = vector2;
            Vector2 point3 = new Vector2(vector1.X, vector2.Y);
            Vector2 point4 = new Vector2(vector2.X, vector1.Y);

            drawLine(drawTex, batch, lineWidth, color, point1, point4);
            drawLine(drawTex, batch, lineWidth, color, point1, point3);
            drawLine(drawTex, batch, lineWidth, color, point2, point4);
            drawLine(drawTex, batch, lineWidth, color, new Vector2(vector1.X - lineWidth, vector2.Y),
                new Vector2(vector2.X + lineWidth, vector2.Y));
        }

        /// <summary>
        /// Draws a rectangle to the spritebatch from the vector,
        /// length, and width
        /// </summary>
        /// <param name="drawTex">A blank texture to draw width</param>
        /// <param name="batch">The SpiteBatch to draw to</param>
        /// <param name="lineWidth"> The width of the lines to draw</param>
        /// <param name="color">The color of the rectangle</param>
        /// <param name="vector1">The point of orgin for the rectangle</param>
        /// <param name="width">The rectangle's width</param>
        /// <param name="height">The rectangle's height</param>
        public static void drawRectangle(Texture2D drawTex, SpriteBatch batch, float lineWidth, Color color, Vector2 vector1, int width, int height)
        {
            Vector2 point1 = vector1; // top left
            Vector2 point2 = new Vector2(vector1.X + width, vector1.Y + height); // bottom right
            Vector2 point3 = new Vector2(vector1.X, vector1.Y + height); // bottom left
            Vector2 point4 = new Vector2(vector1.X + width, vector1.Y); // top right

            drawLine(drawTex, batch, lineWidth, color, point1, point4);
            drawLine(drawTex, batch, lineWidth, color, point1, point3);
            drawLine(drawTex, batch, lineWidth, color, point2, point4);
            drawLine(drawTex, batch, lineWidth, color, new Vector2(vector1.X - lineWidth, vector1.Y + height), 
                new Vector2(vector1.X + width + lineWidth, vector1.Y + height));
        }

        /// <summary>
        /// Draw a circle from a center and a radius
        /// </summary>
        /// <param name="drawTex">A blank texture to draw from</param>
        /// <param name="batch">The SpriteBatch to draw to</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="stepping">The number of line segments to draw</param>
        /// <param name="radius">The circle's radius</param>
        /// <param name="color">The color to draw in</param>
        public static void drawCircle(Texture2D drawTex, SpriteBatch batch, Color color, Vector2 center, int stepping, int radius)
        {
            //figure out the difference
            double increment = (Math.PI * 2) / stepping;

            //render
            double angle = 0;
            for (int i = 0; i < stepping; i++)
            {
                //draw outline
                drawLine(drawTex, batch, 2F, color, extraMath.calculateVector(center, angle, radius),
                    extraMath.calculateVector(center, angle + increment, radius));
                angle += increment;
            }
        }

        /// <summary>
        /// Draws a slightly buggy triangle
        /// </summary>
        /// <param name="drawTex">A blank texture to use</param>
        /// <param name="batch">The SpriteBatch to draw to</param>
        /// <param name="drawColor">The color to draw in</param>
        /// <param name="lineWidth">The width of the lines to draw</param>
        /// <param name="point1">The top point of the triangle</param>
        /// <param name="point2">The lower left point of the triangle</param>
        /// <param name="point3">The lower right point of the triangle</param>
        public static void drawTriangle(Texture2D drawTex, SpriteBatch batch, Color drawColor, float lineWidth, Vector2 point1, Vector2 point2, Vector2 point3)
        {
                 //P1\\
                //    \\
               //      \\ 
              //        \\
             //P2========P3
            drawLine(drawTex, batch, lineWidth, drawColor, point1, point2);
            drawLine(drawTex, batch, lineWidth, drawColor, point2, point3);
            drawLine(drawTex, batch, lineWidth, drawColor, point3, point1);
        }

        /// <summary>
        /// Draws the texture tiled over the rectangle, with the tiling starting at the 
        /// top - left of the rectangle
        /// </summary>
        /// <param name="batch">The SpriteBatch to draw with</param>
        /// <param name="tex">The textur to tile</param>
        /// <param name="rect">The rectangle to tile in</param>
        /// <param name="color">The color to draw the textures with</param>
        /// <param name="size">The width/height to draw each texture tile</param>
        /// <param name="offset">The offset to draw everything at</param>
        public static void drawOffsetTiledTexture(SpriteBatch batch, Texture2D tex, Rectangle rect, Color color, float size, Vector2 offset)
        {
            rect.Width = (int)(rect.Width / size);
            rect.Height = (int)(rect.Height / size);

            //end standard drawing and begin advanced drawing
            batch.End();
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearWrap,
                DepthStencilState.Default, RasterizerState.CullNone);

            //hold temporary sampler state
            SamplerState temp = batch.GraphicsDevice.SamplerStates[0];
            //set the sampler state
            batch.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            //draw the object
            batch.Draw(tex, new Vector2(rect.X - offset.X, rect.Y - offset.Y), new Rectangle(0, 0, rect.Width, rect.Height)
                , color, 0F, Vector2.Zero, size, SpriteEffects.None, 0F);
            //reset sampler state
            batch.GraphicsDevice.SamplerStates[0] = temp;

            //end fancy drawing and return to normal drawing
            batch.End();
            batch.Begin();
        }

        /// <summary>
        /// Draws the texture tiled over the rectangle
        /// </summary>
        /// <param name="batch">The SpriteBatch to draw with</param>
        /// <param name="tex">The textur to tile</param>
        /// <param name="rect">The rectangle to tile in</param>
        /// <param name="color">The color to draw the textures with</param>
        /// <param name="size">The width/height to draw each texture tile</param>
        /// <param name="offset">The offset to draw everything at</param>
        public static void drawTiledTexture(SpriteBatch batch, Texture2D tex, Rectangle rect, Color color, float size, Vector2 offset)
        {

            rect.Width = (int)(rect.Width / size);
            rect.Height = (int)(rect.Height / size);

            //end standard drawing and begin advanced drawing
            batch.End();
            batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearWrap,
                DepthStencilState.Default, RasterizerState.CullNone);

            //hold temporary sampler state
            SamplerState temp = batch.GraphicsDevice.SamplerStates[0];
            //set the sampler state
            batch.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            //draw the object
            batch.Draw(tex, new Vector2(rect.X - offset.X, rect.Y - offset.Y), rect, color, 0F, Vector2.Zero, size, SpriteEffects.None, 0F);
            //reset sampler state
            batch.GraphicsDevice.SamplerStates[0] = temp;

            //end fancy drawing and return to normal drawing
            batch.End();
            batch.Begin();
        }

        /// <summary>
        /// Draws the texture tiled over the rectangle
        /// </summary>
        /// <param name="batch">The SpriteBatch to draw with</param>
        /// <param name="tex">The textur to tile</param>
        /// <param name="rect">The rectangle to tile in</param>
        /// <param name="color">The color to draw the textures with</param>
        /// <param name="size">The width/height to draw each texture tile</param>
        public static void drawTiledTexture(SpriteBatch batch, Texture2D tex, Rectangle rect, Color color, float size)
        {
            drawTiledTexture(batch, tex, rect, color, size, new Vector2(0,0));
        }
            
        /// <summary>
        /// Draws the texture tiled over the rectangle
        /// </summary>
        /// <param name="batch">The SpriteBatch to draw with</param>
        /// <param name="tex">The textur to tile</param>
        /// <param name="rect">The rectangle to tile in</param>
        /// <param name="color">The color to draw the textures with</param>
        /// <param name="size">The width/height to draw each texture tile</param>
        public static void drawTiledTexture(SpriteBatch batch, Texture2D tex, Rectangle rect, Color color, Vector2 offset)
        {
            drawTiledTexture(batch, tex, rect, color, 1F, offset);
        }

        /// <summary>
        /// Draws the texture tiled over the rectangle
        /// </summary>
        /// <param name="batch">The SpriteBatch to draw with</param>
        /// <param name="tex">The textur to tile</param>
        /// <param name="rect">The rectangle to tile in</param>
        /// <param name="color">The color to draw the textures with</param>
        public static void drawTiledTexture(SpriteBatch batch, Texture2D tex, Rectangle rect, Color color)
        {
            drawTiledTexture(batch, tex, rect, color, 1F, new Vector2(0, 0));
        }

        public static void drawCenteredText(SpriteBatch batch, SpriteFont font, string text, Vector2 pos, Color color)
        {
            batch.DrawString(font, text, pos, color, 0F, new Vector2(
                font.MeasureString(text).X / 2,
                font.MeasureString(text).Y / 2),
                1F, SpriteEffects.None, 1F);
        }

        public static void drawHorizontalCenteredText(SpriteBatch batch, SpriteFont font, string text, Vector2 pos, Color color)
        {
            batch.DrawString(font, text, pos, color, 0F, new Vector2(
                font.MeasureString(text).X / 2, 0),
                1F, SpriteEffects.None, 1F);
        }
    }

    /// <summary>
    /// Draws advanced shapes and such using the GraphicsDevice
    /// </summary>
    public abstract class AdvancedDrawFuncs
    {
        /// <summary>
        /// Draws a line
        /// </summary>
        /// <param name="basicEffect">The BasicEffect to draw with</param>
        /// <param name="vect">The 1st point</param>
        /// <param name="vect2">The 2nd point</param>
        /// <param name="col1">The color of point 1</param>
        /// <param name="col2">The color of point 2</param>
        public static void DrawLine(BasicEffect basicEffect, Vector2 vect, Vector2 vect2, Color col1, Color col2)
        {
            float x1 = vect.X;
            float y1 = vect.Y;
            float x2 = vect2.X;
            float y2 = vect2.Y;
            VertexPositionColor[] vertices;
            vertices = new VertexPositionColor[2];
            vertices[0].Position = new Vector3(x1, y1, 0);
            vertices[0].Color = col1;
            vertices[1].Position = new Vector3(x2, y2, 0);
            vertices[1].Color = col2;
            basicEffect.CurrentTechnique.Passes[0].Apply();
            basicEffect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 1);
        }

        /// <summary>
        /// Draws a line
        /// </summary>
        /// <param name="basicEffect">The BasicEffect to draw in</param>
        /// <param name="x1">The x co-ordinate of point 1</param>
        /// <param name="y1">The y co-ordinate of point 1</param>
        /// <param name="x2">The x co-ordinate of point 2</param>
        /// <param name="y2">The y co-ordinate of point 2</param>
        /// <param name="col1">The color of point 1</param>
        /// <param name="col2">The color of point 2</param>
        public static void DrawLine(BasicEffect basicEffect, float x1, float y1, float x2, float y2, Color col1, Color col2)
        {
            VertexPositionColor[] vertices;
            vertices = new VertexPositionColor[2];
            vertices[0].Position = new Vector3(x1, y1, 0);
            vertices[0].Color = col1;
            vertices[1].Position = new Vector3(x2, y2, 0);
            vertices[1].Color = col2;
            basicEffect.CurrentTechnique.Passes[0].Apply();
            basicEffect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 1);
        }

        /// <summary>
        /// Draws a rectangle between the 4 corners
        /// </summary>
        /// <param name="basicEffect">The BasicEffect to draw in</param>
        /// <param name="x1">The top-left x co-ordinate</param>
        /// <param name="y1">The top-left y co-ordinate</param>
        /// <param name="x2">The bottom-right x co-ordinate</param>
        /// <param name="y2">The bottom-right y co-ordinate</param>
        /// <param name="col1">The color of the outline</param>
        /// <param name="col2">The inner color</param>
        /// <param name="filled">true if the rectangle should be filled</param>
        public static void DrawRect(BasicEffect basicEffect, float x1, float y1, float x2, float y2, Color col1, Color col2, bool filled)
        {
            if (filled == true)
            {
                drawRectFill(basicEffect, x1, y1, x2, y2, col2);
            }
            DrawLine(basicEffect, x1, y1, x2, y1, col1, col1);
            DrawLine(basicEffect, x1, y2, x2, y2, col1, col1);
            DrawLine(basicEffect, x1, y1, x1, y2, col1, col1);
            DrawLine(basicEffect, x2, y1, x2, y2, col1, col1);
        }

        /// <summary>
        /// Draws the interior of the rectangle
        /// </summary>
        /// <param name="basicEffect">The BasicEffect to draw in</param>
        /// <param name="x1">The top-left x of the rectangle</param>
        /// <param name="y1">The top-left y of the rectangle</param>
        /// <param name="x2">The bottom-right x of the rectangle</param>
        /// <param name="y2">The bottom-right y of the rectangle</param>
        /// <param name="col">The color to draw with</param>
        public static void drawRectFill(BasicEffect basicEffect, float x1, float y1, float x2, float y2, Color col)
        {
            VertexPositionColor[] vertices;
            vertices = new VertexPositionColor[6];
            vertices[0].Position = new Vector3(x1, y1, 0.5F);
            vertices[0].Color = col;
            vertices[1].Position = new Vector3(x1, y2, 0);
            vertices[1].Color = col;
            vertices[2].Position = new Vector3(x2, y2, 0);
            vertices[2].Color = col;
            vertices[3].Position = new Vector3(x2, y1, 0);
            vertices[3].Color = col;
            vertices[4].Position = new Vector3(x1, y1, 0);
            vertices[4].Color = col;
            vertices[5].Position = new Vector3(x1, y2, 0);
            vertices[5].Color = col;
            basicEffect.CurrentTechnique.Passes[0].Apply();
            
            short[] triangleStripIndices = new short[6] { 0, 1, 2, 3, 4, 5 };
            basicEffect.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, 6, triangleStripIndices, 0, 4);
        }

        /// <summary>
        /// Draws a texture on a quad, currently broken
        /// </summary>
        /// <param name="basicEffect">The BasicEffect to draw in</param>
        /// <param name="x1">The top-left x co-ordinate</param>
        /// <param name="y1">The top-left y co-ordinate</param>
        /// <param name="x2">The bottom-right x co-ordinate</param>
        /// <param name="y2">The bottom-right y co-ordinate</param>
        /// <param name="tex">The texture to draw</param>
        /// <param name="col">The color to draw in</param>
        public static void drawTexture(BasicEffect basicEffect, float x1, float y1, float x2, float y2, Texture2D tex, Color col, Vector2 winPos)
        {
            Texture2D temp = basicEffect.Texture;
            int[] indexData = new int[6];
            indexData[0] = 0;
            indexData[1] = 2;
            indexData[2] = 3;

            indexData[3] = 0;
            indexData[4] = 1;
            indexData[5] = 2;

            VertexPositionColorTexture[] vertexData = new VertexPositionColorTexture[4];

            vertexData[0] = new VertexPositionColorTexture(new Vector3(x1, y1, 0), col, new Vector2(0, 0));
            vertexData[1] = new VertexPositionColorTexture(new Vector3(x2, y1, 0), col, new Vector2(1, 0));
            vertexData[2] = new VertexPositionColorTexture(new Vector3(x2, y2, 0), col, new Vector2(1, 1));
            vertexData[3] = new VertexPositionColorTexture(new Vector3(x1, y2, 0), col, new Vector2(0, 1));

            bool texEnabled = basicEffect.TextureEnabled;


            basicEffect.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            basicEffect.Texture = tex;
            basicEffect.TextureEnabled = true;
            basicEffect.DiffuseColor = Color.White.ToVector3();
            basicEffect.CurrentTechnique.Passes[0].Apply();

            basicEffect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            basicEffect.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertexData, 0, 4, indexData, 0, 2);
            basicEffect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            basicEffect.Texture = temp;
            basicEffect.TextureEnabled = texEnabled;
        }
        
        /// <summary>
        /// Draws a texture on a quad, currently broken
        /// </summary>
        /// <param name="basicEffect">The BasicEffect to draw in</param>
        /// <param name="x1">The top-left x co-ordinate</param>
        /// <param name="y1">The top-left y co-ordinate</param>
        /// <param name="x2">The bottom-right x co-ordinate</param>
        /// <param name="y2">The bottom-right y co-ordinate</param>
        /// <param name="tex">The texture to draw</param>
        /// <param name="col">The color to draw in</param>
        public static void drawTiledTexture(BasicEffect basicEffect, float x1, float y1, float x2, float y2, Texture2D tex, Color col, Vector2 winPos)
        {

            VertexPositionColorTexture[] vertexData = new VertexPositionColorTexture[6];

            vertexData[0] = new VertexPositionColorTexture(new Vector3(x1, y1, 0), col, new Vector2(x1 / tex.Width, y1 / tex.Height));
            vertexData[1] = new VertexPositionColorTexture(new Vector3(x2, y1, 0), col, new Vector2(x2 / tex.Width, y1 / tex.Height));
            vertexData[2] = new VertexPositionColorTexture(new Vector3(x2, y2, 0), col, new Vector2(x2 / tex.Width, y2 / tex.Height));
            vertexData[3] = new VertexPositionColorTexture(new Vector3(x1, y1, 0), col, new Vector2(x1 / tex.Width, y1 / tex.Height));
            vertexData[4] = new VertexPositionColorTexture(new Vector3(x1, y2, 0), col, new Vector2(x1 / tex.Width, y2 / tex.Height));
            vertexData[5] = new VertexPositionColorTexture(new Vector3(x2, y2, 0), col, new Vector2(x2 / tex.Width, y2 / tex.Height));

            bool texEnabled = basicEffect.TextureEnabled;
            Texture2D temp = basicEffect.Texture;

            basicEffect.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            basicEffect.Texture = tex;
            basicEffect.TextureEnabled = true;
            basicEffect.DiffuseColor = Color.White.ToVector3();
            basicEffect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            basicEffect.CurrentTechnique.Passes[0].Apply();

            basicEffect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertexData, 0, 2);
            basicEffect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            basicEffect.Texture = temp;
            basicEffect.TextureEnabled = texEnabled;
        }

        //draw a rectangle using 2 vectors
        public static void DrawRect(BasicEffect basicEffect, Vector2 vect, Vector2 vect2, Color col1, Color col2, bool filled)
        {
            float x1 = vect.X;
            float y1 = vect.Y;
            float x2 = vect2.X;
            float y2 = vect2.Y;

            if (filled == true)
            {
                drawRectFill(basicEffect, vect.X, vect.Y, vect2.X, vect2.Y, col2);
            }
            DrawLine(basicEffect, x1, y1, x2, y1, col1, col1);
            DrawLine(basicEffect, x1, y2, x2, y2, col1, col1);
            DrawLine(basicEffect, x1, y1, x1, y2, col1, col1);
            DrawLine(basicEffect, x2, y1, x2, y2, col1, col1);
        }

        //draw a rectangle from a rectangle
        public static void DrawRect(BasicEffect basicEffect, Rectangle rect, Color col1, Color col2, bool filled)
        {
            float x1 = rect.X;
            float y1 = rect.Y;
            float x2 = rect.X + rect.Width;
            float y2 = rect.Y + rect.Height;

            if (filled == true)
            {
                drawRectFill(basicEffect, rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height, col2);
            }
            DrawLine(basicEffect, x1, y1, x2, y1, col1, col1);
            DrawLine(basicEffect, x1, y2, x2, y2, col1, col1);
            DrawLine(basicEffect, x1, y1, x1, y2, col1, col1);
            DrawLine(basicEffect, x2, y1, x2, y2, col1, col1);
        }

        //draws a triangle
        public static void drawTriangle(BasicEffect basicEffect, Vector2 pos1, Vector2 pos2, Vector2 pos3, Color col)
        {
            VertexPositionColor[] vertices;
            vertices = new VertexPositionColor[3];
            vertices[0].Position = new Vector3(pos1.X, pos1.Y, 0);
            vertices[0].Color = col;
            vertices[1].Position = new Vector3(pos2.X, pos2.Y, 0);
            vertices[1].Color = col;
            vertices[2].Position = new Vector3(pos3.X, pos3.Y, 0);
            vertices[2].Color = col;
            basicEffect.CurrentTechnique.Passes[0].Apply();
            basicEffect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1, VertexPositionColor.VertexDeclaration);
        }

        //WTF do you think it does?
        public static void drawCircle(BasicEffect basicEffect, Vector2 center, double radius, int stepping, Color innerColor, Color outerColor)
        {
            List<VertexPositionColor> vertices = new List<VertexPositionColor>();

            //figure out the difference
            double increment = (Math.PI * 2) / stepping;

            //render
            double angle = 0;
            for (int i = 0; i < stepping; i++, angle += increment)
            {
                vertices.Add(new VertexPositionColor(new Vector3(extraMath.calculateVector(center, angle, radius), 0), outerColor));
                vertices.Add(new VertexPositionColor(new Vector3(center, 0), innerColor));
                vertices.Add(new VertexPositionColor(new Vector3(extraMath.calculateVector(center, angle + increment, radius), 0), outerColor));
            }
            basicEffect.CurrentTechnique.Passes[0].Apply();
            basicEffect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count / 3, VertexPositionColor.VertexDeclaration);
        }

        public static void drawUnfilledCircle(BasicEffect basicEffect, Vector2 center, double radius, int stepping, Color col)
        {
            List<VertexPositionColor> vertices = new List<VertexPositionColor>();

            //figure out the difference
            double increment = (Math.PI * 2) / stepping;

            //render
            double angle = 0;
            for (int i = 0; i <= stepping; i++, angle += increment)
            {
                vertices.Add(new VertexPositionColor(new Vector3(extraMath.calculateVector(center, angle, radius), 0), col));
            }
            basicEffect.CurrentTechnique.Passes[0].Apply();
            basicEffect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices.ToArray(), 0, vertices.Count - 1, VertexPositionColor.VertexDeclaration);
        }

        //draw a simple flag :P
        public static void drawFlag(BasicEffect basicEffect, Vector2 vect, double size, Color col, int win_x, int win_y)
        {
            //bottom of shaft
            float x1 = vect.X - win_x;
            float y1 = vect.Y - win_y;

            //top of shaft
            float x2 = x1;
            float y2 = (float)(y1 - size);

            //tip of flag
            float x3 = (float)(x1 + (size / 4));
            float y3 = (float)(y2 + (size / 8));

            //where bottom of flag meets the mast
            float x4 = x1;
            float y4 = (float)(y2 + (size / 4));
            //draw flag
            DrawLine(basicEffect, x1, y1, x2, y2, Color.Red, Color.Red);
            drawTriangle(basicEffect, new Vector2(x2, y2), new Vector2(x3, y3), new Vector2(x4, y4), col);
        }

        //get a color from a gradient
        public static Color getColorFromGradient(Color color1, Color color2, int stretch, float getPos, int alpha)
        {
            float R;
            float G;
            float B;

            int R1 = color1.R;
            int G1 = color1.G;
            int B1 = color1.B;

            int R2 = color2.R;
            int G2 = color2.G;
            int B2 = color2.B;

            float Rchange;
            float Gchange;
            float Bchange;

            Rchange = (R2 - R1) / stretch;
            Bchange = (B2 - B1) / stretch;
            Gchange = (G2 - G1) / stretch;

            R = R2 - (Rchange * getPos);
            G = G2 - (Gchange * getPos);
            B = B2 - (Bchange * getPos);

            return Color.FromNonPremultiplied((int)R, (int)G, (int)B, 255);
        }

        public class Plane
        {
            VertexPositionColorTexture[] verts;
            Texture2D tex;

            public Plane(float x1, float y1, float x2, float y2, Texture2D tex, Color col)
            {
                this.tex = tex;

                verts = new VertexPositionColorTexture[6];
                verts[0] = new VertexPositionColorTexture(new Vector3(x1, y1, -0.1F), col, new Vector2(x1 / tex.Width, y1 / tex.Height));
                verts[1] = new VertexPositionColorTexture(new Vector3(x2, y1, -0.1F), col, new Vector2(x2 / tex.Width, y1 / tex.Height));
                verts[2] = new VertexPositionColorTexture(new Vector3(x2, y2, -0.1F), col, new Vector2(x2 / tex.Width, y2 / tex.Height));
                verts[3] = new VertexPositionColorTexture(new Vector3(x1, y1, -0.1F), col, new Vector2(x1 / tex.Width, y1 / tex.Height));
                verts[4] = new VertexPositionColorTexture(new Vector3(x1, y2, -0.1F), col, new Vector2(x1 / tex.Width, y2 / tex.Height));
                verts[5] = new VertexPositionColorTexture(new Vector3(x2, y2, -0.1F), col, new Vector2(x2 / tex.Width, y2 / tex.Height));
            }

            public void render(BasicEffect basicEffect, Vector2 winPos)
            {
                basicEffect.Texture = tex;
                basicEffect.TextureEnabled = true;
                basicEffect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                basicEffect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                basicEffect.CurrentTechnique.Passes[0].Apply();

                basicEffect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, verts, 0, 2);

                basicEffect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
                
                basicEffect.TextureEnabled = false;
                basicEffect.CurrentTechnique.Passes[0].Apply();
            }
        }
    }

    /// <summary>
    /// Acess to some custom Math functions
    /// </summary>
    public abstract class extraMath
    {
        /// <summary>
        /// Calculates the vector at the end of the given line
        /// from another vector
        /// </summary>
        /// <param name="vector">The orgin of the line</param>
        /// <param name="angle">The angle in RADIANS from the line</param>
        /// <param name="length">The length of the line</param>
        /// <returns>Offset Vector</returns>
        public static Vector2 calculateVector(Vector2 vector, double angle, double length)
        {
            Vector2 returnVect = new Vector2(vector.X + (float)lengthdir_x(angle, length), vector.Y + (float)lengthdir_y(angle,length));
            return returnVect;
        }

        public static Vector2 calculateVectorOffset(double angle, double length)
        {
            return calculateVector(Vector2.Zero, angle, length);
        }

        /// <summary>
        /// Calculates the angle between two vectors
        /// </summary>
        /// <param name="point1">The orgin vector to calculate from</param>
        /// <param name="point2">The vector to calculate to</param>
        /// <returns>Angle in radians</returns>
        public static double findAngle(Vector2 point1, Vector2 point2)
        {
            return Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);            
        }

        /// <summary>
        /// Uses the midpoint displacement algorithm to return
        /// an array of values
        /// </summary>
        /// <param name="h">The smoothing level to use (higher = rougher)</param>
        /// <param name="baseVal">The base value to generate around</param>
        /// <param name="length">The length of the array (must be a multiple of 2)</param>
        /// <returns>A integer heightmap array</returns>
        public static float[] MidpointDisplacement(int h, int baseVal, int length)
        {
            //arguments:
            //
            //argument0 = room width
            //
            //argument1 = height change variable
            //
            //argument2 = land id
            //
            float[] output = new float[length + 1];
            Random RandNum = new Random();

            for (int xx = 0; xx <= length; xx++)
            {
                output[xx] = baseVal;
            }
            output[0] = baseVal;

            //generate values
            for (int rep = 2; rep < length; rep *= 2)
            {
                for (int i = 1; i <= rep; i += 1)
                {

                    int x1 = (length / rep) * (i - 1);
                    int x2 = (length / rep) * i;
                    float avg = (output[x1] + output[x2]) / 2;
                    int Rand = RandNum.Next(-h, h);
                    output[(x1 + x2) / 2] = avg + (Rand);
                }
                h /= 2;
            }

            //returns array
            return output;
        }

        /// <summary>
        /// Returns how many radians change there should be given
        /// a speed relative to circumfrence and a height from
        /// the orgin point
        /// </summary>
        /// <param name="radius">The distance from the center point</param>
        /// <param name="speed">The speed in pixels to rotate around the orgin</param>
        /// <returns>The angle change in RADIANS</returns>
        public static double findCircumfenceAngleChange(double radius, double speed)
        {
            //determine value of n
            double n = Math.Acos((Math.Pow(speed, 2) - 2 * Math.Pow(radius, 2)) / (-2 * Math.Pow(radius, 2)));
            //make sure n is non NAN
            if (n == Double.NaN)
            {
                //set the return to be -1
                n = -1;
                //throw an exception
                throw (new ArithmeticException("Number is NAN!"));
            }
            //return n
            return n;
        }

        /// <summary>
        /// Determines the altitude from a given eliptical orbit
        /// and an angle in RADIANS
        /// </summary>
        /// <param name="length">The length of the elliptical orbit</param>
        /// <param name="width">The width of the elliptical orbit</param>
        /// <param name="theta">The angle in RADIANS from the orgin</param>
        /// <returns>The altitude at the given point</returns>
        public static double getAltitudeFromCenteredOrbit(double length, double width, double theta)
        {
            return length * width / (Math.Sqrt(Math.Pow((length * Math.Cos(theta)), 2) + Math.Pow((width * Math.Sin(theta)), 2)));
        }

        /// <summary>
        /// Determines the altitude from a given eliptical orbit
        /// and an angle in RADIANS
        /// </summary>
        /// <param name="length">The length of the elliptical orbit</param>
        /// <param name="width">The width of the elliptical orbit</param>
        /// <param name="offset">The offset along the length</param>
        /// <param name="theta">The angle in RADIANS from the orgin</param>
        /// <param name="angleOffset">The angle in RADIANS that the orbit is rotated by</param>
        /// <returns>The altitude at the given point</returns>
        public static double getAltitudeFromOffsetOrbit(double length, double width, double offset, double theta, double angleOffset)
        {
            theta -= angleOffset;
            return ((length * width) / Math.Sqrt(Math.Pow(width * Math.Cos(theta), 2) + Math.Pow(length * Math.Sin(theta), 2)));
        }

        /// <summary>
        /// Maps the given value from one number range to another
        /// </summary>
        /// <param name="lowVal">The low value in the orgin number range</param>
        /// <param name="highVal">The high value in the orgin number range<</param>
        /// <param name="newLowVal">The low value in the new number range<</param>
        /// <param name="newHighVal">The high value in the new number range<</param>
        /// <param name="value">The value to map</param>
        /// <returns>The value mapped to the new range</returns>
        public static double map(double lowVal, double highVal, double newLowVal, double newHighVal, double value)
        {
            double range = newHighVal - newLowVal;
            double oldRange = highVal - lowVal;
            double multiplier = range / oldRange;
            return newLowVal + ((value - lowVal) * multiplier);

        }

        /// <summary>
        /// Returns the angle in RADIANS that is 90° to
        /// the angle from the orgin to the point
        /// </summary>
        /// <param name="orgin">The orgin point</param>
        /// <param name="relativePoint">The point to calculate angle for</param>
        /// <returns>Angle in RADIANS that is 90° from the angle to orgin</returns>
        public static double getDrawAngle(Vector2 orgin, Vector2 relativePoint)
        {
            return -findAngle(orgin, relativePoint) + Math.PI / 2;
        }

        /// <summary>
        /// Returns the percentage that value is of maxValue
        /// </summary>
        /// <param name="maxValue">The top number, ex: number of marks on test.
        /// Cannot be 0</param>
        /// <param name="value">The value to be checked. ex: mark on test. 
        /// May be higher than maxValue</param>
        /// <returns>A percentage</returns>
        public static double getPercent(double maxValue, double value)
        {
            return ((value / maxValue) * 100.0);
        }
        
        /// <summary>
        /// Gets an array of colors for the texture. Thanks to Cyral from Stack Exchange
        /// </summary>
        /// <param name="texture">The texture to get the colors from</param>
        /// <returns>A 2D array of colors that represents the texture</returns>
        public static uint[] getTextureData(Texture2D texture)
        {
            //gets the 1D color[] from the teture
            uint[] colorList = new uint[texture.Width * texture.Height];
            //Get the colors
            texture.GetData(colorList);

            return colorList; //Done!
        }

        /// <summary>
        /// Return the Color with opposite RGB values, but with same 
        /// alpha value
        /// </summary>
        /// <param name="color">The color to get the opposite of</param>
        /// <returns>The opposite Color to color</returns>
        public static Color oppositeColor(Color color)
        {
            return Color.FromNonPremultiplied(255 - color.R, 255 - color.G, 255 - color.B, color.A);
        }

        public static double lengthdir_x(double angle, double length)
        {
            return length * Math.Cos(angle);
        }

        public static double dir_x(double length, double xChange)
        {
            return Math.Acos(xChange / length);
        }

        public static double lengthdir_y(double angle, double length)
        {
            return length * Math.Sin(angle);
        }

        public static double dir_y(double length, double yChange)
        {
            return Math.Asin(yChange / length);
        }

        public static double lengthdir_z(double angle, double length)
        {
            return length * Math.Sin(angle);
        }

        public static double anglePullZ(double angle, double length)
        {
            double x =  length * Math.Sin(angle);
            return Math.Sqrt(Math.Pow(length, 2) - Math.Pow(x, 2));
        }

        public static double dir_z(double length, double zChange)
        {
            return Math.Asin(zChange / length);
        }

        public static double getDistance(Vector2 vect1, Vector2 vect2)
        {
            return Math.Sqrt(Math.Pow(vect2.X - vect1.X, 2) + Math.Pow(vect2.Y - vect1.Y, 2));
        }

        public static Vector3 getVector3(Vector2 yawPitch, double length)
        {
            return new Vector3(
                        extraMath.calculateVectorOffset(yawPitch.X, extraMath.anglePullZ(yawPitch.Y, length)),
                        (float)extraMath.lengthdir_z(yawPitch.Y, length));
        }

        public static Texture2D getSubTexture(GraphicsDevice graphics, Rectangle sourceRect, Texture2D orginImage)
        {
            Texture2D cropTexture = new Texture2D(graphics, sourceRect.Width, sourceRect.Height);
            Color[] data = new Color[sourceRect.Width * sourceRect.Height];
            orginImage.GetData(0, sourceRect, data, 0, data.Length);
            cropTexture.SetData(data);
            return cropTexture;
        }
    }

    /// <summary>
    /// Handles all the items to be used by entities, inventories,
    /// etc...
    /// </summary>
    public class ItemHandler
    {
        ItemBase[] itemList;
        int itemCount = 0;

        public ItemHandler(int listSize)
        {
            itemList = new ItemBase[listSize];
        }

        /// <summary>
        /// Adds an item to the list
        /// </summary>
        /// <param name="item">The item to add (casted to an ItemBase)</param>
        /// <returns>The ID of the item, -1 if unsucessfull</returns>
        public int addItemToList(ItemBase item)
        {
            int id = getFirstOpenID();
            if (id != -1)
            {
                item.identifier = id;
                itemList[id] = item;
                itemCount++;
                return id;
            }
            return -1;
        }

        /// <summary>
        /// Removes an item from the list with the given ID
        /// </summary>
        /// <param name="id">The ID of the item to remove</param>
        /// <returns>True if the remove was sucessfull</returns>
        public bool removeItemFromList(int id)
        {
            try
            {
                if (itemList[id] != null)
                {
                    itemList[id] = null;
                    itemCount--;
                    return true;
                }
                return false;
            }
            catch (IndexOutOfRangeException)
            {
            }
            return false;
        }

        /// <summary>
        /// Removes an item from the list with the given name
        /// </summary>
        /// <param name="name">The name of the item to remove</param>
        /// <returns>True if the remove was sucessfull</returns>
        public bool removeItemFromList(string name)
        {
            try
            {
                for (int i = 0; i < itemList.Length; i++)
                {
                    if (itemList[i] != null)
                    {
                        if (itemList[i].name == name)
                        {
                            itemList[i] = null;
                            itemCount--;
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (IndexOutOfRangeException)
            {
            }
            return false;
        }

        /// <summary>
        /// returns the first available ID
        /// </summary>
        /// <returns>First open ID</returns>
        private int getFirstOpenID()
        {
            for (int i = 0; i < itemList.Length; i++)
                if (itemList[i] == null)
                    return i;
            return -1;
        }

        /// <summary>
        /// Gets the number of items that the itmeHandler is 
        /// holding
        /// </summary>
        /// <returns>ItemCount</returns>
        public int getItemCount()
        {
            return itemCount;
        }

        /// <summary>
        /// Gets a string of all the item names seperated by newlines
        /// </summary>
        /// <returns>List of names</returns>
        public string getCombinedNames()
        {
            string r = "";
            foreach (ItemBase i in itemList)
            {
                if (i != null)
                {
                    r += i.name + "\n";
                }
            }
            return r;
        }

        /// <summary>
        /// Holds the important values and functions for
        /// items
        /// </summary>
        public class ItemBase
        {
            public Texture2D myTexture;
            public int identifier;
            public double metaData;
            public string name;

            public ItemBase(Texture2D texture, string name)
            {
                this.myTexture = texture;
                this.name = name;
            }
        }
    }

    /// <summary>
    /// Handles the calculation of the framerate
    /// </summary>
    public class FPSHandler
    {
        int framesInSecond = 0, FPS = 0;
        double timer = 0;

        /// <summary>
        /// Handles the FPS counter for the draw event
        /// </summary>
        /// <param name="gameTime">The current GameTime</param>
        public void onDraw(GameTime gameTime)
        {
            framesInSecond++;
            timer += gameTime.ElapsedGameTime.Milliseconds;

            if (timer >= 1000)
            {
                timer = 0;
                FPS = framesInSecond;
                framesInSecond = 0;
            }
        }

        /// <summary>
        /// Returns the framerate as coounted by this object
        /// </summary>
        /// <returns>Framerate</returns>
        public int getFrameRate()
        {
            return FPS;
        }

        /// <summary>
        /// Gets a multiplier that concides with 60 FPS
        /// </summary>
        /// <returns></returns>
        public double getCommonDiff()
        {
            //return 60D / FPS;
            return 1D;
        }
    }

    /// <summary>
    /// Handles inventories for players or other things
    /// </summary>
    public class Inventory
    {
        public ItemHandler.ItemBase[] items;
        public Inventory(int slotCount)
        {
        }

        private class Item
        {
            short stackSize;
            ItemHandler.ItemBase BaseItem;

            public Item(ItemHandler.ItemBase BaseItem, short stackSize)
            {
                this.BaseItem = BaseItem;
                this.stackSize = stackSize;
            }

            public short getItemCount()
            {
                return stackSize;
            }
        }
    }

    /// <summary>
    /// Watches a key for a keypress. Note that to make sure action does not 
    /// repeat, use "keywatcher.wasPressed = false;" after action
    /// </summary>
    public class KeyWatcher
    {
        Keys key;
        List<Keys> keys;
        List<keyState> keyStates = new List<keyState>();
        bool isKeyDown = false, wasPreviouslyDown = false;
        public bool wasPressed = false, wasReleased = false;
        byte type = 0;

        /// <summary>
        /// Create a new keywatcher
        /// </summary>
        /// <param name="key">The key to watch</param>
        public KeyWatcher(Keys key)
        {
            this.key = key;
        }

        /// <summary>
        /// Create a new keywatcher that uses multiple keys for one action
        /// or each individual key for that action. Ex: Ctrl + S, + / Add
        /// </summary>
        /// <param name="key">The keys to watch</param>
        /// <param name="all">True if all keys need to be pressed at once</param>
        public KeyWatcher(List<Keys> keys, bool all)
        {
            this.keys = keys;
            if (all)
                this.type = 1;
            else
                this.type = 2;
            foreach (Keys k in keys)
            {
                keyStates.Add(new keyState(k));
            }
        }

        /// <summary>
        /// Updates the key watcher
        /// </summary>
        public void update()
        {
            switch (type)
            {
                #region single key type
                case 0:
                    if (Keyboard.GetState().IsKeyDown(key))
                    {
                        if (!isKeyDown)
                        {
                            wasPressed = true;
                            isKeyDown = true;
                        }
                        else
                        {
                            wasPressed = false;
                        }
                        wasPreviouslyDown = true;
                    }
                    else
                    {
                        if (wasPreviouslyDown)
                            wasReleased = true;
                        else
                            wasReleased = false;
                        isKeyDown = false;
                        wasPreviouslyDown = false;
                    }
                    break;
                #endregion

                #region all down type
                case 1:
                    int count = 0;
                    foreach (Keys k in keys)
                    {
                        if (Keyboard.GetState().IsKeyDown(k))
                            count ++;
                    }

                    if (count == keys.Count)
                    {
                        if (!isKeyDown)
                        {
                            wasPressed = true;
                            isKeyDown = true;
                        }
                        else
                        {
                            wasPressed = false;
                        }
                    }
                    else
                    {
                        if (wasPreviouslyDown)
                            wasReleased = true;
                        else
                            wasReleased = false;
                        isKeyDown = false;
                    }
                    break;
                #endregion

                #region multiple type
                case 2:
                    bool temp2 = false;
                    foreach (keyState k in keyStates)
                    {
                        k.update();
                        if (k.isKeyDown)
                            temp2 = true;
                    }

                    if (temp2)
                    {
                        if (!isKeyDown)
                        {
                            wasPressed = true;
                            isKeyDown = true;
                        }
                        else
                        {
                            wasPressed = false;
                        }
                    }
                    else
                    {
                        if (wasPreviouslyDown)
                            wasReleased = true;
                        else
                            wasReleased = false;
                        isKeyDown = false;
                    }
                    break;
                #endregion
            }
        }

        /// <summary>
        /// Holds values for multiple key types
        /// </summary>
        private class keyState
        {
            public bool isKeyDown = false;
            public bool wasPressed;
            Keys key;

            public keyState(Keys key)
            {
                this.key = key;
            }

            public void update()
            {
                if (Keyboard.GetState().IsKeyDown(key))
                {
                    if (!isKeyDown)
                    {
                        wasPressed = true;
                        isKeyDown = true;
                    }
                    else
                    {
                        wasPressed = false;
                    }
                }
                else
                {
                    isKeyDown = false;
                }
            }
        }
    }

    /// <summary>
    /// Handles visual buttons for GUI's and such
    /// </summary>
    public class VisualButton
    {
        Point orgin;
        Rectangle rect;
        Texture2D texture;
        bool isPressed, wasPressed, wasPreviouslyDown, wasReleased;
        int type;
        public const byte T_NORMAL = 0, T_TOGGLE = 1;
        SpriteFont font;
        string text;
        int stringWidthHalf, stringHeightHalf;
        Color upColor, downColor, color;
        BasicEffect basicEffect;

        public VisualButton(Rectangle rect, Texture2D tex, byte type, Color upColor, Color downColor)
        {
            this.rect = rect;
            this.orgin = new Point(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
            this.texture = tex;
            this.type = type;
            this.upColor = upColor;
            this.downColor = downColor;
        }

        public VisualButton(Point centre, Texture2D tex, byte type, Color upColor, Color downColor, SpriteFont font = null, string text = null, BasicEffect basicEffect = null)
        {
            this.orgin = centre;
            this.texture = tex;
            this.type = type;
            this.upColor = upColor;
            this.downColor = downColor;
            this.font = font;
            this.text = text;
            this.basicEffect = basicEffect;

            if (text != null & font != null)
            {
                stringWidthHalf = (int)font.MeasureString(this.text).X / 2;
                stringHeightHalf = (int)font.MeasureString(this.text).Y / 2;
            }
        }

        /// <summary>
        /// Updates the button and checks for input
        /// </summary>
        public void tick()
        {
            if (font != null)
                rect = new Rectangle(orgin.X - stringWidthHalf - 2, orgin.Y - stringHeightHalf, stringWidthHalf * 2 + 4, stringHeightHalf * 2);
            else
                rect = new Rectangle(orgin.X - rect.Width / 2, orgin.Y - rect.Height / 2, rect.Width, rect.Height);
            MouseState m = Mouse.GetState();

            if (wasPressed)
                wasPressed = false;

            if (m.LeftButton == ButtonState.Pressed)
            {
                if (!isPressed)
                {
                    if (rect.Contains(new Point(m.X, m.Y)))
                    {
                        isPressed = true;
                        wasPressed = true;
                    }
                }
            }
            else
            {
                if (wasPreviouslyDown & rect.Contains(new Point(m.X, m.Y)))
                {
                   wasReleased = true;
                }
                else
                    wasReleased = false;
                isPressed = false;
            }

            if (isPressed & rect.Contains(new Point(m.X, m.Y)))
                color = downColor;
            else
                color = upColor;

            wasPreviouslyDown = isPressed;
        }

        /// <summary>
        /// Renders the button using hte specified SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with</param>
        public void render(SpriteBatch spriteBatch)
        {
            if (font != null)
            {
                spriteBatch.Draw(texture, 
                    new Rectangle(orgin.X - stringWidthHalf - 2, orgin.Y - stringHeightHalf, stringWidthHalf * 2 + 4, stringHeightHalf * 2), color);
                spriteBatch.DrawString(font, text, new Vector2(orgin.X - stringWidthHalf, orgin.Y - stringHeightHalf), extraMath.oppositeColor(color));
                if (basicEffect != null)
                {
                    AdvancedDrawFuncs.DrawRect(basicEffect, rect, Color.Red, Color.Red, false);
                }
            }
            else
                spriteBatch.Draw(texture, rect, color);
        }

        /// <summary>
        /// Sets the texture for the button to use
        /// </summary>
        /// <param name="tex">The texture to use</param>
        public void setTexture(Texture2D tex)
        {
            this.texture = tex;
        }

        /// <summary>
        /// Returns true if the button was just pressed
        /// </summary>
        /// <returns>wasPressed</returns>
        public bool getPressed()
        {
            return wasPressed;
        }

        /// <summary>
        /// Returns true if th button was just released
        /// </summary>
        /// <returns>wasReleased</returns>
        public bool getReleased()
        {
            return wasReleased;
        }

        /// <summary>
        /// Returns true if the button is currently down
        /// </summary>
        /// <returns>isPressed</returns>
        public bool getIsDown()
        {
            return isPressed;
        }

        /// <summary>
        /// Sets the Color of this button
        /// </summary>
        /// <param name="color">The color to draw in</param>
        public void setColor(Color color)
        {
            this.color = color;
            this.upColor = color;
            this.downColor = color;
        }

        /// <summary>
        /// Sets the dimensions of the rectangle. Only used when there
        /// is no text to display
        /// </summary>
        /// <param name="width">The width of the button</param>
        /// <param name="height">The height of the button</param>
        public void setRectDimensions(int width, int height)
        {
            rect.Width = width;
            rect.Height = height;
        }

        /// <summary>
        /// Set the x co-ordinate for the orgin
        /// </summary>
        /// <param name="X">The X co-ordinate to move to</param>
        public void setX(int X)
        {
            orgin.X = X;
        }

        /// <summary>
        /// Set the y co-ordinate for the orgin
        /// </summary>
        /// <param name="Y">The Y co-ordinate to move to</param>
        public void setY(int Y)
        {
            orgin.Y = Y;
        }

        /// <summary>
        /// Set the X and Y co-ordinates of the orgin
        /// </summary>
        /// <param name="X">The X co-ordinate to move to</param>
        /// <param name="Y">The Y co-ordinate to move to</param>
        public void setXY(int X, int Y)
        {
            orgin.X = X;
            orgin.Y = Y;
        }
    }

    /// <summary>
    /// Handles menu sliders
    /// </summary>
    public class Slider
    {
        double value = 0;
        byte type;
        SpriteBatch spriteBatch;
        Rectangle rect;
        Texture2D horizontal, vertical;

        public const byte C_HORIZONTAL = 0, C_VERTICAL = 1;

        /// <summary>
        /// Initializes the slider
        /// </summary>
        /// <param name="value">The initial value of the slider</param>
        /// <param name="spriteBatch">The SpriteBatch to render with</param>
        /// <param name="rect">The rectangle to draw in</param>
        /// <param name="horizontal">The horizontal texture</param>
        /// <param name="vertical">The vertical texture</param>
        public Slider(float value, SpriteBatch spriteBatch, Rectangle rect, Texture2D horizontal, Texture2D vertical, byte type = 0)
        {
            this.value = value;
            this.spriteBatch = spriteBatch;
            this.rect = rect;
            this.horizontal = horizontal;
            this.vertical = vertical;
            this.type = type;
        }

        /// <summary>
        /// Updates the slider
        /// </summary>
        /// <param name="doRender">True is the slider should render</param>
        public void update(bool doRender)
        {
            MouseState m = Mouse.GetState();

            switch (type)
            {
                case 0:
                    if (m.LeftButton == ButtonState.Pressed)
                    {
                        if (m.X > rect.X & m.X < rect.X + rect.Width)
                        {
                            if (m.Y > rect.Y & m.Y < rect.Y + rect.Height)
                            {
                                value = ((m.X - (double)rect.X) / rect.Width);
                            }
                        }
                    }
                    break;
                case 1:
                    if (m.LeftButton == ButtonState.Pressed)
                    {
                        if (m.X > rect.X & m.X < rect.X + rect.Width)
                        {
                            if (m.Y > rect.Y & m.Y < rect.Y + rect.Height)
                            {
                                value = ((m.Y - (double)rect.Y) / (double)rect.Height);
                            }
                        }
                    }
                    break;
        }

            if (doRender)
                render();
        }

        /// <summary>
        /// Renders the slider
        /// </summary>
        public void render()
        {
            switch(type)
            {
                case 0:
                    spriteBatch.Draw(horizontal, new Rectangle(rect.X, rect.Y + rect.Height / 2 - (horizontal.Height / 2), rect.Width, horizontal.Height), Color.White);
                    spriteBatch.Draw(vertical, new Rectangle(rect.X + (int)(rect.Width * value), rect.Y, vertical.Width, rect.Height), Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(horizontal, new Rectangle(rect.X + rect.Width/2, rect.Y, 1, rect.Height), Color.White);
                    spriteBatch.Draw(vertical, new Rectangle(rect.X, rect.Y + (int)(rect.Height * value), rect.Width, 1), Color.White);
                    break;
            }
        }

        /// <summary>
        /// Gets the value in the slider, between 0.0F and 1.0F
        /// </summary>
        /// <returns>Value</returns>
        public float getValue() { return (float)value; }

        /// <summary>
        /// Sets the slider to the specified position
        /// </summary>
        /// <param name="value">Position between 0 and 1 to set to</param>
        public void setValue(float value) 
        { 
            this.value = MathHelper.Clamp(value, 0, 1);  
        }

        /// <summary>
        /// Sets the slider's retangle
        /// </summary>
        /// <param name="rect">The new rectangle to use</param>
        public void setRect(Rectangle rect)
        {
            this.rect = rect;
        }
    }

    /// <summary>
    /// Holds some basic values that can be inherited to allow easier
    /// parenting stuff
    /// </summary>
    public class Instance
    {
        public Vector2 position;
        public double speed, direction, HP, metaData;
    }

    /// <summary>
    /// Handles 2D skeletal animations
    /// </summary>
    namespace SkeletalAnimation
    {
        public class Skeleton : IComparable
        {
            public Vector2 orgin;
            public Limb centralLimb = null;
            List<Limb> limbs = new List<Limb>();
            public string name;

            public Skeleton(Vector2 orgin, string name = "New Skeleton")
            {
                this.name = name;
                this.orgin = orgin;
                this.addCentralLimb(orgin);
            }

            public Limb addCentralLimb(Vector2 position)
            {
                limbs.Add(new Limb(this, null, null));
                return limbs.Last();
            }

            public Limb addLimb(Limb parent, Texture2D tex, double angle = 0, double length = 0)
            {
                limbs.Add(new Limb(this, parent, tex, angle, length));
                return limbs.Last();
            }

            public Limb addLimb(Limb parent, Texture2D tex, Vector2 spriteOrgin, double angle = 0, double length = 0)
            {
                limbs.Add(new Limb(this, spriteOrgin, parent, tex, angle, length));
                return limbs.Last();
            }

            public void buildWaveForLimb(int id)
            {
                Limb limb = limbs[id];

                limb.addTimerNode(0, 1000, 1.2);
                limb.addTimerNode(1.2, 2000, -1.2);
                limb.addTimerNode(-1.2, 1000, 0);
            }

            public void update(GameTime gameTime, Vector2 position)
            {
                orgin = position;

                foreach (Limb limb in limbs)
                    limb.update(gameTime);
            }

            public void draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
            {
                foreach (Limb limb in limbs)
                    limb.draw(spriteBatch, effects);
            }

            public Limb getLastLimb()
            {
                return limbs.Last();
            }

            public Limb getLimb(int ID)
            {
                try
                {
                    return limbs.ElementAt(ID);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return null;
                }
            }

            public List<Limb> getCollection()
            {
                return limbs;
            }

            public int getParentID(Limb parent)
            {
                return limbs.IndexOf(parent);
            }

            public int CompareTo(Object skeleton)
            {
                return name.CompareTo(((Skeleton)skeleton).name);
            }

            public void writeToStream(BinaryWriter binWriter)
            {
                binWriter.Write("Skeleton Save Version 0.0.1");
                binWriter.Write(name);
                binWriter.Write(orgin.X);
                binWriter.Write(orgin.Y);

                binWriter.Write(limbs.Count);
                foreach (Limb limb in limbs)
                {
                    limb.writeToStream(binWriter);
                }
            }

            public static Skeleton readFromStream(GraphicsDevice graphics, BinaryReader binReader)
            {
                binReader.ReadString();
                string name = binReader.ReadString();
                Vector2 orgin = new Vector2(binReader.ReadSingle(), binReader.ReadSingle());

                Skeleton skeleton = new Skeleton(orgin, name);
                skeleton.limbs.RemoveAt(0);

                int rep = binReader.ReadInt32();
                for (int i = 0; i < rep; i++)
                {
                    skeleton.limbs.Add(Limb.readFromStream(skeleton, graphics, binReader));
                }

                return skeleton;
            }

            /// <summary>
            /// A limb as part of the skeleton, can have other limbs jointed to it
            /// </summary>
            public class Limb : Joint
            {
                string name;
                double time = 0;
                int reference = 0;
                Skeleton system;
                Limb parent;
                Texture2D texture;
                List<TimerNodes> timerNodes = new List<TimerNodes>();
                Vector2 spriteOrgin = new Vector2(0, 0);

                /// <summary>
                /// Creates a new limb with the sprite orgin of (0,0)
                /// </summary>
                /// <param name="skeleton">The skeleton that the limb is being added to</param>
                /// <param name="parent">The parent limb for this limb</param>
                /// <param name="texture">The texture for this limb to draw in</param>
                /// <param name="startAngle">The initial angle of the limb in RADIANS</param>
                /// <param name="startLength">The length of the limb</param>
                public Limb(Skeleton skeleton, Limb parent, Texture2D texture, double startAngle = 0, double startLength = 0, string name = "New Limb")
                    : base(Vector2.Zero, startAngle, startLength)
                {
                    this.system = skeleton;
                    this.parent = parent;
                    this.texture = texture;
                    this.angle = startAngle;
                    this.length = startLength;
                    this.spriteOrgin = new Vector2(0, 0);
                    this.name = name;
                }

                /// <summary>
                /// Creates a new limb with an offset sprite orgin
                /// </summary>
                /// <param name="skeleton">The skeleton that this limb is being added to</param>
                /// <param name="spriteOrgin">The orgin of the sprite</param>
                /// <param name="parent">The parent limb for this limb</param>
                /// <param name="texture">The texture of the limb</param>
                /// <param name="startAngle">The initial angle of the limb, in RADIANS</param>
                /// <param name="startLength">The length of the limb</param>
                public Limb(Skeleton skeleton, Vector2 spriteOrgin, Limb parent, Texture2D texture, double startAngle = 0, double startLength = 0, string name = "New Limb")
                    : base(Vector2.Zero, startAngle, startLength)
                {
                    this.system = skeleton;
                    this.parent = parent;
                    this.texture = texture;
                    this.angle = startAngle;
                    this.length = startLength;
                    this.spriteOrgin = spriteOrgin;
                    this.name = name;
                }

                /// <summary>
                /// Adds a new timer node to the end of the animation
                /// </summary>
                /// <param name="startAngle">The angle for the limb to start at</param>
                /// <param name="timer">The number of milliseconds between this 
                /// Keyframe and the next</param>
                public void addTimerNode(double startAngle = 0, double timer = 0, double endAngle = 0)
                {
                    timerNodes.Add(new TimerNodes(startAngle, timer, endAngle));
                }

                /// <summary>
                /// Adds a new timer node to the end of the animation
                /// </summary>
                private void addTimerNode(TimerNodes timerNode)
                {
                    timerNodes.Add(timerNode);
                }

                /// <summary>
                /// Updates this limb and all it's children limbs
                /// </summary>
                /// <param name="gameTime">The GameTime of the game, used for timing<param>
                public void update(GameTime gameTime)
                {
                    if (timerNodes.Count >= 1)
                    {
                        time += gameTime.ElapsedGameTime.Milliseconds;

                        if (time >= timerNodes[reference].timerToNext)
                        {
                            time = 0;
                            reference += 1;
                            if (reference == timerNodes.Count)
                                reference = 0;
                        }

                        angle = (double)MathHelper.Lerp(
                            (float)(timerNodes[reference].startAngle),
                            (float)(timerNodes[reference].endAngle),
                            (float)(extraMath.getPercent(timerNodes[reference].timerToNext, time) / 100D)
                            );
                    }

                    endPoint = extraMath.calculateVector(position, angle, length);

                    if (parent != null)
                    {
                        position = parent.endPoint;
                    }
                    else
                    {
                        position = system.orgin;
                    }
                }

                /// <summary>
                /// Draws this limb and all it's children limbs
                /// </summary>
                /// <param name="spriteBatch">The spritebatch to draw with</param>
                /// <param name="effects">The SpriteEffects to use, defaults to none</param>
                public void draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
                {
                    if (texture != null)
                        spriteBatch.Draw(texture, position, null, Color.White, -(float)(angle), spriteOrgin, new Vector2((float)(length / texture.Width), 1F), SpriteEffects.None, 1F);
                }

                /// <summary>
                /// Writes this Limb to the stream
                /// </summary>
                /// <param name="binWriter">The BinaryWriter to write to</param>
                public void writeToStream(BinaryWriter binWriter)
                {
                    //writes the name
                    binWriter.Write(name);

                    binWriter.Write(spriteOrgin.X);
                    binWriter.Write(spriteOrgin.Y);

                    if (texture != null)
                    {
                        binWriter.Write(true);
                        //save the texture into the stream
                        uint[] cols = extraMath.getTextureData(texture);

                        binWriter.Write((Int32)texture.Width);
                        binWriter.Write((Int32)texture.Height);

                        for (int i = 0; i < texture.Width * texture.Height; i++)
                        {
                            binWriter.Write((uint)cols[i]);
                        }
                    }
                    else
                        binWriter.Write(false);

                    //writes the ID of the parent limb
                    binWriter.Write(system.getParentID((Limb)parent));

                    //write the values
                    binWriter.Write(angle);
                    binWriter.Write(length);


                    //write the number of timerNodes
                    binWriter.Write(timerNodes.Count);

                    //write the timerNodes
                    foreach (TimerNodes t in timerNodes)
                    {
                        t.WriteToStream(binWriter);
                    }
                }

                /// <summary>
                /// Read the limb from the stream
                /// </summary>
                /// <param name="parent">The skeleton to add this limb to</param>
                /// <param name="limb">The limb that this is attached to</param>
                /// <param name="graphics">The GraphicsDevice to create the texture to</param>
                /// <param name="binReader">The BinaryReader to read from</param>
                /// <returns>A Limb loaded from the file</returns>
                public static Limb readFromStream(Skeleton parent, GraphicsDevice graphics, BinaryReader binReader)
                {
                    string name = binReader.ReadString();

                    Texture2D tex = null;

                    Vector2 spriteOrgin = new Vector2(binReader.ReadSingle(), binReader.ReadSingle());

                    //gets the texture if it exists
                    bool texture = binReader.ReadBoolean();
                    if (texture)
                    {
                        int width = binReader.ReadInt32();
                        int height = binReader.ReadInt32();

                        tex = new Texture2D(graphics, width, height);
                        uint[] dat = new uint[height * width];

                        for (int b = 0; b < height * width; b++)
                        {
                            dat[b] = binReader.ReadUInt32();
                        }

                        tex.SetData(dat);
                    }

                    //finds the index of the parent limb
                    int parentID = binReader.ReadInt32();

                    //creates the limb
                    Limb loadLimb = new Limb(parent, spriteOrgin, parent.getLimb(parentID), tex, binReader.ReadDouble(), binReader.ReadDouble(), name);

                    //gets it's timerNodes
                    int i = binReader.ReadInt32();
                    for (int b = 0; b < i; b++)
                    {
                        loadLimb.addTimerNode(TimerNodes.readFromStream(binReader));
                    }

                    return loadLimb;
                }

                /// <summary>
                /// Handles a timer node, which contains a start and end 
                /// angle and a time in milliseconds between them
                /// </summary>
                private class TimerNodes
                {
                    public double timerToNext;
                    public double startAngle, endAngle;

                    /// <summary>
                    /// Create a new timer node
                    /// </summary>
                    /// <param name="timer">The time in milliseconds between the start and end angle</param>
                    /// <param name="startAngle">The angle to start from</param>
                    /// <param name="endAngle">The angle to end at</param>
                    public TimerNodes(double startAngle, double timer, double endAngle)
                    {
                        this.timerToNext = timer;
                        this.startAngle = startAngle;
                        this.endAngle = endAngle;
                    }

                    /// <summary>
                    /// Writes this TimerNode to the binWriter's stream
                    /// </summary>
                    /// <param name="binWriter">The BinaryWriter to write to</param>
                    public void WriteToStream(BinaryWriter binWriter)
                    {
                        binWriter.Write(startAngle);
                        binWriter.Write(timerToNext);
                        binWriter.Write(endAngle);
                    }

                    /// <summary>
                    /// Gets the TimerNode that is written to the stream
                    /// </summary>
                    /// <param name="binReader">The BinaryReader to read from</param>
                    /// <returns>A TimerNode that is loaded from the stream</returns>
                    public static TimerNodes readFromStream(BinaryReader binReader)
                    {
                        return new TimerNodes(binReader.ReadDouble(), binReader.ReadDouble(), binReader.ReadDouble());
                    }
                }
            }

            public class Joint
            {
                public double angle, length;
                public Vector2 position;
                public Vector2 endPoint = Vector2.Zero;

                /// <summary>
                /// Creates a new joint with the specified parameters
                /// </summary>
                /// <param name="position">The joint's position</param>
                /// <param name="angle">The angle of the joint</param>
                /// <param name="length">The length of the joint</param>
                public Joint(Vector2 position, double angle = 0, double length = 0)
                {
                    this.angle = angle;
                    this.length = length;
                    this.position = position;
                    this.endPoint = position;
                }
            }
        }

        public class StickSkeleton
        {
            List<Limb> limbs = new List<Limb>();
            Vector2 position;

            public StickSkeleton()
            {
                limbs.Add(new Limb(null, 0, 0, Color.Transparent));
            }

            public int addLimb(int parentId, float angle, float length, Color color, byte type = 0)
            {
                limbs.Add(new Limb(limbs.ElementAt(parentId), angle, length, color, type));
                return limbs.Count - 1;
            }

            private int addLimb(Limb limb)
            {
                limbs.Add(limb);
                return limbs.Count - 1;
            }

            public void render(BasicEffect effect, GameTime gameTime)
            {
                limbs[0].setPos(position);

                foreach (Limb limb in limbs)
                    limb.render(effect, position, gameTime);
            }

            public void buildWaveForLimb(int index)
            {
                limbs[index].addTimerNode(MathHelper.ToRadians(270), 100, MathHelper.ToRadians(310));
                limbs[index].addTimerNode(MathHelper.ToRadians(310), 100, MathHelper.ToRadians(230));
                limbs[index].addTimerNode(MathHelper.ToRadians(230), 50, MathHelper.ToRadians(270));
            }

            public StickSkeleton getDuplicate()
            {
                StickSkeleton skele = new StickSkeleton();

                skele.limbs = this.limbs;

                for (int i = 1; i < limbs.Count; i++)
                {
                    skele.limbs[i].changeParent(skele.limbs[limbs.IndexOf(limbs[i].parent)]);
                }

                return skele;
            }

            public void setPos(Vector2 pos)
            {
                position = pos;
            }

            private class Limb
            {
                double angle, length;
                double time;
                int reference;
                byte type;
                Vector3 pos, endpos;
                Color color;
                public Limb parent;
                List<VertexPositionColor> vertLines = new List<VertexPositionColor>();
                List<TimerNodes> timerNodes = new List<TimerNodes>();

                public Limb(Limb parent, double angle, double length, Color color, byte type = 0)
                {
                    this.parent = parent;
                    this.angle = angle;
                    this.length = length;
                    this.color = color;
                    this.type = type;

                    switch (type)
                    {
                        case 0:
                            vertLines.Add(new VertexPositionColor(new Vector3(0, 0, 0), color));
                            vertLines.Add(new VertexPositionColor(new Vector3((float)length, 0, 0), color));
                            break;
                        case 1: 
                            double increment = (Math.PI * 2) / 20;
                            double a = 0;
                            for (int i = 0; i <= 20; i++, a += increment)
                            {
                                vertLines.Add(new VertexPositionColor(new Vector3(
                                    extraMath.calculateVector(extraMath.calculateVectorOffset(0, length/2), a, length/2), 0), color));
                            }
                            break;    
                    }
                }

                public void render(BasicEffect effect, Vector2 orgin, GameTime gameTime)
                {
                    if (parent != null)
                        pos = parent.endpos;

                    #region timerNodes
                    if (timerNodes.Count >= 1)
                    {
                        time += gameTime.ElapsedGameTime.Milliseconds;

                        if (time >= timerNodes[reference].timerToNext)
                        {
                            time = 0;
                            reference += 1;
                            if (reference == timerNodes.Count)
                                reference = 0;
                        }

                        angle = (double)MathHelper.Lerp(
                            (float)(timerNodes[reference].startAngle),
                            (float)(timerNodes[reference].endAngle),
                            (float)(extraMath.getPercent(timerNodes[reference].timerToNext, time) / 100D)
                            );
                    }
                    #endregion

                    endpos = new Vector3(extraMath.calculateVector(new Vector2(pos.X, pos.Y), angle, length), 0);

                    Matrix world = effect.World;
                    Matrix transform =  Matrix.CreateRotationZ((float)angle) * Matrix.CreateTranslation(pos);
                    effect.World = transform;
                    effect.CurrentTechnique.Passes[0].Apply();
                    effect.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertLines.ToArray(), 0, vertLines.Count / 2);
                    effect.World = world;
                }

                public void changeParent(Limb newParent)
                {
                    this.parent = newParent;
                }

                public void setPos(Vector2 position)
                {
                    this.pos = new Vector3(position, 0);
                }

                public void addTimerNode(double startAngle, double timer, double endAngle)
                {
                    timerNodes.Add(new TimerNodes(startAngle, timer, endAngle));
                }

                private void addTimerNode(TimerNodes timerNode)
                {
                    timerNodes.Add(timerNode);
                }

                public void saveToStream(BinaryWriter binWriter)
                {
                    binWriter.Write(angle);
                    binWriter.Write(length);
                    binWriter.Write(color.ToVector4().X);
                    binWriter.Write(color.ToVector4().Y);
                    binWriter.Write(color.ToVector4().Z);
                    binWriter.Write(color.ToVector4().W);
                    binWriter.Write(type);

                    binWriter.Write(timerNodes.Count);
                    foreach (TimerNodes timerNode in timerNodes)
                        timerNode.WriteToStream(binWriter);
                }

                public static Limb readFromStream(Limb parent, BinaryReader binReader)
                {
                    Limb ret =  new Limb(parent, binReader.ReadSingle(), binReader.ReadSingle(),
                        new Color(binReader.ReadSingle(), binReader.ReadSingle(), binReader.ReadSingle(), binReader.ReadSingle()),
                        binReader.ReadByte());

                    int tnc = binReader.ReadInt32();
                    for (int i = 0; i < tnc; i++)
                    {
                        ret.addTimerNode(TimerNodes.readFromStream(binReader));
                    }

                    return ret;
                }
                
                /// <summary>
                /// Handles a timer node, which contains a start and end 
                /// angle and a time in milliseconds between them
                /// </summary>
                private class TimerNodes
                {
                    public double timerToNext;
                    public double startAngle, endAngle;

                    /// <summary>
                    /// Create a new timer node
                    /// </summary>
                    /// <param name="timer">The time in milliseconds between the start and end angle</param>
                    /// <param name="startAngle">The angle to start from</param>
                    /// <param name="endAngle">The angle to end at</param>
                    public TimerNodes(double startAngle, double timer, double endAngle)
                    {
                        this.timerToNext = timer;
                        this.startAngle = startAngle;
                        this.endAngle = endAngle;
                    }

                    /// <summary>
                    /// Writes this TimerNode to the binWriter's stream
                    /// </summary>
                    /// <param name="binWriter">The BinaryWriter to write to</param>
                    public void WriteToStream(BinaryWriter binWriter)
                    {
                        binWriter.Write(startAngle);
                        binWriter.Write(timerToNext);
                        binWriter.Write(endAngle);
                    }

                    /// <summary>
                    /// Gets the TimerNode that is written to the stream
                    /// </summary>
                    /// <param name="binReader">The BinaryReader to read from</param>
                    /// <returns>A TimerNode that is loaded from the stream</returns>
                    public static TimerNodes readFromStream(BinaryReader binReader)
                    {
                        return new TimerNodes(binReader.ReadDouble(), binReader.ReadDouble(), binReader.ReadDouble());
                    }
                }
            }
        }
    }

    /// <summary>
    /// A visual prop
    /// </summary>
    public class VisualProp
    {
        public bool canBeDisposed = false;
        Texture2D texture;
        string text = null;
        float stringWidth, stringHeight;
        Color color = Color.White;
        double alphaFade, redFade, blueFade, greenFade, r, g, b, a;
        Keys key;
        SpriteFont font;
        Rectangle rect;

        /// <summary>
        /// Creates a new VisualProp
        /// </summary>
        /// <param name="color">The color to draw with</param>
        /// <param name="text">The text to draw</param>
        /// <param name="texture">The texture to draw</param>
        /// <param name="font">The SpriteFont to draw the text with</param>
        public VisualProp(Color color, Rectangle rect, string text = null, Texture2D texture = null, SpriteFont font = null)
        {
            this.color = color;
            r = color.R;
            g = color.G;
            b = color.B;
            a = color.A;

            this.text = text;
            this.texture = texture;
            this.font = font;
            this.rect = rect;

            if (font != null)
            {
                this.stringWidth = font.MeasureString(text).X;
                this.stringHeight = font.MeasureString(text).Y;
            }
        }

        /// <summary>
        /// Makes this visualprop fade out over the requested time
        /// </summary>
        /// <param name="startAlpha">The initial alpha</param>
        /// <param name="time">The time to fully fade out</param>
        public void setFadeOut(int startAlpha, TimeSpan time)
        {
            a = startAlpha;
            alphaFade = -(startAlpha / time.TotalMilliseconds);
        }
        
        /// <summary>
        /// Makes this visualprop fade out over the requested time
        /// </summary>
        /// <param name="startAlpha">The initial alpha</param>
        /// <param name="time">The time to fully fade out</param>
        public void setColorFade(Color startColor, Color endColor, TimeSpan time)
        {
            color = startColor;
            a = color.A;
            r = color.R;
            g = color.G;
            b = color.B;

            alphaFade = -((a - endColor.A) / time.TotalMilliseconds);
            redFade = -((r - endColor.R) / time.TotalMilliseconds);
            greenFade = -((g - endColor.G) / time.TotalMilliseconds);
            blueFade = -((b - endColor.B) / time.TotalMilliseconds);
        }

        /// <summary>
        /// Makes it so pressing the specified key set's it's disposable state to true
        /// </summary>
        /// <param name="key">The key to dispose on</param>
        public void setRemoveOnKey(Keys key)
        {
            this.key = key;
        }

        /// <summary>
        /// Ticks this VisualProp, and draws it using the spriteBatch
        /// </summary>
        /// <param name="batch">The spritebatch to draw with</param>
        /// <param name="rect">The rectangle to draw in</param>
        public void tick(SpriteBatch batch, GameTime time)
        {
            if (key != Keys.None)
                if (Keyboard.GetState().IsKeyDown(key))
                    canBeDisposed = true;

            r += redFade * time.ElapsedGameTime.TotalMilliseconds;
            g += greenFade * time.ElapsedGameTime.TotalMilliseconds;
            b += blueFade * time.ElapsedGameTime.TotalMilliseconds;
            a += alphaFade * time.ElapsedGameTime.TotalMilliseconds;

            if (a <= 0)
            {
                canBeDisposed = true;
                return;
            }
            color = Color.FromNonPremultiplied((int)r,(int)g,(int)b,(int)a);

            if (texture != null)
                batch.Draw(texture, rect, color);
            if (text != null & font != null)
                batch.DrawString(font, text,
                    new Vector2(rect.X + (int)((rect.Width / 2) - stringWidth / 2), rect.Y + (int)((rect.Height / 2) - stringHeight / 2)), color);
        }

        /// <summary>
        /// Ticks this VisualProp, and draws it using the spriteBatch
        /// </summary>
        /// <param name="batch">The spritebatch to draw with</param>
        /// <param name="rect">The rectangle to draw in</param>
        public void tick(SpriteBatch batch, Vector2 winPos, GameTime time)
        {
            if (key != Keys.None)
                if (Keyboard.GetState().IsKeyDown(key))
                    canBeDisposed = true;

            r += redFade * time.ElapsedGameTime.Seconds;
            g += greenFade * time.ElapsedGameTime.Seconds;
            b += blueFade * time.ElapsedGameTime.Seconds;
            a += alphaFade * time.ElapsedGameTime.Seconds;

            if (a <= 0)
            {
                canBeDisposed = true;
                return;
            }
            color = Color.FromNonPremultiplied((int)r, (int)g, (int)b, (int)a);

            if (texture != null)
                batch.Draw(texture, new Rectangle(rect.X - (int)winPos.X, rect.Y - (int)winPos.Y, rect.Width, rect.Height), color);
            if (text != null & font != null)
                batch.DrawString(font, text,
                    new Vector2(rect.X + (int)((rect.Width / 2) - stringWidth / 2), rect.Y + (int)((rect.Height / 2) - stringHeight / 2)), color);
        }
    }

    public class Timer
    {
        TimeSpan timeRemaining;
        bool active = true;

        public Timer(TimeSpan time)
        {
            timeRemaining = time;
        }

        public bool tick(GameTime gameTime)
        {
            if (active)
            {
                timeRemaining -= gameTime.ElapsedGameTime;
                if (timeRemaining <= TimeSpan.Zero)
                {
                    active = false;
                    return true;
                }
                return false;
            }
            return false;
        }
    }

    /// <summary>
    /// Handles a sprite animated from a tile sheet
    /// </summary>
    public class AnimatedSprite
    {
        Rectangle sourceRect;
        SpriteBatch batch;
        int frameWidth, frameHeight, hFrames, vFrames;
        Texture2D texture;
        public int frameRate;
        public int myFrame, drawFrame;

        /// <summary>
        /// Creates a new animated sprite
        /// </summary>
        /// <param name="batch">The spritebatch used to draw</param>
        /// <param name="texture">The texture sheet for the sprite</param>
        /// <param name="frameWidth">How wide one frame is</param>
        /// <param name="frameHeight">How tall one frame is</param>
        /// <param name="hFrames">The number of horizontal frames</param>
        /// <param name="vFrames">the number of vertical frames</param>
        /// <param name="ticksBtwnFrames"></param>
        public AnimatedSprite(SpriteBatch batch, Texture2D texture, int frameWidth = -1, int frameHeight = -1, int hFrames = 1, int vFrames = 1, int ticksBtwnFrames = 1)
        {
            if (frameWidth == -1)
                frameWidth = texture.Width;
            if (frameHeight == -1)
                frameHeight = texture.Height;
            this.batch = batch;
            this.texture = texture;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.hFrames = hFrames - 1;
            this.vFrames = vFrames - 1;
            this.frameRate = ticksBtwnFrames;

            sourceRect = new Rectangle(0, 0, frameWidth, frameHeight);
        }

        /// <summary>
        /// Ticks the animated sprite and draws it to the rectangle
        /// </summary>
        /// <param name="stateID">The state of the animation</param>
        /// <param name="destinationRect">The rectangle to draw to</param>
        public void tickFrames(Rectangle destinationRect, byte stateID = 0)
        {
            if (frameRate > 0)
            {
                myFrame++;
                if (myFrame >= frameRate)
                {
                    myFrame = 0;
                    drawFrame++;
                    if (drawFrame > hFrames)
                    {
                        drawFrame = 0;
                    }
                }

                sourceRect = new Rectangle(drawFrame * frameWidth, stateID * frameHeight, frameWidth, frameHeight);
            }
            else
                drawFrame = 0;
            batch.Draw(texture, destinationRect, sourceRect, Color.White);
        }

        /// <summary>
        /// Ticks the animated sprite and draws it to the rectangle
        /// </summary>
        /// <param name="stateID">The state of the animation</param>
        /// <param name="destinationRect">The position to draw from</param>
        public void tickFrames(Vector2 position, byte stateID = 0, float scale = 1F)
        {
            if (frameRate > 0)
            {
                myFrame++;
                if (myFrame >= frameRate)
                {
                    myFrame = 0;
                    drawFrame++;
                    if (drawFrame > hFrames)
                    {
                        drawFrame = 0;
                    }
                }

                sourceRect = new Rectangle(drawFrame * frameWidth, stateID * frameHeight, frameWidth, frameHeight);
            }
            else
                drawFrame = 0;
            batch.Draw(texture, position, sourceRect, Color.White, 0F, new Vector2(frameWidth/2, frameHeight), scale, SpriteEffects.None, 0);
        }

        public AnimatedSprite getCopy()
        {
            return new AnimatedSprite(batch, texture, frameWidth, frameHeight, hFrames, vFrames, frameRate);
        }
    }
}
