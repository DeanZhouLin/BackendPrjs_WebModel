using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Com.BaseLibrary.Configuration;

namespace Jufine.Backend.WebModel
{
    [XmlRoot("websiteConfiguration")]
    public class WebsiteConfiguration
    {
        public static WebsiteConfiguration Current
        {
            get
            {
                return ConfigurationManager.LoadConfiguration<WebsiteConfiguration>("Config/Website.config");
            }
        }

        [XmlElement("refundableItemTypes")]
        public string RefundableItemTypes { get; set; }

        public List<int> RefundableItemTypesList
        {
            get
            {
                var res = new List<int>();
                if (string.IsNullOrEmpty(RefundableItemTypes))
                {
                    return res;
                }
                foreach (var refundableItemType in RefundableItemTypes.Split(',').Where(t => !string.IsNullOrEmpty(t.Trim())))
                {
                    int t;
                    if (int.TryParse(refundableItemType, out t))
                    {
                        res.Add(t);
                    }
                }
                return res;
            }
        }

        [XmlElement("previewEDMUrl")]
        public string PreviewEDMUrl { get; set; }

        [XmlElement("isShow3rdPrice")]
        public bool IsShow3rdPrice { get; set; }

        [XmlElement("additionalPriceRangEditAble")]
        public bool AdditionalPriceRangEditAble { get; set; }
        [XmlElement("additionalPriceNumQty")]
        public string AdditionalPriceNumQty { get; set; }
        [XmlElement("additionalPriceOverDueDays")]
        public int AdditionalPriceOverDueDays { get; set; }

        [XmlElement("addedScore")]
        public int AddedScore { get; set; }

        [XmlElement("bonusScore")]
        public int BonusScore { get; set; }

        [XmlArray("itemDescriptionReplace")]
        [XmlArrayItem("replace", typeof(ItemDescriptionReplace))]
        public List<ItemDescriptionReplace> ItemDescriptionReplaceList { get; set; }

        [XmlArray("merchantCooperationModes")]
        [XmlArrayItem("CooperationMode", typeof(MerchantCooperationModes))]
        public List<MerchantCooperationModes> MerchantCooperationModesList { get; set; }

        [XmlElement("wwwItemUrl")]
        public string WWWItemUrl { get; set; }


        [XmlElement("watermarkImageName")]
        public string WatermarkImageName { get; set; }

        [XmlElement("scoreForSalePercent")]
        public decimal ScoreForSalePercent { get; set; }
        [XmlElement("scoreForSaleMax")]
        public int ScoreForSaleMax { get; set; }

        [XmlElement("jhbForSalePercent")]
        public decimal JhbForSalePercent { get; set; }
        [XmlElement("jhbForSaleMax")]
        public decimal jhbForSaleMax { get; set; }

        [XmlElement("jhbdecimalPlaces")]
        public int jhbdecimalPlaces { get; set; }

        [XmlElement("itemStockImportTemp")]
        public string ItemStockImportTemp { get; set; }

        [XmlElement("itemImportTemp")]
        public string ItemImportTemp { get; set; }

        [XmlArray("uploadFileSetting")]
        [XmlArrayItem("setting", typeof(UploadFileSettingInfo))]
        public List<UploadFileSettingInfo> UploadFileSettingList { get; set; }

        [XmlElement("unzipMagazinePath")]
        public string UnzipMagazinePath { get; set; }

        [XmlElement("magazineURLPath")]
        public string MagazineURLPath { get; set; }

        [XmlElement("flashXMLPath")]
        public string FlashXMLPath { get; set; }

        [XmlElement("AuditScoreCSManager")]
        public string AuditScoreCSManager { get; set; }

        [XmlElement("AuditScorePMManager")]
        public string AuditScorePMManager { get; set; }

        [XmlElement("AuditScoreFINManager")]
        public string AuditScoreFINManager { get; set; }

        [XmlElement("AuditScoreCEO")]
        public string AuditScoreCEO { get; set; }

        [XmlElement("RMBToScore")]
        public int RMBToScore { get; set; }

        [XmlElement("ExportScoreFile")]
        public string ExportScoreFile { get; set; }

        [XmlElement("ExportScoreFileUrl")]
        public string ExportScoreFileUrl { get; set; }

    }

    public class ItemDescriptionReplace
    {
        [XmlElement("oldString")]
        public string OldString { get; set; }
        [XmlElement("newString")]
        public string NewString { get; set; }
    }

    public class MerchantCooperationModes
    {
        [XmlElement("key")]
        public string Key { get; set; }
        [XmlElement("value")]
        public string Value { get; set; }
    }
    public class UploadFileSettingInfo
    {

        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public bool IsUploadImage { get; set; }
        [XmlAttribute]
        public string ImageUrlPath { get; set; }
        [XmlAttribute]
        public int ImageWidth { get; set; }
        [XmlAttribute]
        public int ImageHeight { get; set; }

        [XmlAttribute]
        public string FileExtentions { get; set; }

        [XmlAttribute]
        public string ButtonText { get; set; }

        [XmlAttribute]
        public bool NotShowOKMessage { get; set; }

        [XmlElement("item")]
        public List<UploadFileItemInfo> UploadFileItemList { get; set; }

        [XmlArray("copyTo")]
        [XmlArrayItem("copyToItem", typeof(CopyToItem))]
        public List<CopyToItem> CopyTo { get; set; }
    }

    public class UploadFileItemInfo
    {
        [XmlAttribute]
        public string UploadFilePath { get; set; }

        [XmlAttribute]
        public bool IsLocalPath { get; set; }

        [XmlAttribute]
        public string UserName { get; set; }
        [XmlAttribute]
        public string Password { get; set; }
        [XmlAttribute]
        public string Domain { get; set; }


    }

    public class CopyToItem
    {
        [XmlAttribute]
        public string NewImagePath { get; set; }

        [XmlAttribute]
        public int ImageWidth { get; set; }

        [XmlAttribute]
        public int ImageHeight { get; set; }

        [XmlAttribute]
        public bool IsCcale { get; set; }

        [XmlAttribute]
        public bool IsLocalPath { get; set; }

        [XmlAttribute]
        public string NewUserName { get; set; }

        [XmlAttribute]
        public string NewPassword { get; set; }

        [XmlAttribute]
        public string Domain { get; set; }

        [XmlAttribute]
        public bool OrignalImageFromFlile { get; set; }

    }
}
