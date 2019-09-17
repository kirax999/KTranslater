namespace KTranslate {
    public class KTranslateUtils
    {
        public static string ClearString(string item) {
            if (item.Length > 0 && item[0] == '"') {
                item = item.Remove(0, 1);
                item = item.Remove(item.Length - 1, 1);
            }

            return item;
        }
    }
}