using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSwitchToGeneralOnDeath;
public class ModConfig {
    public static ModConfig Loaded { get; set; } = new();

    public bool MoveRuinedClutterMessagesToCurrentTab { get; set; } = false;
    public bool NoSwitchOnRuinedClutterMessages { get; set; } = true;
}
