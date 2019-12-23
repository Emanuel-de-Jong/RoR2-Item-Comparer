using BepInEx;
using RoR2;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace ItemCounter
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.KillBottt.ItemCounter", "ItemCounter", "0.1.0")]
    public class ItemCounter : BaseUnityPlugin
    {
        CharacterBody cachedCharacterBody;
        Notification notification;
        List<User> users = new List<User>();

        double t1Value = 1;
        double t2Value = 1.75;
        double t3Value = 3;
        double tBossValue = 4;

        public double T1Value1 { get => t1Value; set => t1Value = value; }
        public double T2Value { get => t2Value; set => t2Value = value; }
        public double T3Value { get => t3Value; set => t3Value = value; }
        public double TBossValue { get => tBossValue; set => tBossValue = value; }

        public void Update()
        {
            LocalUser localUser = LocalUserManager.GetFirstLocalUser();

            if (cachedCharacterBody == null && localUser != null)
            {
                cachedCharacterBody = localUser.cachedBody;
            }

            if (notification == null && cachedCharacterBody != null)
            {
                notification = cachedCharacterBody.gameObject.AddComponent<Notification>();
                notification.transform.SetParent(cachedCharacterBody.transform);
                notification.SetPosition(new Vector3((float)(Screen.width * 10 / 100f), (float)(Screen.height * 35 / 100f), 0));
                notification.GetTitle = () => "Item Values";
                notification.GenericNotification.fadeTime = 1f;
                notification.GenericNotification.duration = 86400f;
                Notification.SetFontSize(notification.GenericNotification.titleText, 18);
                Notification.SetFontSize(notification.GenericNotification.descriptionText, 14);
            }

            if (cachedCharacterBody == null && notification != null)
            {
                Destroy(notification);
            }



            
            

            foreach (PlayerCharacterMasterController playerCharacterMasterController in PlayerCharacterMasterController.instances)
            {
                User user = new User();
                user.Name = playerCharacterMasterController.GetDisplayName();
                user.ItemValue =
                    playerCharacterMasterController.master.inventory.GetTotalItemCountOfTier(ItemTier.Tier1) * t1Value +
                    playerCharacterMasterController.master.inventory.GetTotalItemCountOfTier(ItemTier.Tier2) * t2Value +
                    playerCharacterMasterController.master.inventory.GetTotalItemCountOfTier(ItemTier.Tier3) * t3Value +
                    playerCharacterMasterController.master.inventory.GetTotalItemCountOfTier(ItemTier.Boss) * tBossValue;
                users.Add(user);
            }

            notification.GetDescription = "";
            foreach (User user in users)
            {
                notification.GetDescription += user.Name + ": " + Math.Round(user.ItemValue) + "\n";
            }


            notification.SetSize(250, 50 + (users.Count * 20));





            if (notification != null && notification.RootObject != null)
            {
                notification.RootObject.SetActive(true);
            }
            
            users.Clear();
        }
    }
}