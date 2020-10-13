using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace EGRP
{
    /// <summary>
    /// Handle incoming connections,
    /// defers until wl confirmed via sql
    /// </summary>

    class Whitelist : CitizenFX.Core.BaseScript
    {
        async void OnPlayerConnecting([FromSource]Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            //Defers join request until later tick.
            //Delay is mandatory
            deferrals.defer();
            await Delay(0);

            //Fetch license of incoming connection
            var licenseIdentifier = player.Identifiers["steam"];

            //Just updating the client
            deferrals.update($"Checking whitelist.. Identifier[{licenseIdentifier}])");

            //Confirm whitelist via sql.
            string lic = await Database.ExecuteQuery($"SELECT * FROM egrp.whitelist WHERE license = '{licenseIdentifier}'");
            if (lic != licenseIdentifier && !string.IsNullOrEmpty(licenseIdentifier))
            {
                deferrals.done($"User not whitelisted.");
            }
            else {
                deferrals.update($"User whitelisted.");
                deferrals.done();
            }
        }
        /// <summary>
        /// Init event(s) in constructor
        /// </summary>
        public Whitelist(){
            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(OnPlayerConnecting);
        }
    }
}
