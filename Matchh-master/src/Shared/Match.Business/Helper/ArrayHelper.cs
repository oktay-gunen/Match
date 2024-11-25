using Match.Entities.Models.ReportDbModel;

namespace Match.Business.Helper
{
    public static class ArrayHelper
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        //Gönderilen Data içerisinde dönerek array sırasını ay sırasına göre döner
        public static double[] GetDataByMonthArray(this List<FP_GeneralSummary_01> list)
        {
            if (list == null || !list.Any())
            {
                return new double[12];
            }

            var dataArray = new double[12];
            var months = Enumerable.Range(1, 12).ToDictionary(i => i, i => i - 1);

            foreach (var item in list)
            {
                if (months.TryGetValue(item.ReportMonth, out int index))
                {
                    dataArray[index] = item.Price;
                }
            }

            return dataArray;
        }
        public static double[] GetDataByMonthArray(this List<FP_GeneralSummary_03> list)
        {
            if (list == null || !list.Any())
            {
                return new double[12];
            }

            var dataArray = new double[12];
            var months = Enumerable.Range(1, 12).ToDictionary(i => i, i => i - 1);

            foreach (var item in list)
            {
                var mon = Convert.ToInt32(item.Month);
                if (months.TryGetValue(mon, out int index))
                {
                    dataArray[index] = item.Total_TL ?? 0;
                }
            }

            return dataArray;
        }
        public static double[] GetDataByMonthArray(this List<FP_GeneralSummary_04> list)
        {
            if (list == null || !list.Any())
            {
                return new double[12];
            }

            var dataArray = new double[12];
            var months = Enumerable.Range(1, 12).ToDictionary(i => i, i => i - 1);

            foreach (var item in list)
            {
                var mon = Convert.ToInt32(item.Month);
                if (months.TryGetValue(mon, out int index))
                {
                    dataArray[index] = item?.Sales ?? 0;
                }
            }

            return dataArray;
        }
        public static double[] GetDataByMonthArray(this List<FP_GeneralSummary_05> list)
        {
            if (list == null || !list.Any())
            {
                return new double[12];
            }

            var dataArray = new double[12];
            var months = Enumerable.Range(1, 12).ToDictionary(i => i, i => i - 1);

            foreach (var item in list)
            {
                var mon = Convert.ToInt32(item.Month);
                if (months.TryGetValue(mon, out int index))
                {
                    dataArray[index] = item?.Price ?? 0;
                }
            }

            return dataArray;
        }
    }
}