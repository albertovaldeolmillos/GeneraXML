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
    
    public partial class UNITS_STATUS
    {
        public decimal UNST_ID { get; set; }
        public string UNST_DESCRIPTON { get; set; }
        public Nullable<decimal> UNST_UNI_ID { get; set; }
        public Nullable<decimal> UNST_GRP_ID { get; set; }
        public Nullable<decimal> UNST_GRPT_ID { get; set; }
        public System.DateTime UNST_INI_APPLY_DATE { get; set; }
        public System.DateTime UNST_END_APPLY_DATE { get; set; }
        public Nullable<decimal> UNST_DAT_ID { get; set; }
        public Nullable<decimal> UNST_DAH_ID { get; set; }
        public string UNST_IN_DAY_INI_HOUR { get; set; }
        public string UNST_IN_DAY_END_HOUR { get; set; }
        public Nullable<System.DateTime> UNST_IN_YEAR_INI_DATE { get; set; }
        public Nullable<System.DateTime> UNST_IN_YEAR_END_DATE { get; set; }
        public int UNST_STATUS { get; set; }
    
        public virtual DAY_HOURS_INTERVALS DAY_HOURS_INTERVALS { get; set; }
        public virtual DAY_TYPES DAY_TYPES { get; set; }
        public virtual GROUPS GROUPS { get; set; }
        public virtual GROUPS_TYPES GROUPS_TYPES { get; set; }
        public virtual UNITS UNITS { get; set; }
    }
}
