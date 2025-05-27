using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RGZ_SortAlghoritms.Scripts
{
    public static class AnimationManager
    {
        public static async Task AnimateMovementAsync(FrameworkElement frameworkElement, int index1, int index2, double durationMs = 500, int steps = 50)
        {
            double startX = (frameworkElement.ActualWidth + frameworkElement.Margin.Right) * index1;
            double delta = (index2 * (frameworkElement.ActualWidth + frameworkElement.Margin.Right)) - startX;

            for (int i = 0; i <= steps; i++)
            {
                ((TranslateTransform)frameworkElement.RenderTransform).X = (delta * i / steps);

                await Task.Delay((int)(durationMs / steps));
            }
        }

        public static async Task AnimateChoosingAsync(TextBlock textBlock, int height, double durationMs = 50, int steps = 10)
        {
            for (int i = 0; i <= steps; i++)
            {
                ((TranslateTransform)textBlock.RenderTransform).Y = height * i / steps;
                await Task.Delay((int)(durationMs / steps));
            }
        }

        public static async Task AnimateStopChoosingAsync(TextBlock textBlock, int height, double durationMs = 50, int steps = 10)
        {
            for (int i = 0; i <= steps; i++)
            {
                ((TranslateTransform)textBlock.RenderTransform).Y = -height + (height * i / steps);
                await Task.Delay((int)(durationMs / steps));
            }
        }
    }
}
