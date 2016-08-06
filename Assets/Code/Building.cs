﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Building : MonoBehaviour {

    [SerializeField]
    string name; 
    public string Name { get { return name; } }
    [SerializeField]
	float efficiency = 1.0f;
	float currentCycle = 0.0f; 

    Job[] jobs;

    //The lists are so we can view and set values in the inspector
    [SerializeField]
    List<ResourceMax> _inputStock;
    //The dictionary is what we use in game. Having both reference the same resource instance should keep it all linked. 
    Dictionary<string, ResourceMax> inputStock;
    [SerializeField]
    List<ResourceMax> _outputStock;
    Dictionary<string, ResourceMax> outputStock; 
    [SerializeField]
    List<Resource> productionInput;
    [SerializeField]
    List<Resource> productionOutput;
    [SerializeField]
    List<Resource> creationCost; 
    [SerializeField]
    List<Building> upgradePossibilities;

    Dictionary<string, int> upgradeStock;
    Building upgradingTo; 

	[SerializeField]
	List<Recipe> recipies;

	Recipe currentRecipe;


	// Use this for initialization
	void Start () {
        jobs = GetComponentsInChildren<Job>();
        GameManager.RegisterBuilding(this);

		currentRecipe = recipies[0];
	}

    internal void UpdateEfficiency()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update () {
	
	}

    void Produce()
    {
        Debug.Log("Producing: " + currentRecipe); 
    }

    public void NextCycle()
    {
		if (currentRecipe != null) {
			currentCycle++;

			if (currentCycle > (currentRecipe.GetCyclesToProduce() * efficiency))
			{
				currentCycle -= (currentRecipe.GetCyclesToProduce() * efficiency);
				Produce(); 
			}
		}
        
    }
    public void ClickedOn()
    {
        Debug.Log("clicked on" + name);
        UIManager.DisplayUpgradePossiblities(this, upgradePossibilities); 
    }

    public void UpgradeBuliding(Building upgradeBuilding)
    {
        upgradingTo = upgradeBuilding; 
    }

    public int RequestFromOutput(string reqResource, int reqAmount)
    {
        int owendAmount = outputStock.ContainsKey(reqResource) ? outputStock[reqResource].Amount : 0;
        int removedAmount = Math.Min(owendAmount, reqAmount); 
        if (removedAmount != 0)
        {
            outputStock[reqResource].Amount -= removedAmount; 
        }
        return removedAmount; 
    }
    public void AddResourcesToInput(string resource, int amount)
    {
        if (inputStock.ContainsKey(resource))
        {
            inputStock[resource].Amount += amount;
        } else
        {
            ResourceMax newResource = new ResourceMax();
            newResource.Type = resource;
            newResource.Amount = amount;
            newResource.Max = -1; //This needs to be figured out, right now leaving -1 as a 'no max' 
            inputStock[resource] = newResource; 
        }
    }
}
