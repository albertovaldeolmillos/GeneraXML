using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using GeneraXML.Modelo;

namespace ConsoleGeneraXML
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new BCTOTA2PAEntities())
            {
                var metadata = ((IObjectContextAdapter)dbContext).ObjectContext.MetadataWorkspace;

                var tables = metadata.GetItemCollection(DataSpace.SSpace)
                    .GetItems<EntityContainer>()
                    .Single()
                    .BaseEntitySets
                    .OfType<EntitySet>()
                    .Where(s => !s.MetadataProperties.Contains("Type")
                    || s.MetadataProperties["Type"].ToString() == "Tables");

                List<string> tablas = new List<string>();
                foreach (var table in tables)
                {
                    var tableName = table.MetadataProperties.Contains("Table")
                        && table.MetadataProperties["Table"].Value != null
                        ? table.MetadataProperties["Table"].Value.ToString()
                        : table.Name;

                    var tableSchema = table.MetadataProperties["Schema"].Value.ToString();

                    IEnumerable<ReferentialConstraint> fksTotales = table.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints);

                    IEnumerable<ReferentialConstraint> fks = fksTotales.Where(z => z.ToRole.Name == tableName);

                    IEnumerable<ReferentialConstraint> Inversefks = fksTotales.Where(z => z.FromRole.Name == tableName);


                    ReferentialConstraint fk = fks.Where(z => z.ToRole.Name == tableName).FirstOrDefault();

                    string fkId = (fk == null) ? "" : fk.ToProperties.FirstOrDefault().Name;


                    //Console.WriteLine(tableSchema + "." + tableName);

                    tablas.Add(tableName);
                }



                XElement xml = new XElement("XmlDatabase",
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"));

                XElement xmlEntitiesArray = new XElement("EntitiesArray");
                foreach (var t in tables)
                {
                    XElement xmlListsEntitiesDictionary = new XElement("ListsEntitiesDictionary");

                    XElement xmlEntityKey = new XElement("Key", convertString(t.Name));
                    XElement xmlEntityValue = new XElement("Value");

                    if (t.Name == "COUNTRIES") countriesXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "CURRENCIES") currenciesXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "GROUPS") groupsXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "GROUPS_HIERARCHY") groupsHierarchyXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "GROUPS_TYPES") groupsTypesXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "GROUPS_TYPES_ASSIGNATIONS") groupsTypesAssignationsXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "INSTALLATIONS") installationsXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "PARKING_SPACES") parkingSpacesXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "UNIT_ALARM_TYPES") unitAlarmTypesXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "UNITS") unitsXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "UNITS_GROUPS") unitsGroupsXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "UNITS_LOGICAL_PARKING_SPACES") unitsLogicalParkingSpacesXML(t, dbContext, xmlEntityValue);
                    if (t.Name == "UNITS_PHYSICAL_PARKING_SPACES") unitsPhysicalParkingSpacesXML(t, dbContext, xmlEntityValue);


                    xmlListsEntitiesDictionary.Add(xmlEntityKey);
                    xmlListsEntitiesDictionary.Add(xmlEntityValue);

                    xmlEntitiesArray.Add(xmlListsEntitiesDictionary);
                }

                xml.Add(xmlEntitiesArray);



                /*XElement xml = new XElement("XmlDatabase",
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"), 
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XElement("EntitiesArray",
                        from t in tables select
                            new XElement("ListsEntitiesDictionary",
                                new XElement("Key", t.Name),
                                new XElement("Value",
                                    (t.Name != "COUNTRIES") ? null : from cou in dbContext.COUNTRIES.ToList() select
                                    new XElement("BaseEntity",
                                        new XElement("FieldsPK",
                                            from keymember in t.ElementType.KeyMembers select
                                                new XElement("string", keymember.Name)
                                        ),
                                        (t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name).Count() == 0) ? null : new XElement("FKs",
                                        from fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name) select
                                            new XElement("FKInfo",
                                                new XElement("Field", fks.ToProperties.FirstOrDefault().Name),
                                                new XElement("Id", "")
                                            )
                                        ),
                                        //(t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name).Count() == 0) ? null : new XElement("InverseFKs",
                                        //from fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name) select                     
                                        //    new XElement("InverseFKInfo",
                                        //        new XElement("Field", fks.ToRole.Name),
                                        //        new XElement("IdsString", "")
                                        //    )
                                        //),
                                        from member in t.ElementType.Members select
                                            new XElement(member.Name,
                                                ((t.Name == "COUNTRIES") && (member.Name == "COU_ID")) ? cou.COU_ID + "" : "",
                                                ((t.Name == "COUNTRIES") && (member.Name == "COU_CUR_ID")) ? cou.COU_CUR_ID + "" : "",
                                                ((t.Name == "COUNTRIES") && (member.Name == "COU_DESCRIPTION")) ? cou.COU_DESCRIPTION + "" : "",
                                                ((t.Name == "COUNTRIES") && (member.Name == "COU_CODE")) ? cou.COU_CODE + "" : "",
                                                ((t.Name == "COUNTRIES") && (member.Name == "COU_TEL_PREFIX")) ? cou.COU_TEL_PREFIX + "" : ""
                                            )
                                        )
                                )
                            ) 
                    )
                );*/

                xml.Save("PruebaInfraestructure.xml");
            }

        }

        public static string convertString(string str)
        {
            string nuevoStr = "";
            var arrStr = str.Split('_');
            foreach(string item in arrStr)
            {
                string modif = item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower();
                nuevoStr = nuevoStr + modif;
            }
            return nuevoStr;
        }

        public static void countriesXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "COUNTRIES") foreach (var cou in dbContext.COUNTRIES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1,atr2);
                    atr1.Remove();

                    //xmlBaseEntity.SetAttributeValue(xsytype + "type", convertString(t.Name));

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "COUNTRIES") && (fks.ToProperties.FirstOrDefault().Name == "COU_CUR_ID")) ? cou.COU_CUR_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "COUNTRIES") && (inversefks.ToRole.Name == "INSTALLATIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "INS_COU_ID")) ? string.Join(",", cou.INSTALLATIONS.Select(x => x.INS_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if (member.Name != "COU_CUR_ID")
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "COUNTRIES") && (member.Name == "COU_ID")) ? cou.COU_ID + "" : "",
                                                //((t.Name == "COUNTRIES") && (member.Name == "COU_CUR_ID")) ? cou.COU_CUR_ID + "" : "",
                                                ((t.Name == "COUNTRIES") && (member.Name == "COU_DESCRIPTION")) ? cou.COU_DESCRIPTION + "" : "",
                                                ((t.Name == "COUNTRIES") && (member.Name == "COU_CODE")) ? cou.COU_CODE + "" : "",
                                                ((t.Name == "COUNTRIES") && (member.Name == "COU_TEL_PREFIX")) ? cou.COU_TEL_PREFIX + "" : ""
                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void currenciesXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "CURRENCIES") foreach (var curr in dbContext.CURRENCIES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id", "");
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "CURRENCIES") && (inversefks.ToRole.Name == "INSTALLATIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "INS_COU_ID")) ? string.Join(",", curr.INSTALLATIONS.Select(x => x.INS_ID).ToList()) + "" : "",
                            ((t.Name == "CURRENCIES") && (inversefks.ToRole.Name == "COUNTRIES") && (inversefks.ToProperties.FirstOrDefault().Name == "COU_CUR_ID")) ? string.Join(",", curr.COUNTRIES.Select(x => x.COU_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        XElement xmlMember = new XElement(convertString(member.Name),
                                            ((t.Name == "CURRENCIES") && (member.Name == "CUR_FACT")) ? curr.CUR_FACT + "" : "",
                                            ((t.Name == "CURRENCIES") && (member.Name == "CUR_ID")) ? curr.CUR_ID + "" : "",
                                            ((t.Name == "CURRENCIES") && (member.Name == "CUR_ISO_CODE")) ? curr.CUR_ISO_CODE + "" : "",
                                            ((t.Name == "CURRENCIES") && (member.Name == "CUR_ISO_CODE_NUM")) ? curr.CUR_ISO_CODE_NUM + "" : "",
                                            ((t.Name == "CURRENCIES") && (member.Name == "CUR_MASK")) ? curr.CUR_MASK + "" : "",
                                            ((t.Name == "CURRENCIES") && (member.Name == "CUR_NAME")) ? curr.CUR_NAME + "" : "",
                                            ((t.Name == "CURRENCIES") && (member.Name == "CUR_SYMBOL")) ? curr.CUR_SYMBOL + "" : ""
                            );
                        xmlBaseEntity.Add(xmlMember);
                        if (xmlMember.Value == "")
                        {
                            xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void groupsXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "GROUPS") foreach (var gro in dbContext.GROUPS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "GROUPS") && (fks.ToProperties.FirstOrDefault().Name == "GRP_INS_ID")) ? gro.GRP_INS_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "GROUPS1") && (inversefks.ToProperties.FirstOrDefault().Name == "GRP_LOG_GRP_ID")) ? string.Join(",", gro.GROUPS1.Select(x => x.GRP_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "GROUPS_HIERARCHY") && (inversefks.ToProperties.FirstOrDefault().Name == "GRHI_GRP_ID")) ? string.Join(",", gro.GROUPS_HIERARCHY.Select(x => x.GRHI_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "GROUPS_HIERARCHY") && (inversefks.ToProperties.FirstOrDefault().Name == "GRHI_GRP_ID_PARENT")) ? string.Join(",", gro.GROUPS_HIERARCHY1.Select(x => x.GRHI_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "GROUPS_TYPES_ASSIGNATIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "GTA_GRP_ID")) ? string.Join(",", gro.GROUPS_TYPES_ASSIGNATIONS.Select(x => x.GTA_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "PARKING_SPACES") && (inversefks.ToProperties.FirstOrDefault().Name == "PSP_GRP_ID")) ? string.Join(",", gro.PARKING_SPACES.Select(x => x.PSP_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "UNITS_GROUPS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNGR_GRP_ID")) ? string.Join(",", gro.UNITS_GROUPS.Select(x => x.UNGR_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "GRP_INS_ID") && (member.Name != "GRP_EXT2_ID") && (member.Name != "GRP_EXT3_ID") && (member.Name != "GRP_HASH") && (member.Name != "GRP_LOG_GRP_ID") && (member.Name != "GRP_QUERY_EXT_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "GROUPS") && (member.Name == "GRP_ID")) ? gro.GRP_ID + "" : "",
                                                //((t.Name == "GROUPS") && (member.Name == "GRP_INS_ID")) ? gro.GRP_INS_ID + "" : "",
                                                ((t.Name == "GROUPS") && (member.Name == "GRP_DESCRIPTION")) ? gro.GRP_DESCRIPTION + "" : "",
                                                ((t.Name == "GROUPS") && (member.Name == "GRP_EXT1_ID")) ? gro.GRP_EXT1_ID + "" : "",
                                                //((t.Name == "GROUPS") && (member.Name == "GRP_EXT2_ID")) ? gro.GRP_EXT2_ID + "" : "",
                                                //((t.Name == "GROUPS") && (member.Name == "GRP_EXT3_ID")) ? gro.GRP_EXT3_ID + "" : "",
                                                //((t.Name == "GROUPS") && (member.Name == "GRP_HASH")) ? gro.GRP_HASH + "" : "",
                                                //((t.Name == "GROUPS") && (member.Name == "GRP_LOG_GRP_ID")) ? gro.GRP_LOG_GRP_ID + "" : "",
                                                //((t.Name == "GROUPS") && (member.Name == "GRP_QUERY_EXT_ID")) ? gro.GRP_QUERY_EXT_ID + "" : "",
                                                ((t.Name == "GROUPS") && (member.Name == "GRP_TREE_TYPE")) ? gro.GRP_TREE_TYPE + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void groupsHierarchyXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "GROUPS_HIERARCHY") foreach (var grh in dbContext.GROUPS_HIERARCHY.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "GROUPS_HIERARCHY") && (fks.ToProperties.FirstOrDefault().Name == "GRHI_GRP_ID")) ? grh.GRHI_GRP_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "GRHI_GRP_ID") && (member.Name != "GRHI_GRP_ID_PARENT"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "GROUPS_HIERARCHY") && (member.Name == "GRHI_ID")) ? grh.GRHI_ID + "" : "",
                                                //((t.Name == "GROUPS_HIERARCHY") && (member.Name == "GRHI_GRP_ID")) ? grh.GRHI_GRP_ID + "" : "",
                                                ((t.Name == "GROUPS_HIERARCHY") && (member.Name == "GRHI_INI_APPLY_DATE")) ? grh.GRHI_INI_APPLY_DATE + "" : "",
                                                ((t.Name == "GROUPS_HIERARCHY") && (member.Name == "GRHI_END_APPLY_DATE")) ? grh.GRHI_END_APPLY_DATE + "" : ""
                                                //((t.Name == "GROUPS_HIERARCHY") && (member.Name == "GRHI_GRP_ID_PARENT")) ? grh.GRHI_GRP_ID_PARENT + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void groupsTypesXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "GROUPS_TYPES") foreach (var grt in dbContext.GROUPS_TYPES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "GROUPS_TYPES") && (fks.ToProperties.FirstOrDefault().Name == "GRPT_INS_ID")) ? grt.GRPT_INS_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "GROUPS_TYPES") && (inversefks.ToRole.Name == "GROUPS_TYPES_ASSIGNATIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "GTA_GRPT_ID")) ? string.Join(",", grt.GROUPS_TYPES_ASSIGNATIONS.Select(x => x.GTA_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "GRPT_INS_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "GROUPS_TYPES") && (member.Name == "GRPT_ID")) ? grt.GRPT_ID + "" : "",
                                                //((t.Name == "GROUPS_TYPES") && (member.Name == "GRPT_INS_ID")) ? grt.GRPT_INS_ID + "" : "",
                                                ((t.Name == "GROUPS_TYPES") && (member.Name == "GRPT_DESCRIPTION")) ? grt.GRPT_DESCRIPTION + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void groupsTypesAssignationsXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "GROUPS_TYPES_ASSIGNATIONS") foreach (var grta in dbContext.GROUPS_TYPES_ASSIGNATIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1,atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (fks.ToProperties.FirstOrDefault().Name == "GTA_GRP_ID")) ? grta.GTA_GRP_ID + "" : "",
                            ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (fks.ToProperties.FirstOrDefault().Name == "GTA_GRPT_ID")) ? grta.GTA_GRPT_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "GTA_GRP_ID") && (member.Name != "GTA_GRPT_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_ID")) ? grta.GTA_ID + "" : "",
                                                //((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_GRP_ID")) ? grta.GTA_GRP_ID + "" : "",
                                                //((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_GRPT_ID")) ? grta.GTA_GRPT_ID + "" : "",
                                                ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_DESCRIPTION")) ? grta.GTA_DESCRIPTION + "" : "",
                                                ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_INI_APPLY_DATE")) ? grta.GTA_INI_APPLY_DATE + "" : "",
                                                ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_END_APPLY_DATE")) ? grta.GTA_END_APPLY_DATE + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void installationsXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "INSTALLATIONS") foreach (var ins in dbContext.INSTALLATIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "INSTALLATIONS") && (fks.ToProperties.FirstOrDefault().Name == "INS_COU_ID")) ? ins.INS_COU_ID + "" : "",
                            ((t.Name == "INSTALLATIONS") && (fks.ToProperties.FirstOrDefault().Name == "INS_CUR_ID")) ? ins.INS_CUR_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString", 
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "GROUPS") && (inversefks.ToProperties.FirstOrDefault().Name == "GRP_INS_ID")) ? string.Join(",", ins.GROUPS.Select(x => x.GRP_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "GROUPS_TYPES") && (inversefks.ToProperties.FirstOrDefault().Name == "GRPT_INS_ID")) ? string.Join(",", ins.GROUPS_TYPES.Select(x => x.GRPT_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "UNITS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNI_INS_ID")) ? string.Join(",", ins.UNITS.Select(x => x.UNI_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "INS_COU_ID") && (member.Name != "INS_CUR_ID") && (member.Name != "INS_RECHARGE_INCREMENTS") && (member.Name != "INS_RECHARGE_DECREMENTS"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_ID")) ? ins.INS_ID + "" : "",
                                                //((t.Name == "INSTALLATIONS") && (member.Name == "INS_COU_ID")) ? ins.INS_COU_ID + "" : "",
                                                //((t.Name == "INSTALLATIONS") && (member.Name == "INS_CUR_ID")) ? ins.INS_CUR_ID + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_DESCRIPTION")) ? ins.INS_DESCRIPTION + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_SHORTDESC")) ? ins.INS_SHORTDESC + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_TIMEZONE_ID")) ? ins.INS_TIMEZONE_ID + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_SECOND1_CUR_ID")) ? ins.INS_SECOND1_CUR_ID + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_SECOND2_CUR_ID")) ? ins.INS_SECOND2_CUR_ID + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_INFO_SCREEN_IMAGE_WIDTH")) ? ins.INS_INFO_SCREEN_IMAGE_WIDTH + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_INFO_SCREEN_IMAGE_HEIGHT")) ? ins.INS_INFO_SCREEN_IMAGE_HEIGHT + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_INFO_SCREEN_IMAGE_OFFSET_X")) ? ins.INS_INFO_SCREEN_IMAGE_OFFSET_X + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_INFO_SCREEN_IMAGE_OFFSET_Y")) ? ins.INS_INFO_SCREEN_IMAGE_OFFSET_Y + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_WAKEUP_SCREEN_IMAGE_WIDTH")) ? ins.INS_WAKEUP_SCREEN_IMAGE_WIDTH + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_WAKEUP_SCREEN_IMAGE_HEIGHT")) ? ins.INS_WAKEUP_SCREEN_IMAGE_HEIGHT + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_WAKEUP_SCREEN_IMAGE_OFFSET_X")) ? ins.INS_WAKEUP_SCREEN_IMAGE_OFFSET_X + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_WAKEUP_SCREEN_IMAGE_OFFSET_Y")) ? ins.INS_WAKEUP_SCREEN_IMAGE_OFFSET_Y + "" : "",
                                                //((t.Name == "INSTALLATIONS") && (member.Name == "INS_RECHARGE_INCREMENTS")) ? ins.INS_RECHARGE_INCREMENTS + "" : "",
                                                //((t.Name == "INSTALLATIONS") && (member.Name == "INS_RECHARGE_DECREMENTS")) ? ins.INS_RECHARGE_DECREMENTS + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_RECHARGE_MIN_ALLOWED")) ? ins.INS_RECHARGE_MIN_ALLOWED + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_RECHARGE_MAX_ALLOWED")) ? ins.INS_RECHARGE_MAX_ALLOWED + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_WALLET_MAX_BALANCE")) ? ins.INS_WALLET_MAX_BALANCE + "" : ""

                                );                               
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name),atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void parkingSpacesXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "PARKING_SPACES") foreach (var psp in dbContext.PARKING_SPACES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "PSP_EXT_ID")) ? psp.PSP_EXT_ID + "" : "",
                            ((t.Name == "PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "PSP_GRP_ID")) ? psp.PSP_GRP_ID + "" : "",
                            ((t.Name == "PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "PSP_OPE_ID")) ? psp.PSP_OPE_ID + "" : "",
                            ((t.Name == "PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "PSP_PHY_ID")) ? psp.PSP_PHY_ID + "" : "",
                            ((t.Name == "PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "PSP_STSE_ID")) ? psp.PSP_STSE_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "PARKING_SPACES") && (inversefks.ToRole.Name == "UNITS_LOGICAL_PARKING_SPACES") && (inversefks.ToProperties.FirstOrDefault().Name == "PSP_EXT_ID")) ? string.Join(",", psp.UNITS_LOGICAL_PARKING_SPACES.Select(x => x.UNLS_ID).ToList()) + "" : "",
                            ((t.Name == "PARKING_SPACES") && (inversefks.ToRole.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (inversefks.ToProperties.FirstOrDefault().Name == "PSP_PHY_ID")) ? string.Join(",", psp.UNITS_PHYSICAL_PARKING_SPACES.Select(x => x.UNPS_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "PSP_EXT_ID") && (member.Name != "PSP_GRP_ID") && (member.Name != "PSP_OPE_ID") && (member.Name != "PSP_PHY_ID") && (member.Name != "PSP_STSE_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_ID")) ? psp.PSP_ID + "" : "",
                                                //((t.Name == "PARKING_SPACES") && (member.Name == "PSP_EXT_ID")) ? psp.PSP_EXT_ID + "" : "",
                                                //((t.Name == "PARKING_SPACES") && (member.Name == "PSP_GRP_ID")) ? psp.PSP_GRP_ID + "" : "",
                                                //((t.Name == "PARKING_SPACES") && (member.Name == "PSP_OPE_ID")) ? psp.PSP_OPE_ID + "" : "",
                                                //((t.Name == "PARKING_SPACES") && (member.Name == "PSP_PHY_ID")) ? psp.PSP_PHY_ID + "" : "",
                                                //((t.Name == "PARKING_SPACES") && (member.Name == "PSP_STSE_ID")) ? psp.PSP_STSE_ID + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_DESCRIPTION")) ? psp.PSP_DESCRIPTION + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_COMM_STATUS")) ? psp.PSP_COMM_STATUS + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_COMM_UTC_DATE")) ? psp.PSP_COMM_UTC_DATE + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_GPS_LATITUDE")) ? psp.PSP_GPS_LATITUDE + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_GPS_LONGITUDE")) ? psp.PSP_GPS_LONGITUDE + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_LOG_STATUS")) ? psp.PSP_LOG_STATUS + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_LOG_STATUS_UTC_DATE")) ? psp.PSP_LOG_STATUS_UTC_DATE + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_PHY_STATUS")) ? psp.PSP_PHY_STATUS + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_PHY_STATUS_UTC_DATE")) ? psp.PSP_PHY_STATUS_UTC_DATE + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void unitAlarmTypesXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "UNIT_ALARM_TYPES") foreach (var uat in dbContext.UNIT_ALARM_TYPES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id","");
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "PSP_EXT_ID") && (member.Name != "PSP_GRP_ID") && (member.Name != "PSP_OPE_ID") && (member.Name != "PSP_PHY_ID") && (member.Name != "PSP_STSE_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "UNIT_ALARM_TYPES") && (member.Name == "UAT_ID")) ? uat.UAT_ID + "" : "",
                                                ((t.Name == "UNIT_ALARM_TYPES") && (member.Name == "UAT_DESCRIPTION")) ? uat.UAT_DESCRIPTION + "" : "",
                                                ((t.Name == "UNIT_ALARM_TYPES") && (member.Name == "UAT_NAME")) ? uat.UAT_NAME + "" : "",
                                                ((t.Name == "UNIT_ALARM_TYPES") && (member.Name == "UAT_LEVEL")) ? uat.UAT_LEVEL + "" : "",
                                                ((t.Name == "UNIT_ALARM_TYPES") && (member.Name == "UAT_COLOR")) ? uat.UAT_COLOR + "" : "",
                                                ((t.Name == "UNIT_ALARM_TYPES") && (member.Name == "UAT_HW_MASK")) ? uat.UAT_HW_MASK + "" : "",
                                                ((t.Name == "UNIT_ALARM_TYPES") && (member.Name == "UAT_SALES_MODE")) ? uat.UAT_SALES_MODE + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void unitsXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "UNITS") foreach (var uni in dbContext.UNITS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS") && (fks.ToProperties.FirstOrDefault().Name == "UNI_INS_ID")) ? uni.UNI_INS_ID + "" : "",
                            ((t.Name == "UNITS") && (fks.ToProperties.FirstOrDefault().Name == "UNI_ULIT_ID")) ? uni.UNI_ULIT_ID + "" : "",
                            ((t.Name == "UNITS") && (fks.ToProperties.FirstOrDefault().Name == "UNI_USET_ID")) ? uni.UNI_USET_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_GROUPS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNGR_UNI_ID")) ? string.Join(",", uni.UNITS_GROUPS.Select(x => x.UNGR_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_LOGICAL_PARKING_SPACES") && (inversefks.ToProperties.FirstOrDefault().Name == "UNLS_UNI_ID")) ? string.Join(",", uni.UNITS_LOGICAL_PARKING_SPACES.Select(x => x.UNLS_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (inversefks.ToProperties.FirstOrDefault().Name == "UNPS_UNI_ID")) ? string.Join(",", uni.UNITS_PHYSICAL_PARKING_SPACES.Select(x => x.UNPS_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_SYNC_VERSIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "USYV_UNI_ID")) ? string.Join(",", uni.UNITS_SYNC_VERSIONS.Select(x => x.USYV_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UNI_INS_ID") && (member.Name != "UNI_ULIT_ID") && (member.Name != "UNI_USET_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "UNITS") && (member.Name == "UNI_ID")) ? uni.UNI_ID + "" : "",
                                                //((t.Name == "UNITS") && (member.Name == "UNI_INS_ID")) ? uni.UNI_INS_ID + "" : "",
                                                //((t.Name == "UNITS") && (member.Name == "UNI_ULIT_ID")) ? uni.UNI_ULIT_ID + "" : "",
                                                //((t.Name == "UNITS") && (member.Name == "UNI_USET_ID")) ? uni.UNI_USET_ID + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_EXTERNAL_ID")) ? uni.UNI_EXTERNAL_ID + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_DESCRIPTION")) ? uni.UNI_DESCRIPTION + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_DATE")) ? uni.UNI_DATE + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_STATUS")) ? uni.UNI_STATUS + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_STATUS_DATE")) ? uni.UNI_STATUS_DATE + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_CMD_STATUS")) ? uni.UNI_CMD_STATUS + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_CMD_STATUS_DATE")) ? uni.UNI_CMD_STATUS_DATE + "" : "",
                                                //((t.Name == "UNITS") && (member.Name == "UNI_HW_VERSION")) ? uni.UNI_HW_VERSION + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_INCOMPLETION_FLAG")) ? uni.UNI_INCOMPLETION_FLAG + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void unitsGroupsXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "UNITS_GROUPS") foreach (var ung in dbContext.UNITS_GROUPS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS_GROUPS") && (fks.ToProperties.FirstOrDefault().Name == "UNGR_GRP_ID")) ? ung.UNGR_GRP_ID + "" : "",
                            ((t.Name == "UNITS_GROUPS") && (fks.ToProperties.FirstOrDefault().Name == "UNGR_UNI_ID")) ? ung.UNGR_UNI_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UNGR_GRP_ID") && (member.Name != "UNGR_UNI_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_ID")) ? ung.UNGR_ID + "" : "",
                                                //((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_GRP_ID")) ? ung.UNGR_GRP_ID + "" : "",
                                                //((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_UNI_ID")) ? ung.UNGR_UNI_ID + "" : "",
                                                ((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_INI_APPLY_DATE")) ? ung.UNGR_INI_APPLY_DATE + "" : "",
                                                ((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_END_APPLY_DATE")) ? ung.UNGR_END_APPLY_DATE + "" : "",
                                                ((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_DESCRIPTION")) ? ung.UNGR_DESCRIPTION + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void unitsLogicalParkingSpacesXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "UNITS_LOGICAL_PARKING_SPACES") foreach (var ulps in dbContext.UNITS_LOGICAL_PARKING_SPACES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "UNLS_PSP_ID")) ? ulps.UNLS_PSP_ID + "" : "",
                            ((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "UNLS_UNI_ID")) ? ulps.UNLS_UNI_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UNLS_PSP_ID") && (member.Name != "UNLS_UNI_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (member.Name == "UNLS_ID")) ? ulps.UNLS_ID + "" : "",
                                                //((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (member.Name == "UNLS_PSP_ID")) ? ulps.UNLS_PSP_ID + "" : "",
                                                //((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (member.Name == "UNLS_UNI_ID")) ? ulps.UNLS_UNI_ID + "" : "",
                                                ((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (member.Name == "UNLS_INI_APPLY_DATE")) ? ulps.UNLS_INI_APPLY_DATE + "" : "",
                                                ((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (member.Name == "UNLS_END_APPLY_DATE")) ? ulps.UNLS_END_APPLY_DATE + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        public static void unitsPhysicalParkingSpacesXML(EntitySet t, BCTOTA2PAEntities dbContext, XElement xmlEntityValue)
        {
            if (t.Name == "UNITS_PHYSICAL_PARKING_SPACES") foreach (var upps in dbContext.UNITS_PHYSICAL_PARKING_SPACES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", convertString(t.Name));
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    foreach (var fks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("FKs");
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertString(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "UNPS_PSP_ID")) ? upps.UNPS_PSP_ID + "" : "",
                            ((t.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "UNPS_UNI_ID")) ? upps.UNPS_UNI_ID + "" : ""
                            );
                        xmlFKInfo.Add(xmlField);
                        xmlFKInfo.Add(xmlFieldId);
                        xmlFKs.Add(xmlFKInfo);
                        xmlBaseEntity.Add(xmlFKs);
                    }

                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKs = new XElement("InverseFKs");
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                            xmlBaseEntity.Add(xmlFKs);
                        }
                    }

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UNPS_PSP_ID") && (member.Name != "UNPS_UNI_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (member.Name == "UNPS_ID")) ? upps.UNPS_ID + "" : ""
                                                //((t.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (member.Name == "UNPS_PSP_ID")) ? upps.UNPS_PSP_ID + "" : "",
                                                //((t.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (member.Name == "UNPS_UNI_ID")) ? upps.UNPS_UNI_ID + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

    }
}
