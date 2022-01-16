using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WindowsInput;
using WindowsInput.Native;
using CSGOObserverHUDPlugin.Backend;

namespace CSGOObserverHUDPlugin
{
    [PluginActionId("dk.minetech.csgoobserverhudplugin.playerswitch")]
    public class PlayerSwitch : PluginBase
    {
        public class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings
                {
                    ObserverSlot = -1
                };
                return instance;
            }

            [FilenameProperty]
            [JsonProperty(PropertyName = "observerSlot")]
            public int ObserverSlot { get; set; }
        }

        #region Private Members

        private PluginSettings settings;
        private InputSimulator iis = new InputSimulator();

        #endregion
        public PlayerSwitch(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                this.settings = PluginSettings.CreateDefaultSettings();
                SaveSettings();
            }
            else
            {
                this.settings = payload.Settings.ToObject<PluginSettings>();
            }

            GSIManager.KeyWillAppear(connection, this.settings);
        }




        public override void Dispose()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"Destructor called");

            GSIManager.DisposeKey(Connection.ContextId);
        }

        public override void KeyPressed(KeyPayload payload)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "Key Pressed");
            var keys = new List<VirtualKeyCode> { VirtualKeyCode.VK_0, VirtualKeyCode.VK_1, VirtualKeyCode.VK_2, VirtualKeyCode.VK_3, VirtualKeyCode.VK_4, VirtualKeyCode.VK_5, VirtualKeyCode.VK_6, VirtualKeyCode.VK_7, VirtualKeyCode.VK_8, VirtualKeyCode.VK_9 };
            
            iis.Keyboard.KeyPress(keys[settings.ObserverSlot]);
        }

        public override void KeyReleased(KeyPayload payload) { }

        public override void OnTick() { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings();
            GSIManager.UpdateKeySettings(Connection, settings);
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        #region Private Methods

        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        #endregion
    }
}