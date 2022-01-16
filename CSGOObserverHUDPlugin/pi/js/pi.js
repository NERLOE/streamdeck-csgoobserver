function PI(inContext, inLanguage, inStreamDeckVersion, inPluginVersion) {
  // Init PI
  var instance = this;

  // Public localizations for the UI
  this.localization = {};

  // Private function to return the action identifier
  function getAction() {
    var action;

    // Find out type of action
    if (instance instanceof PlayerSwitch) {
      action = "dk.minetech.csgoobserverhudplugin.playerswitch";
    }

    return action;
  }

  // Public function to save the settings
  this.saveSettings = function () {
    saveSettings(getAction(), inContext, settings);
  };

  // Public function to send data to the plugin
  this.sendToPlugin = function (inData) {
    sendToPlugin(getAction(), inContext, inData);
  };
}
