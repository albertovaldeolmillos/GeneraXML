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
    
    public partial class GROUPS_TYPES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GROUPS_TYPES()
        {
            this.GROUPS_TYPES_ASSIGNATIONS = new HashSet<GROUPS_TYPES_ASSIGNATIONS>();
        }
    
        public decimal GRPT_ID { get; set; }
        public decimal GRPT_INS_ID { get; set; }
        public string GRPT_DESCRIPTION { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GROUPS_TYPES_ASSIGNATIONS> GROUPS_TYPES_ASSIGNATIONS { get; set; }
        public virtual INSTALLATIONS INSTALLATIONS { get; set; }
    }
}