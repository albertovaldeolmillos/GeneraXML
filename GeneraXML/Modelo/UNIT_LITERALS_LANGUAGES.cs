//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GeneraXML.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class UNIT_LITERALS_LANGUAGES
    {
        public decimal ULTL_ID { get; set; }
        public decimal ULTL_ULT_ID { get; set; }
        public decimal ULTL_ULK_ID { get; set; }
        public decimal ULTL_LAN_ID { get; set; }
        public string ULTL_LITERAL { get; set; }
    
        public virtual LANGUAGES LANGUAGES { get; set; }
        public virtual UNIT_LITERAL_KEYS UNIT_LITERAL_KEYS { get; set; }
        public virtual UNIT_LITERALS UNIT_LITERALS { get; set; }
    }
}
