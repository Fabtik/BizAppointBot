namespace DAL
{
    public class ReflectionHelper
    {

        public static void CopyObjectPropertiesWithoutSpecified<T>(T target, T source, params string[] propertiesToSkip)
        {
            var sourceProps = typeof(T).GetProperties();

            foreach (var prop in sourceProps)
            {
                if (!propertiesToSkip.Contains(prop.Name, StringComparer.OrdinalIgnoreCase))
                {
                    var value = prop.GetValue(source);
                    prop.SetValue(target, value);
                }
            }
        }
    }
}
