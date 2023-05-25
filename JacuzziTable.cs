using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;
using System.Collections.Generic;
using static KitchenLib.Utils.KitchenPropertiesUtils;
using UnityEngine;
using ApplianceLib.Api;
using KitchenLib.References;
using ApplianceLib.Api.References;
using Unity.Entities;

namespace KitchenJacuzziTable
{
    class JacuzziTable : CustomAppliance
    {
        public override string UniqueNameID => "Jacuzzi Table";
        static GameObject _prefabCache = null;
        public override GameObject Prefab
        {
            get
            {
                if (_prefabCache == null)
                {
                    GameObject container = new GameObject("Hider");
                    container.SetActive(false);
                    _prefabCache = Object.Instantiate((GDOUtils.GetExistingGDO(ApplianceReferences.SinkSoak) as Appliance).Prefab);
                    _prefabCache.transform.SetParent(container.transform);
                }
                return _prefabCache;
            }
        }
        public override PriceTier PriceTier => PriceTier.Free;
        public override RarityTier RarityTier => RarityTier.Common;
        public override bool IsPurchasable => true;
        public override bool IsPurchasableAsUpgrade => true;
        public override ShoppingTags ShoppingTags => ShoppingTags.FrontOfHouse | ShoppingTags.Plumbing;
        public override IEffectRange EffectRange => new CEffectRangeTableSet();
        public override IEffectCondition EffectCondition => new CEffectAlways();

        public override IEffectType EffectType => new CTableModifier()
        {
            OrderingModifiers = new OrderingValues() { PriceModifier = 0.2f, MessFactor = .5f }
        };

        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (Locale.English, new ApplianceInfo()
            {
                Name = "Jacuzzi Table",
                Description = "Fine dining in your own filth",
                Sections = new List<Appliance.Section>
                {
                    new Appliance.Section
                    {
                        Title = "Filthy Rich",
                        Description = "Customers pay <color=#00FF00>20%</color> more <sprite name=\"coin\"> but make <color=#ff1111>50%</color> more <sprite name=\"mess\">"
                    },
                    new Appliance.Section
                    {
                        Title = "Spa Treatment",
                        Description = "Gently massages your dishes clean. Does not combine with other tables"
                    }
                }
            })
        };

        public override List<IApplianceProperty> Properties => new()
        {
            GetCApplianceTable(true, false, true, false, true, true, Orientation.Null),
            new CItemHolder(),
            GetCItemStorage(0, 16, true, true),
            new CHolderFirstIfStorage(),
            new CChangeRestrictedReceiverKeyAfterDuration()
            {
                ApplianceKey = RestrictedTransferKeys.CleanedItems
            }
        };

        public override List<Appliance.ApplianceProcesses> Processes => new List<Appliance.ApplianceProcesses>()
        {
            new Appliance.ApplianceProcesses()
            {
                Process = ((Process) GDOUtils.GetExistingGDO(ProcessReferences.Clean)),
                Speed = 0.25f,
                IsAutomatic = true
            }
        };

        bool isRegistered = false;

        public override void OnRegister(Appliance gameDataObject)
        {
            base.OnRegister(gameDataObject);

            if (!isRegistered)
            {
                ApplyMaterials();
                ApplyComponents();
                isRegistered = true;
            }
        }

        private void ApplyMaterials()
        {
            var materials = new Material[1];
            materials[0] = MaterialUtils.GetExistingMaterial("Rug - Gold");
            MaterialUtils.ApplyMaterial(Prefab, "SinkSoak/Cube.001", materials);
            materials[0] = MaterialUtils.GetExistingMaterial("Wallpaper - Turkey");
            MaterialUtils.ApplyMaterial(Prefab, "SinkSoak/Cube.002", materials);
            materials[0] = MaterialUtils.GetExistingMaterial("Wallpaper - Leaves");
            MaterialUtils.ApplyMaterial(Prefab, "SinkSoak/Cube.003", materials);
            materials[0] = MaterialUtils.GetExistingMaterial("Cashew");
            MaterialUtils.ApplyMaterial(Prefab, "Active/Water", materials);
        }

        private void ApplyComponents()
        {

        }
    }
}
