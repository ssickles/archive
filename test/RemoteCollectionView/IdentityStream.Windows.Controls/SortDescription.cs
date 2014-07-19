using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace IdentityStream.Windows.Controls
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SortDescription
    {
        private string _propertyName;
        private ListSortDirection _direction;
        private bool _sealed;
        public SortDescription(string propertyName, ListSortDirection direction)
        {
            if ((direction != ListSortDirection.Ascending) && (direction != ListSortDirection.Descending))
            {
                throw new InvalidEnumArgumentException("direction", (int)direction, typeof(ListSortDirection));
            }
            this._propertyName = propertyName;
            this._direction = direction;
            this._sealed = false;
        }

        public string PropertyName
        {
            get
            {
                return this._propertyName;
            }
            set
            {
                if (this._sealed)
                {
                    throw new InvalidOperationException("The SortDescription can not be changed after it has been sealed.");
                }
                this._propertyName = value;
            }
        }
        public ListSortDirection Direction
        {
            get
            {
                return this._direction;
            }
            set
            {
                if (this._sealed)
                {
                    throw new InvalidOperationException("The SortDescription can not be changed after it has been sealed.");
                }
                if ((value < ListSortDirection.Ascending) || (value > ListSortDirection.Descending))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ListSortDirection));
                }
                this._direction = value;
            }
        }
        public bool IsSealed
        {
            get
            {
                return this._sealed;
            }
        }
        public override bool Equals(object obj)
        {
            return ((obj is SortDescription) && (this == ((SortDescription)obj)));
        }

        public static bool operator ==(SortDescription sd1, SortDescription sd2)
        {
            return ((sd1.PropertyName == sd2.PropertyName) && (sd1.Direction == sd2.Direction));
        }

        public static bool operator !=(SortDescription sd1, SortDescription sd2)
        {
            return !(sd1 == sd2);
        }

        public override int GetHashCode()
        {
            int hashCode = this.Direction.GetHashCode();
            if (this.PropertyName != null)
            {
                hashCode = this.PropertyName.GetHashCode() + hashCode;
            }
            return hashCode;
        }

        internal void Seal()
        {
            this._sealed = true;
        }
    }
}
