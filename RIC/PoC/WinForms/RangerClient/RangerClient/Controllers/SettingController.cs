using FujiXerox.RangerClient.Helpers;
using FujiXerox.RangerClient.Models.Settings;
using FujiXerox.RangerClient.Views;
using Serilog;

namespace FujiXerox.RangerClient.Controllers
{
    public class SettingController
    {
        private readonly ISettingsView _settingsView;
        private readonly GenericOptions _genericOptions;

        public SettingController(ISettingsView view)
        {
            _settingsView = view;
            _genericOptions = new GenericOptions(GetGenericOption);
        }

        public void Initialize()
        {
            _genericOptions.Load();
        }

        public string GetGenericOption(string sectionName, string valueName)
        {
            var result = _settingsView.AxRanger.GetGenericOption(sectionName, valueName);
            Log.Debug("SettingController: GetGenericOption({0}, {1}) returns {2}", sectionName, valueName, result);
            return result;
        }

        public string GetGenericOptionFileName()
        {
            var result = _settingsView.AxRanger.GetGenericOptionFileName();
            _settingsView.GeneralOptionFilename = result;
            Log.Debug("SettingController: GetGenericOptionFileName() returns {0}", result);
            return result;
        }

        public bool SetGenericOptionFilename(string filename)
        {
            var result = _settingsView.AxRanger.SetGenericOptionFileName(filename);
            _settingsView.GeneralOptionFilename = filename;
            Log.Debug("SettingController: SetGenericOptionFileName({0}) returns {1}", filename, result);
            return result;
        }

        public string GetDriverOptionFileName()
        {
            var result = _settingsView.AxRanger.GetDriverOptionFileName();
            _settingsView.DriverOptionFilename = result;
            Log.Debug("SettingController: GetDriverOptionFileName() returns {0}", result);
            return result;
        }

        public bool SetDriverOptionFilename(string filename)
        {
            var result = _settingsView.AxRanger.SetDriverOptionFileName(filename);
            _settingsView.DriverOptionFilename = filename;
            Log.Debug("SettingController: SetDriverOptionFileName({0}) returns {1}", filename, result);
            return result;
        }
    }
}