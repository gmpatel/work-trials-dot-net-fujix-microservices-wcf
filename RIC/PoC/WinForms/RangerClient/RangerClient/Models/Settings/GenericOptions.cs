using System;
using AxRANGERLib;
using FujiXerox.RangerClient.Helpers;

namespace FujiXerox.RangerClient.Models.Settings
{
    public class GenericOptions
    {
        public string Filename { get; set; }
        public OptionalDevices OptionalDevices { get; set; }
        public IqaGeneral IqaGeneral { get; set; }
        public OptionsLogging OptionsLogging { get; set; }
        public FrontEndorser FrontEndorser { get; set; }
        public RearEndorser RearEndorser { get; set; }

        private AxRanger AxRanger { get; set; }

        private Func<string, string, string> GetGenericFunc { get; set; }
 
        public GenericOptions(Func<string, string, string>  getGenericOptions)
        {
            GetGenericFunc = getGenericOptions;
            IqaGeneral = new IqaGeneral();
            OptionalDevices = new OptionalDevices();
            OptionsLogging = new OptionsLogging();
            FrontEndorser = new FrontEndorser();
            RearEndorser = new RearEndorser();
        }

        private object GetSetting(string sectionName, string valueName)
        {
            var result = GetGenericFunc(sectionName, valueName);
            return result;
        }

        public void Load()
        {
            AttributeHelper.UpdatePropertyValueFromAttribute(IqaGeneral, GetSetting);
            AttributeHelper.UpdatePropertyValueFromAttribute(OptionalDevices, GetSetting);
            AttributeHelper.UpdatePropertyValueFromAttribute(OptionsLogging, GetSetting);
            AttributeHelper.UpdatePropertyValueFromAttribute(FrontEndorser, GetSetting);
            AttributeHelper.UpdatePropertyValueFromAttribute(RearEndorser, GetSetting);
        }

        public void Save()
        {
            
        }

    }
}