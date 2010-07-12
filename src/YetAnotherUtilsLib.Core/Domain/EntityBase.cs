using System;

namespace YetAnotherUtilsLib.Core.Domain
{
    public class EntityBase<TKey>
    {
        public virtual TKey Id { get; set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as EntityBase<TKey>;

            if (compareTo == null) return false;
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != compareTo.GetTypeUnproxied() || IsTransient) return false;
            return compareTo.Id.Equals(Id);
        }

        public virtual Type GetTypeUnproxied()
        {
            return GetType();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(EntityBase<TKey> left, EntityBase<TKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EntityBase<TKey> left, EntityBase<TKey> right)
        {
            return !Equals(left, right);
        }

        public virtual bool IsTransient
        {
            get { return Id.Equals(default(TKey)); }
        }
    }
}