using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGZ_SortAlghoritms.Models.Alghoritms
{
    public static class SortAlghoritm<T> where T : IComparable
    {
        public static event Func<int, int, Task> OnArrayElementsSwapped;

        private static void Swap(ref T value1, ref T value2)
        {
            (value1, value2) = (value2, value1);
        }

        private static async Task SwapWithDelay(T[] array, int index1, int index2)
        {
            if (index1 == index2) return;

            // Уведомляем UI о изменении
            await OnArrayElementsSwapped?.Invoke(index1, index2);

            // Выполняем перестановку
            (array[index1], array[index2]) = (array[index2], array[index1]);
        }


        private static async Task<T[]> QuickSortInternal(T[] array, int leftIndex, int rightIndex)
        {
            var i = leftIndex;
            var j = rightIndex;
            var pivot = array[leftIndex];
            while (i <= j)
            {
                while (array[i].CompareTo(pivot) < 0)
                {
                    i++;
                }

                while (array[j].CompareTo(pivot) > 0)
                {
                    j--;
                }
                if (i <= j)
                {
                    await SwapWithDelay(array, i, j);
                    i++;
                    j--;
                }
            }

            if (leftIndex < j)
                await QuickSortInternal(array, leftIndex, j);
            if (i < rightIndex)
                await QuickSortInternal(array, i, rightIndex);
            return array;
        }

        public static void BubbleSort(T[] list)
        {
            if (list == null || list.Length < 2) return;
            int n = list.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (list[j].CompareTo(list[j + 1]) > 0)
                    {
                        Swap(ref list[j], ref list[j + 1]);

                    }
                }
            }
        }

        public static async Task QuickSort(T[] array)
        {
            await QuickSortInternal(array, 0, array.Length - 1);
        }

        

        public static void InsertionSort(T[] list)
        {
            for (int i = 1; i < list.Length; i++)
            {
                T current = list[i];
                int j = i - 1;

                // Сдвигаем элементы больше выбранного вправо
                while (j >= 0 && list[j].CompareTo(current) > 0)
                {
                    Swap(ref list[j + 1], ref list[j]);
                    j--;
                }

                // Вставляем выбранный элемент в правильную позицию
                Swap(ref list[j + 1], ref current);
            }
        }

        

    }
}
