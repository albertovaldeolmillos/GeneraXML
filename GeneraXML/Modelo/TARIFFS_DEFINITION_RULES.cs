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
    
    public partial class TARIFFS_DEFINITION_RULES
    {
        public decimal TARDR_ID { get; set; }
        public string TARDR_DESCRIPTON { get; set; }
        public decimal TARDR_TAR_ID { get; set; }
        public Nullable<decimal> TARDR_GRP_ID { get; set; }
        public Nullable<decimal> TARDR_GRPT_ID { get; set; }
        public decimal TARDR_RBS_ID { get; set; }
        public decimal TARDR_TCS_ID { get; set; }
        public System.DateTime TARDR_INI_APPLY_DATE { get; set; }
        public System.DateTime TARDR_END_APPLY_DATE { get; set; }
        public Nullable<decimal> TARDR_DAT_ID { get; set; }
        public Nullable<decimal> TARDR_DAH_ID { get; set; }
        public string TARDR_IN_DAY_INI_HOUR { get; set; }
        public string TARDR_IN_DAY_END_HOUR { get; set; }
    
        public virtual DAY_HOURS_INTERVALS DAY_HOURS_INTERVALS { get; set; }
        public virtual DAY_TYPES DAY_TYPES { get; set; }
        public virtual GROUPS GROUPS { get; set; }
        public virtual GROUPS_TYPES GROUPS_TYPES { get; set; }
        public virtual RATE_BEHAVIOR_SETS RATE_BEHAVIOR_SETS { get; set; }
        public virtual TARIFF_CONSTRAINTS_SETS TARIFF_CONSTRAINTS_SETS { get; set; }
        public virtual TARIFFS TARIFFS { get; set; }
    }
}
