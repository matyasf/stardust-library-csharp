﻿using System.Xml.Linq;
using Stardust.Emitters;
using Stardust.Xml;

namespace Stardust.Sparrow.Player.Emitters
{
    public static class EmitterBuilder
    {
        private static XmlBuilder _builder;

        public static Emitter BuidEmitter(XElement source, string uniqueEmitterId)
        {
            CreateBuilderIfNeeded();
            _builder.BuildFromXml(source);
            Emitter emitter = (_builder.GetElementByClass(typeof(Emitter)))[0] as Emitter;
            emitter.Name = uniqueEmitterId;
            return emitter;
        }
        
        
        /// <summary>
        /// Returns the builder that is used to parse the XML descriptor.
        /// You can use it to register new custom classes from your XML.
        /// </summary>
        public static XmlBuilder Builder
        {
            get
            {
                CreateBuilderIfNeeded();
                return _builder;
            }
        }

        private static void CreateBuilderIfNeeded()
        {
            if (_builder == null)
            {
                _builder = new XmlBuilder();
                _builder.RegisterClassesFromClassPackage(CommonClassPackage.GetInstance());
                _builder.RegisterClass(typeof(SparrowHandler));
            }
        }
    }
}