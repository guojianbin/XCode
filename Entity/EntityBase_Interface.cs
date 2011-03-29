﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace XCode
{
    public partial class EntityBase : INotifyPropertyChanging, INotifyPropertyChanged, ICustomTypeDescriptor, IEditableObject
    //, IDataErrorInfo
    {
        #region INotifyPropertyChanged接口
        /// <summary>
        /// 属性改变。重载时记得调用基类的该方法，以设置脏数据属性，否则数据将无法Update到数据库。
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="newValue">新属性值</param>
        /// <returns>是否允许改变</returns>
        protected virtual Boolean OnPropertyChanging(String fieldName, Object newValue)
        {
            if (_PropertyChanging != null) _PropertyChanging(this, new PropertyChangingEventArgs(fieldName));
            Dirtys[fieldName] = true;
            return true;
        }

        /// <summary>
        /// 属性改变。重载时记得调用基类的该方法，以设置脏数据属性，否则数据将无法Update到数据库。
        /// </summary>
        /// <param name="fieldName">字段名</param>
        protected virtual void OnPropertyChanged(String fieldName)
        {
            if (_PropertyChanged != null) _PropertyChanged(this, new PropertyChangedEventArgs(fieldName));
        }

        [field: NonSerialized]
        event PropertyChangingEventHandler _PropertyChanging;
        /// <summary>
        /// 属性将更改
        /// </summary>
        event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
        {
            add { _PropertyChanging += value; }
            remove { _PropertyChanging -= value; }
        }

        [field: NonSerialized]
        event PropertyChangedEventHandler _PropertyChanged;
        /// <summary>
        /// 属性已更改
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { _PropertyChanged += value; }
            remove { _PropertyChanged -= value; }
        }
        #endregion

        #region ICustomTypeDescriptor 成员
        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            AttributeCollection atts = TypeDescriptor.GetAttributes(this, true);

            if (atts != null && !ContainAttribute(atts, typeof(DisplayNameAttribute)))
            {
                List<Attribute> list = new List<Attribute>();
                String description = null;
                foreach (Attribute item in atts)
                {
                    if (item.GetType() == typeof(DescriptionAttribute))
                    {
                        description = (item as DescriptionAttribute).Description;
                        if (!String.IsNullOrEmpty(description)) break;
                    }
                    if (item.GetType() == typeof(DescriptionAttribute))
                    {
                        description = (item as DescriptionAttribute).Description;
                        if (!String.IsNullOrEmpty(description)) break;
                    }
                }

                if (!String.IsNullOrEmpty(description))
                {
                    list.Add(new DisplayNameAttribute(description));
                    atts = new AttributeCollection(list.ToArray());
                }
            }

            return atts;
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            //return TypeDescriptor.GetClassName(this, true);
            return this.GetType().FullName;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(this, attributes, true);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        static Boolean ContainAttribute(AttributeCollection attributes, Type type)
        {
            if (attributes == null || attributes.Count < 1 || type == null) return false;

            foreach (Attribute item in attributes)
            {
                if (type.IsAssignableFrom(item.GetType())) return true;
            }
            return false;
        }
        #endregion

        #region IEditableObject 成员
        void IEditableObject.BeginEdit()
        {
            //throw new NotImplementedException();
        }

        void IEditableObject.CancelEdit()
        {
            //throw new NotImplementedException();
        }

        void IEditableObject.EndEdit()
        {
            //throw new NotImplementedException();
            Update();
        }
        #endregion

        #region IDataErrorInfo 成员
        //string IDataErrorInfo.Error
        //{
        //    get { throw new NotImplementedException(); }
        //}

        //string IDataErrorInfo.this[string columnName]
        //{
        //    get { throw new NotImplementedException(); }
        //}
        #endregion
    }
}