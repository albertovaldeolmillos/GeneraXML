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
    
    public partial class UNITS_PHYSICAL_PARKING_SPACES
    {
        public decimal UNPS_ID { get; set; }
        public decimal UNPS_UNI_ID { get; set; }
        public decimal UNPS_PSP_ID { get; set; }
    
        public virtual PARKING_SPACES PARKING_SPACES { get; set; }
        public virtual UNITS UNITS { get; set; }
    }
}
