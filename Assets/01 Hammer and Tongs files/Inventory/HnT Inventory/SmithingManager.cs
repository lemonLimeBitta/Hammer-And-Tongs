using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameCreator.Variables;
using UnityEngine.UI;
using System;
using System.Linq;
using GameCreator.Core;
using UnityEngine.Audio;

public class SmithingManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Item thisItem;
    [SerializeField] private Inventory inv;
    [SerializeField] private int itemTier;
    [SerializeField] private int ironIngot;
    [SerializeField] private int bronzeIngot;
    [SerializeField] private int steelIngot;
    [SerializeField] private int goldIngot;


    //error messages
    string invFullErrorString = "The inventory is full";
    string insufficientResources = "Not enough ingots";

    //audio
    [SerializeField] private AudioClip audioClip;
    private AudioMixerGroup audioMixer;

    //getters
    public int GetSellValue()
    {
        return thisItem.GetItemValue();
    }
    private void Start()
    {
        audioMixer = DatabaseGeneral.Load().soundAudioMixer;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AttemptCraft(ironIngot, bronzeIngot, steelIngot, goldIngot);
    }

    public void AttemptCraft(int localIron, int localBronze, int localSteel, int localGold)
    {
        if ((float)VariablesManager.GetGlobal("IronIngot") >= localIron && (float)VariablesManager.GetGlobal("BronzeIngot") >= localBronze && (float)VariablesManager.GetGlobal("SteelIngot") >= localSteel &&
              (float)VariablesManager.GetGlobal("GoldIngot") >= localGold && itemTier <= (float) VariablesManager.GetGlobal("HammerTier"))
            //check to see if inventory is full
        {
            if (inv.CheckIfFull())
            {
                ActionManager.DisplayErrorMessage(invFullErrorString);
                //ActionErrorMessage.InstantExecute()
               
                return;
            }
            else
            {
                Debug.Log("Crafting that ingot");
                // VariablesManager.SetGlobal(ingotName, (float)VariablesManager.GetGlobal(ingotName) + 1);
                VariablesManager.SetGlobal("IronIngot", (float)VariablesManager.GetGlobal("IronIngot") - localIron);
                VariablesManager.SetGlobal("BronzeIngot", (float)VariablesManager.GetGlobal("BronzeIngot") - localBronze);
                VariablesManager.SetGlobal("SteelIngot", (float)VariablesManager.GetGlobal("SteelIngot") - localSteel);
                VariablesManager.SetGlobal("GoldIngot", (float)VariablesManager.GetGlobal("GoldIngot") - localGold);

                ActionManager.AddItemAction(thisItem);
                //play audio sound
                AudioManager.Instance.PlaySound2D(audioClip, 0, 0.5f, audioMixer);
            }

        }
        else
        {
            ActionManager.DisplayErrorMessage(insufficientResources);
            //put in a gamrplay visual of this.
        }
    }
}
