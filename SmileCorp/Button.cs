using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace SmileCorp
{
    public delegate void OnButtonClickDelegate();

    class Button
    {
        // Fields
        private Rectangle buttonLocation;
        private Texture2D onButtonImage;
        private Texture2D offButtonImage;
        private MouseState preMState;
        private Color buttonColor;


        // Events
        public event OnButtonClickDelegate OnLeftButtonClick;

        // Properties
        public Color SetButtonColor
        {
            get { return buttonColor; }
            set { buttonColor = value; }
        }

        public Rectangle SetLocation
        {
            get { return buttonLocation; }
            set { buttonLocation = value; }
        }

        // Constructor 
        public Button(GraphicsDevice device, Rectangle position, Texture2D buttonImage, Texture2D OffButtonImage)
        {
            // Assign parameters to fields 
            this.buttonLocation = position;
            this.onButtonImage = buttonImage;
            this.offButtonImage = OffButtonImage;

            buttonColor = Color.White;
        }

        //Update the game based on the clicked button's feature
        public void Update()
        {
            MouseState mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Released && preMState.LeftButton == ButtonState.Pressed && buttonLocation.Contains(mState.Position))
            {
                if (OnLeftButtonClick != null)
                {
                    // Call ALL methods attached to this button
                    OnLeftButtonClick();
                }
            }

            preMState = mState;
        }

        // Draw 
        public void Draw(SpriteBatch spriteBatch)
        {
            MouseState mState = Mouse.GetState();

            // Draw the button
            if (buttonLocation.Contains(mState.Position))
            {
                spriteBatch.Draw(offButtonImage, buttonLocation, Color.DeepSkyBlue);
            }
            else
            {
                spriteBatch.Draw(onButtonImage, buttonLocation, buttonColor);
            }
        }
    }
}
