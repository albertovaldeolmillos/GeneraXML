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
    
    public partial class UNITS_SETTINGS_DETAILS_EXCEPTIONS
    {
        public decimal UNIS_ID { get; set; }
        public decimal UNIS_UNI_ID { get; set; }
        public decimal UNIS_USP_ID { get; set; }
        public string UNIS_PARAM_VALUE { get; set; }
    
        public virtual UNIT_SETTING_PARAMETERS UNIT_SETTING_PARAMETERS { get; set; }
        public virtual UNITS UNITS { get; set; }
    }
}