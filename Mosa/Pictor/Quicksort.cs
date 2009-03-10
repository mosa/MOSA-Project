/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (<mailto:rootnode@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Pictor
{
    public class QuickSortAntiAliasedCell
    {
        public QuickSortAntiAliasedCell()
        {
        }

        public void Sort(AntiAliasingCell[] dataToSort)
        {
            Sort(dataToSort, 0, (uint)(dataToSort.Length- 1));
        }

        public void Sort(AntiAliasingCell[] dataToSort, uint beg, uint end)
        {
            if (end == beg)
            {
                return;
            }
            else
            {
                uint pivot = GetPivotPoint(dataToSort, beg, end);
                if (pivot > beg)
                {
                    Sort(dataToSort, beg, pivot - 1);
                }

                if (pivot < end)
                {
                    Sort(dataToSort, pivot + 1, end);
                }
            }
        }

        private uint GetPivotPoint(AntiAliasingCell[] dataToSort, uint begPoint, uint endPoint)
        {
            uint pivot = begPoint;
            uint m = begPoint+1;
            uint n = endPoint;
            while ((m < endPoint)
                && dataToSort[pivot].x >= dataToSort[m].x)
            {
                m++;
            }

            while ((n > begPoint) && (dataToSort[pivot].x <= dataToSort[n].x))
            {
                n--;
            }
            while (m < n)
            {
                AntiAliasingCell temp = dataToSort[m];
                dataToSort[m] = dataToSort[n];
                dataToSort[n] = temp;

                while ((m < endPoint) && (dataToSort[pivot].x >= dataToSort[m].x))
                {
                    m++;
                }

                while ((n > begPoint) && (dataToSort[pivot].x <= dataToSort[n].x))
                {
                    n--;
                }

            }
            if (pivot != n)
            {
                AntiAliasingCell temp2 = dataToSort[n];
                dataToSort[n] = dataToSort[pivot];
                dataToSort[pivot] = temp2;
                
            }
            return n;
        }
    }

    public class QuickSortRangeAdaptorUint
    {
        public QuickSortRangeAdaptorUint()
        {
        }

        public void Sort(VectorPOD_RangeAdaptor dataToSort)
        {
            Sort(dataToSort, 0, (uint)(dataToSort.Size() - 1));
        }

        public void Sort(VectorPOD_RangeAdaptor dataToSort, uint beg, uint end)
        {
            if (end == beg)
            {
                return;
            }
            else
            {
                uint pivot = GetPivotPoint(dataToSort, beg, end);
                if (pivot > beg)
                {
                    Sort(dataToSort, beg, pivot - 1);
                }

                if (pivot < end)
                {
                    Sort(dataToSort, pivot + 1, end);
                }
            }
        }

        private uint GetPivotPoint(VectorPOD_RangeAdaptor dataToSort, uint begPoint, uint endPoint)
        {
            uint pivot = begPoint;
            uint m = begPoint + 1;
            uint n = endPoint;
            while ((m < endPoint)
                && dataToSort[pivot] >= dataToSort[m])
            {
                m++;
            }

            while ((n > begPoint) && (dataToSort[pivot] <= dataToSort[n]))
            {
                n--;
            }
            while (m < n)
            {
                uint temp = dataToSort[m];
                dataToSort[m] = dataToSort[n];
                dataToSort[n] = temp;

                while ((m < endPoint) && (dataToSort[pivot] >= dataToSort[m]))
                {
                    m++;
                }

                while ((n > begPoint) && (dataToSort[pivot] <= dataToSort[n]))
                {
                    n--;
                }

            }
            if (pivot != n)
            {
                uint temp2 = dataToSort[n];
                dataToSort[n] = dataToSort[pivot];
                dataToSort[pivot] = temp2;

            }
            return n;
        }
    }
}
