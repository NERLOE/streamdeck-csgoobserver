function convertImgToBase64(url, callback, outputFormat) {
  var canvas = document.createElement("CANVAS");
  var ctx = canvas.getContext("2d");
  var img = new Image();
  img.crossOrigin = "Anonymous";
  img.onload = function () {
    canvas.height = img.height;
    canvas.width = img.width;
    ctx.drawImage(img, 0, 0);
    var dataURL = canvas.toDataURL(outputFormat || "image/png");
    callback.call(this, dataURL);
    // Clean up
    canvas = null;
  };
  img.src = url;
}

const cachedBase64Images = [];

function convertImgElementToBase64(id, outputFormat) {
  if (cachedBase64Images[id]) return cachedBase64Images[id];

  var canvas = document.createElement("CANVAS");
  var ctx = canvas.getContext("2d");
  var img = document.getElementById(id);

  canvas.height = img.height;
  canvas.width = img.width;
  ctx.drawImage(img, 0, 0);
  var dataURL = canvas.toDataURL(outputFormat || "image/png");

  cachedBase64Images[id] = dataURL;

  //callback.call(this, dataURL);
  // Clean up
  canvas = null;

  return dataURL;
}

// Register the plugin or PI
function registerPluginOrPI(inEvent, inUUID) {
  if (websocket) {
    var json = {
      event: inEvent,
      uuid: inUUID,
    };

    websocket.send(JSON.stringify(json));
  }
}

// Save settings
function saveSettings(inAction, inUUID, inSettings) {
  if (websocket) {
    const json = {
      action: inAction,
      event: "setSettings",
      context: inUUID,
      payload: inSettings,
    };

    websocket.send(JSON.stringify(json));
  }
}

// Save global settings
function saveGlobalSettings(inUUID) {
  if (websocket) {
    const json = {
      event: "setGlobalSettings",
      context: inUUID,
      payload: globalSettings,
    };

    websocket.send(JSON.stringify(json));
  }
}

// Request global settings for the plugin
function requestGlobalSettings(inUUID) {
  if (websocket) {
    var json = {
      event: "getGlobalSettings",
      context: inUUID,
    };

    websocket.send(JSON.stringify(json));
  }
}

// Log to the global log file
function log(inMessage) {
  // Log to the developer console
  var time = new Date();
  var timeString = time.toLocaleDateString() + " " + time.toLocaleTimeString();
  console.log(timeString, inMessage);

  // Log to the Stream Deck log file
  if (websocket) {
    var json = {
      event: "logMessage",
      payload: {
        message: inMessage,
      },
    };

    websocket.send(JSON.stringify(json));
  }
}

// Set image on the key
function setImage(inUUID, image) {
  if (websocket) {
    var json = {
      event: "setImage",
      context: inUUID,
      payload: {
        image: image,
        target: 0,
      },
    };

    websocket.send(JSON.stringify(json));
  }
}

// Set title on the key
function setTitle(inUUID, title) {
  if (websocket) {
    var json = {
      event: "setTitle",
      context: inUUID,
      payload: {
        title: title,
        target: 0,
      },
    };

    websocket.send(JSON.stringify(json));
  }
}

// Show alert icon on the key
function showAlert(inUUID) {
  if (websocket) {
    var json = {
      event: "showAlert",
      context: inUUID,
    };

    websocket.send(JSON.stringify(json));
  }
}

// Set the state of a key
function setState(inContext, inState) {
  console.log("CHANGING STATE", inContext, inState);
  if (websocket) {
    var json = {
      event: "setState",
      context: inContext,
      payload: {
        state: inState,
      },
    };

    websocket.send(JSON.stringify(json));
  }
}

// Set data to PI
function sendToPropertyInspector(inAction, inContext, inData) {
  if (websocket) {
    var json = {
      action: inAction,
      event: "sendToPropertyInspector",
      context: inContext,
      payload: inData,
    };

    websocket.send(JSON.stringify(json));
  }
}

// Set data to plugin
function sendToPlugin(inAction, inContext, inData) {
  if (websocket) {
    var json = {
      action: inAction,
      event: "sendToPlugin",
      context: inContext,
      payload: inData,
    };

    websocket.send(JSON.stringify(json));
  }
}

// Load the localizations
function getLocalization(inLanguage, inCallback) {
  var url = "../" + inLanguage + ".json";
  var xhr = new XMLHttpRequest();
  xhr.open("GET", url, true);

  xhr.onload = function () {
    if (xhr.readyState === XMLHttpRequest.DONE) {
      try {
        data = JSON.parse(xhr.responseText);
        var localization = data["Localization"];
        inCallback(true, localization);
      } catch (e) {
        inCallback(false, "Localizations is not a valid json.");
      }
    } else {
      inCallback(false, "Could not load the localizations.");
    }
  };

  xhr.onerror = function () {
    inCallback(false, "An error occurred while loading the localizations.");
  };

  xhr.ontimeout = function () {
    inCallback(false, "Localization timed out.");
  };

  xhr.send();
}
