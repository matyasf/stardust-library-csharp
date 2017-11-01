﻿using System.Xml.Linq;
using Stardust.Xml;

namespace Stardust
{
    public abstract class SortableElement : StardustElement
    {
        public delegate void ElementHandler(SortableElement element);
        public event ElementHandler PriorityChange;
        public static event ElementHandler Added;
        public static event ElementHandler Removed;
        
        protected int _priority;
        
        /// <summary>
        /// Denotes if its is active, true by default.
        /// </summary>
        public bool Active;

        public SortableElement()
        {
            _priority = 0;
            Active = true;
        }
        
        /// <summary>
        /// These will be sorted according to their priorities.
        ///
        /// <para>
        /// This is important,
        /// since some elements may rely on others to perform actions beforehand.
        /// You can alter the priority of an element, but it is recommended that you use the default values.
        /// </para>
        /// </summary>
        public int Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                PriorityChange?.Invoke(this);
            }
        }
        
        public void DispatchAddEvent()
        {
            Added?.Invoke(this);
        }
        
        public void DispatchRemoveEvent()
        {
            Removed?.Invoke(this);
        }
        

        #region XML

        public override XElement ToXml()
        {
            XElement xml = base.ToXml();
            xml.SetAttributeValue("active", Active);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            Active = bool.Parse(xml.Attribute("active").Value);
        }

        #endregion
    }
}