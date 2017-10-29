using System.Xml.Linq;
using Stardust.Xml;

namespace Stardust.Initializers
{
    /// <summary>
    /// An initializer is used to alter just once (i.e. initialize) a particle's properties upon the particle's birth.
    ///
    /// <para>
    /// An initializer can be associated with an emitter or a particle factory.
    /// </para>
    ///
    /// <para>
    /// Default priority = 0;
    /// </para>
    /// </summary>
    public class Initializer : StardustElement
    {
        
        
        
        #region XML

        public override void ParseXml(XElement xml, XmlBuilder builder = null)
        {
            
        }        

        #endregion

    }
}