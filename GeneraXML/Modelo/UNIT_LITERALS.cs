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
    
    public partial class UNIT_LITERALS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UNIT_LITERALS()
        {
            this.UNIT_LITERALS_LANGUAGES = new HashSet<UNIT_LITERALS_LANGUAGES>();
            this.UNIT_LITERALS1 = new HashSet<UNIT_LITERALS>();
            this.UNITS = new HashSet<UNITS>();
        }
    
        public decimal ULT_ID { get; set; }
        public string ULT_NAME { get; set; }
        public Nullable<decimal> ULT_PARENT_ULT_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNIT_LITERALS_LANGUAGES> UNIT_LITERALS_LANGUAGES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNIT_LITERALS> UNIT_LITERALS1 { get; set; }
        public virtual UNIT_LITERALS UNIT_LITERALS2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNITS> UNITS { get; set; }
    }
}
