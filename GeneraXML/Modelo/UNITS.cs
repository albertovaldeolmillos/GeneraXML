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
    
    public partial class UNITS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UNITS()
        {
            this.UNITS_GROUPS = new HashSet<UNITS_GROUPS>();
            this.UNITS_LOGICAL_PARKING_SPACES = new HashSet<UNITS_LOGICAL_PARKING_SPACES>();
            this.UNITS_PHYSICAL_PARKING_SPACES = new HashSet<UNITS_PHYSICAL_PARKING_SPACES>();
            this.UNITS_SYNC_VERSIONS = new HashSet<UNITS_SYNC_VERSIONS>();
        }
    
        public decimal UNI_ID { get; set; }
        public string UNI_EXTERNAL_ID { get; set; }
        public string UNI_DESCRIPTION { get; set; }
        public string UNI_SW_VERSION { get; set; }
        public string UNI_HW_VERSION { get; set; }
        public string UNI_IP { get; set; }
        public Nullable<System.DateTime> UNI_DATE { get; set; }
        public Nullable<int> UNI_STATUS { get; set; }
        public Nullable<System.DateTime> UNI_STATUS_DATE { get; set; }
        public Nullable<decimal> UNI_USET_ID { get; set; }
        public Nullable<decimal> UNI_ULIT_ID { get; set; }
        public decimal UNI_INS_ID { get; set; }
        public string UNI_CMD_SERVICE { get; set; }
        public Nullable<int> UNI_CMD_STATUS { get; set; }
        public Nullable<System.DateTime> UNI_CMD_STATUS_DATE { get; set; }
        public Nullable<int> UNI_INCOMPLETION_FLAG { get; set; }
    
        public virtual INSTALLATIONS INSTALLATIONS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNITS_GROUPS> UNITS_GROUPS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNITS_LOGICAL_PARKING_SPACES> UNITS_LOGICAL_PARKING_SPACES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNITS_PHYSICAL_PARKING_SPACES> UNITS_PHYSICAL_PARKING_SPACES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UNITS_SYNC_VERSIONS> UNITS_SYNC_VERSIONS { get; set; }
    }
}
