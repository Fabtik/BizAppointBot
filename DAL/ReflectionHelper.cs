namespace DAL
{
    public class ReflectionHelper
    {

        public static void CopyObjectPropertiesWithoutID<T>(T target, T source)
        {
            var sourceProps = typeof(T).GetProperties();

            foreach (var prop in sourceProps)
            {
                if (!string.Equals(prop.Name, "ID", StringComparison.OrdinalIgnoreCase))
                {
                    var value = prop.GetValue(source);
                    prop.SetValue(target, value);
                }
            }

        }
    }
}
