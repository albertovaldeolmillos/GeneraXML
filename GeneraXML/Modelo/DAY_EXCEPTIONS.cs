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
    
    public partial class DAY_EXCEPTIONS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DAY_EXCEPTIONS()
        {
            this.RATE_BEHAVIOR_STEP = new HashSet<RATE_BEHAVIOR_STEP>();
        }
    
        public decimal DEX_ID { get; set; }
        public System.DateTime DEX_DATE { get; set; }
        public decimal DEX_DAT_ID { get; set; }
        public int DEX_REPEAT_EVERY_YEAR { get; set; }
    
        public virtual DAY_TYPES DAY_TYPES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RATE_BEHAVIOR_STEP> RATE_BEHAVIOR_STEP { get; set; }
    }
}
