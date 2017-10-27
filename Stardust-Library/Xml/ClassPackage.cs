using System;
using System.Collections.Generic;

namespace Stardust.Xml
{
    /// <summary>
    /// An <code>XMLBuilder</code> object needs to know the mapping between an XML tag's name and an actual class.
    /// This class encapsulates multiple classes for the <code>XMLBuilder.RegisterClassesFromClassPackage()</code> method
    /// to register multiple classes (i.e. build the mapping relations).
    /// </summary>
    public abstract class ClassPackage
    {
        protected IEnumerable<Type> classes;

        public ClassPackage()
        {
            classes = new List<Type>();
            PopulateClasses();
        }

        /// <summary>
        /// Returns an array of classes. It returns a shallow copy.
        /// </summary>
        public IEnumerable<Type> GetClasses()
        {
            return new List<Type>(classes);
        }

        /// <summary>
        /// Populates classes.
        /// </summary>
        protected abstract void PopulateClasses();
    }
}