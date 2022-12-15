using System.ComponentModel.DataAnnotations.Schema;

namespace Blogs.CMS.Commons
{
    public static class Helpers
    {
        public static string[] auditFields = { "Id", "CreatedDate", "UpdatedDate", "CreatedBy", "UpdatedBy" };

        public static T MergeFieldsChanged<T>(T source, T destination)
        {
            var properties = typeof(T).GetProperties().Where(_ => !auditFields.Contains(_.Name));
            // only update not null
            foreach (var property in properties)
            {
                try
                {
                    var newValue = property.GetValue(source, null);
                    var oldValue = property.GetValue(destination, null);
                    //hard bypass foreign key
                    if (!IsChangedValue(oldValue!, newValue!) || Attribute.IsDefined(property, typeof(NotMappedAttribute)))
                        continue;

                    property.SetValue(destination, newValue, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return destination;
        }
        private static bool IsChangedValue(object oldValue, object newValue)
        {
            if (newValue == null
                || newValue == oldValue)
                return false;
            return true;
        }
    }
}
