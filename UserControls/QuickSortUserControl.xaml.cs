using RGZ_SortAlghoritms.Models.Alghoritms;
using RGZ_SortAlghoritms.Models.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RGZ_SortAlghoritms.UserControls
{
    /// <summary>
    /// Логика взаимодействия для QuickSortUserControl.xaml
    /// </summary>
    public partial class QuickSortUserControl : UserControl
    {
        private int[] list;

        public QuickSortUserControl()
        {
            Loaded += QuickSortUserControl_Loaded;
            InitializeComponent();
        }

        private void QuickSortUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            list = new int[15];
        }

        private async void Sort_Click(object sender, RoutedEventArgs e)
        {
            Clear();

            Random random = new Random();
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = random.Next(10, 100);
            }

            CreateArrayUI(list, listStackPanel);
            CreateArrayUI(list, sortedListStackPanel);

            await Sort();
        }

        private async Task Sort()
        {
            SortAlghoritm<int>.OnArrayElementsSwapped += SwapElementsFromBackground;

            await SortAlghoritm<int>.QuickSort(list);

            SortAlghoritm<int>.OnArrayElementsSwapped -= SwapElementsFromBackground;
        }

        private void SwapElementsFromBackground(int index1, int index2)
        {
            // Проверяем, находимся ли мы в UI-потоке
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                // Если нет, переключаемся в UI-поток через Dispatcher
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SwapElements(index1, index2);
                });
            }
            else
            {
                // Если уже в UI-потоке, вызываем напрямую
                SwapElements(index1, index2);
            }
        }

        private async void SwapElements(int index1, int index2)
        {
            if (!(sortedListStackPanel.Children[index1] is TextBlock textBlock1) || !(sortedListStackPanel.Children[index2] is TextBlock textBlock2))
            {
                throw new NotImplementedException();
            }
            ShowArrayUI(list, sortedListStackPanel);
            await SwapElementsUI(sortedListStackPanel, index1, index2);
        }

        private void CreateArrayUI(int[] array, StackPanel stackPanel)
        {
            if (stackPanel.Children.Count > 0) return;
            for (int i = 0; i < array.Length; i++)
            {
                stackPanel.Children.Add(new TextBlock());
            }
            ShowArrayUI(array, stackPanel);
        }

        private void ShowArrayUI(int[] array, StackPanel stackPanel)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (!(stackPanel.Children[i] is TextBlock textBlock1))
                {
                    throw new TypeAccessException("Stack Panel's child is not TextBlock");
                }
                textBlock1.Text = array[i].ToString();
                textBlock1.Margin = new Thickness(0, 0, 10, 0);
                textBlock1.FontSize = 20;
                textBlock1.RenderTransform = new TranslateTransform();
            }
        }

        private async Task SwapElementsUI(StackPanel stackPanel, int index1, int index2)
        {
            if (!(sortedListStackPanel.Children[index1] is TextBlock textBlock1) || !(sortedListStackPanel.Children[index2] is TextBlock textBlock2))
            {
                throw new NotImplementedException();
            }
            AnimateChoosing((TextBlock)stackPanel.Children[index1], 20);
            AnimateChoosing((TextBlock)stackPanel.Children[index2], 40);

            await Task.Delay(500);
            AnimateMovement((TextBlock)stackPanel.Children[index1], index1, index2, 20);
            AnimateMovement((TextBlock)stackPanel.Children[index2], index2, index1, 40);
            await Task.Delay(1000);

            AnimateChoosing((TextBlock)stackPanel.Children[index1], -20);
            AnimateChoosing((TextBlock)stackPanel.Children[index2], -40);
            await Task.Delay(500);
        }

        private async Task AnimateMovement(TextBlock textBlock, int index1, int index2, int height)
        {
            double durationMs = 1000; // Скорость из Slider
            double startX = (textBlock.ActualWidth + 10) * index1;
            double delta = (index2 * (textBlock.ActualWidth + 10)) - startX;
            int steps = 10;

            for (int i = 0; i <= steps; i++)
            {
                ((TranslateTransform)textBlock.RenderTransform).X = (delta * i / steps);
                
                await Task.Delay((int)(durationMs / steps));
            }
        }

        private async Task AnimateChoosing(TextBlock textBlock, int height)
        {
            double durationMs = 500;
            int steps = 10;

            for (int i = 0; i <= steps; i++) 
            {
                if(height < 0)
                    ((TranslateTransform)textBlock.RenderTransform).Y = -height - (-height * i / steps);
                else
                    ((TranslateTransform)textBlock.RenderTransform).Y = height * i / steps;
                await Task.Delay((int)(durationMs / steps));
            }
        }
        


        private void Clear()
        {
            listStackPanel.Children.Clear();
            sortedListStackPanel.Children.Clear();
        }
    }
}
