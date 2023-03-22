using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;

namespace AlleyTweaks {
    public class PlayerAlleyReflection {
        private object original;
        public Alley Alley { get; private set; }
        public Alley UnderAttackBy { get; private set; }
        public TroopRoster TroopRoster { get; private set; }
        public CampaignTime AttackResponseDueDate { get; private set; }

        private Dictionary<string, FieldInfo> fields = new();

        public PlayerAlleyReflection(object obj) {
            original = obj;

            Alley = (Alley)GenerateField("Alley", BindingFlags.NonPublic | BindingFlags.Instance);
            UnderAttackBy = (Alley)GenerateField("UnderAttackBy", BindingFlags.NonPublic | BindingFlags.Instance);
            TroopRoster = (TroopRoster)GenerateField("TroopRoster", BindingFlags.NonPublic | BindingFlags.Instance);
            AttackResponseDueDate = (CampaignTime)GenerateField("AttackResponseDueDate", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private object GenerateField(string fieldName, BindingFlags bindingAttr) {
            FieldInfo fieldInfo = original.GetType().GetField(fieldName, bindingAttr);
            fields[fieldName] = fieldInfo;
            return fieldInfo.GetValue(original);
        }

        public void ChangeOriginalField(string fieldName, object value) {
            fields[fieldName].SetValue(original, value);
        }
    }
}
