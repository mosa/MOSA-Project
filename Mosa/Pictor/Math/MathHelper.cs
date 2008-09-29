namespace Pictor.Math
{
    /// <summary>
    /// 
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Calculates the square distance.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <returns></returns>
        public static double CalculateSquareDistance(double x1, double y1, double x2, double y2)
        {
            double dx = x2 - x1;
            double dy = y2 - y1;

            return dx * dx + dy * dy;
        }
    }
}