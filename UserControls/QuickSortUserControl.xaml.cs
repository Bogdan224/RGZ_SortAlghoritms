using RGZ_SortAlghoritms.Models.Alghoritms;
using RGZ_SortAlghoritms.Models.Converters;
using RGZ_SortAlghoritms.Scripts;
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
        private int[] sortedList;
        private int[] list;

        private Func<Task> sort;

        public QuickSortUserControl()
        {
            Loaded += QuickSortUserControl_Loaded;
            Unloaded += QuickSortUserControl_Unloaded;
            InitializeComponent();
        }

        private void QuickSortUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            SortAlghoritm<int>.OnArrayElementsSwapped -= SwapElementsFromBackgroundAsync;
        }

        private void QuickSortUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            sortedList = new int[15];
            list = new int[15];
            quickSortDescriptionTextBlock.Text = "Алгоритм сначала выбирает опорный элемент (pivot). Затем делит массив на два подмассива: " +
                "в первом элементы меньше или равны pivot, а во втором — больше. После этого каждый подмассив сортируется независимо. " +
                "Затем процесс рекурсивно повторяется для обоих подмассивов, что позволяет эффективно упорядочить элементы.";
            SortAlghoritm<int>.OnArrayElementsSwapped += SwapElementsFromBackgroundAsync;
            sort = () => SortAlghoritm<int>.QuickSort(sortedList);

        }

        private async void Sort_Click(object sender, RoutedEventArgs e)
        {
            arrayButton.IsEnabled = false;
            alghoritmButton.IsEnabled = false;

            await sort?.Invoke();

            arrayButton.IsEnabled = true;
            alghoritmButton.IsEnabled = true;
        }


        private void CreateArray_Click(object sender, RoutedEventArgs e)
        {
            Clear();

            Random random = new Random();
            for (int i = 0; i < sortedList.Length; i++)
            {
                int rand = random.Next(10, 100);
                list[i] = rand;
                sortedList[i] = rand;
            }

            CreateArrayUI(sortedList, sortedListStackPanel);
        }

        private async Task SwapElementsFromBackgroundAsync(int index1, int index2)
        {
            // Проверяем, находимся ли мы в UI-потоке
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                // Если нет, переключаемся в UI-поток через Dispatcher
                await Application.Current.Dispatcher.Invoke(async () =>
                {
                    await SwapElementsAsync(index1, index2);
                });
            }
            else
            {
                // Если уже в UI-потоке, вызываем напрямую
                await SwapElementsAsync(index1, index2);
            }
        }

        private async Task SwapElementsAsync(int index1, int index2)
        {
            if (!(sortedListStackPanel.Children[index1] is TextBlock) || !(sortedListStackPanel.Children[index2] is TextBlock))
            {
                throw new NotImplementedException();
            }
            ShowArrayUI(sortedList, sortedListStackPanel);
            await SwapElementsUIAsync(sortedListStackPanel, index1, index2, 20);
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

        public static async Task SwapElementsUIAsync(Panel panel, int index1, int index2, int height)
        {
            if (!(panel.Children[index1] is TextBlock) || !(panel.Children[index2] is TextBlock))
            {
                throw new NotImplementedException();
            }

            //Анимация опускания
            AnimationManager.AnimateChoosingAsync((TextBlock)panel.Children[index1], height);
            await AnimationManager.AnimateChoosingAsync((TextBlock)panel.Children[index2], -height);

            //Анимация перемещения
            AnimationManager.AnimateMovementAsync((TextBlock)panel.Children[index1], index1, index2);
            await AnimationManager.AnimateMovementAsync((TextBlock)panel.Children[index2], index2, index1);

            //Анимация поднятия
            AnimationManager.AnimateStopChoosingAsync((TextBlock)panel.Children[index1], -height);
            await AnimationManager.AnimateStopChoosingAsync((TextBlock)panel.Children[index2], height);
        }



        private void Clear()
        {
            sortedListStackPanel.Children.Clear();
        }
    }
}
