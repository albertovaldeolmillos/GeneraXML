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
    
    public partial class DAY_TYPES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DAY_TYPES()
        {
            this.DAY_EXCEPTIONS = new HashSet<DAY_EXCEPTIONS>();
            this.RATE_BEHAVIOR_STEP = new HashSet<RATE_BEHAVIOR_STEP>();
            this.TARIFFS_APPLICATION_RULES = new HashSet<TARIFFS_APPLICATION_RULES>();
            this.TARIFFS_DEFINITION_RULES = new HashSet<TARIFFS_DEFINITION_RULES>();
            this.TICKETS_TYPES_FEATURES = new HashSet<TICKETS_TYPES_FEATURES>();
            this.UNITS_STATUS = new HashSet<UNITS_STATUS>();
        }
    
        public decimal DAT_ID { get; set; }
        public string DAT_DESCRIPTION { get; set; }
        public string DAT_WEEK_MASK { get; set; }
        public decimal DAT_INS_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DAY_EXCEPTIONS> DAY_EXCEPTIONS { get; set; }
        public virtual INSTALLATIONS INSTALLATIONS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RATE_BEHAVIOR_STEP> RATE_BEHAVIOR_STEP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TARIFFS_APPLICATION_RULES> TARIFFS_APPLICATION_RULES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TARIFFS_DEFINITION_RULES> TARIFFS_DEFINITION_RULES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TICKETS_TYPES_FEATURES> TICKETS_TYPES_FEATURES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNITS_STATUS> UNITS_STATUS { get; set; }
    }
}
