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
    
    public partial class TARIFF_CONSTRAINTS_SETS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TARIFF_CONSTRAINTS_SETS()
        {
            this.TARIFF_CONSTRAINT_ENTRIES = new HashSet<TARIFF_CONSTRAINT_ENTRIES>();
            this.TARIFFS_APPLICATION_RULES = new HashSet<TARIFFS_APPLICATION_RULES>();
            this.TARIFFS_DEFINITION_RULES = new HashSet<TARIFFS_DEFINITION_RULES>();
        }
    
        public decimal TCS_ID { get; set; }
        public string TCS_DESCRIPTION { get; set; }
        public decimal TCS_INS_ID { get; set; }
    
        public virtual INSTALLATIONS INSTALLATIONS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TARIFF_CONSTRAINT_ENTRIES> TARIFF_CONSTRAINT_ENTRIES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TARIFFS_APPLICATION_RULES> TARIFFS_APPLICATION_RULES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TARIFFS_DEFINITION_RULES> TARIFFS_DEFINITION_RULES { get; set; }
    }
}
