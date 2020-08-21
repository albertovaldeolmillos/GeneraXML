using GeneraXML.Modelo;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace ConsoleGeneraXML
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BCTOTA2PAEntitiesCompleto dbContext = new BCTOTA2PAEntitiesCompleto())
            {
                MetadataWorkspace metadata = ((IObjectContextAdapter)dbContext).ObjectContext.MetadataWorkspace;

                IEnumerable<EntitySet> tables = metadata.GetItemCollection(DataSpace.SSpace)
                    .GetItems<EntityContainer>()
                    .Single()
                    .BaseEntitySets
                    .OfType<EntitySet>()
                    .Where(s => !s.MetadataProperties.Contains("Type")
                    || s.MetadataProperties["Type"].ToString() == "Tables");

                generaInfraestructureDatabaseXML(tables, dbContext);
                generaConfigurationDatabaseXML(tables, dbContext);
                generaTariffsDatabaseXML(tables, dbContext);
            }
        }

        /// <summary>
        /// Genera en el fichero PruebaInfraestructure.xml las tablas y su contenido
        /// </summary>
        /// <param name="tables">tablas</param>
        /// <param name="dbContext">contexto</param>
        public static void generaInfraestructureDatabaseXML(IEnumerable<EntitySet> tables, BCTOTA2PAEntitiesCompleto dbContext)
        {
            List<string> tablas = new List<string>() { "CURRENCIES", "COUNTRIES", "INSTALLATIONS", "GROUPS_TYPES", "GROUPS_HIERARCHY", "GROUPS_TYPES_ASSIGNATIONS", "GROUPS", "UNITS", 
                "UNITS_PHYSICAL_PARKING_SPACES", "UNITS_LOGICAL_PARKING_SPACES", "UNITS_GROUPS", "PARKING_SPACES", "UNIT_ALARM_TYPES" };
            List<string> nombresTablas = new List<string>() { "Currency", "Country", "Installation", "GroupsType", "GroupsHierarchy", "GroupsTypesAssignation", "Group", "Unit",
                "UnitsPhysicalParkingSpace", "UnitsLogicalParkingSpace", "UnitsGroup", "ParkingSpace", "UnitAlarmType" };
            XElement xml = new XElement("XmlDatabase",
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"));

            XElement xmlEntitiesArray = new XElement("EntitiesArray");
            for (int i= 0; i< tablas.Count; i++)
            {
                EntitySet t = tables.Where(x => x.Name == tablas[i]).FirstOrDefault();

                string nombreTabla = nombresTablas[tablas.FindIndex(ind => ind.Equals(t.Name))];
                XElement xmlListsEntitiesDictionary = new XElement("ListsEntitiesDictionary");
                XElement xmlEntityKey = new XElement("Key", nombreTabla);
                XElement xmlEntityValue = new XElement("Value");

                if (t.Name == "CURRENCIES") currenciesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "COUNTRIES") countriesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "INSTALLATIONS") installationsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "GROUPS_TYPES") groupsTypesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "GROUPS_HIERARCHY") groupsHierarchyXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "GROUPS_TYPES_ASSIGNATIONS") groupsTypesAssignationsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "GROUPS") groupsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNITS") unitsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNITS_PHYSICAL_PARKING_SPACES") unitsPhysicalParkingSpacesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNITS_LOGICAL_PARKING_SPACES") unitsLogicalParkingSpacesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNITS_GROUPS") unitsGroupsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "PARKING_SPACES") parkingSpacesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNIT_ALARM_TYPES") unitAlarmTypesXML(t, dbContext, xmlEntityValue, nombreTabla);

                xmlListsEntitiesDictionary.Add(xmlEntityKey);
                xmlListsEntitiesDictionary.Add(xmlEntityValue);
                xmlEntitiesArray.Add(xmlListsEntitiesDictionary);

            }

            xml.Add(xmlEntitiesArray);

            xml.Save(ConfigurationManager.AppSettings["InfraestructureXML"]);
        }

        /// <summary>
        /// Genera en el fichero PruebaConfiguration.xml las tablas y su contenido
        /// </summary>
        /// <param name="tables">tablas</param>
        /// <param name="dbContext">contexto</param>
        public static void generaConfigurationDatabaseXML(IEnumerable<EntitySet> tables, BCTOTA2PAEntitiesCompleto dbContext)
        {
            List<string> tablas = new List<string>() { "UNIT_LITERAL_VERSIONS", "UNIT_LITERAL_KEYS", "LANGUAGES", "UNIT_LITERALS", "UNITS_LITERALS_LANGUAGES_EXCEPTIONS", "UNIT_LITERALS_LANGUAGES", 
                "UNIT_SETTING_VERSIONS", "UNIT_SETTINGS", "UNIT_SETTING_SECTIONS", "UNIT_SETTING_PARAMETERS", "UNITS_SETTINGS_DETAILS_EXCEPTIONS", "UNIT_SETTINGS_DETAILS", "UNITS_STATUS", "UNITS_LOCATIONS" };
            List<string> nombresTablas = new List<string>() { "UnitLiteralVersion", "UnitLiteralKey", "Language", "UnitLiteral", "UnitsLiteralsLanguagesException", "UnitLiteralsLanguage",
                "UnitSettingVersion", "UnitSetting", "UnitSettingSection", "UnitSettingParameter", "UnitsSettingsDetailsException", "UnitSettingsDetail", "UnitsStatus", "UnitsLocation" };
            XElement xml = new XElement("XmlDatabase",
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"));

            XElement xmlEntitiesArray = new XElement("EntitiesArray");
            for (int i = 0; i < tablas.Count; i++)
            {
                EntitySet t = tables.Where(x => x.Name == tablas[i]).FirstOrDefault();

                string nombreTabla = nombresTablas[tablas.FindIndex(ind => ind.Equals(t.Name))];
                XElement xmlListsEntitiesDictionary = new XElement("ListsEntitiesDictionary");
                XElement xmlEntityKey = new XElement("Key", nombreTabla);
                XElement xmlEntityValue = new XElement("Value");

                if (t.Name == "UNIT_LITERAL_VERSIONS") unitLiteralVersionsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNIT_LITERAL_KEYS") unitLiteralKeysXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "LANGUAGES") languagesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNIT_LITERALS") unitLiteralsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") unitsLiteralsLanguagesExceptionsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNIT_LITERALS_LANGUAGES") unitLiteralsLanguagesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNIT_SETTING_VERSIONS") unitSettingVersionsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNIT_SETTINGS") unitSettingsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNIT_SETTING_SECTIONS") unitSettingSectionsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNIT_SETTING_PARAMETERS") unitSettingParametersXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNITS_SETTINGS_DETAILS_EXCEPTIONS") unitsSettingDetailsExceptionsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNIT_SETTINGS_DETAILS") unitSettingDetailsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNITS_STATUS") unitsStatusXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "UNITS_LOCATIONS") unitsLocationsXML(t, dbContext, xmlEntityValue, nombreTabla);

                xmlListsEntitiesDictionary.Add(xmlEntityKey);
                xmlListsEntitiesDictionary.Add(xmlEntityValue);
                xmlEntitiesArray.Add(xmlListsEntitiesDictionary);

            }

            xml.Add(xmlEntitiesArray);

            xml.Save(ConfigurationManager.AppSettings["ConfigurationXML"]);
        }

        /// <summary>
        /// Genera en el fichero PruebaTariffs.xml las tablas y su contenido
        /// </summary>
        /// <param name="tables">tablas</param>
        /// <param name="dbContext">contexto</param>
        public static void generaTariffsDatabaseXML(IEnumerable<EntitySet> tables, BCTOTA2PAEntitiesCompleto dbContext)
        {
            List<string> tablas = new List<string>() { "RATE_BEHAVIOR_SETS", "TICKET_TYPES", "TICKETS_TYPES_FEATURES", "RATE_STEPS", "TARIFF_CONSTRAINT_ENTRIES", "TARIFFS_DEFINITION_RULES",
                "DAY_TYPES", "RATE_TYPES", "RATE_BEHAVIOR_STEP", "DAY_EXCEPTIONS", "TARIFF_CONSTRAINTS_SETS", "TARIFFS", "TARIFFS_APPLICATION_RULES", "DAY_HOURS_INTERVALS" };
            List<string> nombresTablas = new List<string>() { "RateBehaviorSet", "RateBehaviorSet", "TicketsTypesFeature", "RateStep", "TariffConstraintEntry", "TariffsDefinitionRule",
                "DayType", "RateType", "RateBehaviorStep", "DayException", "TariffConstraintsSet", "Tariff", "TariffsApplicationRule", "DayHoursInterval" };
            XElement xml = new XElement("XmlDatabase",
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"));

            XElement xmlEntitiesArray = new XElement("EntitiesArray");
            for (int i = 0; i < tablas.Count; i++)
            {
                EntitySet t = tables.Where(x => x.Name == tablas[i]).FirstOrDefault();

                string nombreTabla = nombresTablas[tablas.FindIndex(ind => ind.Equals(t.Name))];
                XElement xmlListsEntitiesDictionary = new XElement("ListsEntitiesDictionary");
                XElement xmlEntityKey = new XElement("Key", nombreTabla);
                XElement xmlEntityValue = new XElement("Value");

                if (t.Name == "RATE_BEHAVIOR_SETS") rateBehaviourSetsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "TICKET_TYPES") ticketTypesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "TICKETS_TYPES_FEATURES") ticketTypesFeaturesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "RATE_STEPS") rateStepsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "TARIFF_CONSTRAINT_ENTRIES") tariffConstraintEntriesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "TARIFFS_DEFINITION_RULES") tariffsDefinitionRulesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "DAY_TYPES") dayTypesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "RATE_TYPES") rateTypesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "RATE_BEHAVIOR_STEP") rateBehaviorStepXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "DAY_EXCEPTIONS") dayExceptionsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "TARIFF_CONSTRAINTS_SETS") tariffConstraintsSetsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "TARIFFS") tariffsXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "TARIFFS_APPLICATION_RULES") tariffsApplicationRulesXML(t, dbContext, xmlEntityValue, nombreTabla);
                if (t.Name == "DAY_HOURS_INTERVALS") dayHoursIntervalsXML(t, dbContext, xmlEntityValue, nombreTabla);

                xmlListsEntitiesDictionary.Add(xmlEntityKey);
                xmlListsEntitiesDictionary.Add(xmlEntityValue);
                xmlEntitiesArray.Add(xmlListsEntitiesDictionary);

            }

            xml.Add(xmlEntitiesArray);

            xml.Save(ConfigurationManager.AppSettings["TariffsXML"]);
        }

        /// <summary>
        /// Convierte el string pasado como argumento en minusculas y capitalizado
        /// Ejemplo: TARIFFS_APPLICATION_RULES ---> TariffsApplicationRules
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string convertString(string str)
        {
            string nuevoStr = "";
            var arrStr = str.Split('_');
            foreach (string item in arrStr)
            {
                string modif = item.Substring(0, 1).ToUpper() + item.Substring(1).ToLower();
                nuevoStr = nuevoStr + modif;
            }
            return nuevoStr;
        }

        /// <summary>
        /// Convierte el string pasado como argumento en minusculas y capitalizado y Elimina el Id final
        /// Ejemplo: COU_CUR_ID ---> CouCur
        /// NOTA: El único FK no acabado en _ID es RBSS_DAH (RbssDah)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string convertStringFK(string str)
        {
            string nuevoStr = convertString(str);
            if (nuevoStr.Substring(nuevoStr.Length - 2) == "Id") nuevoStr = nuevoStr.Substring(0, nuevoStr.Length - 2);
            return nuevoStr;
        }

        #region INFRESTRUCTURE

        /// <summary>
        /// Genera el XML de CURRENCIES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void currenciesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "CURRENCIES") foreach (var curr in dbContext.CURRENCIES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id", ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "CURRENCIES") && (inversefks.ToRole.Name == "INSTALLATIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "INS_CUR_ID")) ? string.Join(",", curr.INSTALLATIONS.Select(x => x.INS_ID).ToList()) + "" : "",
                            ((t.Name == "CURRENCIES") && (inversefks.ToRole.Name == "COUNTRIES") && (inversefks.ToProperties.FirstOrDefault().Name == "COU_CUR_ID")) ? string.Join(",", curr.COUNTRIES.Select(x => x.COU_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        //if ()
                        //{
                        XElement xmlMember = new XElement(convertString(member.Name),
                            ((t.Name == "CURRENCIES") && (member.Name == "CUR_FACT")) ? curr.CUR_FACT?.ToString(CultureInfo.InvariantCulture) + "" : "",
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
                        //}
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        /// <summary>
        /// Genera el XML de COUNTRIES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void countriesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "COUNTRIES") foreach (var cou in dbContext.COUNTRIES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    //xmlBaseEntity.SetAttributeValue(xsytype + "type", nombreTabla);

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "COUNTRIES") && (fks.ToProperties.FirstOrDefault().Name == "COU_CUR_ID")) ? cou.COU_CUR_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "COUNTRIES") && (inversefks.ToRole.Name == "INSTALLATIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "INS_COU_ID")) ? string.Join(",", cou.INSTALLATIONS.Select(x => x.INS_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

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

        /// <summary>
        /// Genera el XML de INSTALLATIONS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void installationsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "INSTALLATIONS") foreach (var ins in dbContext.INSTALLATIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "INSTALLATIONS") && (fks.ToProperties.FirstOrDefault().Name == "INS_COU_ID")) ? ins.INS_COU_ID + "" : "",
                            ((t.Name == "INSTALLATIONS") && (fks.ToProperties.FirstOrDefault().Name == "INS_CUR_ID")) ? ins.INS_CUR_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "DAY_HOURS_INTERVALS") && (inversefks.ToProperties.FirstOrDefault().Name == "DAH_INS_ID")) ? string.Join(",", ins.DAY_HOURS_INTERVALS.Select(x => x.DAH_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "DAY_TYPES") && (inversefks.ToProperties.FirstOrDefault().Name == "DAT_INS_ID")) ? string.Join(",", ins.DAY_TYPES.Select(x => x.DAT_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "GROUPS") && (inversefks.ToProperties.FirstOrDefault().Name == "GRP_INS_ID")) ? string.Join(",", ins.GROUPS.Select(x => x.GRP_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "GROUPS_TYPES") && (inversefks.ToProperties.FirstOrDefault().Name == "GRPT_INS_ID")) ? string.Join(",", ins.GROUPS_TYPES.Select(x => x.GRPT_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_SETS") && (inversefks.ToProperties.FirstOrDefault().Name == "RBS_INS_ID")) ? string.Join(",", ins.RATE_BEHAVIOR_SETS.Select(x => x.RBS_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "RATE_TYPES") && (inversefks.ToProperties.FirstOrDefault().Name == "RAT_INS_ID")) ? string.Join(",", ins.RATE_TYPES.Select(x => x.RAT_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "TARIFF_CONSTRAINTS_SETS") && (inversefks.ToProperties.FirstOrDefault().Name == "TCS_INS_ID")) ? string.Join(",", ins.TARIFF_CONSTRAINTS_SETS.Select(x => x.TCS_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "TARIFFS") && (inversefks.ToProperties.FirstOrDefault().Name == "TAR_INS_ID")) ? string.Join(",", ins.TARIFFS.Select(x => x.TAR_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "TICKET_TYPES") && (inversefks.ToProperties.FirstOrDefault().Name == "TITY_INS_ID")) ? string.Join(",", ins.TICKET_TYPES.Select(x => x.TITY_ID).ToList()) + "" : "",
                            ((t.Name == "INSTALLATIONS") && (inversefks.ToRole.Name == "UNITS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNI_INS_ID")) ? string.Join(",", ins.UNITS.Select(x => x.UNI_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

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
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_SECOND1_CUR_ID")) ? ins.INS_SECOND1_CUR_ID?.ToString(CultureInfo.InvariantCulture) + "" : "",
                                                ((t.Name == "INSTALLATIONS") && (member.Name == "INS_SECOND2_CUR_ID")) ? ins.INS_SECOND2_CUR_ID?.ToString(CultureInfo.InvariantCulture) + "" : "",
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
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        }
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        /// <summary>
        /// Genera el XML de GROUPS_TYPES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void groupsTypesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "GROUPS_TYPES") foreach (var grt in dbContext.GROUPS_TYPES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "GROUPS_TYPES") && (fks.ToProperties.FirstOrDefault().Name == "GRPT_INS_ID")) ? grt.GRPT_INS_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        string strField = convertString(inversefks.ToRole.Name);
                        if ((t.Name == "GROUPS_TYPES") && (inversefks.ToRole.Name == "UNITS_STATUS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNST_GRPT_ID")) strField = "UnitsStatusesByUnstGrpt";
                        XElement xmlField = new XElement("Field", strField);
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "GROUPS_TYPES") && (inversefks.ToRole.Name == "GROUPS_TYPES_ASSIGNATIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "GTA_GRPT_ID")) ? string.Join(",", grt.GROUPS_TYPES_ASSIGNATIONS.Select(x => x.GTA_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS_TYPES") && (inversefks.ToRole.Name == "TARIFF_CONSTRAINT_ENTRIES") && (inversefks.ToProperties.FirstOrDefault().Name == "TCE_GRPT_ID")) ? string.Join(",", grt.TARIFF_CONSTRAINT_ENTRIES.Select(x => x.TCE_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS_TYPES") && (inversefks.ToRole.Name == "TARIFFS_APPLICATION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TAPR_GRPT_ID")) ? string.Join(",", grt.TARIFFS_APPLICATION_RULES.Select(x => x.TAPR_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS_TYPES") && (inversefks.ToRole.Name == "TARIFFS_DEFINITION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TARDR_GRPT_ID")) ? string.Join(",", grt.TARIFFS_DEFINITION_RULES.Select(x => x.TARDR_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS_TYPES") && (inversefks.ToRole.Name == "UNITS_STATUS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNST_GRPT_ID")) ? string.Join(",", grt.UNITS_STATUS.Select(x => x.UNST_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

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

        /// <summary>
        /// Genera el XML de GROUPS_HIERARCHY
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void groupsHierarchyXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "GROUPS_HIERARCHY") foreach (var grh in dbContext.GROUPS_HIERARCHY.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "GROUPS_HIERARCHY") && (fks.ToProperties.FirstOrDefault().Name == "GRHI_GRP_ID")) ? grh.GRHI_GRP_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString", ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "GRHI_GRP_ID") && (member.Name != "GRHI_GRP_ID_PARENT"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "GROUPS_HIERARCHY") && (member.Name == "GRHI_ID")) ? grh.GRHI_ID + "" : "",
                                //((t.Name == "GROUPS_HIERARCHY") && (member.Name == "GRHI_GRP_ID")) ? grh.GRHI_GRP_ID + "" : "",
                                ((t.Name == "GROUPS_HIERARCHY") && (member.Name == "GRHI_INI_APPLY_DATE")) ? grh.GRHI_INI_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "GROUPS_HIERARCHY") && (member.Name == "GRHI_END_APPLY_DATE")) ? grh.GRHI_END_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : ""
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

        /// <summary>
        /// Genera el XML de GROUPS_TYPES_ASSIGNATIONS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void groupsTypesAssignationsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "GROUPS_TYPES_ASSIGNATIONS") foreach (var grta in dbContext.GROUPS_TYPES_ASSIGNATIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (fks.ToProperties.FirstOrDefault().Name == "GTA_GRP_ID")) ? grta.GTA_GRP_ID + "" : "",
                            ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (fks.ToProperties.FirstOrDefault().Name == "GTA_GRPT_ID")) ? grta.GTA_GRPT_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString", ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "GTA_GRP_ID") && (member.Name != "GTA_GRPT_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_ID")) ? grta.GTA_ID + "" : "",
                                                //((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_GRP_ID")) ? grta.GTA_GRP_ID + "" : "",
                                                //((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_GRPT_ID")) ? grta.GTA_GRPT_ID + "" : "",
                                                ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_DESCRIPTION")) ? grta.GTA_DESCRIPTION + "" : "",
                                                ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_INI_APPLY_DATE")) ? grta.GTA_INI_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                                ((t.Name == "GROUPS_TYPES_ASSIGNATIONS") && (member.Name == "GTA_END_APPLY_DATE")) ? grta.GTA_END_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : ""

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

        /// <summary>
        /// Genera el XML de GROUPS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void groupsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "GROUPS") foreach (var gro in dbContext.GROUPS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "GROUPS") && (fks.ToProperties.FirstOrDefault().Name == "GRP_INS_ID")) ? gro.GRP_INS_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        string strField = convertString(inversefks.ToRole.Name);
                        if ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "GROUPS_HIERARCHY") && (inversefks.ToProperties.FirstOrDefault().Name == "GRHI_GRP_ID")) strField = "GroupsHierarchiesByGrhiGrp";
                        if ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "GROUPS_HIERARCHY") && (inversefks.ToProperties.FirstOrDefault().Name == "GRHI_GRP_ID_PARENT")) strField = "GroupsHierarchiesByGrhiGrpIdParent";
                        if ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "UNITS_GROUPS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNGR_GRP_ID")) strField = "UnitsGroupsByUngrGrp";
                        XElement xmlField = new XElement("Field", strField);
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "GROUPS1") && (inversefks.ToProperties.FirstOrDefault().Name == "GRP_LOG_GRP_ID")) ? string.Join(",", gro.GROUPS1.Select(x => x.GRP_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "GROUPS_HIERARCHY") && (inversefks.ToProperties.FirstOrDefault().Name == "GRHI_GRP_ID")) ? string.Join(",", gro.GROUPS_HIERARCHY.Select(x => x.GRHI_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "GROUPS_HIERARCHY") && (inversefks.ToProperties.FirstOrDefault().Name == "GRHI_GRP_ID_PARENT")) ? string.Join(",", gro.GROUPS_HIERARCHY1.Select(x => x.GRHI_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "GROUPS_TYPES_ASSIGNATIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "GTA_GRP_ID")) ? string.Join(",", gro.GROUPS_TYPES_ASSIGNATIONS.Select(x => x.GTA_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "PARKING_SPACES") && (inversefks.ToProperties.FirstOrDefault().Name == "PSP_GRP_ID")) ? string.Join(",", gro.PARKING_SPACES.Select(x => x.PSP_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "TARIFF_CONSTRAINT_ENTRIES") && (inversefks.ToProperties.FirstOrDefault().Name == "TCE_GRP_ID")) ? string.Join(",", gro.TARIFF_CONSTRAINT_ENTRIES.Select(x => x.TCE_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "TARIFFS_DEFINITION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TARDR_GRP_ID")) ? string.Join(",", gro.TARIFFS_DEFINITION_RULES.Select(x => x.TARDR_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "UNITS_GROUPS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNGR_GRP_ID")) ? string.Join(",", gro.UNITS_GROUPS.Select(x => x.UNGR_ID).ToList()) + "" : "",
                            ((t.Name == "GROUPS") && (inversefks.ToRole.Name == "UNITS_STATUS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNST_GRP_ID")) ? string.Join(",", gro.UNITS_STATUS.Select(x => x.UNST_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

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

        /// <summary>
        /// Genera el XML de UNITS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNITS") foreach (var uni in dbContext.UNITS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS") && (fks.ToProperties.FirstOrDefault().Name == "UNI_INS_ID")) ? uni.UNI_INS_ID + "" : "",
                            ((t.Name == "UNITS") && (fks.ToProperties.FirstOrDefault().Name == "UNI_ULIT_ID")) ? uni.UNI_ULIT_ID + "" : "",
                            ((t.Name == "UNITS") && (fks.ToProperties.FirstOrDefault().Name == "UNI_USET_ID")) ? uni.UNI_USET_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        string strField = convertString(inversefks.ToRole.Name);
                        if ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_GROUPS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNGR_UNI_ID")) strField = "UnitsGroupsByUngrUni";
                        XElement xmlField = new XElement("Field", strField);
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "TARIFFS_APPLICATION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TAPR_UNI_ID")) ? string.Join(",", uni.TARIFFS_APPLICATION_RULES.Select(x => x.TAPR_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_GROUPS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNGR_UNI_ID")) ? string.Join(",", uni.UNITS_GROUPS.Select(x => x.UNGR_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNIL_UNI_ID")) ? string.Join(",", uni.UNITS_LITERALS_LANGUAGES_EXCEPTIONS.Select(x => x.UNIL_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_LOCATIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNLO_UNI_ID")) ? string.Join(",", uni.UNITS_LOCATIONS.Select(x => x.UNLO_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_LOGICAL_PARKING_SPACES") && (inversefks.ToProperties.FirstOrDefault().Name == "UNLS_UNI_ID")) ? string.Join(",", uni.UNITS_LOGICAL_PARKING_SPACES.Select(x => x.UNLS_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (inversefks.ToProperties.FirstOrDefault().Name == "UNPS_UNI_ID")) ? string.Join(",", uni.UNITS_PHYSICAL_PARKING_SPACES.Select(x => x.UNPS_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_SETTINGS_DETAILS_EXCEPTIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNIS_UNI_ID")) ? string.Join(",", uni.UNITS_SETTINGS_DETAILS_EXCEPTIONS.Select(x => x.UNIS_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_STATUS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNST_UNI_ID")) ? string.Join(",", uni.UNITS_STATUS.Select(x => x.UNST_ID).ToList()) + "" : "",
                            ((t.Name == "UNITS") && (inversefks.ToRole.Name == "UNITS_SYNC_VERSIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "USYV_UNI_ID")) ? string.Join(",", uni.UNITS_SYNC_VERSIONS.Select(x => x.USYV_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

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
                                                ((t.Name == "UNITS") && (member.Name == "UNI_DATE")) ? uni.UNI_DATE?.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_STATUS")) ? uni.UNI_STATUS + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_STATUS_DATE")) ? uni.UNI_STATUS_DATE?.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_CMD_STATUS")) ? uni.UNI_CMD_STATUS + "" : "",
                                                ((t.Name == "UNITS") && (member.Name == "UNI_CMD_STATUS_DATE")) ? uni.UNI_CMD_STATUS_DATE?.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
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

        /// <summary>
        /// Genera el XML de UNITS_PHYSICAL_PARKING_SPACES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitsPhysicalParkingSpacesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNITS_PHYSICAL_PARKING_SPACES") foreach (var upps in dbContext.UNITS_PHYSICAL_PARKING_SPACES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "UNPS_PSP_ID")) ? upps.UNPS_PSP_ID + "" : "",
                            ((t.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "UNPS_UNI_ID")) ? upps.UNPS_UNI_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString", ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

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

        /// <summary>
        /// Genera el XML de UNITS_LOGICAL_PARKING_SPACES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitsLogicalParkingSpacesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNITS_LOGICAL_PARKING_SPACES") foreach (var ulps in dbContext.UNITS_LOGICAL_PARKING_SPACES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "UNLS_PSP_ID")) ? ulps.UNLS_PSP_ID + "" : "",
                            ((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "UNLS_UNI_ID")) ? ulps.UNLS_UNI_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString", ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UNLS_PSP_ID") && (member.Name != "UNLS_UNI_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (member.Name == "UNLS_ID")) ? ulps.UNLS_ID + "" : "",
                                                //((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (member.Name == "UNLS_PSP_ID")) ? ulps.UNLS_PSP_ID + "" : "",
                                                //((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (member.Name == "UNLS_UNI_ID")) ? ulps.UNLS_UNI_ID + "" : "",
                                                ((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (member.Name == "UNLS_INI_APPLY_DATE")) ? ulps.UNLS_INI_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                                ((t.Name == "UNITS_LOGICAL_PARKING_SPACES") && (member.Name == "UNLS_END_APPLY_DATE")) ? ulps.UNLS_END_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : ""

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

        /// <summary>
        /// Genera el XML de UNITS_GROUPS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitsGroupsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNITS_GROUPS") foreach (var ung in dbContext.UNITS_GROUPS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS_GROUPS") && (fks.ToProperties.FirstOrDefault().Name == "UNGR_GRP_ID")) ? ung.UNGR_GRP_ID + "" : "",
                            ((t.Name == "UNITS_GROUPS") && (fks.ToProperties.FirstOrDefault().Name == "UNGR_UNI_ID")) ? ung.UNGR_UNI_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString", ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UNGR_GRP_ID") && (member.Name != "UNGR_UNI_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                                ((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_ID")) ? ung.UNGR_ID + "" : "",
                                                //((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_GRP_ID")) ? ung.UNGR_GRP_ID + "" : "",
                                                //((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_UNI_ID")) ? ung.UNGR_UNI_ID + "" : "",
                                                ((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_INI_APPLY_DATE")) ? ung.UNGR_INI_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                                ((t.Name == "UNITS_GROUPS") && (member.Name == "UNGR_END_APPLY_DATE")) ? ung.UNGR_END_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
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

        /// <summary>
        /// Genera el XML de PARKING_SPACES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void parkingSpacesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "PARKING_SPACES") foreach (var psp in dbContext.PARKING_SPACES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "PSP_EXT_ID")) ? psp.PSP_EXT_ID + "" : "",
                            ((t.Name == "PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "PSP_GRP_ID")) ? psp.PSP_GRP_ID + "" : "",
                            ((t.Name == "PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "PSP_OPE_ID")) ? psp.PSP_OPE_ID + "" : "",
                            ((t.Name == "PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "PSP_PHY_ID")) ? psp.PSP_PHY_ID + "" : "",
                            ((t.Name == "PARKING_SPACES") && (fks.ToProperties.FirstOrDefault().Name == "PSP_STSE_ID")) ? psp.PSP_STSE_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "PARKING_SPACES") && (inversefks.ToRole.Name == "UNITS_LOGICAL_PARKING_SPACES") && (inversefks.ToProperties.FirstOrDefault().Name == "PSP_EXT_ID")) ? string.Join(",", psp.UNITS_LOGICAL_PARKING_SPACES.Select(x => x.UNLS_ID).ToList()) + "" : "",
                            ((t.Name == "PARKING_SPACES") && (inversefks.ToRole.Name == "UNITS_PHYSICAL_PARKING_SPACES") && (inversefks.ToProperties.FirstOrDefault().Name == "PSP_PHY_ID")) ? string.Join(",", psp.UNITS_PHYSICAL_PARKING_SPACES.Select(x => x.UNPS_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

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
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_COMM_UTC_DATE")) ? psp.PSP_COMM_UTC_DATE?.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_GPS_LATITUDE")) ? psp.PSP_GPS_LATITUDE?.ToString(CultureInfo.InvariantCulture) + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_GPS_LONGITUDE")) ? psp.PSP_GPS_LONGITUDE?.ToString(CultureInfo.InvariantCulture) + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_LOG_STATUS")) ? psp.PSP_LOG_STATUS + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_LOG_STATUS_UTC_DATE")) ? psp.PSP_LOG_STATUS_UTC_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_PHY_STATUS")) ? psp.PSP_PHY_STATUS + "" : "",
                                                ((t.Name == "PARKING_SPACES") && (member.Name == "PSP_PHY_STATUS_UTC_DATE")) ? psp.PSP_PHY_STATUS_UTC_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : ""

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

        /// <summary>
        /// Genera el XML de UNIT_ALARM_TYPES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitAlarmTypesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNIT_ALARM_TYPES") foreach (var uat in dbContext.UNIT_ALARM_TYPES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        //if ()
                        //{
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
                        //}
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        #endregion

        #region CONFIGURATION

        /// <summary>
        /// Genera el XML de UNIT_LITERAL_VERSIONS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitLiteralVersionsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNIT_LITERAL_VERSIONS") foreach (var ulv in dbContext.UNIT_LITERAL_VERSIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "UNIT_LITERAL_VERSIONS") && (inversefks.ToRole.Name == "UNIT_LITERAL_KEYS") && (inversefks.ToProperties.FirstOrDefault().Name == "ULK_ULV_ID")) ? string.Join(",", ulv.UNIT_LITERAL_KEYS.Select(x => x.ULK_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        //if ()
                        //{
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNIT_LITERAL_VERSIONS") && (member.Name == "ULV_ID")) ? ulv.ULV_ID + "" : "",
                                ((t.Name == "UNIT_LITERAL_VERSIONS") && (member.Name == "ULV_DESCRIPTION")) ? ulv.ULV_DESCRIPTION + "" : ""
                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        //}
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        /// <summary>
        /// Genera el XML de UNIT_LITERAL_KEYS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitLiteralKeysXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNIT_LITERAL_KEYS") foreach (var ulk in dbContext.UNIT_LITERAL_KEYS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNIT_LITERAL_KEYS") && (fks.ToProperties.FirstOrDefault().Name == "ULK_ULV_ID")) ? ulk.ULK_ULV_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                               ((t.Name == "UNIT_LITERAL_KEYS") && (inversefks.ToRole.Name == "UNIT_LITERALS_LANGUAGES") && (inversefks.ToProperties.FirstOrDefault().Name == "ULTL_ULK_ID")) ? string.Join(",", ulk.UNIT_LITERALS_LANGUAGES.Select(x => x.ULTL_ID).ToList()) + "" : "",
                               ((t.Name == "UNIT_LITERAL_KEYS") && (inversefks.ToRole.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNIL_ULK_ID")) ? string.Join(",", ulk.UNITS_LITERALS_LANGUAGES_EXCEPTIONS.Select(x => x.UNIL_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if (member.Name != "ULK_ULV_ID")
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNIT_LITERAL_KEYS") && (member.Name == "ULK_ID")) ? ulk.ULK_ID + "" : "",
                                //((t.Name == "UNIT_LITERAL_KEYS") && (member.Name == "ULK_ULV_ID")) ? ulk.ULK_ULV_ID + "" : "",
                                ((t.Name == "UNIT_LITERAL_KEYS") && (member.Name == "ULK_KEY")) ? ulk.ULK_KEY + "" : ""
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

        /// <summary>
        /// Genera el XML de LANGUAGES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void languagesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "LANGUAGES") foreach (var lan in dbContext.LANGUAGES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                               ((t.Name == "LANGUAGES") && (inversefks.ToRole.Name == "UNIT_LITERALS_LANGUAGES") && (inversefks.ToProperties.FirstOrDefault().Name == "ULTL_LAN_ID")) ? string.Join(",", lan.UNIT_LITERALS_LANGUAGES.Select(x => x.ULTL_ID).ToList()) + "" : "",
                               ((t.Name == "LANGUAGES") && (inversefks.ToRole.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNIL_LAN_ID")) ? string.Join(",", lan.UNITS_LITERALS_LANGUAGES_EXCEPTIONS.Select(x => x.UNIL_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        //if ()
                        //{
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "LANGUAGES") && (member.Name == "LAN_ID")) ? lan.LAN_ID + "" : "",
                                ((t.Name == "LANGUAGES") && (member.Name == "LAN_DESCRIPTION")) ? lan.LAN_DESCRIPTION + "" : "",
                                ((t.Name == "LANGUAGES") && (member.Name == "LAN_CULTURE")) ? lan.LAN_CULTURE + "" : ""
                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        //}
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        /// <summary>
        /// Genera el XML de UNIT_LITERALS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitLiteralsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNIT_LITERALS") foreach (var uli in dbContext.UNIT_LITERALS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        string strField = convertString(inversefks.ToRole.Name);
                        if ((t.Name == "UNIT_LITERALS") && (inversefks.ToRole.Name == "UNIT_LITERALS1") && (inversefks.ToProperties.FirstOrDefault().Name == "ULT_PARENT_ULT_ID")) strField = "UnitLiterals";
                        XElement xmlField = new XElement("Field", strField);
                        XElement xmlFieldId = new XElement("IdsString",
                               ((t.Name == "UNIT_LITERALS") && (inversefks.ToRole.Name == "UNIT_LITERALS_LANGUAGES") && (inversefks.ToProperties.FirstOrDefault().Name == "ULTL_ULT_ID")) ? string.Join(",", uli.UNIT_LITERALS_LANGUAGES.Select(x => x.ULTL_ID).ToList()) + "" : "",
                               ((t.Name == "UNIT_LITERALS") && (inversefks.ToRole.Name == "UNIT_LITERALS1") && (inversefks.ToProperties.FirstOrDefault().Name == "ULT_PARENT_ULT_ID")) ? string.Join(",", uli.UNIT_LITERALS1.Select(x => x.ULT_ID).ToList()) + "" : "",
                               ((t.Name == "UNIT_LITERALS") && (inversefks.ToRole.Name == "UNITS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNI_ULIT_ID")) ? string.Join(",", uli.UNITS.Select(x => x.UNI_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        //if ()
                        //{
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNIT_LITERALS") && (member.Name == "ULT_ID")) ? uli.ULT_ID + "" : "",
                                ((t.Name == "UNIT_LITERALS") && (member.Name == "ULT_NAME")) ? uli.ULT_NAME + "" : ""
                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        //}
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        /// <summary>
        /// Genera el XML de UNITS_LITERALS_LANGUAGES_EXCEPTIONS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitsLiteralsLanguagesExceptionsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") foreach (var ulle in dbContext.UNITS_LITERALS_LANGUAGES_EXCEPTIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (fks.ToProperties.FirstOrDefault().Name == "UNIL_LAN_ID")) ? ulle.UNIL_LAN_ID + "" : "",
                            ((t.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (fks.ToProperties.FirstOrDefault().Name == "UNIL_ULK_ID")) ? ulle.UNIL_ULK_ID + "" : "",
                            ((t.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (fks.ToProperties.FirstOrDefault().Name == "UNIL_UNI_ID")) ? ulle.UNIL_UNI_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UNIL_LAN_ID") && (member.Name != "UNIL_ULK_ID") && (member.Name != "UNIL_UNI_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (member.Name == "UNIL_ID")) ? ulle.UNIL_ID + "" : "",
                                //((t.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (member.Name == "UNIL_LAN_ID")) ? ulle.UNIL_LAN_ID + "" : "",
                                //((t.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (member.Name == "UNIL_ULK_ID")) ? ulle.UNIL_ULK_ID + "" : "",
                                //((t.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (member.Name == "UNIL_UNI_ID")) ? ulle.UNIL_UNI_ID + "" : "",
                                ((t.Name == "UNITS_LITERALS_LANGUAGES_EXCEPTIONS") && (member.Name == "UNIL_LITERAL")) ? ulle.UNIL_LITERAL + "" : ""
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

        /// <summary>
        /// Genera el XML de UNIT_LITERALS_LANGUAGES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitLiteralsLanguagesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNIT_LITERALS_LANGUAGES") foreach (var ull in dbContext.UNIT_LITERALS_LANGUAGES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNIT_LITERALS_LANGUAGES") && (fks.ToProperties.FirstOrDefault().Name == "ULTL_LAN_ID")) ? ull.ULTL_LAN_ID + "" : "",
                            ((t.Name == "UNIT_LITERALS_LANGUAGES") && (fks.ToProperties.FirstOrDefault().Name == "ULTL_ULK_ID")) ? ull.ULTL_ULK_ID + "" : "",
                            ((t.Name == "UNIT_LITERALS_LANGUAGES") && (fks.ToProperties.FirstOrDefault().Name == "ULTL_ULT_ID")) ? ull.ULTL_ULT_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "ULTL_LAN_ID") && (member.Name != "ULTL_ULK_ID") && (member.Name != "ULTL_ULT_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNIT_LITERALS_LANGUAGES") && (member.Name == "ULTL_ID")) ? ull.ULTL_ID + "" : "",
                                //((t.Name == "UNIT_LITERALS_LANGUAGES") && (member.Name == "ULTL_LAN_ID")) ? ull.ULTL_LAN_ID + "" : "",
                                //((t.Name == "UNIT_LITERALS_LANGUAGES") && (member.Name == "ULTL_ULK_ID")) ? ull.ULTL_ULK_ID + "" : "",
                                //((t.Name == "UNIT_LITERALS_LANGUAGES") && (member.Name == "ULTL_ULT_ID")) ? ull.ULTL_ULT_ID + "" : "",
                                ((t.Name == "UNIT_LITERALS_LANGUAGES") && (member.Name == "UNIL_LITERAL")) ? ull.ULTL_LITERAL + "" : ""
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

        /// <summary>
        /// Genera el XML de UNIT_SETTING_VERSIONS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitSettingVersionsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNIT_SETTING_VERSIONS") foreach (var usv in dbContext.UNIT_SETTING_VERSIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "UNIT_SETTING_VERSIONS") && (inversefks.ToRole.Name == "UNIT_SETTING_PARAMETERS") && (inversefks.ToProperties.FirstOrDefault().Name == "USP_USV_ID")) ? string.Join(",", usv.UNIT_SETTING_PARAMETERS.Select(x => x.USP_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        //if ()
                        //{
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNIT_SETTING_VERSIONS") && (member.Name == "USV_ID")) ? usv.USV_ID + "" : "",
                                ((t.Name == "UNIT_SETTING_VERSIONS") && (member.Name == "USV_DESCRIPTION")) ? usv.USV_DESCRIPTION + "" : ""

                                );
                            xmlBaseEntity.Add(xmlMember);
                            if (xmlMember.Value == "")
                            {
                                xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                            }
                        //}
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        /// <summary>
        /// Genera el XML de UNIT_SETTINGS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitSettingsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNIT_SETTINGS") foreach (var use in dbContext.UNIT_SETTINGS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNIT_SETTINGS") && (fks.ToProperties.FirstOrDefault().Name == "UST_PARENT_USET_ID")) ? use.UST_PARENT_USET_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        string strField = convertString(inversefks.ToRole.Name);
                        if ((t.Name == "UNIT_SETTINGS") && (inversefks.ToRole.Name == "UNIT_SETTINGS1") && (inversefks.ToProperties.FirstOrDefault().Name == "UST_PARENT_USET_ID")) strField = "UnitSettings";
                        XElement xmlField = new XElement("Field", strField);
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "UNIT_SETTINGS") && (inversefks.ToRole.Name == "UNIT_SETTINGS_DETAILS") && (inversefks.ToProperties.FirstOrDefault().Name == "USTD_UST_ID")) ? string.Join(",", use.UNIT_SETTINGS_DETAILS.Select(x => x.USTD_ID).ToList()) + "" : "",
                            ((t.Name == "UNIT_SETTINGS") && (inversefks.ToRole.Name == "UNIT_SETTINGS1") && (inversefks.ToProperties.FirstOrDefault().Name == "UST_PARENT_USET_ID")) ? string.Join(",", use.UNIT_SETTINGS1.Select(x => x.UST_ID).ToList()) + "" : "",
                            ((t.Name == "UNIT_SETTINGS") && (inversefks.ToRole.Name == "UNITS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNI_USET_ID")) ? string.Join(",", use.UNITS.Select(x => x.UNI_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UST_PARENT_USET_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNIT_SETTINGS") && (member.Name == "UST_ID")) ? use.UST_ID + "" : "",
                                //((t.Name == "UNIT_SETTINGS") && (member.Name == "UST_PARENT_USET_ID")) ? use.UST_PARENT_USET_ID + "" : "",
                                ((t.Name == "UNIT_SETTINGS") && (member.Name == "UST_NAME")) ? use.UST_NAME + "" : ""

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

        /// <summary>
        /// Genera el XML de UNIT_SETTING_SECTIONS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitSettingSectionsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNIT_SETTING_SECTIONS") foreach (var uss in dbContext.UNIT_SETTING_SECTIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "UNIT_SETTING_SECTIONS") && (inversefks.ToRole.Name == "UNIT_SETTING_PARAMETERS") && (inversefks.ToProperties.FirstOrDefault().Name == "USP_USS_ID")) ? string.Join(",", uss.UNIT_SETTING_PARAMETERS.Select(x => x.USP_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        //if ()
                        //{
                        XElement xmlMember = new XElement(convertString(member.Name),
                            ((t.Name == "UNIT_SETTING_SECTIONS") && (member.Name == "USS_ID")) ? uss.USS_ID + "" : "",
                            ((t.Name == "UNIT_SETTING_SECTIONS") && (member.Name == "USS_NAME")) ? uss.USS_NAME + "" : "", 
                            ((t.Name == "UNIT_SETTING_SECTIONS") && (member.Name == "USS_DESCRIPTION")) ? uss.USS_DESCRIPTION + "" : ""
                            );
                        xmlBaseEntity.Add(xmlMember);
                        if (xmlMember.Value == "")
                        {
                            xmlMember.ReplaceWith(new XElement(convertString(member.Name), atr3));
                        }
                        //}
                    }

                    xmlEntityValue.Add(xmlBaseEntity);
                }
        }

        /// <summary>
        /// Genera el XML de UNIT_SETTING_PARAMETERS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitSettingParametersXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNIT_SETTING_PARAMETERS") foreach (var usp in dbContext.UNIT_SETTING_PARAMETERS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNIT_SETTING_PARAMETERS") && (fks.ToProperties.FirstOrDefault().Name == "USP_USS_ID")) ? usp.USP_USS_ID + "" : "",
                            ((t.Name == "UNIT_SETTING_PARAMETERS") && (fks.ToProperties.FirstOrDefault().Name == "USP_USV_ID")) ? usp.USP_USV_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "UNIT_SETTING_PARAMETERS") && (inversefks.ToRole.Name == "UNIT_SETTINGS_DETAILS") && (inversefks.ToProperties.FirstOrDefault().Name == "USTD_USP_ID")) ? string.Join(",", usp.UNIT_SETTINGS_DETAILS.Select(x => x.USTD_ID).ToList()) + "" : "",
                            ((t.Name == "UNIT_SETTING_PARAMETERS") && (inversefks.ToRole.Name == "UNITS_SETTINGS_DETAILS_EXCEPTIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNIS_USP_ID")) ? string.Join(",", usp.UNITS_SETTINGS_DETAILS_EXCEPTIONS.Select(x => x.UNIS_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "USP_USS_ID") && (member.Name != "USP_USV_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNIT_SETTING_PARAMETERS") && (member.Name == "USP_ID")) ? usp.USP_ID + "" : "",
                                //((t.Name == "UNIT_SETTING_PARAMETERS") && (member.Name == "USP_USS_ID")) ? usp.USP_USS_ID + "" : "",
                                //((t.Name == "UNIT_SETTING_PARAMETERS") && (member.Name == "USP_USV_ID")) ? usp.USP_USV_ID + "" : "",
                                ((t.Name == "UNIT_SETTING_PARAMETERS") && (member.Name == "USP_NAME")) ? usp.USP_NAME + "" : ""
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

        /// <summary>
        /// Genera el XML de UNITS_SETTINGS_DETAILS_EXCEPTIONS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitsSettingDetailsExceptionsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNITS_SETTINGS_DETAILS_EXCEPTIONS") foreach (var usde in dbContext.UNITS_SETTINGS_DETAILS_EXCEPTIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS_SETTINGS_DETAILS_EXCEPTIONS") && (fks.ToProperties.FirstOrDefault().Name == "UNIS_UNI_ID")) ? usde.UNIS_UNI_ID + "" : "",
                            ((t.Name == "UNITS_SETTINGS_DETAILS_EXCEPTIONS") && (fks.ToProperties.FirstOrDefault().Name == "UNIS_USP_ID")) ? usde.UNIS_USP_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UNIS_UNI_ID") && (member.Name != "UNIS_USP_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNITS_SETTINGS_DETAILS_EXCEPTIONS") && (member.Name == "UNIS_ID")) ? usde.UNIS_ID + "" : "",
                                //((t.Name == "UNITS_SETTINGS_DETAILS_EXCEPTIONS") && (member.Name == "UNIS_UNI_ID")) ? usde.UNIS_UNI_ID + "" : "",
                                //((t.Name == "UNITS_SETTINGS_DETAILS_EXCEPTIONS") && (member.Name == "UNIS_USP_ID")) ? usde.UNIS_USP_ID + "" : "",
                                ((t.Name == "UNITS_SETTINGS_DETAILS_EXCEPTIONS") && (member.Name == "UNIS_PARAM_VALUE")) ? usde.UNIS_PARAM_VALUE + "" : ""
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

        /// <summary>
        /// Genera el XML de UNIT_SETTINGS_DETAILS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitSettingDetailsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNIT_SETTINGS_DETAILS") foreach (var usd in dbContext.UNIT_SETTINGS_DETAILS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNIT_SETTINGS_DETAILS") && (fks.ToProperties.FirstOrDefault().Name == "USTD_USP_ID")) ? usd.USTD_USP_ID + "" : "",
                            ((t.Name == "UNIT_SETTINGS_DETAILS") && (fks.ToProperties.FirstOrDefault().Name == "USTD_UST_ID")) ? usd.USTD_UST_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "USTD_USP_ID") && (member.Name != "USTD_UST_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNIT_SETTINGS_DETAILS") && (member.Name == "USTD_ID")) ? usd.USTD_ID + "" : "",
                                //((t.Name == "UNIT_SETTINGS_DETAILS") && (member.Name == "USTD_USP_ID")) ? usd.USTD_USP_ID + "" : "",
                                //((t.Name == "UNIT_SETTINGS_DETAILS") && (member.Name == "USTD_UST_ID")) ? usd.USTD_UST_ID + "" : "",
                                ((t.Name == "UNIT_SETTINGS_DETAILS") && (member.Name == "USTD_PARAM_VALUE")) ? usd.USTD_PARAM_VALUE + "" : ""
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

        /// <summary>
        /// Genera el XML de UNITS_STATUS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitsStatusXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNITS_STATUS") foreach (var uns in dbContext.UNITS_STATUS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS_STATUS") && (fks.ToProperties.FirstOrDefault().Name == "UNST_DAH_ID")) ? uns.UNST_DAH_ID + "" : "",
                            ((t.Name == "UNITS_STATUS") && (fks.ToProperties.FirstOrDefault().Name == "UNST_DAT_ID")) ? uns.UNST_DAT_ID + "" : "",
                            ((t.Name == "UNITS_STATUS") && (fks.ToProperties.FirstOrDefault().Name == "UNST_GRP_ID")) ? uns.UNST_GRP_ID + "" : "",
                            ((t.Name == "UNITS_STATUS") && (fks.ToProperties.FirstOrDefault().Name == "UNST_GRPT_ID")) ? uns.UNST_GRPT_ID + "" : "",
                            ((t.Name == "UNITS_STATUS") && (fks.ToProperties.FirstOrDefault().Name == "UNST_UNI_ID")) ? uns.UNST_UNI_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UNST_DAH_ID") && (member.Name != "UNST_DAT_ID") && (member.Name != "UNST_GRP_ID") && (member.Name != "UNST_GRPT_ID") && (member.Name != "UNST_UNI_ID") && (member.Name != "UNST_IN_DAY_INI_HOUR") && (member.Name != "UNST_IN_DAY_END_HOUR"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNITS_STATUS") && (member.Name == "UNST_ID")) ? uns.UNST_ID + "" : "",
                                //((t.Name == "UNITS_STATUS") && (member.Name == "UNST_DAH_ID")) ? uns.UNST_DAH_ID + "" : "",
                                //((t.Name == "UNITS_STATUS") && (member.Name == "UNST_DAT_ID")) ? uns.UNST_DAT_ID + "" : "",
                                //((t.Name == "UNITS_STATUS") && (member.Name == "UNST_GRP_ID")) ? uns.UNST_GRP_ID + "" : "",
                                //((t.Name == "UNITS_STATUS") && (member.Name == "UNST_GRPT_ID")) ? uns.UNST_GRPT_ID + "" : "",
                                //((t.Name == "UNITS_STATUS") && (member.Name == "UNST_UNI_ID")) ? uns.UNST_UNI_ID + "" : "",
                                ((t.Name == "UNITS_STATUS") && (member.Name == "UNST_STATUS")) ? uns.UNST_STATUS + "" : "",
                                ((t.Name == "UNITS_STATUS") && (member.Name == "UNST_DESCRIPTON")) ? uns.UNST_DESCRIPTON + "" : "",
                                ((t.Name == "UNITS_STATUS") && (member.Name == "UNST_INI_APPLY_DATE")) ? uns.UNST_INI_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "UNITS_STATUS") && (member.Name == "UNST_END_APPLY_DATE")) ? uns.UNST_END_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                //((t.Name == "UNITS_STATUS") && (member.Name == "UNST_IN_DAY_INI_HOUR")) ? uns.UNST_IN_DAY_INI_HOUR + "" : "",
                                //((t.Name == "UNITS_STATUS") && (member.Name == "UNST_IN_DAY_END_HOUR")) ? uns.UNST_IN_DAY_END_HOUR + "" : "",
                                ((t.Name == "UNITS_STATUS") && (member.Name == "UNST_IN_YEAR_INI_DATE")) ? uns.UNST_IN_YEAR_INI_DATE?.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "UNITS_STATUS") && (member.Name == "UNST_IN_YEAR_END_DATE")) ? uns.UNST_IN_YEAR_END_DATE?.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : ""
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

        /// <summary>
        /// Genera el XML de UNITS_LOCATIONS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void unitsLocationsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "UNITS_LOCATIONS") foreach (var unl in dbContext.UNITS_LOCATIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "UNITS_LOCATIONS") && (fks.ToProperties.FirstOrDefault().Name == "UNLO_UNI_ID")) ? unl.UNLO_UNI_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "UNLO_UNI_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "UNITS_LOCATIONS") && (member.Name == "UNLO_ID")) ? unl.UNLO_ID + "" : "",
                                //((t.Name == "UNITS_LOCATIONS") && (member.Name == "UNLO_UNI_ID")) ? unl.UNLO_UNI_ID + "" : "",
                                ((t.Name == "UNITS_LOCATIONS") && (member.Name == "UNLO_DESCRIPTION")) ? unl.UNLO_DESCRIPTION + "" : "",
                                ((t.Name == "UNITS_LOCATIONS") && (member.Name == "UNLO_INI_APPLY_DATE")) ? unl.UNLO_INI_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "UNITS_LOCATIONS") && (member.Name == "UNLO_END_APPLY_DATE")) ? unl.UNLO_END_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "UNITS_LOCATIONS") && (member.Name == "UNLO_GPS_LATITUDE")) ? unl.UNLO_GPS_LATITUDE.ToString(CultureInfo.InvariantCulture) + "" : "",
                                ((t.Name == "UNITS_LOCATIONS") && (member.Name == "UNLO_GPS_LONGITUDE")) ? unl.UNLO_GPS_LONGITUDE.ToString(CultureInfo.InvariantCulture) + "" : ""
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

        #endregion

        #region TARIFFS

        /// <summary>
        /// Genera el XML de RATE_BEHAVIOR_SETS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void rateBehaviourSetsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "RATE_BEHAVIOR_SETS") foreach (var rbs in dbContext.RATE_BEHAVIOR_SETS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "RATE_BEHAVIOR_SETS") && (fks.ToProperties.FirstOrDefault().Name == "RBS_INS_ID")) ? rbs.RBS_INS_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {                      
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        string strField = convertString(inversefks.ToRole.Name);
                        if ((t.Name == "RATE_BEHAVIOR_SETS") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_STEP") && (inversefks.ToProperties.FirstOrDefault().Name == "RBSS_RBS_ID")) strField = "RateBehaviorSteps";
                        XElement xmlField = new XElement("Field", strField);
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "RATE_BEHAVIOR_SETS") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_STEP") && (inversefks.ToProperties.FirstOrDefault().Name == "RBSS_RBS_ID")) ? string.Join(",", rbs.RATE_BEHAVIOR_STEP.Select(x => x.RBSS_ID).ToList()) + "" : "",
                            ((t.Name == "RATE_BEHAVIOR_SETS") && (inversefks.ToRole.Name == "TARIFFS_DEFINITION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TARDR_RBS_ID")) ? string.Join(",", rbs.TARIFFS_DEFINITION_RULES.Select(x => x.TARDR_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);     
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "RBS_INS_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "RATE_BEHAVIOR_SETS") && (member.Name == "RBS_ID")) ? rbs.RBS_ID + "" : "",
                                //((t.Name == "RATE_BEHAVIOR_SETS") && (member.Name == "RBS_INS_ID")) ? rbs.RBS_INS_ID + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_SETS") && (member.Name == "RBS_DESCRIPTION")) ? rbs.RBS_DESCRIPTION + "" : ""
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

        /// <summary>
        /// Genera el XML de TICKET_TYPES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void ticketTypesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "TICKET_TYPES") foreach (var tty in dbContext.TICKET_TYPES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {                       
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "TICKET_TYPES") && (fks.ToProperties.FirstOrDefault().Name == "TITY_INS_ID")) ? tty.TITY_INS_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "TICKET_TYPES") && (inversefks.ToRole.Name == "TICKETS_TYPES_FEATURES") && (inversefks.ToProperties.FirstOrDefault().Name == "TITF_TITY_ID")) ? string.Join(",", tty.TICKETS_TYPES_FEATURES.Select(x => x.TITF_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);                            
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "TITY_INS_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "TICKET_TYPES") && (member.Name == "TITY_ID")) ? tty.TITY_ID + "" : "",
                                //((t.Name == "TICKET_TYPES") && (member.Name == "TITY_INS_ID")) ? tty.TITY_INS_ID + "" : "",
                                ((t.Name == "TICKET_TYPES") && (member.Name == "TITY_DESCRIPTION1")) ? tty.TITY_DESCRIPTION1 + "" : "",
                                ((t.Name == "TICKET_TYPES") && (member.Name == "TITY_DESCRIPTION2")) ? tty.TITY_DESCRIPTION2 + "" : ""
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

        /// <summary>
        /// Genera el XML de TICKETS_TYPES_FEATURES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void ticketTypesFeaturesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "TICKETS_TYPES_FEATURES") foreach (var ttf in dbContext.TICKETS_TYPES_FEATURES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "TICKETS_TYPES_FEATURES") && (fks.ToProperties.FirstOrDefault().Name == "TITF_DAT_ID")) ? ttf.TITF_DAT_ID + "" : "",
                            ((t.Name == "TICKETS_TYPES_FEATURES") && (fks.ToProperties.FirstOrDefault().Name == "TITF_TITY_ID")) ? ttf.TITF_TITY_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);


                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "TITF_DAT_ID") && (member.Name != "TITF_TITY_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_ID")) ? ttf.TITF_ID + "" : "",
                                //((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_DAT_ID")) ? ttf.TITF_DAT_ID + "" : "",
                                //((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_TITY_ID")) ? ttf.TITF_TITY_ID + "" : "",
                                ((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_AMOUNT")) ? ttf.TITF_AMOUNT + "" : "",
                                ((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_COUNCIL_AMOUNT")) ? ttf.TITF_COUNCIL_AMOUNT + "" : "",
                                ((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_PAYABLE")) ? ttf.TITF_PAYABLE + "" : "",
                                ((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_PERIOD_TYPE")) ? ttf.TITF_PERIOD_TYPE + "" : "",
                                ((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_INIDATE")) ? ttf.TITF_INIDATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_ENDDATE")) ? ttf.TITF_ENDDATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_INI_NUM_MINUTES")) ? ttf.TITF_INI_NUM_MINUTES + "" : "",
                                ((t.Name == "TICKETS_TYPES_FEATURES") && (member.Name == "TITF_END_NUM_MINUTES")) ? ttf.TITF_END_NUM_MINUTES + "" : ""
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

        /// <summary>
        /// Genera el XML de RATE_STEPS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void rateStepsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "RATE_STEPS") foreach (var rst in dbContext.RATE_STEPS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "RATE_STEPS") && (fks.ToProperties.FirstOrDefault().Name == "RAS_RAT_ID")) ? rst.RAS_RAT_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "RATE_STEPS") && (inversefks.ToRole.Name == "RATE_TYPES") && (inversefks.ToProperties.FirstOrDefault().Name == "RAT_REPEAT_FROM_RAS_ID_INFINITELY")) ? string.Join(",", rst.RATE_TYPES1.Select(x => x.RAT_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "RAS_RAT_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "RATE_STEPS") && (member.Name == "RAS_ID")) ? rst.RAS_ID + "" : "",
                                //((t.Name == "RATE_STEPS") && (member.Name == "RAS_RAT_ID")) ? rst.RAS_RAT_ID + "" : "",
                                ((t.Name == "RATE_STEPS") && (member.Name == "RAS_DESCRIPTION")) ? rst.RAS_DESCRIPTION + "" : "",
                                ((t.Name == "RATE_STEPS") && (member.Name == "RAS_AMOUNT")) ? rst.RAS_AMOUNT.ToString(CultureInfo.InvariantCulture) + "" : "",
                                ((t.Name == "RATE_STEPS") && (member.Name == "RAS_HASH")) ? rst.RAS_HASH + "" : "",
                                ((t.Name == "RATE_STEPS") && (member.Name == "RAS_ORDER")) ? rst.RAS_ORDER + "" : "",
                                ((t.Name == "RATE_STEPS") && (member.Name == "RAS_NUM_MINUTES")) ? rst.RAS_NUM_MINUTES.ToString(CultureInfo.InvariantCulture) + "" : "",
                                ((t.Name == "RATE_STEPS") && (member.Name == "RAS_INTERVAL_VALUE_TYPE")) ? rst.RAS_INTERVAL_VALUE_TYPE + "" : "",
                                ((t.Name == "RATE_STEPS") && (member.Name == "RAS_INTERMEDIATE_VALUES_VALID")) ? rst.RAS_INTERMEDIATE_VALUES_VALID + "" : ""
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

        /// <summary>
        /// Genera el XML de TARIFF_CONSTRAINT_ENTRIES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void tariffConstraintEntriesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "TARIFF_CONSTRAINT_ENTRIES") foreach (var tce in dbContext.TARIFF_CONSTRAINT_ENTRIES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (fks.ToProperties.FirstOrDefault().Name == "TCE_GRP_ID")) ? tce.TCE_GRP_ID + "" : "",
                            ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (fks.ToProperties.FirstOrDefault().Name == "TCE_TCS_ID")) ? tce.TCE_TCS_ID + "" : "",
                            ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (fks.ToProperties.FirstOrDefault().Name == "TCE_GRPT_ID")) ? tce.TCE_GRPT_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "TCE_GRP_ID") && (member.Name != "TCE_TCS_ID") && (member.Name != "TCE_GRPT_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_ID")) ? tce.TCE_ID + "" : "",
                                //((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_GRP_ID")) ? tce.TCE_GRP_ID + "" : "",
                                //((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_TCS_ID")) ? tce.TCE_TCS_ID + "" : "",
                                //((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_GRPT_ID")) ? tce.TCE_GRPT_ID + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_DESCRIPTION")) ? tce.TCE_DESCRIPTION + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_ALLOW_EXTENSIONS")) ? tce.TCE_ALLOW_EXTENSIONS + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_ALLOW_REFUNDS")) ? tce.TCE_ALLOW_REFUNDS + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_COURTESY_TIME_IN_EXTENSION")) ? tce.TCE_COURTESY_TIME_IN_EXTENSION + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_COURTESY_TIME_MUST_BE_PAID")) ? tce.TCE_COURTESY_TIME_MUST_BE_PAID + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_EXTENSION_MAX_SLIDING_WINDOW")) ? tce.TCE_EXTENSION_MAX_SLIDING_WINDOW + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_INTRA_GROUP_PARK")) ? tce.TCE_INTRA_GROUP_PARK + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_INTRA_GROUP_PARK_COURTESY_TIME")) ? tce.TCE_INTRA_GROUP_PARK_COURTESY_TIME + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_NUM_TARIFF_MINUTES_FOR_NOT_APP_REENTRY")) ? tce.TCE_NUM_TARIFF_MINUTES_FOR_NOT_APP_REENTRY + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_PARALEL_PARKING_ALLOWED")) ? tce.TCE_PARALEL_PARKING_ALLOWED + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_QUERY_HISTORY_OPERATIONS")) ? tce.TCE_QUERY_HISTORY_OPERATIONS + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_MAX_PARKING_AMOUNT")) ? tce.TCE_MAX_PARKING_AMOUNT + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_MAX_PARKING_TIME")) ? tce.TCE_MAX_PARKING_TIME + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_MIN_AMOUNT_TO_BE_CHARGED_IN_EXTENSIONS")) ? tce.TCE_MIN_AMOUNT_TO_BE_CHARGED_IN_EXTENSIONS + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_MIN_AMOUNT_TO_BE_CHARGED_IN_REFUNDS")) ? tce.TCE_MIN_AMOUNT_TO_BE_CHARGED_IN_REFUNDS + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_MIN_PARKING_AMOUNT")) ? tce.TCE_MIN_PARKING_AMOUNT + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINT_ENTRIES") && (member.Name == "TCE_MIN_REENTRY_TIME")) ? tce.TCE_MIN_REENTRY_TIME + "" : ""
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

        /// <summary>
        /// Genera el XML de TARIFFS_DEFINITION_RULES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void tariffsDefinitionRulesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "TARIFFS_DEFINITION_RULES") foreach (var tdr in dbContext.TARIFFS_DEFINITION_RULES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
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

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "TARIFFS_DEFINITION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TARDR_DAH_ID")) ? tdr.TARDR_DAH_ID + "" : "",
                            ((t.Name == "TARIFFS_DEFINITION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TARDR_DAT_ID")) ? tdr.TARDR_DAT_ID + "" : "",
                            ((t.Name == "TARIFFS_DEFINITION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TARDR_GRPT_ID")) ? tdr.TARDR_GRPT_ID + "" : "",
                            ((t.Name == "TARIFFS_DEFINITION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TARDR_GRP_ID")) ? tdr.TARDR_GRP_ID + "" : "",
                            ((t.Name == "TARIFFS_DEFINITION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TARDR_RBS_ID")) ? tdr.TARDR_RBS_ID + "" : "",
                            ((t.Name == "TARIFFS_DEFINITION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TARDR_TAR_ID")) ? tdr.TARDR_TAR_ID + "" : "",
                            ((t.Name == "TARIFFS_DEFINITION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TARDR_TCS_ID")) ? tdr.TARDR_TCS_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "TARDR_DAH_ID") && (member.Name != "TARDR_DAT_ID") && (member.Name != "TARDR_GRPT_ID") && (member.Name != "TARDR_GRP_ID") && (member.Name != "TARDR_RBS_ID") && (member.Name != "TARDR_TAR_ID") && (member.Name != "TARDR_TCS_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_ID")) ? tdr.TARDR_ID + "" : "",
                                //((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_DAH_ID")) ? tdr.TARDR_DAH_ID + "" : "",
                                //((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_DAT_ID")) ? tdr.TARDR_DAT_ID + "" : "",
                                //((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_GRPT_ID")) ? tdr.TARDR_GRPT_ID + "" : "",
                                //((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_GRP_ID")) ? tdr.TARDR_GRP_ID + "" : "",
                                //((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_RBS_ID")) ? tdr.TARDR_RBS_ID + "" : "",
                                //((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_TAR_ID")) ? tdr.TARDR_TAR_ID + "" : "",
                                //((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_TCS_ID")) ? tdr.TARDR_TCS_ID + "" : "",
                                ((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_DESCRIPTON")) ? tdr.TARDR_DESCRIPTON + "" : "",
                                ((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_INI_APPLY_DATE")) ? tdr.TARDR_INI_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_END_APPLY_DATE")) ? tdr.TARDR_END_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_IN_DAY_INI_HOUR")) ? tdr.TARDR_IN_DAY_INI_HOUR + "" : "",
                                ((t.Name == "TARIFFS_DEFINITION_RULES") && (member.Name == "TARDR_IN_DAY_END_HOUR")) ? tdr.TARDR_IN_DAY_END_HOUR + "" : ""
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

        /// <summary>
        /// Genera el XML de DAY_TYPES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void dayTypesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "DAY_TYPES") foreach (var dty in dbContext.DAY_TYPES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    //xmlBaseEntity.SetAttributeValue(xsytype + "type", nombreTabla);

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "DAY_TYPES") && (fks.ToProperties.FirstOrDefault().Name == "DAT_INS_ID")) ? dty.DAT_INS_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        string strField = convertString(inversefks.ToRole.Name);
                        if ((t.Name == "DAY_TYPES") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_STEP") && (inversefks.ToProperties.FirstOrDefault().Name == "RBSS_DAT_ID")) strField = "RateBehaviorSteps";
                        XElement xmlField = new XElement("Field", strField);
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "DAY_TYPES") && (inversefks.ToRole.Name == "DAY_EXCEPTIONS") && (inversefks.ToProperties.FirstOrDefault().Name == "DEX_DAT_ID")) ? string.Join(",", dty.DAY_EXCEPTIONS.Select(x => x.DEX_ID).ToList()) + "" : "",
                            ((t.Name == "DAY_TYPES") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_STEP") && (inversefks.ToProperties.FirstOrDefault().Name == "RBSS_DAT_ID")) ? string.Join(",", dty.RATE_BEHAVIOR_STEP.Select(x => x.RBSS_ID).ToList()) + "" : "",
                            ((t.Name == "DAY_TYPES") && (inversefks.ToRole.Name == "TARIFFS_APPLICATION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TAPR_DAT_ID")) ? string.Join(",", dty.TARIFFS_APPLICATION_RULES.Select(x => x.TAPR_ID).ToList()) + "" : "",
                            ((t.Name == "DAY_TYPES") && (inversefks.ToRole.Name == "TARIFFS_DEFINITION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TARDR_DAT_ID")) ? string.Join(",", dty.TARIFFS_DEFINITION_RULES.Select(x => x.TARDR_ID).ToList()) + "" : "",
                            ((t.Name == "DAY_TYPES") && (inversefks.ToRole.Name == "TICKETS_TYPES_FEATURES") && (inversefks.ToProperties.FirstOrDefault().Name == "TITF_DAT_ID")) ? string.Join(",", dty.TICKETS_TYPES_FEATURES.Select(x => x.TITF_ID).ToList()) + "" : "",
                            ((t.Name == "DAY_TYPES") && (inversefks.ToRole.Name == "UNITS_STATUS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNST_DAT_ID")) ? string.Join(",", dty.UNITS_STATUS.Select(x => x.UNST_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if (member.Name != "DAT_INS_ID")
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "DAY_TYPES") && (member.Name == "DAT_ID")) ? dty.DAT_ID + "" : "",
                                //((t.Name == "DAY_TYPES") && (member.Name == "DAT_INS_ID")) ? dty.DAT_INS_ID + "" : "",
                                ((t.Name == "DAY_TYPES") && (member.Name == "DAT_DESCRIPTION")) ? dty.DAT_DESCRIPTION + "" : "",
                                ((t.Name == "DAY_TYPES") && (member.Name == "DAT_WEEK_MASK")) ? dty.DAT_WEEK_MASK + "" : ""
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

        /// <summary>
        /// Genera el XML de RATE_TYPES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void rateTypesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "RATE_TYPES") foreach (var rty in dbContext.RATE_TYPES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    //xmlBaseEntity.SetAttributeValue(xsytype + "type", nombreTabla);

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "RATE_TYPES") && (fks.ToProperties.FirstOrDefault().Name == "RAT_INS_ID")) ? rty.RAT_INS_ID + "" : "",
                            ((t.Name == "RATE_TYPES") && (fks.ToProperties.FirstOrDefault().Name == "RAT_REPEAT_FROM_RAS_ID_INFINITELY")) ? rty.RAT_REPEAT_FROM_RAS_ID_INFINITELY + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        string strField = convertString(inversefks.ToRole.Name);
                        if ((t.Name == "RATE_TYPES") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_STEP") && (inversefks.ToProperties.FirstOrDefault().Name == "RBSS_RAT_ID")) strField = "RateBehaviorSteps";
                        XElement xmlField = new XElement("Field", strField);
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "RATE_TYPES") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_STEP") && (inversefks.ToProperties.FirstOrDefault().Name == "RBSS_RAT_ID")) ? string.Join(",", rty.RATE_BEHAVIOR_STEP.Select(x => x.RBSS_ID).ToList()) + "" : "",
                            ((t.Name == "RATE_TYPES") && (inversefks.ToRole.Name == "RATE_STEPS") && (inversefks.ToProperties.FirstOrDefault().Name == "RAS_RAT_ID")) ? string.Join(",", rty.RATE_STEPS.Select(x => x.RAS_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "RAT_INS_ID") && (member.Name != "RAT_REPEAT_FROM_RAS_ID_INFINITELY"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "RATE_TYPES") && (member.Name == "RAT_ID")) ? rty.RAT_ID + "" : "",
                                //((t.Name == "RATE_TYPES") && (member.Name == "RAT_INS_ID")) ? rty.RAT_INS_ID + "" : "",
                                //((t.Name == "RATE_TYPES") && (member.Name == "RAT_REPEAT_FROM_RAS_ID_INFINITELY")) ? rty.RAT_REPEAT_FROM_RAS_ID_INFINITELY + "" : "",
                                ((t.Name == "RATE_TYPES") && (member.Name == "RAT_DESCRIPTION")) ? rty.RAT_DESCRIPTION + "" : ""
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

        /// <summary>
        /// Genera el XML de RATE_BEHAVIOR_STEP
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void rateBehaviorStepXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "RATE_BEHAVIOR_STEP") foreach (var rbs in dbContext.RATE_BEHAVIOR_STEP.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    //xmlBaseEntity.SetAttributeValue(xsytype + "type", nombreTabla);

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "RATE_BEHAVIOR_STEP") && (fks.ToProperties.FirstOrDefault().Name == "RBSS_DAT_ID")) ? rbs.RBSS_DAT_ID + "" : "",
                            ((t.Name == "RATE_BEHAVIOR_STEP") && (fks.ToProperties.FirstOrDefault().Name == "RBSS_DEX_ID")) ? rbs.RBSS_DEX_ID + "" : "",
                            ((t.Name == "RATE_BEHAVIOR_STEP") && (fks.ToProperties.FirstOrDefault().Name == "RBSS_RAT_ID")) ? rbs.RBSS_RAT_ID + "" : "",
                            ((t.Name == "RATE_BEHAVIOR_STEP") && (fks.ToProperties.FirstOrDefault().Name == "RBSS_RBS_ID")) ? rbs.RBSS_RBS_ID + "" : "",
                            ((t.Name == "RATE_BEHAVIOR_STEP") && (fks.ToProperties.FirstOrDefault().Name == "RBSS_DAH")) ? rbs.RBSS_DAH + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "RBSS_DAT_ID") && (member.Name != "RBSS_DEX_ID") && (member.Name != "RBSS_RAT_ID") && (member.Name != "RBSS_RBS_ID") && (member.Name != "RBSS_DAH"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_ID")) ? rbs.RBSS_ID + "" : "",
                                //((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_DAT_ID")) ? rbs.RBSS_DAT_ID + "" : "",
                                //((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_DEX_ID")) ? rbs.RBSS_DEX_ID + "" : "",
                                //((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_RAT_ID")) ? rbs.RBSS_RAT_ID + "" : "",
                                //((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_RBS_ID")) ? rbs.RBSS_RBS_ID + "" : "",
                                //((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_DAH")) ? rbs.RBSS_DAH + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_DESCRIPTION")) ? rbs.RBSS_DESCRIPTION + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_DATE")) ? rbs.RBSS_DATE?.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_DAY_NUM_OF_APPLICATION")) ? rbs.RBSS_DAY_NUM_OF_APPLICATION + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_ALLOW_JUMP_NEXT_BLOCK")) ? rbs.RBSS_ALLOW_JUMP_NEXT_BLOCK + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_CONDITIONAL_VALUE_FOR_NEXT_BLOCK")) ? rbs.RBSS_CONDITIONAL_VALUE_FOR_NEXT_BLOCK + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_INI_APPLY_DATE")) ? rbs.RBSS_INI_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_END_APPLY_DATE")) ? rbs.RBSS_END_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_INI_HOUR")) ? rbs.RBSS_INI_HOUR + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_END_HOUR")) ? rbs.RBSS_END_HOUR + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_ORDER")) ? rbs.RBSS_ORDER + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_RESET_AMOUNT_WHEN_JUMP_NEXT_BLOCK")) ? rbs.RBSS_RESET_AMOUNT_WHEN_JUMP_NEXT_BLOCK + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_RESET_TIME_WHEN_JUMP_NEXT_BLOCK")) ? rbs.RBSS_RESET_TIME_WHEN_JUMP_NEXT_BLOCK + "" : "",
                                ((t.Name == "RATE_BEHAVIOR_STEP") && (member.Name == "RBSS_ROUND_FINAL_PARKING_TO_STEP_END")) ? rbs.RBSS_ROUND_FINAL_PARKING_TO_STEP_END + "" : ""
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

        /// <summary>
        /// Genera el XML de DAY_EXCEPTIONS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void dayExceptionsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "DAY_EXCEPTIONS") foreach (var dex in dbContext.DAY_EXCEPTIONS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    //xmlBaseEntity.SetAttributeValue(xsytype + "type", nombreTabla);

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "DAY_EXCEPTIONS") && (fks.ToProperties.FirstOrDefault().Name == "DEX_DAT_ID")) ? dex.DEX_DAT_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        string strField = convertString(inversefks.ToRole.Name);
                        if ((t.Name == "DAY_EXCEPTIONS") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_STEP") && (inversefks.ToProperties.FirstOrDefault().Name == "RBSS_DEX_ID")) strField = "RateBehaviorSteps";
                        XElement xmlField = new XElement("Field", strField);
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "DAY_EXCEPTIONS") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_STEP") && (inversefks.ToProperties.FirstOrDefault().Name == "RBSS_DEX_ID")) ? string.Join(",", dex.RATE_BEHAVIOR_STEP.Select(x => x.RBSS_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if (member.Name != "DEX_DAT_ID")
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "DAY_EXCEPTIONS") && (member.Name == "DEX_ID")) ? dex.DEX_ID + "" : "",
                                //((t.Name == "DAY_EXCEPTIONS") && (member.Name == "DEX_DAT_ID")) ? dex.DEX_DAT_ID + "" : "",
                                ((t.Name == "DAY_EXCEPTIONS") && (member.Name == "DEX_DATE")) ? dex.DEX_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "DAY_EXCEPTIONS") && (member.Name == "DEX_REPEAT_EVERY_YEAR")) ? dex.DEX_REPEAT_EVERY_YEAR + "" : ""
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

        /// <summary>
        /// Genera el XML de TARIFF_CONSTRAINTS_SETS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void tariffConstraintsSetsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "TARIFF_CONSTRAINTS_SETS") foreach (var tcs in dbContext.TARIFF_CONSTRAINTS_SETS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    //xmlBaseEntity.SetAttributeValue(xsytype + "type", nombreTabla);

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "TARIFF_CONSTRAINTS_SETS") && (fks.ToProperties.FirstOrDefault().Name == "TCS_INS_ID")) ? tcs.TCS_INS_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "TARIFF_CONSTRAINTS_SETS") && (inversefks.ToRole.Name == "TARIFFS_APPLICATION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TAPR_TCS_ID")) ? string.Join(",", tcs.TARIFFS_APPLICATION_RULES.Select(x => x.TAPR_ID).ToList()) + "" : "",
                            ((t.Name == "TARIFF_CONSTRAINTS_SETS") && (inversefks.ToRole.Name == "TARIFFS_DEFINITION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TARDR_TCS_ID")) ? string.Join(",", tcs.TARIFFS_DEFINITION_RULES.Select(x => x.TARDR_ID).ToList()) + "" : "",
                            ((t.Name == "TARIFF_CONSTRAINTS_SETS") && (inversefks.ToRole.Name == "TARIFF_CONSTRAINT_ENTRIES") && (inversefks.ToProperties.FirstOrDefault().Name == "TCE_TCS_ID")) ? string.Join(",", tcs.TARIFF_CONSTRAINT_ENTRIES.Select(x => x.TCE_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if (member.Name != "TCS_INS_ID")
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "TARIFF_CONSTRAINTS_SETS") && (member.Name == "TCS_ID")) ? tcs.TCS_ID + "" : "",
                                //((t.Name == "TARIFF_CONSTRAINTS_SETS") && (member.Name == "TCS_INS_ID")) ? tcs.TCS_INS_ID + "" : "",
                                ((t.Name == "TARIFF_CONSTRAINTS_SETS") && (member.Name == "TCS_DESCRIPTION")) ? tcs.TCS_DESCRIPTION + "" : ""
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

        /// <summary>
        /// Genera el XML de TARIFFS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void tariffsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "TARIFFS") foreach (var tar in dbContext.TARIFFS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    //xmlBaseEntity.SetAttributeValue(xsytype + "type", nombreTabla);

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "TARIFFS") && (fks.ToProperties.FirstOrDefault().Name == "TAR_INS_ID")) ? tar.TAR_INS_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "TARIFFS") && (inversefks.ToRole.Name == "TARIFFS_APPLICATION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TAPR_TAR_ID")) ? string.Join(",", tar.TARIFFS_APPLICATION_RULES.Select(x => x.TAPR_ID).ToList()) + "" : "",
                            ((t.Name == "TARIFFS") && (inversefks.ToRole.Name == "TARIFFS_DEFINITION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TARDR_TAR_ID")) ? string.Join(",", tar.TARIFFS_DEFINITION_RULES.Select(x => x.TARDR_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if (member.Name != "TAR_INS_ID")
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "TARIFFS") && (member.Name == "TAR_ID")) ? tar.TAR_ID + "" : "",
                                //((t.Name == "TARIFFS") && (member.Name == "TAR_INS_ID")) ? tar.TAR_INS_ID + "" : "",
                                ((t.Name == "TARIFFS") && (member.Name == "TAR_DESCRIPTION")) ? tar.TAR_DESCRIPTION + "" : "",
                                ((t.Name == "TARIFFS") && (member.Name == "TAR_QUERY_EXT_ID")) ? tar.TAR_QUERY_EXT_ID + "" : "",
                                ((t.Name == "TARIFFS") && (member.Name == "TAR_EXT1_ID")) ? tar.TAR_EXT1_ID + "" : "",
                                ((t.Name == "TARIFFS") && (member.Name == "TAR_EXT2_ID")) ? tar.TAR_EXT2_ID + "" : "",
                                ((t.Name == "TARIFFS") && (member.Name == "TAR_EXT3_ID")) ? tar.TAR_EXT3_ID + "" : ""
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

        /// <summary>
        /// Genera el XML de TARIFFS_APPLICATION_RULES
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void tariffsApplicationRulesXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "TARIFFS_APPLICATION_RULES") foreach (var tar in dbContext.TARIFFS_APPLICATION_RULES.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    //xmlBaseEntity.SetAttributeValue(xsytype + "type", nombreTabla);

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "TARIFFS_APPLICATION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TAPR_DAH_ID")) ? tar.TAPR_DAH_ID + "" : "",
                            ((t.Name == "TARIFFS_APPLICATION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TAPR_DAT_ID")) ? tar.TAPR_DAT_ID + "" : "",
                            ((t.Name == "TARIFFS_APPLICATION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TAPR_GRPT_ID")) ? tar.TAPR_GRPT_ID + "" : "",
                            ((t.Name == "TARIFFS_APPLICATION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TAPR_GRP_ID")) ? tar.TAPR_GRP_ID + "" : "",
                            ((t.Name == "TARIFFS_APPLICATION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TAPR_TAR_ID")) ? tar.TAPR_TAR_ID + "" : "",
                            ((t.Name == "TARIFFS_APPLICATION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TAPR_TCS_ID")) ? tar.TAPR_TCS_ID + "" : "",
                            ((t.Name == "TARIFFS_APPLICATION_RULES") && (fks.ToProperties.FirstOrDefault().Name == "TAPR_UNI_ID")) ? tar.TAPR_UNI_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        XElement xmlField = new XElement("Field", convertString(inversefks.ToRole.Name));
                        XElement xmlFieldId = new XElement("IdsString",""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if ((member.Name != "TAPR_DAH_ID") && (member.Name != "TAPR_DAT_ID") && (member.Name != "TAPR_GRPT_ID") && (member.Name != "TAPR_GRP_ID") && (member.Name != "TAPR_TAR_ID") && (member.Name != "TAPR_TCS_ID") && (member.Name != "TAPR_UNI_ID"))
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_ID")) ? tar.TAPR_ID + "" : "",
                                //((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_DAH_ID")) ? tar.TAPR_DAH_ID + "" : "",
                                //((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_DAT_ID")) ? tar.TAPR_DAT_ID + "" : "",
                                //((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_GRPT_ID")) ? tar.TAPR_GRPT_ID + "" : "",
                                //((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_GRP_ID")) ? tar.TAPR_GRP_ID + "" : "",
                                //((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_TAR_ID")) ? tar.TAPR_TAR_ID + "" : "",
                                //((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_TCS_ID")) ? tar.TAPR_TCS_ID + "" : "",
                                //((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_UNI_ID")) ? tar.TAPR_UNI_ID + "" : 
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_DESCRIPTON")) ? tar.TAPR_DESCRIPTON + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_INSERT_PLATE")) ? tar.TAPR_INSERT_PLATE + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_CONNECTION_REQUIRED")) ? tar.TAPR_CONNECTION_REQUIRED + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_ACCESS_CARD_NUM")) ? tar.TAPR_ACCESS_CARD_NUM + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_ACCESS_CARD_TYPE")) ? tar.TAPR_ACCESS_CARD_TYPE + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_ALLOW_REMOTE_EXTENSION")) ? tar.TAPR_ALLOW_REMOTE_EXTENSION + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_ALLOW_USER_SELECT")) ? tar.TAPR_ALLOW_USER_SELECT + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_INI_APPLY_DATE")) ? tar.TAPR_INI_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_END_APPLY_DATE")) ? tar.TAPR_END_APPLY_DATE.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_IN_YEAR_INI_DATE")) ? tar.TAPR_IN_YEAR_INI_DATE?.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_IN_YEAR_END_DATE")) ? tar.TAPR_IN_YEAR_END_DATE?.ToString("yyyy-MM-ddTHH:mm:ssK") + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TARP_IN_DAY_INI_HOUR")) ? tar.TARP_IN_DAY_INI_HOUR + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TARP_IN_DAY_END_HOUR")) ? tar.TARP_IN_DAY_END_HOUR + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_TIMEAMOUNT_SEL_TYPE")) ? tar.TAPR_TIMEAMOUNT_SEL_TYPE + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_TARIFF_ENGINE")) ? tar.TAPR_TARIFF_ENGINE + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_SPACE_MANAGMENT_TYPE")) ? tar.TAPR_SPACE_MANAGMENT_TYPE + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_SHOW_ORDER")) ? tar.TAPR_SHOW_ORDER + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_PLATE_TARIFF_ASSIGN_TYPE")) ? tar.TAPR_PLATE_TARIFF_ASSIGN_TYPE + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_PAY_MODE")) ? tar.TAPR_PAY_MODE + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_INC1")) ? tar.TAPR_INC1 + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_INC2")) ? tar.TAPR_INC2 + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_INC3")) ? tar.TAPR_INC3 + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_INC4")) ? tar.TAPR_INC4 + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_INC5")) ? tar.TAPR_INC5 + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_DEC1")) ? tar.TAPR_DEC1 + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_DEC2")) ? tar.TAPR_DEC2 + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_DEC3")) ? tar.TAPR_DEC3 + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_DEC4")) ? tar.TAPR_DEC4 + "" : "",
                                ((t.Name == "TARIFFS_APPLICATION_RULES") && (member.Name == "TAPR_DEC5")) ? tar.TAPR_DEC5 + "" : ""
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

        /// <summary>
        /// Genera el XML de DAY_HOURS_INTERVALS
        /// </summary>
        /// <param name="t">entidad tabla</param>
        /// <param name="dbContext">contexto</param>
        /// <param name="xmlEntityValue">elemneto XML padre donde se incluye</param>
        public static void dayHoursIntervalsXML(EntitySet t, BCTOTA2PAEntitiesCompleto dbContext, XElement xmlEntityValue, string nombreTabla)
        {
            if (t.Name == "DAY_HOURS_INTERVALS") foreach (var dhi in dbContext.DAY_HOURS_INTERVALS.ToList())
                {
                    XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                    XAttribute atr1 = new XAttribute(XNamespace.Xmlns + "xsi", xsi);
                    XAttribute atr2 = new XAttribute(xsi + "type", nombreTabla);
                    XAttribute atr3 = new XAttribute(xsi + "nil", "true");
                    XElement xmlBaseEntity = new XElement("BaseEntity", atr1, atr2);
                    atr1.Remove();

                    //xmlBaseEntity.SetAttributeValue(xsytype + "type", nombreTabla);

                    foreach (var keymember in t.ElementType.KeyMembers)
                    {
                        XElement xmlFieldsPK = new XElement("FieldsPK");
                        XElement xmlField = new XElement("string", convertString(keymember.Name));
                        xmlFieldsPK.Add(xmlField);
                        xmlBaseEntity.Add(xmlFieldsPK);
                    }

                    XElement xmlFKs = new XElement("FKs");
                    IEnumerable<ReferentialConstraint> arrFKs = t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.ToRole.Name == t.Name);
                    foreach (var fks in arrFKs)
                    {
                        XElement xmlFKInfo = new XElement("FKInfo");
                        XElement xmlField = new XElement("Field", convertStringFK(fks.ToProperties.FirstOrDefault().Name));
                        XElement xmlFieldId = new XElement("Id",
                            ((t.Name == "DAY_HOURS_INTERVALS") && (fks.ToProperties.FirstOrDefault().Name == "DAH_INS_ID")) ? dhi.DAH_INS_ID + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlFKs.Add(xmlFKInfo);
                        }
                    }
                    if (arrFKs.Any()) xmlBaseEntity.Add(xmlFKs);

                    XElement xmlInverseFKs = new XElement("InverseFKs");
                    foreach (var inversefks in t.EntityContainer.AssociationSets.SelectMany(x => x.ElementType.ReferentialConstraints).Where(z => z.FromRole.Name == t.Name))
                    {
                        XElement xmlFKInfo = new XElement("InverseFKInfo");
                        string strField = convertString(inversefks.ToRole.Name);
                        if ((t.Name == "DAY_HOURS_INTERVALS") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_STEP") && (inversefks.ToProperties.FirstOrDefault().Name == "RBSS_DAH")) strField = "RateBehaviorSteps";
                        XElement xmlField = new XElement("Field", strField);
                        XElement xmlFieldId = new XElement("IdsString",
                            ((t.Name == "DAY_HOURS_INTERVALS") && (inversefks.ToRole.Name == "RATE_BEHAVIOR_STEP") && (inversefks.ToProperties.FirstOrDefault().Name == "RBSS_DAH")) ? string.Join(",", dhi.RATE_BEHAVIOR_STEP.Select(x => x.RBSS_ID).ToList()) + "" : "",
                            ((t.Name == "DAY_HOURS_INTERVALS") && (inversefks.ToRole.Name == "TARIFFS_APPLICATION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TAPR_DAH_ID")) ? string.Join(",", dhi.TARIFFS_APPLICATION_RULES.Select(x => x.TAPR_ID).ToList()) + "" : "",
                            ((t.Name == "DAY_HOURS_INTERVALS") && (inversefks.ToRole.Name == "TARIFFS_DEFINITION_RULES") && (inversefks.ToProperties.FirstOrDefault().Name == "TARDR_DAH_ID")) ? string.Join(",", dhi.TARIFFS_DEFINITION_RULES.Select(x => x.TARDR_ID).ToList()) + "" : "",
                            ((t.Name == "DAY_HOURS_INTERVALS") && (inversefks.ToRole.Name == "UNITS_STATUS") && (inversefks.ToProperties.FirstOrDefault().Name == "UNST_DAH_ID")) ? string.Join(",", dhi.UNITS_STATUS.Select(x => x.UNST_ID).ToList()) + "" : ""
                            );
                        if (xmlFieldId.Value != "")
                        {
                            xmlFKInfo.Add(xmlField);
                            xmlFKInfo.Add(xmlFieldId);
                            xmlInverseFKs.Add(xmlFKInfo);
                        }
                    }
                    xmlBaseEntity.Add(xmlInverseFKs);

                    foreach (var member in t.ElementType.Members)
                    {
                        if (member.Name != "DAH_INS_ID")
                        {
                            XElement xmlMember = new XElement(convertString(member.Name),
                                ((t.Name == "DAY_HOURS_INTERVALS") && (member.Name == "DAH_ID")) ? dhi.DAH_ID + "" : "",
                                //((t.Name == "DAY_HOURS_INTERVALS") && (member.Name == "DAH_INS_ID")) ? dhi.DAH_INS_ID + "" : "",
                                ((t.Name == "DAY_HOURS_INTERVALS") && (member.Name == "DAH_DESCRIPTION")) ? dhi.DAH_DESCRIPTION + "" : "",
                                ((t.Name == "DAY_HOURS_INTERVALS") && (member.Name == "DAH_INI_HOUR")) ? dhi.DAH_INI_HOUR + "" : "",
                                ((t.Name == "DAY_HOURS_INTERVALS") && (member.Name == "DAH_END_HOUR")) ? dhi.DAH_END_HOUR + "" : ""
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

        #endregion

    }
}
