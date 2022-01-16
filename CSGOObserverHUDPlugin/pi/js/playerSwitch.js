function PlayerSwitch(
  inContext,
  inLanguage,
  inStreamDeckVersion,
  inPluginVersion
) {
  // Init BrightnessPI
  var instance = this;

  // Inherit from PI
  PI.call(this, inContext, inLanguage, inStreamDeckVersion, inPluginVersion);

  document.getElementById("pi").innerHTML = `
  <div class="sdpi-item">
    <div class="sdpi-item-label" id="observerslot-label">Observer Slot</div>
    <select class="sdpi-item-value select" id="observerslot-select">
      <option id="observerslot-none" value="-1">Select an observerslot</option>
      ${loadObserverslots()}
    </select>
  </div>`;

  // Add event listener
  const observerSlot = document.getElementById("observerslot-select");

  console.log("observerslot-select", observerSlot);

  observerSlot.addEventListener("change", observerslotChanged);

  // Brightness changed
  function observerslotChanged(inEvent) {
    console.log("observerslot changed", settings);
    // Save the new observerslot settings
    settings.observerSlot = parseInt(inEvent.target.value);
    instance.saveSettings();

    // Inform the plugin that a new player is set
    instance.sendToPlugin({ piEvent: "valueChanged" });
  }

  function loadObserverslots() {
    const list = [];

    for (let i = 0; i <= 9; i++) {
      list.push(
        `<option id="observerslot-${i}" value="${i}" ${
          settings.observerSlot == i ? "selected" : ""
        }>${i}</option>`
      );
    }

    return list.join(" ");
  }
}
