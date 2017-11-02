using System.Xml.Linq;
using Stardust.Xml;

namespace Stardust.Zones
{
    /// <summary>
    /// Zone with no thickness
    /// </summary>
    public abstract class Contour : Zone
    {

        protected float VirtualThicknessVal;

        public Contour()
        {
            VirtualThicknessVal = 1;
        }
        
        /// <summary>
        /// Used to calculate "virtual area" for the <code>CompositeZone</code> class,
        /// since contours have zero thickness.
        /// The larger the virtual thickness, the larger the virtual area.
        /// </summary>
        public float VirtualThickness
        {
            get => VirtualThicknessVal;
            set
            {
                VirtualThicknessVal = value;
                UpdateArea();
            }
        }

        #region XML

        public override XElement ToXml()
        {
            var xml = base.ToXml();
            xml.SetAttributeValue("virtualThickness", VirtualThickness);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);
            VirtualThickness = float.Parse(xml.Attribute("virtualThickness").Value);
        }

        #endregion
    }
}