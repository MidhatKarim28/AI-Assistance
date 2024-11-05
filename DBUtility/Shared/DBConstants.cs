
using System.ComponentModel;

namespace DBUtility
{
    public static class DBConstants
    {
        public readonly static string ALL_HIS_FACILITIES = "*"; 
    }

    #region Enums 
    public enum enDatabaseConnections
    {
        [Description("DEV")]
        XDEV,
        [Description("HIS.DEV")]
        XHIS_DEV,
        [Description("HIS.UAT")]
        HIS_UAT,
        [Description("PCI.DEV")]
        XPCI_DEV,
        [Description("PCI.UAT")]
        PCI_UAT,
        [Description("BB.UAT")]
        BB_UAT,
        [Description("OPM.DEV")]
        OPMS_DEV,
        [Description("OPM.UAT")]
        OPMS_UAT,
        ILMS_DEV,
        ILMS_UAT,
        CSSA_DEV,
        CSSA_UAT,
        CCM_DEV,
        CCM_UAT,
        ENR_DEV,
        ENR_UAT,
        OPM_DEV,
        OPM_UAT,
        RAD_DEV,
        RAD_UAT,
        IR_DEV,
        IR_UAT,
        PCIDefaultDataSource,
        PRIDefaultDataSource,
        PHADefaultDataSource,
        BBDefaultDataSource,
        HISDefaultDataSource,
        HISStadiumDataSource,
        RADDefaultDataSource,
        CCMDefaultDataSource,
        ENRDefaultDataSource,
        CPSDefaultDataSource,
        NPSDefaultDataSource,
        OPMDefaultDataSource,
        ILMDefaultDataSource,
        IRDefaultDataSource,
        CSSADefaultDataSource,
        ISMDefaultDataSource,
        IIBDefaultDataSource,
        IBMDefaultDataSource,
        ADTDefaultDataSource,
        MRIDefaultDataSource
    }

    // From DB 
    //STADIUM         Y Stadium Road
    //GARDEN          Y Garden
    //KARIMABAD       Y Karimabad
    //KHARADAR        Y Kharadar
    //HYDERABAD       Y Hyderabad
    public enum enHISDBSchema
    {
        PCI,
        HIS,
        CSSA,
        ADM, 
        OPM,
        ILM,
        CPS,
        NPS,
        BB,
        RAD,
        PHA,
        ISM
    }

    public enum enHISFacility
    {
        [Description("DEV")]
        DEV,
        [Description("Stadium Road")]
        UAT,
        [Description("Stadium Road")]
        STADIUM,
        [Description("Stadium Road")]
        STADIUMROAD,
        [Description("Garden")]
        GARDEN,
        [Description("Karimabad")]
        KARIMABAD,
        [Description("Kharadar")]
        KHARADAR,
        [Description("Hyderabad")]
        HYDERABAD
    }

    public enum enHISFacilityDataSource
    {
        [Description("dev")]
        DEV,
        [Description("nakuprod")]
        UAT,
        [Description("akuprod")] 
        STADIUM,
        [Description("akuprod")] 
        STADIUMROAD,
        [Description("akhsprdg")]
        GARDEN,
        [Description("akhsprdr")]
        KARIMABAD,
        [Description("akhsprdk")]
        KHARADAR,
        [Description("akhsprdh")]
        HYDERABAD
    }

    #endregion
}
