using HarmonyLib;
using SandBox.CampaignBehaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace AlleyTweaks.Patches {
    [HarmonyPatch(typeof(AlleyCampaignBehavior), "StartNewAlleyAttack")]
    internal class AlleyAttackPatch {
        [HarmonyPrefix]
        static bool Prefix(object[] __args) {
            PlayerAlleyReflection playerAlleyData = new(__args[0]);

            Alley? attackingAlley = playerAlleyData.Alley.Settlement.Alleys.Where((Alley x) => x.State == Alley.AreaState.OccupiedByGangLeader && x.Owner.GetRelationWithPlayer() < 40).GetRandomElementInefficiently();
            if (attackingAlley == null) return false;

            playerAlleyData.ChangeOriginalField("UnderAttackBy", attackingAlley);
            attackingAlley.Owner.SetHasMet();
            float alleyAttackResponseTimeInDays = Campaign.Current.Models.AlleyModel.GetAlleyAttackResponseTimeInDays(playerAlleyData.TroopRoster);
            playerAlleyData.ChangeOriginalField("AttackResponseDueDate", CampaignTime.DaysFromNow(alleyAttackResponseTimeInDays));
            TextObject textObject = new TextObject("{=5bIpeW9X}Your alley in {SETTLEMENT} is under attack from neighboring gangs. Unless you go to their help, the alley will be lost in {RESPONSE_TIME} days.");
            textObject.SetTextVariable("SETTLEMENT", playerAlleyData.Alley.Settlement.Name);
            textObject.SetTextVariable("RESPONSE_TIME", alleyAttackResponseTimeInDays);
            ChangeRelationAction.ApplyPlayerRelation(attackingAlley.Owner, -5);
            Campaign.Current.CampaignInformationManager.NewMapNoticeAdded(new AlleyUnderAttackMapNotification(playerAlleyData.Alley, textObject));


            return false;
        }
    }
}
