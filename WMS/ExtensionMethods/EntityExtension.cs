using Microsoft.EntityFrameworkCore;

namespace WMS.ExtensionMethods
{
    public static class EntityExtension
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
