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
    
    public partial class UNIT_SETTINGS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UNIT_SETTINGS()
        {
            this.UNIT_SETTINGS_DETAILS = new HashSet<UNIT_SETTINGS_DETAILS>();
            this.UNIT_SETTINGS1 = new HashSet<UNIT_SETTINGS>();
            this.UNITS = new HashSet<UNITS>();
        }
    
        public decimal UST_ID { get; set; }
        public string UST_NAME { get; set; }
        public Nullable<decimal> UST_PARENT_USET_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNIT_SETTINGS_DETAILS> UNIT_SETTINGS_DETAILS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNIT_SETTINGS> UNIT_SETTINGS1 { get; set; }
        public virtual UNIT_SETTINGS UNIT_SETTINGS2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNITS> UNITS { get; set; }
    }
}