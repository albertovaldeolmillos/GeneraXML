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
    
    public partial class GROUPS_HIERARCHY
    {
        public decimal GRHI_ID { get; set; }
        public Nullable<decimal> GRHI_GRP_ID_PARENT { get; set; }
        public decimal GRHI_GRP_ID { get; set; }
        public System.DateTime GRHI_INI_APPLY_DATE { get; set; }
        public System.DateTime GRHI_END_APPLY_DATE { get; set; }
    
        public virtual GROUPS GROUPS { get; set; }
        public virtual GROUPS GROUPS1 { get; set; }
    }
}
