using System.Linq;

namespace Tenjin.Extensions
{
    public static class ObjectExtensions
    {
        public static bool DoesNotEqual<TObject>(this TObject? left, TObject? right)
        {
            return !Equals(left, right);
        }

        public static bool EqualsAll(this object? root, params object?[] objects)
        {
            return objects.All(obj => Equals(root, obj));
        }

        public static bool EqualsAny(this object? root, params object?[] objects)
        {
            return objects.Any(obj => Equals(root, obj));
        }

        public static bool DoesNotEqualAll(this object? root, params object?[] objects)
        {
            return objects.All(obj => !Equals(root, obj));
        }

        public static bool DoesNotEqualAny(this object? root, params object?[] objects)
        {
            return objects.Any(obj => !Equals(root, obj));
        }
    }
}
