namespace MathInfection
{
    class Box
    {
        // X, Y is the upper left point of the box
        public int X { get; set; }
        public int Y { get; set; }

        // The X and Y width of the box
        int XWidth { get; set; }
        int YWidth { get; set; }

        // The X and Y velocity of the box
        int VelX { get; set; }
        int VelY { get; set; }

        // Has a collision with this box been detected this tick?
        public bool Colliding { get; set; }

        public Box(int ix, int iy, int xwidth, int ywidth, int ivx, int ivy)
        {
            X = ix;
            Y = iy;
            XWidth = xwidth;
            YWidth = ywidth;
            VelX = ivx;
            VelY = ivy;
        }

        public void update()
        {
            X += VelX;
            Y += VelY;
            bounce_edges();
        }


        public void bounce_edges()
        {
            // Check for left edge collide
            if (X < 0)
            {
                // put the square back out into the visible space
                X = -X;
                // reverse direction
                VelX = -VelX;
            }

            // Check for top edge collide
            if (Y < 0)
            {
                Y = -Y;
                VelY = -VelY;
            }

            // Check for right edge collide
            if (X + XWidth > 900)
            {
                int over;

                over = X + XWidth - 900;
                X = X - (VelX - over);
                VelX = -VelX;
            }

            // Check for bottom edge collide
            if (Y + YWidth > 600)
            {
                VelY = -VelY;
            }
        }

    }
}
